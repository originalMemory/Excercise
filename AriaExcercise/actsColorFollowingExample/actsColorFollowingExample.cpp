#include "Aria.h"



// Chase是将机器人移动到当前视野中最大对象的动作
class Chase : public ArAction
{
  
public:
  
  // chase动作的状态
  enum State {
    NO_TARGET,      // 视野中无目标
    TARGET,         // 视野中有目标
  };

  // 构造函数
  Chase(ArACTS_1_2 *acts, ArVCC4 *camera);
  
  // 析构函数
  ~Chase(void);
  
  // 动作
  ArActionDesired *fire(ArActionDesired currentDesired);

  // 设置想到获取的对象的ACTS通道
  bool setChannel(int channel);

  // 返回动作当前状态
  State getState(void) { return myState; }

  // 抓帧器抓取图像的总宽高
  enum {
    WIDTH = 160,
    HEIGHT = 120
  };

protected:
  ArActionDesired myDesired;
  ArACTS_1_2 *myActs;		//与ACTS通信类
  ArVCC4 *myCamera;			//控制佳能VC-C4和VC-C50i相机的平移，倾斜和缩放机制
  ArTime myLastSeen;		//用于时间读数和测量持续时间的类
  State myState;
  int myChannel;
  int myMaxTime;
};


  // 构造函数
Chase::Chase(ArACTS_1_2 *acts, ArVCC4 *camera) :
    ArAction("Chase", "Chases the largest blob.")
{
  myActs = acts;
  myCamera = camera;
  myChannel = 0;
  myState = NO_TARGET;
  setChannel(1);
  myLastSeen.setToNow();		//重置时间
  myMaxTime = 1000;
}

  // 析构函数
Chase::~Chase(void) {}


// chase动作
ArActionDesired *Chase::fire(ArActionDesired currentDesired)
{
  ArACTSBlob blob;				//ACTS对象类
  ArACTSBlob largestBlob;

  bool flag = false;			//判断是否有最大对象

  int numberOfBlobs;
  int blobArea = 10;			//对比像素数，用于找出最大的对象

  double xRel, yRel;

  // 重置动作期望
  myDesired.reset();
  
  numberOfBlobs = myActs->getNumBlobs(myChannel);		//从给定通道获取对象数量

  // 如果探测到了对象，记录当前时间
  if(numberOfBlobs != 0)
  {
    for(int i = 0; i < numberOfBlobs; i++)
    {
      myActs->getBlob(myChannel, i + 1, &blob);			//获取探测到的对象的信息
	  //判断该对象的像素数是否大于对比数
      if(blob.getArea() > blobArea)
      {
		  //更新对比数
		  flag = true;
		  blobArea = blob.getArea();
		  largestBlob = blob;
      }
    }
	//更新时间
    myLastSeen.setToNow();
  }

  // 判断是否在限定时间内发现了对象
  if (myLastSeen.mSecSince() > myMaxTime)
  {
    if(myState != NO_TARGET) ArLog::log(ArLog::Normal, "Target Lost");
    myState = NO_TARGET;
  }
  else
  {
    // 如果是第一次发现对象，输出日志
    if(myState != TARGET) {
      ArLog::log(ArLog::Normal, "Target Aquired");
      ArLog::log(ArLog::Normal, "(Using channel %d with %d blobs)", myChannel, numberOfBlobs);
    }
	//设置状态，表示已经找到最大对象
    myState = TARGET;
  }

  if(myState == TARGET && flag == true)
  { 
    // 确定最大斑点重心相对于相机中心的位置
    xRel = (double)(largestBlob.getXCG() - WIDTH/2.0)  / (double)WIDTH;
    yRel = (double)(largestBlob.getYCG() - HEIGHT/2.0) / (double)HEIGHT;
      
    // 调整摄像头指向对象
    if(!(ArMath::fabs(yRel) < .20))
    {
      if (-yRel > 0)
        myCamera->tiltRel(1);
      else
        myCamera->tiltRel(-1);
    }

    // 设置机器人的航向及速度
    if (ArMath::fabs(xRel) < .10)
    {
      myDesired.setDeltaHeading(0);
    }
    else
    {
      if (ArMath::fabs(-xRel * 10) <= 10)
        myDesired.setDeltaHeading(-xRel * 10);
      else if (-xRel > 0)
        myDesired.setDeltaHeading(10);
      else
        myDesired.setDeltaHeading(-10);
     
    }

    myDesired.setVel(200);
    return &myDesired;    
  }

  // 如果没有目标则不进行动作
  return &myDesired;
}

