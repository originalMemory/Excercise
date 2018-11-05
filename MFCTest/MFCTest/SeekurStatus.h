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

#ifndef SEEKURSTATUS
#define SEEKURSTATUS
class SeekurStatus
{
public:
	void SetValue(double x, double y, double heading, double vel, double rotVel, double leftVel, double rightVel);
	double GetX();
	double GetY();
	double GetHeading();
	double GetVel();
	double GetRotVel();
	double GetLeftVel();
	double GetRightVel();
	void Lock();
	void UnLock();
	SeekurStatus(ArRobot *robot);
	~SeekurStatus();

private:
	double X;
	double Y;
	double Heading;		//航向
	double Vel;
	double LeftVel;
	double RightVel;
	double RotVel;

	bool IsLock;	//是否上锁
	ArRobot *Robot;		//机器人指针
};
#endif

