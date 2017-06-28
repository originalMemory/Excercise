/* 陀螺仪的使用示例 */
#include "Aria.h"
#include "ArAnalogGyro.h"


class GyroTask
{
public:
  // 构造函数，需在构造时初始化其回调函数
  GyroTask(ArRobot *robot);
  // 空的析构函数
  ~GyroTask(void) {}
  
  // 目标
  void doTask(void);
protected:
  //double myHeading;
  ArAnalogGyro *myGyro;
  ArRobot *myRobot;
  ArFunctorC<GyroTask> myTaskCB;		//回调函数
};


// 构造函数
GyroTask::GyroTask(ArRobot *robot) :
  myTaskCB(this, &GyroTask::doTask)		//绑定回调具体调用函数
{
	ArKeyHandler *keyHandler;
	myRobot = robot;
	// 将回调函数添加至机器人循环队列中，该队列中的函数每个周期都会调用一次
	myRobot->addUserTask("GyroTask", 50, &myTaskCB);		//第一个参数为目标名称，第二个参数为调用优先级，第三个参数为调用的函数指针
	myGyro = new ArAnalogGyro(myRobot);
	//绑定键盘操作句柄
	if ((keyHandler = Aria::getKeyHandler()) == NULL)
	{
		keyHandler = new ArKeyHandler;
		Aria::setKeyHandler(keyHandler);
		if (myRobot != NULL)
			myRobot->attachKeyHandler(keyHandler);
		else
			ArLog::log(ArLog::Terse, "GyroTask: No robot to attach a keyHandler to, keyHandling won't work... either make your own keyHandler and drive it yourself, make a keyhandler and attach it to a robot, or give this a robot to attach to.");
	}  
	keyHandler->addKeyHandler('1', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 10));
	keyHandler->addKeyHandler('2', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 20));
	keyHandler->addKeyHandler('3', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 30));
	keyHandler->addKeyHandler('4', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 40));
	keyHandler->addKeyHandler('5', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 50));
	keyHandler->addKeyHandler('6', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 60));
	keyHandler->addKeyHandler('7', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 70));
	keyHandler->addKeyHandler('8', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 80));
	keyHandler->addKeyHandler('9', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 90));
	keyHandler->addKeyHandler('0', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, 100));

	keyHandler->addKeyHandler('q', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -10));
	keyHandler->addKeyHandler('w', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -20));
	keyHandler->addKeyHandler('e', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -30));
	keyHandler->addKeyHandler('r', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -40));
	keyHandler->addKeyHandler('t', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -50));
	keyHandler->addKeyHandler('y', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -60));
	keyHandler->addKeyHandler('u', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -70));
	keyHandler->addKeyHandler('i', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -80));
	keyHandler->addKeyHandler('o', new ArFunctor1C<ArRobot, double>(myRobot,&ArRobot::setRotVel, -90));
	keyHandler->addKeyHandler('p', new ArFunctor1C<ArRobot, double>(myRobot, &ArRobot::setRotVel, -100));

	keyHandler->addKeyHandler('a', new ArFunctor1C<ArRobot, double>(myRobot, &ArRobot::setHeading, 0));
	keyHandler->addKeyHandler('s', new ArFunctor1C<ArRobot, double>(myRobot, &ArRobot::setHeading, 90));
	keyHandler->addKeyHandler('d', new ArFunctor1C<ArRobot, double>(myRobot, &ArRobot::setHeading, 180));
	keyHandler->addKeyHandler('f', new ArFunctor1C<ArRobot, double>(myRobot, &ArRobot::setHeading, 270));

  }

void GyroTask::doTask(void)
{
  /*
  double degrees = -((myRobot->getAnalog() * 5.0 / 255) - 2.509) * 150 / 2.5 * 1.265;
  if (fabs(degrees) < 2)
    degrees = 0;
  myHeading += degrees * .025;
  printf("%10f %10f %10f %10f\n", myRobot->getAnalog() * 5.0 / 255, degrees,
     myRobot->getRotVel(), myHeading);
  fflush(stdout);
  */
  printf("gyro th (mode 1 only):%8.4f  encoder th:%8.4f   ArRobot mixed th:%8.4f  temp:%d  ave:%g\n", myGyro->getHeading(), myRobot->getRawEncoderPose().getTh(), myRobot->getTh(), myGyro->getTemperature(), myGyro->getAverage());
}



int main(int argc, char **argv)
{
  Aria::init();
  ArRobot robot;
  
  // 遥控驱动动作
  ArActionJoydrive joydriveAct;
  // 键盘驱动动作
  ArActionKeydrive keydriveAct;

  GyroTask gyro(&robot);

  // 声纳，用以启动防碰撞程序
  ArSonarDevice sonar;

  ArSimpleConnector connector(&argc, argv);
  if (!connector.parseArgs() || argc > 1)
  {
    connector.logOptions();
    Aria::exit(1);
    return 1;
  }
  

  printf("This program will allow you to use a joystick or keyboard to control the robot.\nYou can use the arrow keys to drive, and the spacebar to stop.\nFor joystick control press the trigger button and then drive.\nPress escape to exit.\n");


  if (!joydriveAct.joystickInited())
    printf("Do not have a joystick, only the arrow keys on the keyboard will work.\n");
  
  // 设置遥控状态，当按键没有被按下时不做任何操作
  joydriveAct.setStopIfNoButtonPressed(false);

  // 添加声纳
  robot.addRangeDevice(&sonar);

  if (!connector.connectRobot(&robot))
  {
    printf("Could not connect to robot... exiting\n");
    Aria::exit(1);
    return 1;
  }

  robot.comInt(ArCommands::ENABLE, 1);		//启动发动机

  robot.addAction(&joydriveAct, 50);
  robot.addAction(&keydriveAct, 45);

  // 设置遥控状态，当按键没有被按下时不做任何操作。从而键盘驱动可以操作机器人
  // set the joydrive action so it'll let the keydrive action fire if
  // there is no button pressed
  joydriveAct.setStopIfNoButtonPressed(false);

  
  // 启动机器人，true表示当连接断开时结束运行
  robot.run(true);
  
  // 结束程序
  Aria::exit(0);
  return 0;
}