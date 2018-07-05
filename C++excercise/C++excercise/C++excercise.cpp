#include "stdafx.h"
#include<iostream>
#include"Fuzzy_controller.h"
#define NB -3
#define NM -2
#define NS -1
#define ZO 0
#define PS 1
#define PM 2
#define PB 3


int main()
{
	float target = 600;
	float actual = 0;
	float u = 0;
	int ruleMatrix[7][7] = { { NB, NB, NM, NM, NS, ZO, ZO },
	{ NB, NB, NM, NS, NS, ZO, PS },
	{ NM, NM, NM, NS, ZO, PS, PS },
	{ NM, NM, NS, ZO, PS, PM, PM },
	{ NS, NS, ZO, PS, PS, PM, PM },
	{ NS, ZO, PS, PM, PM, PM, PB },
	{ ZO, ZO, PM, PM, PM, PB, PB } };//模糊规则表
	float e_mf_paras[21] = { -3, -3, -2, -3, -2, -1, -2, -1, 0, -1, 0, 1, 0, 1, 2, 1, 2, 3, 2, 3, 3 };//误差e的隶属度函数参数，这里隶属度函数为三角型，所以3个数据为一组
	float de_mf_paras[21] = { -3, -3, -2, -3, -2, -1, -2, -1, 0, -1, 0, 1, 0, 1, 2, 1, 2, 3, 2, 3, 3 };//误差变化率de的模糊隶属度函数参数
	float u_mf_paras[21] = { -3, -3, -2, -3, -2, -1, -2, -1, 0, -1, 0, 1, 0, 1, 2, 1, 2, 3, 2, 3, 3 };//输出量u的隶属度函数参数
	Fuzzy_controller fuzzy(1000, 650, 500);//控制器初始化，设定误差，误差变化率，输出的最大值
	fuzzy.setMf("trimf", e_mf_paras, "trimf", de_mf_paras, "trimf", u_mf_paras);//设定模糊隶属度函数
	fuzzy.setRule(ruleMatrix);//设定模糊规则
	cout << "num   target    actual" << endl;
	for (int i = 0; i<100; i++)
	{
		u = fuzzy.realize(target, actual);
		actual += u;
		cout << i << "      " << target << "      " << actual << endl;
	}
	fuzzy.showInfo();
	system("pause");
	return 0;
}