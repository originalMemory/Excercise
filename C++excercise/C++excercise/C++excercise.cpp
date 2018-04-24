// C++excercise.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <string>  
#include <iostream>  
#include <math.h>
using namespace std;

#define PI 3.1415926  

int _tmain(int argc, _TCHAR* argv[])
{
	double a = atan2(0.000025152166671, 0.000099356);
	double b = atan2(1, -1);
	double c = atan2(-1, -1);
	double d = atan2(-1, 1);
	double e = atan(1);
	double f = atan(-1);

	cout << a * 180 / PI << endl;      //第一象限
	cout << b * 180 / PI << endl;    //第二象限
	cout << c * 180 / PI << endl;   //第三象限
	cout << d * 180 / PI << endl;   //第四象限
	cout << e * 180 / PI << endl;
	cout << f * 180 / PI << endl;


	/*double jl_jd = 102834.74258026089786013677476285;
	double jl_wd = 111712.69150641055729984301412873;
	double b = abs((116.27698527749999 - 116.27679663166667) * jl_jd);
	double a = abs((39.944094107166663 - 39.944652798500002) * jl_wd);
	double dis= sqrt((a * a + b * b));
	cout << dis << endl;

	double x1 = 116.27698527749999, y1 = 39.944094107166663;
	double x2 = 116.27679663166667, y2 = 39.944652798500002;
	double ax = abs(x2 - x1);
	double ay = abs(y2 - y1);
	double len = sqrt((ax*ax + ay*ay));
	cout << len << endl;*/

	system("pause");

	return 0;
}

