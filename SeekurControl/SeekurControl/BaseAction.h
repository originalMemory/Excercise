/******************************************************************* 
 *  文件名称:基础动作类头文件
 *  简要描述: Seekur机器人相关基础动作的类的头文件
 *   
 *  创建日期: 2017/09/6
 *  作者: 吴厚波
 *  说明: 无
 *   
 *  修改日期: 
 *  作者: 
 *  说明: 
 ******************************************************************/ 

#include "Aria.h"

/*
这是一个连接处理程序类，用于演示如何在其中运行代码以
   响应诸如连接或与机器人断开连接的程序等事件。
   */
class ConnHandler
{
public:
	//构造函数
	ConnHandler(ArRobot *robot);
	//析构函数
	~ConnHandler(void) {}
	// 连接建立时调用
	void connected(void);
	// 连接失败时调用
	void connFail(void);
	// 连接断开时调用
	void disconnected(void);
protected:
	// 机器人指针
	ArRobot *myRobot;
	// 回调函数
	ArFunctorC<ConnHandler> myConnectedCB;		//成功
	ArFunctorC<ConnHandler> myConnFailCB;		//失败
	ArFunctorC<ConnHandler> myDisconnectedCB;	//断开
};


class BaseAction{
public:
	/*
	描述：构造函数
	参数：
		robot：机器人指针
	返回值：无
	*/
	BaseAction(ArRobot *robot);
	/*
	描述：析构函数
	参数：无
	返回值：无
	*/
	~BaseAction();
	/*
	描述：水平移动某一距离
	参数：
		distacne：移动距离，单位为mm。正数向前运动，负数向后运动
	返回值：无
	*/
	void Move(double distacne);
	/*
	描述：设置速度
	参数：
		velocity：速度，单位为mm/s
	返回值：无
	*/
	void SetVelocity(double velocity);
	/*
	描述：设置左右速度
	参数：
		leftVelocity：左速度，单位为mm/s
		rightVelocity：右速度，单位为mm/s
	返回值：无
	*/
	void SetVelocity(double leftVelocity,double rightVelocity);
	/*
	描述：令机器人停止运动
	参数：无
	返回值：无
	*/
	void Stop();
	/*
	描述：距离运动是否已经结束
	参数：无
	返回值：true表示已经结束，false表示尚未结束
	*/
	bool IsMoveDone();
	// 设置航向
	void SetHeading(double heading);
	/*
	描述：设置旋转速度
	参数：
		velocity：旋转速度，单位为deg/sec。正数向前运动，负数向后运动
	返回值：无
	*/
	void SetRotVel(double velocity);
	// 设置delta航向
	void SetDeltaHeading(double deltaHeading);
	/*
	描述：判断是否已旋转至预定航向
	参数：无
	返回值：true表示已经结束，false表示尚未结束
	*/
	bool IsHeadingDone();
	// 设置横向速度
	void SetLatVel(double latVelocity);
	// 休眠一段时间
	// @ms 毫秒
	void Sleep(unsigned int ms);
	private:
		ArRobot *myRobot;

};