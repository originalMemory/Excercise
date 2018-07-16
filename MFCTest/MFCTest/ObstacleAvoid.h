//#include "stdafx.h"
#include<string>
#include <iostream>
//#include "MFCTestView.h"

#ifndef _OBSTACLEAVOID_H_
#define _OBSTACLEAVOID_H_

class ObstacleAvoid
{
public:
	const static int NANGLE = 7;//角度量化论域模糊子集数
	const static int NDIS = 5;//距离量化论域模糊子集数
private:
	float dis;     //量化后的距离
	float angle;    //量化后的角度
	float dismax;  //距离基本论域上限
	float anglemax; //角度基本论域的上限
	float rotmax;  //输出的上限
	float Kdis;    //Kdis=n/dismax,量化论域为[-4,4],每2个为一次变化
	float Kangle;   //Kangle=n/anglemax,量化论域为[-6,6],每2个为一次变化
	float Krot;    //Krot=rotmax/n,量化论域为[-6,6],每2个为一次变化
	int rule[NDIS][NDIS];//模糊规则表
	float dis_mf_paras[NDIS*3]; //距离的隶属度函数的参数
	float angle_mf_paras[NANGLE * 3];//角度的偏差隶属度函数的参数
	float rot_mf_paras[NANGLE * 3]; //转向角的隶属度函数的参数
	int obstacle[3];	//距机器最近的障碍物信息，内容分别为障碍物左边缘角度（正前方为0，逆时间为正），右边缘角度，距中心点距离。
	//CMFCTestView *m_CView;	//控制对象指针，用以控制机器车运动
public:
	ObstacleAvoid();
	~ObstacleAvoid();
	float trimf(float x, float a, float b, float c);          //三角隶属度函数
	float ComputeAvoidHeading();              //实现模糊控制，计算转向角
	/*
	描述：初始化
	参数：
	@dis_max：最大距离
	@angle_max：最大角度
	@rot_max：最大转向角
	返回值：
	*/
	void Initialize(float dis_max, float angle_max, float rot_max);
	/*
	描述：计算障碍物位置
	参数：
	@laserData：激光数据检测指针
	@len：数据长度
	返回值：
	bool值，是为有障碍物，否为无障碍物
	*/
	bool SetObstaclePosture(int* laserData, int len);
};

#endif