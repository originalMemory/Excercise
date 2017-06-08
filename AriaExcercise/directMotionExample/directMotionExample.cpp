﻿/* 使用ArRobot的直接运动命令方法将一系列运动命令直接发送到机器人。
该演示以自己的线程启动机器人，然后在ArRobot中调用一系列运动命令方法，以将命令直接发送到机器人（而不是使用例如ArAction对象）。 */
#include "Aria.h"


/*
这是一个连接处理程序类，用于演示如何在其中运行代码以
   响应诸如连接或与机器人断开连接的程序等事件。
   */
class ConnHandler
{
public:
	// Constructor
	ConnHandler(ArRobot *robot);
	// Destructor, its just empty
	~ConnHandler(void) {}
	// 连接建立时调用
	void connected(void);
	// 连接失败时调用
	void connFail(void);
	// 连接断开时调用
	void disconnected(void);
protected:
	// 机器人指针
	ArRobot *myRobot;
	// 回调函数
	ArFunctorC<ConnHandler> myConnectedCB;
	ArFunctorC<ConnHandler> myConnFailCB;
	ArFunctorC<ConnHandler> myDisconnectedCB;
};

ConnHandler::ConnHandler(ArRobot *robot) :
myConnectedCB(this, &ConnHandler::connected),  
	myConnFailCB(this, &ConnHandler::connFail),
	myDisconnectedCB(this, &ConnHandler::disconnected)

{
	myRobot = robot;
	myRobot->addConnectCB(&myConnectedCB, ArListPos::FIRST);
	myRobot->addFailedConnectCB(&myConnFailCB, ArListPos::FIRST);
	myRobot->addDisconnectNormallyCB(&myDisconnectedCB, ArListPos::FIRST);
	myRobot->addDisconnectOnErrorCB(&myDisconnectedCB, ArListPos::FIRST);
}

// 连接失败时调用
void ConnHandler::connFail(void)
{
	printf("directMotionDemo connection handler: Failed to connect.\n");
	myRobot->stopRunning();
	Aria::exit(1);
	return;
}

// 启动发动机，关闭声纳和amigobot
void ConnHandler::connected(void)
{
	printf("directMotionDemo connection handler: Connected\n");
	myRobot->comInt(ArCommands::SONAR, 0);
	myRobot->comInt(ArCommands::ENABLE, 1);
	myRobot->comInt(ArCommands::SOUNDTOG, 0);
}

// 连接断开时结束程序
void ConnHandler::disconnected(void)
{
	printf("directMotionDemo connection handler: Lost connection, exiting program.\n");
	Aria::exit(0);
}



