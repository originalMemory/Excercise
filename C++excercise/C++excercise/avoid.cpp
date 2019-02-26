#include "stdafx.h"
#include "avoid.h"

#include "stdafx.h"

#define VN -4
#define N -2
#define N -2
#define M 0
#define F 2
#define VF 4

#define LL -3
#define LM -2
#define LS -1
#define ZO 0
#define RS 1
#define RM 2
#define RL 3

#define TLL -3
#define TLM -2
#define TLS -1
#define TRS 1
#define TRM 2
#define TRL 3

ObstacleAvoid::ObstacleAvoid()
{

}

ObstacleAvoid::~ObstacleAvoid()
{
	/*delete[] dis_mf_paras;
	delete[] angle_mf_paras;
	delete[] rot_mf_paras;*/
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
float ObstacleAvoid::ComputeAvoidHeading(double ang, double d)
{
	angle = Kangle*ang;
	dis = Kdis*d;
	float u_dis[NDIS] = { 0 }, u_angle[NANGLE] = { 0 }, u_rot[NANGLE] = { 0 };
	int u_dis_index[3] = { 0 }, u_angle_index[3] = { 0 };//假设一个输入最多激活3个模糊子集
	float rot;
	int INTER = 3;	//三角函数参数量

	//模糊化，计算隶属度
	int j = 0;
	for (int i = 0; i < NDIS; i++)
	{
		int a = dis_mf_paras[i*INTER], b = dis_mf_paras[i*INTER + 1], c = dis_mf_paras[i*INTER + 2];
		u_dis[i] = trimf(dis, dis_mf_paras[i*INTER], dis_mf_paras[i*INTER + 1], dis_mf_paras[i*INTER + 2]);//e模糊化，计算它的隶属度
		if (u_dis[i] != 0)
			u_dis_index[j++] = i;                                              //存储被激活的模糊子集的下标，可以减小计算量
	}
	for (; j < 3; j++)u_dis_index[j] = 0;

	j = 0;
	for (int i = 0; i < NANGLE; i++)
	{
		int a = angle_mf_paras[i*INTER], b = angle_mf_paras[i*INTER + 1], c = angle_mf_paras[i*INTER + 2];
		u_angle[i] = trimf(angle, angle_mf_paras[i*INTER], angle_mf_paras[i*INTER + 1], angle_mf_paras[i*INTER + 2]);//de模糊化，计算它的隶属度
		if (u_angle[i] != 0)
			u_angle_index[j++] = i;                                                    //存储被激活的模糊子集的下标，可以减小计算量
	}
	for (; j < 3; j++)u_angle_index[j] = 0;

	float den = 0, num = 0;
	for (int m = 0; m < 3; m++)
	for (int n = 0; n < 3; n++)
	{
		int disIdx = u_dis_index[m];
		int angleIdx = u_angle_index[n];
		float m1 = u_dis[disIdx];
		float m2 = u_angle[angleIdx];
		float m3 = rule[angleIdx][disIdx];
		num += u_dis[u_dis_index[m]] * u_angle[u_angle_index[n]] * rule[u_angle_index[n]][u_dis_index[m]];
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
	Kdis = (NDIS/2) / dismax;
	Kangle = (NANGLE/2) / anglemax;
	Krot = rotmax / (NANGLE/2);

	int ruleMatrix[NANGLE][NDIS] = {
		{ TRS, ZO, ZO, ZO, ZO },
		{ TRS, TRS, ZO, ZO, ZO },
		{ TRM, TRM, TRS, ZO, ZO },
		{ TLL, TLL, TLM, TLS, ZO },
		{ TLM, TLM, TLS, ZO, ZO },
		{ TLS, TLS, ZO, ZO, ZO },
		{ TLS, ZO, ZO, ZO, ZO },
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
		TLL, TLL, TLM,
		TLL, TLM, TLS,
		TLM, TLS, ZO,
		TLS, ZO, TRS,
		ZO, TRS, TRM,
		TRS, TRM, TRL,
		TRM, TRL, TRL
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

/// <summary>
/// 计算障碍物位置
/// </summary>
/// <param name="laserData">激光数据检测指针</param>
/// <param name="len">数据长度</param>
/// <returns>是为有障碍物，否为无障碍物</returns>
bool ObstacleAvoid::SetObstaclePosture(int* laserData, int len)
{
	bool ret = false;	//是否有障碍物
	//遍历扫描数据，筛选出10m内最近的障碍物
	bool bScanObs = false;	//是否正在扫描障碍物
	int aObsAngle[181] = { 0 };	//障碍物标记数组
	int aObsNo = 0;	//障碍物编号

	for (int i = 0; i < len; i++){
		if (laserData[i] <= dismax)	//默认以cm为单位，i即为角度，0-180，从右边开始，逆时针增加
		{
			if (!bScanObs){
				bScanObs = true;
				++aObsNo;
			}
			aObsAngle[i] = aObsNo;
		}
		//当一个障碍物遍历完后，重置状态
		else if (laserData[i]>dismax&&bScanObs)
		{
			bScanObs = false;
		}
	}

	if (aObsNo > 0)
	{
		ret = true;
		int idx = 0;	//障碍物角度索引
		int no = 0;		//障碍物编号
		if (aObsAngle[0] > 0){
			//中间有障碍物，则找出障碍物最近的边缘
			for (int i = 1; i <= 90; i++){
				int left = 90 + i, right = 90 - i;
				if (aObsAngle[left] == 0){
					idx = left;
					no = aObsAngle[left];
					break;
				}
				else if (aObsAngle[right] == 0){
					idx = right;
					no = aObsAngle[right];
					break;
				}
			}
		}
		else{
			//中间无障碍物，则找出最近的障碍物的边缘
			for (int i = 1; i <= 90; i++){
				int left = 90 + i, right = 90 - i;
				if (aObsAngle[left] > 0){
					idx = left;
					no = aObsAngle[left];
					break;
				}
				else if (aObsAngle[right] > 0){
					idx = right;
					no = aObsAngle[right];
					break;
				}
			}
		}
		angle = Kangle * (idx - 90);
		dis = Kdis*laserData[idx];

		obsInfo.avoidAngle = idx;
		obsInfo.avoidDis = laserData[idx];

		//更新障碍物信息
		obsInfo.rightAngle = -91;
		for (int i = 0; i < 181; i++){
			if (aObsAngle[i] == no){
				if (obsInfo.rightAngle == -91){
					obsInfo.rightAngle = i;
					obsInfo.rightDis = laserData[i];
				}
				else{
					obsInfo.leftAngle = i;
					obsInfo.leftDis = laserData[i];
				}
			}
		}
	}
	return ret;
}

/// <summary>
/// 获取障碍物信息
/// </summary>
/// <returns></returns>
ObstacleInfo ObstacleAvoid::GetObsInfo(){
	return obsInfo;
}
