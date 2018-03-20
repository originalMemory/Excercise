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

class JoydriveAction : public ArAction
{
public:
	// constructor
	JoydriveAction(void);
	// empty destructor
	virtual ~JoydriveAction(void);
	//the fire which will actually tell the resolver what to do
	virtual ArActionDesired *fire(ArActionDesired currentDesired);
	// whether the joystick is initalized or not
	bool joystickInited(void);
protected:
	// action desired
	ArActionDesired myDesired;
	// joystick handler
	ArJoyHandler myJoyHandler;
};
/*
Note the use of constructor chaining with ArAction.
*/
JoydriveAction::JoydriveAction(void) :
ArAction("Joydrive Action", "This action reads the joystick and sets the translational and rotational velocity based on this.")
{
	// initialize the joystick
	myJoyHandler.init();
	// set up the speed parameters on the joystick
	myJoyHandler.setSpeeds(50, 700);
}
JoydriveAction::~JoydriveAction(void)
{
}
// whether the joystick is there or not
bool JoydriveAction::joystickInited(void)
{
	return myJoyHandler.haveJoystick();
}
// the guts of the thing
ArActionDesired *JoydriveAction::fire(ArActionDesired currentDesired)
{
	int rot, trans;
	// print out some info about hte robot
	printf("\rx %6.1f  y %6.1f  tth  %6.1f vel %7.1f mpacs %3d", myRobot->getX(),
		myRobot->getY(), myRobot->getTh(), myRobot->getVel(),
		myRobot->getMotorPacCount());
	fflush(stdout);
	// see if one of the buttons is pushed, if so drive
	if (myJoyHandler.haveJoystick() && (myJoyHandler.getButton(1) ||
		myJoyHandler.getButton(2)))
	{
		// get the readings from the joystick
		myJoyHandler.getAdjusted(&rot, &trans);
		// set what we want to do
		myDesired.setVel(trans);
		myDesired.setDeltaHeading(-rot);
		// return the actionDesired
		return &myDesired;
	}
	else
	{
		// set what we want to do
		myDesired.setVel(0);
		myDesired.setDeltaHeading(0);
		// return the actionDesired
		return &myDesired;
	}
}

int main(int argc, char **argv){
	


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

	//手柄操纵类
	//JoydriveAction jdAct;
	//// if the joydrive action couldn't find the joystick, then exit.
	//if (!jdAct.joystickInited())
	//{
	//	printf("Do not have a joystick, set up the joystick then rerun the program\n\n");
	//	Aria::exit(1);
	//	return 1;
	//}

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
			//robot->addAction(&jdAct, 100);
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