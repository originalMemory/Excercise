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

#include "SeekurStatus.h"
using namespace std;

SeekurStatus::SeekurStatus(ArRobot *robot)
{
	Robot = robot;
}

SeekurStatus::~SeekurStatus()
{
}

void SeekurStatus::SetValue(double x, double y, double heading, double vel, double rotVel, double leftVel, double rightVel){
	X = x;
	Y = y;
	Heading = heading;
	Vel = vel;
	RotVel = rotVel;
	LeftVel = leftVel;
	RightVel = rightVel;
}
double SeekurStatus::GetX(){
	return X;
}
double SeekurStatus::GetY(){
	return Y;
}
double SeekurStatus::GetHeading(){
	return Heading;
}
double SeekurStatus::GetVel(){
	return Vel;
}
double SeekurStatus::GetRotVel(){
	return RotVel;
}
double SeekurStatus::GetLeftVel(){
	return LeftVel;
}
double SeekurStatus::GetRightVel(){
	return RightVel;
}
void SeekurStatus::Lock(){
	while (IsLock)
	{
		Sleep(20);
	}
	IsLock = true;
}
void SeekurStatus::UnLock(){
	IsLock = false;
}