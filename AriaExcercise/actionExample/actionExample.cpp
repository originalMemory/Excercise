#include "Aria.h"
/* 本实例演示了动作的创建和使用 */

/* 动作：向前直行直至声纳探测到了障碍物为止 */
class ActionGo : public ArAction
{
public:
	// 构造函数，设置 myMaxSpeed 和 myStopDistance
	ActionGo(double maxSpeed, double stopDistance);
	// 析构函数，无操作
	virtual ~ActionGo(void) {};
	// 动作解析器调用该函数来获取该动作要求的行为
	virtual ArActionDesired *fire(ArActionDesired currentDesired);
	// 存储机器人指针和它的ArSonarDevice对象，如果没有声纳，停止动作
	virtual void setRobot(ArRobot *robot);
protected:
	// 使用setRobot()从机器人中获取到的声纳对象
	ArRangeDevice *mySonar;


	/* 动作期望对象，fire()函数修改并将该对象的指针传递给动作解析器
	这个对象应是类中成员，这样即使fire()结束后它也存在
	否则fire()每次调用时都会创建一个该对象，但始终不会删除创建的对象
	*/
	ArActionDesired myDesired;

	double myMaxSpeed;			//最大速度
	double myStopDistance;		//停止距离
};


/* 动作：声纳探测到障碍物时转向 */
class ActionTurn : public ArAction
{
public:
	// 构造函数，设置turnThreshold, turnAmount
	ActionTurn(double turnThreshold, double turnAmount);
	// 析构函数，无操作
	virtual ~ActionTurn(void) {};
	// 动作解析器调用该函数来获取该动作要求的行为
	virtual ArActionDesired *fire(ArActionDesired currentDesired);
	// 存储机器人指针和它的ArSonarDevice对象，如果没有声纳，停止动作
	virtual void setRobot(ArRobot *robot);
protected:
	// 使用setRobot()从机器人中获取到的声纳对象
	ArRangeDevice *mySonar;
	// 动作期望，动作解析器调用fire()后使用
	ArActionDesired myDesired;
	// 距离多远开始转向
	double myTurnThreshold;
	// 转角大小
	double myTurnAmount;
	// 转动方向
	int myTurning; // -1 == left, 1 == right, 0 == none
};

/*
构造函数
*/
ActionGo::ActionGo(double maxSpeed, double stopDistance) :
ArAction("Go")		//"Go"为对该对象的命名
{
	mySonar = NULL;
	myMaxSpeed = maxSpeed;
	myStopDistance = stopDistance;
	setNextArgument(ArArg("maximum speed", &myMaxSpeed, "Maximum speed to go."));		//设置参数类型，依次为参数名、参数指针、参数描述。便于程序中的其他部分找到该动作的参数，并修改
	setNextArgument(ArArg("stop distance", &myStopDistance, "Distance at which to stop."));
}

/*
重写ArAction::setRobot()来从机器人中获取声纳装置或当没有声纳时停止动作
必须调用ArAction::setRobot()才能正确存储ArAction基类中的ArRobot指针。
*/
void ActionGo::setRobot(ArRobot *robot)
{
	ArAction::setRobot(robot);		//将动作与参数机器人绑定
	mySonar = robot->findRangeDevice("sonar");	//查找名为“sonar”的范围装置
	if (robot == NULL)
	{
		/*输出日志信息
		第一个参数是日志等级，有简洁、正常、详细三种
		第二个参数是日志描述内容，该参数可以有多个*/
		ArLog::log(ArLog::Terse, "actionExample: ActionGo: Warning: I found no sonar, deactivating.");
		deactivate();		//停用动作
	}
}

/*
fire是动作的核心点
currentDesired是动作解析器已经调用了的动作组合的期望，本例中无需注意此处。
在myDesired对象中设置向前速度并返回该对象
myDesired必须为类中成员，因为函数返回的是指向myDesired的指针
*/
ArActionDesired *ActionGo::fire(ArActionDesired currentDesired)
{
	double range;
	double speed;

	// 重置myDesired以清除之前的设置
	myDesired.reset();
	// 如果没有声纳，停止动作
	if (mySonar == NULL)
	{
		deactivate();
		return NULL;
	}
	// 获取声纳的范围
	/*使用currentReadingPolar获取角度范围内声纳读数
	myRobot是Araction类中成员变量，在调用过setRobot()后代表当前操作的机器人。
	getRobotRadius()获取机器人半径*/
	range = mySonar->currentReadingPolar(-70, 70) - myRobot->getRobotRadius();	
	// 如果范围内比停止距离大，继续前行
	if (range > myStopDistance)
	{
		// 设置一个基于范围的任意速度
		speed = range * .3;
		// 确认速度不超过预设速度
		if (speed > myMaxSpeed)
			speed = myMaxSpeed;
		// 设置期望速度
		myDesired.setVel(speed);
	}
	// 如果范围内比停止距离小，停止前行
	else
	{
		myDesired.setVel(0);
	}
	// 返回指向myDesired的指针
	return &myDesired;
}