int main(int argc, char **argv) 
{
	Aria::init();

	ArArgumentParser argParser(&argc, argv);
	argParser.loadDefaultArguments();


	ArRobot robot;
	ArRobotConnector con(&argParser, &robot);

	// 从上面建立连接句柄
	ConnHandler ch(&robot);

	if(!Aria::parseArgs())
	{
		Aria::logOptions();
		Aria::exit(1);
		return 1;
	}

	if(!con.connectRobot())
	{
		ArLog::log(ArLog::Normal, "directMotionExample: Could not connect to the robot. Exiting.");

		if(argParser.checkHelpAndWarnUnparsed()) 
		{
			Aria::logOptions();
		}
		Aria::exit(1);
		return 1;
	}

	ArLog::log(ArLog::Normal, "directMotionExample: Connected.");

	if(!Aria::parseArgs() || !argParser.checkHelpAndWarnUnparsed())
	{
		Aria::logOptions();
		Aria::exit(1);
	}

	// 异步运行机器人处理循环，开始循环后的每次操作需注意对机器人锁定与解锁
	robot.runAsync(false);


	// 直接发送给机器人一系列运动命令，休眠几秒以等待机器人运行这些命令
	printf("directMotionExample: Setting rot velocity to 100 deg/sec then sleeping 3 seconds\n");
	robot.lock();
	robot.setRotVel(100);
	robot.unlock();
	ArUtil::sleep(3*1000);
	printf("Stopping\n");
	robot.lock();
	robot.setRotVel(0);
	robot.unlock();
	ArUtil::sleep(200);

	printf("directMotionExample: Telling the robot to go 300 mm on left wheel and 100 mm on right wheel for 5 seconds\n");
	robot.lock();
	robot.setVel2(300, 100);
	robot.unlock();
	ArTime start;
	start.setToNow();
	while (1)
	{
		robot.lock();
		if (start.mSecSince() > 5000)
		{
			robot.unlock();
			break;
		}   
		robot.unlock();
		ArUtil::sleep(50);
	}

	printf("directMotionExample: Telling the robot to move forwards one meter, then sleeping 5 seconds\n");
	robot.lock();
	robot.move(1000);
	robot.unlock();
	start.setToNow();
	while (1)
	{
		robot.lock();
		if (robot.isMoveDone())	//判断机器人是否执行了早先的移动命令
		{
			printf("directMotionExample: Finished distance\n");
			robot.unlock();
			break;
		}
		if (start.mSecSince() > 5000)
		{
			printf("directMotionExample: Distance timed out\n");
			robot.unlock();
			break;
		}   
		robot.unlock();
		ArUtil::sleep(50);
	}
	printf("directMotionExample: Telling the robot to move backwards one meter, then sleeping 5 seconds\n");
	robot.lock();
	robot.move(-1000);
	robot.unlock();
	start.setToNow();
	while (1)
	{
		robot.lock();
		if (robot.isMoveDone())
		{
			printf("directMotionExample: Finished distance\n");
			robot.unlock();
			break;
		}
		if (start.mSecSince() > 10000)
		{
			printf("directMotionExample: Distance timed out\n");
			robot.unlock();
			break;
		}
		robot.unlock();
		ArUtil::sleep(50);
	}
	printf("directMotionExample: Telling the robot to turn to 180, then sleeping 4 seconds\n");
	robot.lock();
	robot.setHeading(180);
	robot.unlock();
	start.setToNow();
	while (1)
	{
		robot.lock();
		if (robot.isHeadingDone(5))
		{
			printf("directMotionExample: Finished turn\n");
			robot.unlock();
			break;
		}
		if (start.mSecSince() > 5000)
		{
			printf("directMotionExample: Turn timed out\n");
			robot.unlock();
			break;
		}
		robot.unlock();
		ArUtil::sleep(100);
	}
	printf("directMotionExample: Telling the robot to turn to 90, then sleeping 2 seconds\n");
	robot.lock();
	robot.setHeading(90);
	robot.unlock();
	start.setToNow();
	while (1)
	{
		robot.lock();
		if (robot.isHeadingDone(5))
		{
			printf("directMotionExample: Finished turn\n");
			robot.unlock();
			break;
		}
		if (start.mSecSince() > 5000)
		{
			printf("directMotionExample: turn timed out\n");
			robot.unlock();
			break;
		}
		robot.unlock();
		ArUtil::sleep(100);
	}
	printf("directMotionExample: Setting vel2 to 200 mm/sec on both wheels, then sleeping 3 seconds\n");
	robot.lock();
	robot.setVel2(200, 200);
	robot.unlock();
	ArUtil::sleep(3000);
	printf("directMotionExample: Stopping the robot, then sleeping for 2 seconds\n");
	robot.lock();
	robot.stop();
	robot.unlock();
	ArUtil::sleep(2000);
	printf("directMotionExample: Setting velocity to 200 mm/sec then sleeping 3 seconds\n");
	robot.lock();
	robot.setVel(200);
	robot.unlock();
	ArUtil::sleep(3000);
	printf("directMotionExample: Stopping the robot, then sleeping for 2 seconds\n");
	robot.lock();
	robot.stop();
	robot.unlock();
	ArUtil::sleep(2000);
	printf("directMotionExample: Setting vel2 with 0 on left wheel, 200 mm/sec on right, then sleeping 5 seconds\n");
	robot.lock();
	robot.setVel2(0, 200);
	robot.unlock();
	ArUtil::sleep(5000);
	printf("directMotionExample: Telling the robot to rotate at 50 deg/sec then sleeping 5 seconds\n");
	robot.lock();
	robot.setRotVel(50);
	robot.unlock();
	ArUtil::sleep(5000);
	printf("directMotionExample: Telling the robot to rotate at -50 deg/sec then sleeping 5 seconds\n");
	robot.lock();
	robot.setRotVel(-50);
	robot.unlock();
	ArUtil::sleep(5000);
	printf("directMotionExample: Setting vel2 with 0 on both wheels, then sleeping 3 seconds\n");
	robot.lock();
	robot.setVel2(0, 0);
	robot.unlock();
	ArUtil::sleep(3000);
	printf("directMotionExample: Now having the robot change heading by -125 degrees, then sleeping for 6 seconds\n");
	robot.lock();
	robot.setDeltaHeading(-125);
	robot.unlock();
	ArUtil::sleep(6000);
	printf("directMotionExample: Now having the robot change heading by 45 degrees, then sleeping for 6 seconds\n");
	robot.lock();
	robot.setDeltaHeading(45);
	robot.unlock();
	ArUtil::sleep(6000);
	printf("directMotionExample: Setting vel2 with 200 on left wheel, 0 on right wheel, then sleeping 5 seconds\n");
	robot.lock();
	robot.setVel2(200, 0);
	robot.unlock();
	ArUtil::sleep(5000);

	printf("directMotionExample: Done, exiting.\n");
	Aria::exit(0);
	return 0;
}