#include "stdafx.h"
#include "ObstacleAvoid.h"

#define VN -4
#define N -2
#define M 0
#define F 2
#define VF 4
#define LL -6
#define LM -4
#define LS -2
#define ZO 0
#define RS 2
#define RM 4
#define RL 6

#define NL -6
#define NM -4
#define NS -2
#define PS 2
#define PM 4
#define PL 6

ObstacleAvoid::ObstacleAvoid() 
{
	
}

ObstacleAvoid::~ObstacleAvoid()
{
	delete[] dis_mf_paras;
	delete[] angle_mf_paras;
	delete[] rot_mf_paras;
}
//三角隶属度函数
float ObstacleAvoid::trimf(float x, float a, float b, float c)
{
	float u;
	if (x >= a&&x <= b)
		u = (x - a) / (b - a);
	else if (x > b&&x <= c)
		u = (c - x) / (c - b);
	else
		u = 0.0;
	return u;

}
//计算模糊结果
float ObstacleAvoid::ComputeAvoidHeading()
{
	float u_dis[NDIS], u_angle[NANGLE], u_rot[NANGLE];
	int u_dis_index[3], u_angle_index[3];//假设一个输入最多激活3个模糊子集
	float rot;
	int INTER = 3;	//三角函数参数量

	//模糊化，计算隶属度
	int j = 0;
	for (int i = 0; i < NDIS; i++)
	{
		u_dis[i] = trimf(dis, dis_mf_paras[i*INTER], dis_mf_paras[i*INTER + 1], dis_mf_paras[i*INTER + 2]);//e模糊化，计算它的隶属度
		if (u_dis[i] != 0)
			u_dis_index[j++] = i;                                              //存储被激活的模糊子集的下标，可以减小计算量
	}
	for (; j < 3; j++)u_dis_index[j] = 0;

	j = 0;
	for (int i = 0; i < NANGLE; i++)
	{
		u_angle[i] = trimf(angle, angle_mf_paras[i*INTER], angle_mf_paras[i*INTER + 1], angle_mf_paras[i*INTER + 2]);//de模糊化，计算它的隶属度
		if (u_angle[i] != 0)
			u_angle_index[j++] = i;                                                    //存储被激活的模糊子集的下标，可以减小计算量
	}
	for (; j < 3; j++)u_angle_index[j] = 0;

	float den = 0, num = 0;
	for (int m = 0; m < 3; m++)
	for (int n = 0; n < 3; n++)
	{
		num += u_dis[u_dis_index[m]] * u_angle[u_angle_index[n]] * rule[u_dis_index[m]][u_angle_index[n]];
		den += u_dis[u_dis_index[m]] * u_angle[u_angle_index[n]];
	}
	rot = num / den;
	rot = Krot*rot;
	if (rot >= rotmax)   rot = rotmax;
	else if (rot <= -rotmax)  rot = -rotmax;
	return rot;
}

// 初始化
void ObstacleAvoid::Initialize(float dis_max, float angle_max, float rot_max)
{
	dismax = dis_max;
	anglemax = angle_max;
	rotmax = rot_max;
	dis = dis_max;
	angle = angle_max;
	Kdis = (NDIS - 1) / dismax;
	Kangle = (NANGLE - 1) / anglemax;
	Krot = rotmax / (NANGLE - 1);

	int ruleMatrix[NANGLE][NDIS] = {
		{ NS, ZO, ZO, ZO, ZO },
		{ NS, NS, ZO, ZO, ZO },
		{ NM, NM, NS, ZO, ZO },
		{ NL, NL, NM, NS, ZO },
		{ PM, PM, PS, ZO, ZO },
		{ PS, PS, ZO, ZO, ZO },
		{ PS, ZO, ZO, ZO, ZO },
	};//模糊规则表
	float dis_tp_paras[NDIS * 3] = {
		VN, VN, N,
		VN, N, M,
		N, M, F,
		M, F, VF,
		F, VF, VF
	};//距离dis的隶属度函数参数，这里隶属度函数为三角型，所以3个数据为一组
	float angle_tp_paras[NANGLE * 3] = {
		LL, LL, LM,
		LL, LM, LS,
		LM, LS, ZO,
		LS, ZO, RS,
		ZO, RS, RM,
		RS, RM, RL,
		RM, RL, RL
	};//角度angle的隶属度函数参数
	float rot_tp_paras[NANGLE * 3] = {
		NL, NL, NM,
		NL, NM, NS,
		NM, NS, ZO,
		NS, ZO, PS,
		ZO, PS, PM,
		PS, PM, PL,
		PM, PL, PL
	};//输出量转角rot的隶属度函数参数

	//设定模糊隶属度函数
	for (int i = 0; i < NDIS * 3; i++)
		dis_mf_paras[i] = dis_tp_paras[i];
	for (int i = 0; i < NANGLE * 3; i++)
		angle_mf_paras[i] = angle_tp_paras[i];
	for (int i = 0; i < NANGLE * 3; i++)
		rot_mf_paras[i] = rot_tp_paras[i];

	//设定模糊规则
	for (int i = 0; i < NANGLE; i++)
	for (int j = 0; j < NDIS; j++)
		rule[i][j] = ruleMatrix[i][j];
}

/*
描述：计算障碍物位置
参数：
@laserData：激光数据检测指针
@len：数据长度
返回值：
bool值，是为有障碍物，否为无障碍物
*/
bool ObstacleAvoid::SetObstaclePosture(int* laserData, int len)
{
	bool result;	//是否有障碍物
	//遍历扫描数据，筛选出10m内最近的障碍物
	int status = 0;	//当前障碍物状态位，0为无障碍物，1为遍历至障碍物左端，2为遍历至障碍物右端
	obstacle[0] = -91;	//将左边缘角度初始化为-91，用于对比障碍物
	for (int i = 0; i < len; i++){
		if (laserData[i] <= 10 * 100)	//默认以cm为单位，i即为角度，0-180，逆时针叠加
		{
			//如果是新的障碍物，对比是否最接近中央
			if (status == 0)
			{
				if (abs(i - 90)>abs(obstacle[0]))
				{
					//大于时更新记录
					obstacle[0] = i - 90;
					status = 1;
				}
			}
			else
			{
				obstacle[1] = i - 90;
			}
		}
		//当一个障碍物遍历完后，重置状态
		else if (status > 0)
		{
			status = 0;
		}
	}

	if (obstacle[0]==-91)
	{
		result = false;
	}
	else
	{
		//判断障碍物位于左侧还是右侧
		//左侧
		if (obstacle[0] >= 0)
		{
			angle = Kangle*obstacle[0];
			dis = Kdis*laserData[obstacle[0] + 90];
		}
		//右侧
		else if (obstacle[1] <= 0)
		{
			angle = Kangle*obstacle[1];
			dis = Kdis*laserData[obstacle[1] + 90];
		}
		//正前方
		else
		{
			int tp_angle = -obstacle[0] < obstacle[1] ? obstacle[0] : obstacle[1];
			angle = Kangle*tp_angle;
			dis = Kdis*laserData[tp_angle + 90];
		}
		result = true;
	}
	return result;
}
