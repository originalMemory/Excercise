/* 连接机器人和GPS，允许远程操作，并打印机器人位置和GPS数据。 */
#include "Aria.h"
#include "ArGPS.h"
#include "ArGPSConnector.h"

#include <assert.h>


/*  
 *  本类封装了一个ArRobot传感器解释任务，用以用标准输出格式输出带本地时间戳和当前机器人姿态的最新的GPS数据
 *  本类也包含一个互斥锁，用以在运行锁定GPS（当访问ArGPS对象时）
 */
class GPSLogTask {

public:
  GPSLogTask(ArRobot *robot, ArGPS *gps, ArJoyHandler *joy = NULL) :
      myRobot(robot), 
      myGPS(gps),
      myTaskFunctor(this, &GPSLogTask::doTask),
      myJoyHandler(joy),
      myButtonDown(false)
  {
	  //在同步任务的传感器插补部分下添加任务
    myRobot->addSensorInterpTask("GPS", ArListPos::LAST, &myTaskFunctor); 
	//这一步暂不清楚是起什么作用
    puts("RobotX\tRobotY\tRobotTh\tRobotVel\tRobotRotVel\tRobotLatVel\tLatitude\tLongitude\tAltitude\tSpeed\tGPSTimeSec\tGPSTimeMSec\tFixType\tNumSats\tPDOP\tHDOP\tVDOP\tGPSDataReceived");
  }

  void lock() { myMutex.lock(); }
  void unlock() { myMutex.unlock(); }

protected:

  void doTask()
  {
    // 如果摇杆按钮被按下，输出一个标记（除了1，它被用于驱动）
    if(myJoyHandler)
    {
      for(unsigned int b = 2; b <= myJoyHandler->getNumButtons(); ++b)
        if(myJoyHandler->getButton(b)) {
          if(!myButtonDown)
            printf("--------------- Joystick button %d pressed.\n", b);
          myButtonDown = true;
        }
        else
          myButtonDown = false;
    }

    lock();
	//从机器人读取数据，限制时间为50ms。为0时读取所有数据再返回
    int f = myGPS->read(50);
    printf("%.4f\t%.4f\t%.4f\t%.4f\t%.4f\t%.4f"
           "\t%2.8f\t%2.8f\t%4.4f\t%4.4f"
           "\t%lu\t%lu\t%s"
           "\t%u\t%2.4f\t%2.4f\t%2.4f"
           "\t%s\n",
      myRobot->getX(), myRobot->getY(), myRobot->getTh(), myRobot->getVel(), myRobot->getRotVel(), (myRobot->hasLatVel())?(myRobot->getLatVel()):0,
      myGPS->getLatitude(), myGPS->getLongitude(), myGPS->getAltitude(), myGPS->getSpeed(),
      myGPS->getGPSPositionTimestamp().getSec(), myGPS->getGPSPositionTimestamp().getMSec(), myGPS->getFixTypeName(),
      myGPS->getNumSatellitesTracked(), myGPS->getPDOP(), myGPS->getHDOP(), myGPS->getVDOP(),
      ((f&ArGPS::ReadUpdated)?"yes":"no")
    );
    unlock();
  }

private:
  ArRobot *myRobot;
  ArGPS *myGPS;
  ArFunctorC<GPSLogTask> myTaskFunctor;
  ArMutex myMutex;
  ArJoyHandler *myJoyHandler;
  bool myButtonDown;
};



int main(int argc, char** argv)
{
  Aria::init();
  ArLog::init(ArLog::StdErr, ArLog::Normal);

  ArArgumentParser argParser(&argc, argv);
  ArSimpleConnector connector(&argParser);
  ArGPSConnector gpsConnector(&argParser);
  ArRobot robot;

  ArActionLimiterForwards nearLimitAction("limit near", 300, 600, 250);
  ArActionLimiterForwards farLimitAction("limit far", 300, 1100, 400);
  ArActionLimiterBackwards limitBackwardsAction;
  ArActionJoydrive joydriveAction;
  ArActionKeydrive keydriveAction;

  ArSonarDevice sonar;
  ArSick laser;

  argParser.loadDefaultArguments();
  if(!Aria::parseArgs() || !argParser.checkHelpAndWarnUnparsed())
  {
    Aria::logOptions();
    return -1;
  }

  robot.addRangeDevice(&sonar);
  robot.addRangeDevice(&laser);

  ArLog::log(ArLog::Normal, "gpsRobotTaskExample: Connecting to robot...");
  if(!connector.connectRobot(&robot))
  {
    ArLog::log(ArLog::Terse, "gpsRobotTaskExample: Could not connect to the robot. Exiting.");
    return -2;
  }
  ArLog::log(ArLog::Normal, "gpsRobotTaskExample: Connected to the robot.");


  // 连接机器人
  ArLog::log(ArLog::Normal, "gpsRobotTaskExample: Connecting to GPS, it may take a few seconds...");
  ArGPS *gps = gpsConnector.createGPS(&robot);
  assert(gps);		//此处作用不明
  if(!gps || !(gps->connect()))
  {
    ArLog::log(ArLog::Terse, "gpsRobotTaskExample: Error connecting to GPS device.  Try -gpsType, -gpsPort, and/or -gpsBaud command-line arguments. Use -help for help. Exiting.");
    return -3;
  }


  // 创建一个GPSLogTask类
  GPSLogTask gpsTask(&robot, gps, joydriveAction.getJoyHandler()->haveJoystick() ? joydriveAction.getJoyHandler() : NULL);


  // 添加动作
  robot.addAction(&nearLimitAction, 100);
  robot.addAction(&farLimitAction, 90);
  robot.addAction(&limitBackwardsAction, 80);
  robot.addAction(&joydriveAction, 50);
  robot.addAction(&keydriveAction, 40);

  // 如果摇杆未被按下时，允许键盘操作
  joydriveAction.setStopIfNoButtonPressed(false);

  // 启动机器人
  robot.runAsync(true);

  // 连接激光
//  connector.setupLaser(&laser);
  //laser.runAsync();
 // if(!laser.blockingConnect())
  //  ArLog::log(ArLog::Normal, "gpsRobotTaskExample: Warning, could not connect to SICK laser, will not use it.");

  robot.lock();

  robot.enableMotors();
  robot.comInt(47, 1);  // 启动摇杆驱动

  // 添加退出回调以重置/解除seekur上的方向盘（如果机器人没有滑动，则为关键）; 对其他机器人没有任何作用
  Aria::addExitCallback(new ArRetFunctor1C<bool, ArRobot, unsigned char>(&robot, &ArRobot::com, (unsigned char)120));
  Aria::addExitCallback(new ArRetFunctor1C<bool, ArRobot, unsigned char>(&robot, &ArRobot::com, (unsigned char)120));

  robot.unlock();

  ArLog::log(ArLog::Normal, "gpsRobotTaskExample: Running... (drive robot with joystick or arrow keys)");
  robot.waitForRunExit();


  return 0;
}