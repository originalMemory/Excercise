#include "stdafx.h"
#include<iostream>
#include <time.h>
#include <Windows.h>
#define NB -3
#define NM -2
#define NS -1
#define ZO 0
#define PS 1
#define PM 2
#define PB 3

using namespace std;
int main()
{
	double a, b,dur;
	a = clock();
	Sleep(1245);
	b = clock();
	dur = ((double)(b - a)) / CLK_TCK;
	cout << a << endl << b << endl << dur << endl << (b - a) / CLK_TCK << endl;
	system("pause");
	return 0;
}