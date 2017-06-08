#include "Aria.h"


int main(int argc, char** argv)
{
  // 初始化部分全局数据
  Aria::init();

  // 如果想ArLog输出"Verbose"级别的信息时取消下行注释
  //ArLog::init(ArLog::StdOut, ArLog::Verbose);

  // 该对象从命令行解析程序选项
  ArArgumentParser parser(&argc, argv);

  // 根据命令行参数，从ARIAARGS中加载默认值。Linux中在/etc/Aria.args
  parser.loadDefaultArguments();

  // 核心对象，用于机器人连接与连接与控制
  ArRobot robot;

  // 使用预设的参数连接机器人或模拟器
  ArRobotConnector robotConnector(&parser, &robot);

  // 如果机器人有模拟陀螺仪，则使用本对象连接它，而且如果机器人没有自动使用陀螺仪纠正航向，
  // 这个对象会从ArRobot读取数据并纠正姿态
  ArAnalogGyro gyro(&robot);

  // 连接机器人，获取初始信息并为其加载参数文件.
  if (!robotConnector.connectRobot())
  {
    // 检查参数加载状态
    if (!parser.checkHelpAndWarnUnparsed())
    {
      ArLog::log(ArLog::Terse, "Could not connect to robot, will not have parameter file so options displayed later may not include everything");
    }
    // 其他错误
    else
    {
      ArLog::log(ArLog::Terse, "Error, could not connect to robot.");
      Aria::logOptions();
      Aria::exit(1);
    }
  }

  if(!robot.isConnected())
  {
    ArLog::log(ArLog::Terse, "Internal error: robot connector succeeded but ArRobot::isConnected() is false!");
  }

  // 连接激光
  ArLaserConnector laserConnector(&parser, &robot, &robotConnector);

  // 连接指南针
  ArCompassConnector compassConnector(&parser);

  // 解析命令行选项
  if (!Aria::parseArgs() || !parser.checkHelpAndWarnUnparsed())
  {    
    Aria::logOptions();
    Aria::exit(1);
    return 1;
  }

  // 使用该对象连接并处理声纳数据
  ArSonarDevice sonarDev;
  
  // 使用该对象管理键盘输入
  ArKeyHandler keyHandler;
  Aria::setKeyHandler(&keyHandler);

  // 将其绑定到键盘上
  robot.attachKeyHandler(&keyHandler);
  printf("You may press escape to exit\n");

  // 绑定声纳处理对象
  robot.addRangeDevice(&sonarDev);

  
  // 启动机器人任务循环
  robot.runAsync(true);

  // 连接（需命令行或参数文件中已配置激光）并运行激光处理进程
  if (!laserConnector.connectLasers(
        false,  // 连接失败后是否重试
        false,  // 只将连接的激光添加到ArRobot
        true    // 添加所有激光到ArRobot
  ))
  {
    printf("Could not connect to lasers... exiting\n");
    Aria::exit(2);
  }

  // 创建并连接指南针
  ArTCM2 *compass = compassConnector.create(&robot);
  if(compass && !compass->blockingConnect()) {
    compass = NULL;
  }
  
  // 休眠1秒，以获取来自机器人和摄像头的初始化返回信息
  ArUtil::sleep(1000);

  // 当机器人任务循环在运行时，如果想要设置不同模式，需要锁住机器人
  robot.lock();

  // 设置机器人不同模式
  ArModeGripper gripper(&robot, "gripper", 'g', 'G');
  ArModeActs actsMode(&robot, "acts", 'a', 'A');
  ArModeTCM2 tcm2(&robot, "tcm2", 'm', 'M', compass);
  ArModeIO io(&robot, "io", 'i', 'I');
  ArModeConfig cfg(&robot, "report robot config", 'o' , 'O');
  ArModeCommand command(&robot, "command", 'd', 'D');
  ArModeCamera camera(&robot, "camera", 'c', 'C');
  ArModePosition position(&robot, "position", 'p', 'P', &gyro);
  ArModeSonar sonar(&robot, "sonar", 's', 'S');
  ArModeBumps bumps(&robot, "bumps", 'b', 'B');
  ArModeLaser laser(&robot, "laser", 'l', 'L');
  ArModeWander wander(&robot, "wander", 'w', 'W');
  ArModeUnguardedTeleop unguardedTeleop(&robot, "unguarded teleop", 'u', 'U');
  ArModeTeleop teleop(&robot, "teleop", 't', 'T');


  // 激活默认模式
  teleop.activate();

  // 启动机器人
  robot.comInt(ArCommands::ENABLE, 1);

  robot.unlock();
  
  // 在这里阻止执行主线程，并等待机器人的任务循环线程退出（例如通过机器人断开，退出键按下或操作系统信号）
  robot.waitForRunExit();

  Aria::exit(0);
  return 0;

}