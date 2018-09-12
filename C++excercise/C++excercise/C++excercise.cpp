#include "stdafx.h"
#include "sort.h"
#include "mytools.h"

int knapsack(int *weight, int *value, int *res, int n, int maxW);
int knapsack_complete(int *weight, int *value, int *res, int n, int maxW);

class Solution {
public:
	vector<string> restoreIpAddresses(string s) {
		vector<string> res;
		//不合法
		if (s.size() > 12)   return res;
		string cur("");
		dfs(s, res, cur, -1, 0, 0);
		return res;
	}
private:
	void dfs(string& s, vector<string>& res, string& cur, int prevIdx, int idx, int count)
	{
		if (count == 3)
		{
			string four = s.substr(idx);
			if (isValid(four))
				res.emplace_back(cur + four);
			return;
		}
		//因为最后一个"."后面必须有数字，所以到s.size() - 1即可
		string tmp = cur;
		for (int i = idx; i < static_cast<int>(s.size()) - 1; ++i)
		{
			//判断是否满足要求
			if (!isValid(s.substr(prevIdx + 1, i - prevIdx)))
				break;

			cur.append(1, s[i]);
			cur.append(1, '.');
			dfs(s, res, cur, i, i + 1, count + 1);
			//回溯的过程需要回到原来的样子，但是这里只弹出了"."的目的是为了继续扩充当前数字
			//不需要回到append(1, s[i])之前的样子，但是return之前需要
			cur.pop_back();
		}
		//当返回时回到原来的样子
		std::swap(cur, tmp);
	}

	bool isValid(string numStr)
	{
		if (numStr.size() > 3 || (numStr.size() > 1 && numStr.find_first_not_of('0') != 0) || (numStr.size() == 3 && numStr > "255"))
			return false;
		else
			return true;
	}
};

struct Tri
{
	int a;
	int b;
	int c;
};

bool isPrime(int x, int y){
	if (x <= 0 || y <= 0 || x == y) 
		return false;
	else if (x == 1 || y == 1)
		return true;
	else{
		while (true){
			int tp = y%x;
			if (tp == 0)
				break;
			else{
				y = x;
				x = tp;
			}
		}
		if (x > 1)
			return false;
		else
			return true;
	}
}

int main(int argc, char *argv[])
{
	int N;
	cin >> N;
	int num = 0;
	for (int i = 0; i <= N; i++)
	{
		for (int j = i; j <= N; j++)
		{
			for (int k = j; k <= N; k++)
			{
				if (i*i + j*j == k*k){
					if (isPrime(i, j) && isPrime(j, k) && isPrime(i, k)){
						num++;
					}
				}
			}
		}
	}
	cout << num << endl;
	system("pause");
	return 0;
}

int knapsack(int *weight, int *value, int *res, int n, int maxW)
{
	int **opt = new int *[n];
	//计算表格
	opt[0] = new int[maxW + 1];
	for (size_t i = 0; i <= maxW; i++)
	{
		opt[0][i] = i >= weight[0] ? value[0] : 0;
	}
	for (size_t i = 1; i < n; i++)
	{
		opt[i] = new int[maxW + 1];
		for (size_t j = 0; j <= maxW; j++)
		{
			if (weight[i]>j)
			{
				opt[i][j] = opt[i - 1][j];
			}
			else
			{
				opt[i][j] = max(opt[i - 1][j], value[i] + opt[i - 1][j - weight[i]]);
			}
		}
	}

	for (size_t i = 0; i < n; i++)
	{
		for (size_t j = 0; j <= maxW; j++)
		{
			cout << opt[i][j] << " ";
		}
		cout << endl;
	}

	int num = n - 1;
	int tpW = maxW;
	while (num)
	{
		if (opt[num][tpW] == opt[num - 1][tpW - weight[num]] + value[num])
		{
			res[num] = 1;
			tpW -= weight[num];
		}
		else
		{
			res[num] = 0;
		}
		num--;
	}
	if (opt[0][maxW])
		res[0] = 1;
	else
	{
		res[0] = 0;
	}

	int max = opt[n - 1][maxW];
	delete[] opt;
	return max;
}


int knapsack_complete(int *weight, int *value, int *res, int n, int maxW)
{
	int **opt = new int *[n];
	//计算表格
	opt[0] = new int[maxW + 1];
	for (size_t i = 0; i <= maxW; i++)
	{
		opt[0][i] = i >= weight[0] ? i / weight[0] * value[0] : 0;
	}
	for (size_t i = 1; i < n; i++)
	{
		opt[i] = new int[maxW + 1];
		for (size_t j = 0; j <= maxW; j++)
		{
			if (weight[i]>j)
			{
				opt[i][j] = opt[i - 1][j];
			}
			else
			{
				opt[i][j] = max(opt[i - 1][j], value[i] + opt[i][j - weight[i]]);
			}
		}
	}

	for (size_t i = 0; i < n; i++)
	{
		for (size_t j = 0; j <= maxW; j++)
		{
			cout << opt[i][j] << " ";
		}
		cout << endl;
	}

	int num = n - 1;
	int tpW = maxW;
	while (num)
	{
		if (opt[num][tpW] == opt[num][tpW - weight[num]] + value[num])
		{
			res[num] = 1;
			tpW -= weight[num];
		}
		else
		{
			res[num] = 0;
		}
		num--;
	}
	if (opt[0][maxW])
		res[0] = 1;
	else
	{
		res[0] = 0;
	}
	res[0] = opt[0][tpW] / value[0];

	int max = opt[n - 1][maxW];
	delete[] opt;
	return max;
}


// 只有1为公约数的两个正整数，两个整数才会互质
// 也就是说两个整数的最大公约数为1，即可判断两个正整数互质
