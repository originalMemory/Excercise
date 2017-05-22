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
