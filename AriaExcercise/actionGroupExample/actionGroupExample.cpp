#include "Aria.h"

/* 本实例演示了动作组的创建和使用 */

ArActionGroup *teleop;		//遥控动作组
ArActionGroup *wander;		//漫步动作组

//激活遥控模式，其余模式全部停止
void teleopMode(void)
{
  teleop->activateExclusive();
  printf("\n== Teleoperation Mode ==\n");
  printf("   Use the arrow keys to drive, and the spacebar to stop.\n    For joystick control hold the trigger button.\n    Press 'w' to switch to wander mode.\n    Press escape to exit.\n");
}

//激活漫步模式，其余模式全部停止
void wanderMode(void)
{
  wander->activateExclusive();
  printf("\n== Wander Mode ==\n");
  printf("    The robot will now just wander around avoiding things.\n    Press 't' to switch to  teleop mode.\n    Press escape to exit.\n");
}


int main(int argc, char** argv)
{
  Aria::init();		//初始化
  ArArgumentParser argParser(&argc, argv);		//参数类
  ArSimpleConnector con(&argParser);			//传统机器人与激光连接类，不推荐使用
  ArRobot robot;
  ArSonarDevice sonar;

  argParser.loadDefaultArguments();				//加载默认环境变量
  //判断是否成功解析程序参数 或 检查帮助字符串和警告未解析参数
  if(!Aria::parseArgs() || !argParser.checkHelpAndWarnUnparsed())
  {
    Aria::logOptions();			//记录程序所有选项
    return 1;
  }

  /* 遥控动作组 */
  teleop = new ArActionGroup(&robot);

  // 防碰桌子功能 (如果机器人有IR桌子传感器)
  teleop->addAction(new ArActionLimiterTableSensor, 100);

  // 限制撞到前方邻近的障碍物
  teleop->addAction(new ArActionLimiterForwards("speed limiter near", 
                        300, 600, 250), 95);

  // 限制撞到前方远处的障碍物
  teleop->addAction(new ArActionLimiterForwards("speed limiter far", 
                           300, 1100, 400), 90);

  // 限制撞到后方的障碍物
  teleop->addAction(new ArActionLimiterBackwards, 85);

  // 摇杆动作
  ArActionJoydrive joydriveAct("joydrive", 400, 15);
  teleop->addAction(&joydriveAct, 50);

  // 键盘动作
  teleop->addAction(new ArActionKeydrive, 45);
  


  /* 漫步模式 */
  wander = new ArActionGroup(&robot);

  // 失速恢复功能
  wander->addAction(new ArActionStallRecover, 100);

  // 碰撞处理功能
  wander->addAction(new ArActionBumpers, 75);

  // 躲避障碍物功能，如果机器车与障碍物距离为225，停止运动
  wander->addAction(new ArActionAvoidFront("Avoid Front Near", 225, 0), 50);

  // 慢速躲避障碍物功能
  wander->addAction(new ArActionAvoidFront, 45);

  // 以恒定速度运行
  wander->addAction(new ArActionConstantVelocity("Constant Velocity", 400), 25);

  

  /* 使用键盘命令切换模式 */

  // 如果Aria没有预备，则自行创建键盘按键句柄并与机器人绑定
  ArKeyHandler *keyHandler = Aria::getKeyHandler();
  if (keyHandler == NULL)
  {
    keyHandler = new ArKeyHandler;
    Aria::setKeyHandler(keyHandler);
    robot.attachKeyHandler(keyHandler);
  }

  // 设置回调函数
  ArGlobalFunctor teleopCB(&teleopMode);
  ArGlobalFunctor wanderCB(&wanderMode);
  keyHandler->addKeyHandler('w', &wanderCB);
  keyHandler->addKeyHandler('W', &wanderCB);
  keyHandler->addKeyHandler('t', &teleopCB);
  keyHandler->addKeyHandler('T', &teleopCB);

  // 检查摇杆是否存在
  if (!joydriveAct.joystickInited())
    printf("Note: Do not have a joystick, only the arrow keys on the keyboard will work.\n");
  
  // 设置摇杆不使用时暂停动作
  joydriveAct.setStopIfNoButtonPressed(false);


  /* 连接机器人，初始以遥控模式运行  */

  robot.addRangeDevice(&sonar);
  if(!con.connectRobot(&robot))
  { 
    ArLog::log(ArLog::Terse, "actionGroupExample: Could not connect to the robot.");
    Aria::exit(1);
  }

  robot.enableMotors();
  teleopMode();
  robot.run(true);

  Aria::exit(0);
}