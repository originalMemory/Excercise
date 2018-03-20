// C++excercise.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <string>  
#include <iostream>  
using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{

	double lon = 11616.61904522;
	int d2 = lon / 100;
	
	double d3 = (lon - d2 * 100) / 60;
	double d4 = d2 + d3;
	cout << lon << endl << d2 << endl << d3 << endl << d4 << endl;
	system("pause");

	return 0;
}

