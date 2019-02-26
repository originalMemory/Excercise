#include<string>
#include <iostream>
//#include "MFCTestView.h"

#ifndef _OBSTACLEAVOID_H_
#define _OBSTACLEAVOID_H_


/// <summary>
/// �ϰ�����Ϣ
/// </summary>
struct ObstacleInfo{
	int leftAngle;		//���Ե�Ƕȣ���ǰ��Ϊ0����ʱ��Ϊ����
	int leftDis;		//���Ե����
	int rightAngle;		//�ұ�Ե�Ƕȣ���ǰ��Ϊ0����ʱ��Ϊ����
	int rightDis;		//�ұ�Ե����
	int avoidAngle;		//����ʹ�ýǶȣ���ǰ��Ϊ0����ʱ��Ϊ����
	int avoidDis;		//����ʹ�þ���
};

class ObstacleAvoid
{
public:
	const static int NANGLE = 7;//�Ƕ���������ģ���Ӽ���
	const static int NDIS = 5;//������������ģ���Ӽ���
	float dis;     //������ľ���
	float angle;    //������ĽǶ�
	float Kdis;    //Kdis=n/dismax,��������Ϊ[-4,4],ÿ2��Ϊһ�α仯
	float Kangle;   //Kangle=n/anglemax,��������Ϊ[-6,6],ÿ2��Ϊһ�α仯
private:
	float dismax;  //���������������
	float anglemax; //�ǶȻ������������
	float rotmax;  //���������
	float Krot;    //Krot=rotmax/n,��������Ϊ[-6,6],ÿ2��Ϊһ�α仯
	int rule[NANGLE][NDIS];//ģ�������
	float dis_mf_paras[NDIS * 3]; //����������Ⱥ����Ĳ���
	float angle_mf_paras[NANGLE * 3];//�Ƕȵ�ƫ�������Ⱥ����Ĳ���
	float rot_mf_paras[NANGLE * 3]; //ת��ǵ������Ⱥ����Ĳ���
	ObstacleInfo obsInfo;	//����������ϰ�����Ϣ
public:
	ObstacleAvoid();
	~ObstacleAvoid();
	float trimf(float x, float a, float b, float c);          //���������Ⱥ���
	float ComputeAvoidHeading(double ang, double d);              //ʵ��ģ�����ƣ�����ת���
	/*
	��������ʼ��
	������
	@dis_max��������
	@angle_max�����Ƕ�
	@rot_max�����ת���
	����ֵ��
	*/
	void Initialize(float dis_max, float angle_max, float rot_max);
	/// <summary>
	/// �����ϰ���λ��
	/// </summary>
	/// <param name="laserData">�������ݼ��ָ��</param>
	/// <param name="len">���ݳ���</param>
	/// <returns>true�����ϰ��false�����ϰ���</returns>
	bool SetObstaclePosture(int* laserData, int len);

	/// <summary>
	/// ��ȡ�ϰ�����Ϣ
	/// </summary>
	/// <returns></returns>
	ObstacleInfo GetObsInfo();
};

#endif