/* 激光连接与数据读取示例*/
#include "Aria.h"

int main(int argc, char **argv)
{
	Aria::init();
	ArLog::init(ArLog::StdErr, ArLog::Verbose);		//另一种日志内初始化方式，在该方式中，会在屏幕上显示所有调用过程
	ArRobot robot;
	ArArgumentParser parser(&argc, argv);
	parser.loadDefaultArguments();

	ArRobotConnector robotConnector(&parser, &robot);
	ArLaserConnector laserConnector(&parser, &robot, &robotConnector);

	// 连接机器人，根据其类型和名称获取一些初始数据，然后加载该机器人的参数文件
	if(!robotConnector.connectRobot())
	{
		ArLog::log(ArLog::Terse, "lasersExample: Could not connect to the robot.");
		if(parser.checkHelpAndWarnUnparsed())
		{
			// -help not given
			Aria::logOptions();
			Aria::exit(1);
		}
	}


	if (!Aria::parseArgs())
	{
		Aria::logOptions();
		Aria::exit(2);
		return 2;
	}

	ArLog::log(ArLog::Normal, "lasersExample: Connected to robot.");

	robot.runAsync(true);


	// 连接在参数文件中定义的激光
	// 一些标签用作connectLasers()的参数，从而控制错误行为和哪个激光会被放到ArRobot的激光存储列表中
	if(!laserConnector.connectLasers())
	{
		ArLog::log(ArLog::Terse, "Could not connect to configured lasers. Exiting.");
		Aria::exit(3);
		return 3;
	}

	// 等待一段时间以读取激光数据
	ArUtil::sleep(500);

	ArLog::log(ArLog::Normal, "Connected to all lasers.");

	// 输出从连接的激光中获取到的数据
	while(robot.isConnected())
	{
		int numLasers = 0;

		// 获取指向ArRobot已连接激光列表的指针，锁定机器人以防被其他进程修改。
		// 每个激光都有一个索引，可以储存某个激光的索引或名称（laser->getName()）并使用它们作为ArRobot::findLaser()的参数来获取指向对应激光的指针。
		robot.lock();
		std::map<int, ArLaser*> *lasers = robot.getLaserMap();			//获取机器人激光的索引列表

		for(std::map<int, ArLaser*>::const_iterator i = lasers->begin(); i != lasers->end(); ++i)
		{
			int laserIndex = (*i).first;		//列表中激光的索引
			ArLaser* laser = (*i).second;		//指向激光的指针
			if(!laser)
				continue;
			++numLasers;
			laser->lockDevice();		//此处无注释，根据函数名推测可能为锁定激光

			// 当前读数是一系列障碍物读数（X,Y坐标和一些其他属性），它们来自激光最新的数据
			std::list<ArPoseWithTime*> *currentReadings = laser->getCurrentBuffer();

			// 有一个功能是获取限定角度范围内的激光最新读数，如果在给定范围内没有有效的最新读数，dist比laser->getMaxRange()更好（？dist will be greater than laser->getMaxRange()）
			double angle = 0;
			double dist = laser->currentReadingPolar(laser->getStartDegrees(), laser->getEndDegrees(), &angle);		//参数分别为开始角度、结束角度和有效角度。第3个参数默认为空，非空时当获取到了最新读数时将当前角度赋值给第3个参数

			ArLog::log(ArLog::Normal, "Laser #%d (%s): %s. Have %d 'current' readings. Closest reading is at %3.0f degrees and is %2.4f meters away.", laserIndex, laser->getName(), (laser->isConnected() ? "connected" : "NOT CONNECTED"), currentReadings->size(), angle, dist/1000.0);
			laser->unlockDevice();
		}
		if(numLasers == 0)
			ArLog::log(ArLog::Normal, "No lasers.");
		else
			ArLog::log(ArLog::Normal, "");

		// 解锁机器人，休息5秒等待下一个循环
		robot.unlock();
		ArUtil::sleep(5000);
	}

	ArLog::log(ArLog::Normal, "lasersExample: exiting.");
	Aria::exit(0);
	return 0;
}