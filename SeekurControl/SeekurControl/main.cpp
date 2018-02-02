#include "BaseAction.h"
#include <process.h>

#include <iostream>

using namespace std;

void* work_thread(ArRobot* robot)
{
	//线程执行体
	while(1){
		double x=robot->getX();
		double y=robot->getY();
		printf("x:%.2f\ty:%.2f",x,y);
	}
	return 0;
}

int main(int argc, char **argv){
	cout << argv[0] << endl;
	cout << argv[1] << endl;
	cout << argv[2] << endl;


	Aria::init();
	ArArgumentParser argParser(&argc,argv);
	argParser.loadDefaultArguments();

	ArRobot* robot = new ArRobot();
	ArRobotConnector con(&argParser,robot);

	//// 从上面建立连接句柄
	//ConnHandler ch(&robot);

	//解析来自命令行的参数
	if(!Aria::parseArgs()){
		Aria::logOptions();
		Aria::exit(1);
		return 1;
	}
	//连接机器人
	if(!con.connectRobot()){
		ArLog::log(ArLog::Normal,"无法连接机器人");
		if(argParser.checkHelpAndWarnUnparsed()){
			ArLog::log(ArLog::Terse, "无法连接机器人");
		}
		Aria::exit(1);
		return 1;
	}

	robot->com2Bytes(116,6,1);
	robot->com2Bytes(116, 7, 1);
	robot->com2Bytes(116, 28, 1);
	robot->com2Bytes(116, 12, 1);

	// 异步运行机器人处理循环
	robot->enableMotors();
	robot->runAsync(true);
	//robot->comInt(ArCommands::ENABLE, 1);

	//激活基本处理类
	BaseAction action(robot);

	bool repeat=true;
	while (repeat) 
	{
		printf("输入运行指令:0、停止运动\t1、直线前进一段距离\t2、直线前进速度\n\
			   3、设置绝对航向\t4、设置相对航向\t5、旋转速度\n");
		printf("指令:");
		int com;
		double value;
		scanf("%d",&com);
		if (com!=0)
		{
			printf("数值:");
			scanf("%lf",&value);
		}
		switch(com){
		case 0:
			action.Stop();
			break;
		case 1:
			action.Move(value);
			break;
		case 2:
			action.SetVelocity(value);
			break;
		case 3:
			action.SetHeading(value);
			break;
		case 4:
			action.SetDeltaHeading(value);
			break;
		case 5:
			action.SetRotVel(value);
			break;
		default:
			repeat=false;
			break;
		}
	}
	robot->disconnect();
	Aria::exit();

}