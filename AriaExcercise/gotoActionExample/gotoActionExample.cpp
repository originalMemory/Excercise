/* 使用ArActionGoto类，通过设置一个2.5*2.5m的正方形的顶点为一个ArActionGoto动作的目标来驱动机器人在方形区域内运动
   它也可以限制速度来避免碰撞，一段时间后，它会取消目标（并且机器人由于停止动作而停止）并退出。 */
#include "Aria.h"

int main(int argc, char **argv)
{
  Aria::init();
  ArArgumentParser parser(&argc, argv);
  parser.loadDefaultArguments();
  ArSimpleConnector simpleConnector(&parser);
  ArRobot robot;
  ArSonarDevice sonar;
  ArAnalogGyro gyro(&robot);	//板载陀螺仪，用于改善机器人姿态
  robot.addRangeDevice(&sonar);

  // 创建按钮句柄，使用ESC可以迅速关闭程序
  ArKeyHandler keyHandler;
  Aria::setKeyHandler(&keyHandler);
  robot.attachKeyHandler(&keyHandler);
  printf("You may press escape to exit\n");

  // 防碰撞
  ArActionLimiterForwards limiterAction("speed limiter near", 300, 600, 250);
  ArActionLimiterForwards limiterFarAction("speed limiter far", 300, 1100, 400);
  ArActionLimiterTableSensor tableLimiterAction;
  robot.addAction(&tableLimiterAction, 100);
  robot.addAction(&limiterAction, 95);
  robot.addAction(&limiterFarAction, 90);

  // Goto动作设为低优先级
  ArActionGoto gotoPoseAction("goto");
  robot.addAction(&gotoPoseAction, 50);
  
  // 停止动作设为低优先级，当没有目标时登上机器人
  ArActionStop stopAction("stop");
  robot.addAction(&stopAction, 40);

  // 解析所有命令行参数
  if (!Aria::parseArgs() || !parser.checkHelpAndWarnUnparsed())
  {    
    Aria::logOptions();
    Aria::exit(1);
    return 1;
  }
  
  // 连接机器人
  if (!simpleConnector.connectRobot(&robot))
  {
    printf("Could not connect to robot... exiting\n");
    Aria::exit(1);
    return 1;
  }
  robot.runAsync(true);

  // 启动机器人发动机，关闭amigobot声音
  robot.enableMotors();
  robot.comInt(ArCommands::SOUNDTOG, 0);

  const int duration = 30000; //单次移动限时
  ArLog::log(ArLog::Normal, "Going to four goals in turn for %d seconds, then cancelling goal and exiting.", duration/1000);

  bool first = true;
  int goalNum = 0;
  ArTime start;
  start.setToNow();
  while (Aria::getRunning()) 
  {
    robot.lock();

    // 如果是第一次循环开始，选择一个新目标，否则先完成早期目标
    if (first || gotoPoseAction.haveAchievedGoal())
    {
      first = false;
      goalNum++;
      if (goalNum > 4)
        goalNum = 1; // 防止目标溢出 #1

      // 为不同目标设置位置
      if (goalNum == 1)
        gotoPoseAction.setGoal(ArPose(2500, 0));
      else if (goalNum == 2)
        gotoPoseAction.setGoal(ArPose(2500, 2500));
      else if (goalNum == 3)
        gotoPoseAction.setGoal(ArPose(0, 2500));
      else if (goalNum == 4)
        gotoPoseAction.setGoal(ArPose(0, 0));

      ArLog::log(ArLog::Normal, "Going to next goal at %.0f %.0f", 
            gotoPoseAction.getGoal().getX(), gotoPoseAction.getGoal().getY());
    }

    if(start.mSecSince() >= duration) {
      ArLog::log(ArLog::Normal, "%d seconds have elapsed. Cancelling current goal, waiting 3 seconds, and exiting.", duration/1000);
      gotoPoseAction.cancelGoal();
      robot.unlock();
      ArUtil::sleep(3000);
      break;
    }

    robot.unlock();
    ArUtil::sleep(10000);
  }
  
  Aria::exit(0);
  return 0;
}