/* 构造函数 */
ActionTurn::ActionTurn(double turnThreshold, double turnAmount) :
ArAction("Turn")
{
	myTurnThreshold = turnThreshold;
	myTurnAmount = turnAmount;
	setNextArgument(ArArg("turn threshold (mm)", &myTurnThreshold, "The number of mm away from obstacle to begin turnning."));
	setNextArgument(ArArg("turn amount (deg)", &myTurnAmount, "The number of degress to turn if turning."));	//设置参数类型，依次为参数名、参数指针、参数描述。便于程序中的其他部分找到该动作的参数，并修改
	myTurning = 0;
}

/*
重写ArAction::setRobot()来从机器人中获取声纳装置或当没有声纳时停止动作
必须调用ArAction::setRobot()才能正确存储ArAction基类中的ArRobot指针。
*/
void ActionTurn::setRobot(ArRobot *robot)
{
	ArAction::setRobot(robot);
	mySonar = robot->findRangeDevice("sonar");
	if (mySonar == NULL)
	{
		/*输出日志信息
		第一个参数是日志等级，有简洁、正常、详细三种
		第二个参数是日志描述内容，该参数可以有多个*/
		ArLog::log(ArLog::Terse, "actionExample: ActionTurn: Warning: I found no sonar, deactivating.");
		deactivate(); 
	}
}

/*
fire是动作的核心点
currentDesired是动作解析器已经调用了的动作组合的期望，本例中无需注意此处。
在myDesired对象中设置向前速度并返回该对象
myDesired必须为类中成员，因为函数返回的是指向myDesired的指针
*/
ArActionDesired *ActionTurn::fire(ArActionDesired currentDesired)
{
	double leftRange, rightRange;
	// 重置myDesired以清除之前的设置
	myDesired.reset();
	// 如果没有声纳，停止动作
	if (mySonar == NULL)
	{
		deactivate();
		return NULL;
	}
	// 获取声纳左右两侧探测范围
	leftRange = (mySonar->currentReadingPolar(0, 100) - 
		myRobot->getRobotRadius());
	rightRange = (mySonar->currentReadingPolar(-100, 0) - 
		myRobot->getRobotRadius());
	// 如果左右范围均小于阈值，重新转向变量并停止转向
	if (leftRange > myTurnThreshold && rightRange > myTurnThreshold)
	{
		myTurning = 0;
		myDesired.setDeltaHeading(0);		//设置转动航向为0
	}
	// 如果已经在转向中，继续往该方向转向
	else if (myTurning)
	{
		myDesired.setDeltaHeading(myTurnAmount * myTurning);
	}
	// 如果还未转向，但需要转向且左侧更近，则转向右侧
	// 修改转向变量myTurning以保证持续往该方向转向
	else if (leftRange < rightRange)
	{
		myTurning = -1;
		myDesired.setDeltaHeading(myTurnAmount * myTurning);
	}
	// 如果还未转向，但需要转向且右侧更近，则转向右侧
	// 修改转向变量myTurning以保证持续往该方向转向
	else 
	{
		myTurning = 1;
		myDesired.setDeltaHeading(myTurnAmount * myTurning);
	}
	// 返回指向myDesired的指针
	return &myDesired;
}



int main(int argc, char** argv)
{
	Aria::init();		//全局初始化

	ArSimpleConnector conn(&argc, argv);		//传统机器人和激光连接器，不推荐使用
	ArRobot robot;
	ArSonarDevice sonar;				//来自机器人的最近声纳读数

	ActionGo go(500, 350);
	ActionTurn turn(400, 10);
	ArActionStallRecover recover;		//失速恢复动作


	// 解析所有命令行参数
	if(!Aria::parseArgs())
	{
		//如果解析程序参数出错，输出日志，记录当前程序所有参数
		Aria::logOptions();
		return 1;
	}

	// 连接机器人
	if(!conn.connectRobot(&robot))
	{
		ArLog::log(ArLog::Terse, "actionExample: Could not connect to robot! Exiting.");
		return 2;
	}

	// 为机器人添加范围装置，这一步必须在添加动作之前
	robot.addRangeDevice(&sonar);


	// 依次添加动作
	robot.addAction(&recover, 100);	//给机器人添加动作，前者为动作类，后者为动作优先级
	robot.addAction(&go, 50);
	robot.addAction(&turn, 49);

	//启动发动机
	robot.enableMotors();

	// 开始机器人进程循环
	// 'true' 意为当断开连接时停止运行
	robot.run(true);

	Aria::exit(0);
}