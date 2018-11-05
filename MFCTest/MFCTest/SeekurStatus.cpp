/******************************************************************* 
*  �ļ�����:����������ʵ���ļ�
*  ��Ҫ����: Seekur��������ػ������������ʵ���ļ�
*   
*  ��������: 2017/09/6
*  ����: ���
*  ˵��: ��
*   
*  �޸�����: 
*  ����: 
*  ˵��: 
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