// C++excercise.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <string>  
#include <math.h>
#include <vector>
#include <stdio.h>  
#include <stdlib.h>  
using namespace std;

#define MAX_LENGTH 100
int _tmain(int argc, _TCHAR* argv[])
{
	int n, k;
	cin >> n >> k;
	vector<int> num;
	for (int i = 0; i < n;i++)
	{
		int tp;
		cin >> tp;
		if (tp%k!=0)
		{
			num.push_back(tp);
		}
	}
	int len = num.size();
	int maxCount = 0;
	for (int i = 0; i < len; i++)
	{
		vector<int> tpbuf;
		tpbuf.push_back(num[i]);
		int j = 1;
		for (int l = i + 1; l < len; l++)
		{
			bool isEqual = false;
			for (int m = 0; m < j;m++)
			{
				if ((tpbuf[m] + num[l]) % k == 0)
				{
					isEqual = true;
					break;
				}
			}
			if (!isEqual)
			{
				tpbuf.push_back(num[l]);
				j++;
			}
		}
		if (maxCount<j)
		{
			maxCount = j;
		}
	}
	cout << maxCount;

	//system("pause");

	return 0;
}

