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
	//构造函数
	BaseAction(ArRobot *robot);
	//析构函数
	~BaseAction();
	void Move(double distacne);
	void SetVelocity(double velocity);
	void SetVelocity(double leftVelocity,double rightVelocity);
	void Stop();
	bool IsMoveDone();
	void 
	private:
		ArRobot *myRobot;

};