// 设置对象信息来源的通道
bool Chase::setChannel(int channel)
{
  if (channel >= 1 && channel <= ArACTS_1_2::NUM_CHANNELS)
  {
    myChannel = channel;
    return true;
  }
  else
    return false;
}




// 回掉启用/禁用键盘驱动动作
void toggleAction(ArAction* action)
{
  if(action->isActive()) {
    action->deactivate();
    ArLog::log(ArLog::Normal, "%s action is now deactivated.", action->getName());
  }
  else {
    action->activate();
    ArLog::log(ArLog::Normal, "%s action is now activated.", action->getName());
  }
}



int main(int argc, char** argv)
{
  Aria::init();

  ArRobot robot;

  // 接收键盘操作的类
  ArKeyHandler keyHandler;
  Aria::setKeyHandler(&keyHandler);		//注册

  ArSonarDevice sonar;

  // 相机 (Cannon VC-C4)
  ArVCC4 vcc4 (&robot);

  // ACTS, 用于跟踪斑点的颜色
  ArACTS_1_2 acts;

  // 命令行参数
  ArArgumentParser argParser(&argc, argv);
  argParser.loadDefaultArguments();
  
  // 传统机器人与激光连接类
  ArSimpleConnector simpleConnector(&argParser);

  // 解析参数             
  if (!Aria::parseArgs())
  {    
    Aria::logOptions();
    keyHandler.restore();
    Aria::exit(1);
    return 1;
  }

  // 机器人限制动作（根据声纳探测到的障碍物）
  ArActionLimiterForwards limiter("speed limiter near", 350, 800, 200);
  ArActionLimiterForwards limiterFar("speed limiter far", 400, 1250, 300);
  ArActionLimiterBackwards backwardsLimiter;
  ArActionConstantVelocity stop("stop", 0);
  //ArActionConstantVelocity backup("backup", -200);
  

  // 颜色跟随动作定义
  Chase chase(&acts, &vcc4);

  // 键盘驱动动作
  ArActionKeydrive keydriveAction;

  // 使用a键来切换键盘驱动
  keyHandler.addKeyHandler('a', new ArGlobalFunctor1<ArAction*>(&toggleAction, &keydriveAction));		//按下a时，以keydriveAction为参数启动toggleAction函数

  // 注册键盘
  Aria::setKeyHandler(&keyHandler);

  // 将键盘与机器人绑定
  robot.attachKeyHandler(&keyHandler);

  // 为机器人添加声纳
  robot.addRangeDevice(&sonar);

  // 连接机器人
  if (!simpleConnector.connectRobot(&robot))
  {
    ArLog::log(ArLog::Terse, "Error: Could not connect to robot... exiting\n");
    keyHandler.restore();
    Aria::exit(1);
  }

  // 打开与ACTS的连接
  if(!acts.openPort(&robot)) 
  {
    ArLog::log(ArLog::Terse, "Error: Could not connect to ACTS... exiting.");
    keyHandler.restore();
    Aria::exit(2);
  }

  // 初始化相机
  vcc4.init();

  // 等待1秒
  ArUtil::sleep(1000);

  // 防止机器人速度过快
  robot.setAbsoluteMaxTransVel(400);

  // 启动发动机
  robot.comInt(ArCommands::ENABLE, 1);
  
  // 关闭amigobot声音
  robot.comInt(ArCommands::SOUNDTOG, 0);

  // Wait....
  ArUtil::sleep(200);

  // 以优先度降序添加动作
  robot.addAction(&limiter, 7);
  robot.addAction(&limiterFar, 6);
  robot.addAction(&backwardsLimiter, 5);
  robot.addAction(&keydriveAction, 4);
  robot.addAction(&chase, 3);
  robot.addAction(&stop, 1);

  // 停用键盘驱动
  keydriveAction.deactivate();

  // 启动机器人
  ArLog::log(ArLog::Normal, "Running. Train ACTS to detect a color to drive towards an object, or use 'a' key to switch to keyboard driving mode.");
  robot.run(true);
  
  Aria::exit(0);
  return 0;
}