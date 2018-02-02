/******************************************************************* 
*  文件名称:基础动作类实现文件
*  简要描述: Seekur机器人相关基础动作的类的实现文件
*   
*  创建日期: 2017/09/6
*  作者: 吴厚波
*  说明: 无
*   
*  修改日期: 
*  作者: 
*  说明: 
******************************************************************/ 
#include "stdafx.h"

#include "BaseAction.h"
#include <iostream>
using namespace std;

#pragma region 基础动作类
BaseAction::BaseAction(){
}
BaseAction::BaseAction(ArRobot *robot){
	myRobot = robot;
}
/*
	描述：析构函数
	参数：无
	返回值：无
	*/
BaseAction::~BaseAction(void){

}


/*
描述：初始化机器人指针
参数：
robot：机器人指针
返回值：无
*/
void BaseAction::SetRobot(ArRobot *robot){
	myRobot = robot;
}


/*
描述：获取机器人指针
参数：
返回值：机器人指针
*/
ArRobot* BaseAction::GetRobot(){
	return myRobot;
}

/*
描述：水平移动某一距离
参数：
distacne：移动距离，单位为mm。正数向前运动，负数向后运动
返回值：无
*/
void BaseAction::Move(double distacne){
	myRobot->lock();
	printf("移动 %.2f mm\n",distacne);
	myRobot->move(distacne);
	myRobot->unlock();
}

/*
描述：设置速度
参数：
velocity：速度，单位为mm/s
返回值：无
*/
void BaseAction::SetVelocity(double velocity){
	myRobot->lock();
	printf("速度设置为 %.2f mm/s\n",velocity);
	myRobot->setVel(velocity);
	myRobot->unlock();
}
/*
描述：设置左右速度
参数：
leftVelocity：左速度，单位为mm/s
rightVelocity：右速度，单位为mm/s
返回值：无
*/
void BaseAction::SetVelocity(double leftVelocity,double rightVelocity){
	myRobot->lock();
	printf("左速度设置为 %.2f mm/s\t右速度设置为 %.2f mm/s",leftVelocity,rightVelocity);
	myRobot->setVel2(leftVelocity,rightVelocity);
	myRobot->unlock();
}
/*
描述：令机器人停止运动
参数：无
返回值：无
*/
void BaseAction::Stop(){
	myRobot->lock();
	printf("机器人停止运动\n");
	myRobot->stop();
	myRobot->unlock();
}
/*
描述：距离运动是否已经结束
参数：无
返回值：true表示已经结束，false表示尚未结束
*/
bool BaseAction::IsMoveDone(){
	bool status;
	myRobot->lock();
	if(myRobot->isMoveDone()){
		status= true;
	}
	else{
		status= false;
	}
	myRobot->unlock();
	return status;
}
/*
描述：设置航向
参数：
heading：旋转角度，单位为deg。正数为逆时针旋转
返回值：无
*/
void BaseAction::SetHeading(double heading){
	myRobot->lock();
	printf("航向设置为 %.2f deg\n",heading);
	myRobot->setHeading(heading);
	myRobot->unlock();
}
/*
描述：设置旋转速度
参数：
velocity：旋转速度，单位为deg/sec。正数向前运动，负数向后运动
返回值：无
*/
void BaseAction::SetRotVel(double velocity){
	myRobot->lock();
	printf("旋转速度设置为 %.2f deg/s\n",velocity);
	myRobot->setRotVel(velocity);
	myRobot->unlock();
}
/*
描述：设置相对航向
参数：
heading：旋转角度，单位为deg。正数为逆时针旋转
返回值：无
*/
void BaseAction::SetDeltaHeading(double deltaHeading){
	myRobot->lock();
	printf("Delta航向设置为 %.2f deg\n",deltaHeading);
	myRobot->setDeltaHeading(deltaHeading);
	myRobot->unlock();
}
/*
描述：判断是否已旋转至预定航向
参数：无
返回值：true表示已经结束，false表示尚未结束
*/
bool BaseAction::IsHeadingDone(){
	bool status;
	myRobot->lock();
	if(myRobot->isHeadingDone()){
		status= true;
	}
	else{
		status= false;
	}
	myRobot->unlock();
	return status;
}
/*
描述：设置横向速度
参数：
velocity：速度，单位为deg/sec。
返回值：无
*/
void BaseAction::SetLatVel(double latVelocity){
	myRobot->lock();
	printf("横向速度设置为 %.2f mm/s\n",latVelocity);
	myRobot->setLatVel(latVelocity);
	myRobot->unlock();
}
/*
描述：休眠一段时间
参数：
ms：时间，单位为毫秒
返回值：无
*/
void BaseAction::Sleep(unsigned int ms){
	myRobot->lock();
	printf("机器睡眠 %.2f mm\n",ms);
	ArUtil::sleep(ms);
	myRobot->unlock();
}
/*
描述：设置水平加速度
参数：
acc：加速度，单位为mm/s2
返回值：无
*/
void BaseAction::SetTransAccel(double acc){
	myRobot->lock();
	printf("水平加速度设置为 %.2f mm/s\n",acc);
	myRobot->setTransAccel(acc);
	myRobot->unlock();
}
/*
描述：设置水平减速度
参数：
decel：减速度，单位为mm/s2
返回值：无
*/
void BaseAction::SetTransDecel(double decel){
	myRobot->lock();
	printf("水平加速度设置为 %.2f mm/s\n",decel);
	myRobot->setTransDecel(decel);
	myRobot->unlock();
}
/*
描述：设置旋转加速度
参数：
acc：加速度，单位为mm/s2
返回值：无
*/
void BaseAction::SetRotAccel(double acc){
	myRobot->lock();
	printf("旋转加速度设置为 %.2f mm/s\n",acc);
	myRobot->setRotAccel(acc);
	myRobot->unlock();
}
/*
描述：设置旋转减速度
参数：
decel：减速度，单位为mm/s2
返回值：无
*/
void BaseAction::SetRotDecel(double decel){
	myRobot->lock();
	printf("旋转加速度设置为 %.2f mm/s\n",decel);
	myRobot->setRotDecel(decel);
	myRobot->unlock();
}
#pragma endregion



