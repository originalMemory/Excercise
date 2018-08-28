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

#ifndef BASEACTION
#define BASEACTION
/*
基础动作类
*/
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
	~BaseAction(void);
	/*
	描述：初始化机器人指针
	参数：
	robot：机器人指针
	返回值：无
	*/
	void SetRobot(ArRobot *robot);
	/*
	描述：获取机器人指针
	参数：
	返回值：机器人指针
	*/
	ArRobot* GetRobot();
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
	/*
	描述：设置绝对航向
	参数：
		heading：旋转角度，单位为deg。正数为逆时针旋转
	返回值：无
	*/
	void SetHeading(double heading);
	/*
	描述：设置旋转速度
	参数：
		velocity：旋转速度，单位为deg/sec。正数为逆时针旋转
	返回值：无
	*/
	void SetRotVel(double velocity);
	/*
	描述：设置相对航向
	参数：
	heading：旋转角度，单位为deg。正数为逆时针旋转
	返回值：无
	*/
	void SetDeltaHeading(double deltaHeading);
	/*
	描述：判断是否已旋转至预定航向
	参数：无
	返回值：true表示已经结束，false表示尚未结束
	*/
	bool IsHeadingDone();
	/*
	描述：设置横向速度
	参数：
		velocity：速度，单位为deg/sec。
	返回值：无
	*/
	void SetLatVel(double latVelocity);
	/*
	描述：休眠一段时间
	参数：
		ms：时间，单位为毫秒
	返回值：无
	*/
	void Sleep(unsigned int ms);
	/*
	描述：设置水平加速度
	参数：
		acc：加速度，单位为mm/s2
	返回值：无
	*/
	void SetTransAccel(double acc);
	/*
	描述：设置水平减速度
	参数：
		decel：减速度，单位为mm/s2
	返回值：无
	*/
	void SetTransDecel(double decel);
	/*
	描述：设置旋转加速度
	参数：
		acc：加速度，单位为mm/s2
	返回值：无
	*/
	void SetRotAccel(double acc);
	/*
	描述：设置旋转减速度
	参数：
		decel：减速度，单位为mm/s2
	返回值：无
	*/
	void SetRotDecel(double decel);
	private:
		ArRobot *myRobot;

public:
	/*
	描述：获取当前速度(mm/s2)
	参数：无
	返回值：当前速度(mm/s2)
	*/
	double GetVel();
	ArPose GetPose();
};
#endif
