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

int getNum(){
	char x;
	int a[4];
	int value = 0;
	int i = 0;
	
	while (true)
	{
		x = cin.get();
		if (x == '\n'||x == ',')
			break;
		a[i] = x - '0';
		i++;
	}
	for (int j = 0; j < i; j++)
	{
		value += a[j]*pow(10, i - j-1);
	}
	return value;
}

int lcm2(int a, int b){
	int t;
	if (a>b)
	for (t = a; t%b; t += a);
	else
	for (t = b; t%a; t += b);
	return t;
}

vector<set<int>> city;
int cpPath( set<int> path, int idx, int used[4], int *x){
	used[idx]++;
	path.insert(idx);
	set<int> target = city[idx];
	int num = 0;
	for (auto iter = target.begin(); iter != target.end();iter++)
	{
		if (*iter==idx)
			continue;
		num += cpPath(path, *iter, used, x) + 1;
	}
	if (used[idx]==1)
		x[idx] += num;
	return num;
}

int reverse(string num){
	int value = 0;
	int len = num.size();
	for (int i = 0; i < len; i++)
	{
		int tp = num[i] - '0';
		if (tp == 3 || tp == 4 || tp == 7)
			return -1;
		switch (tp)
		{
		case 2:
			tp = 5;
			break;
		case 5:
			tp = 2;
			break;
		case 6:
			tp = 9;
			break;
		case 9:
			tp = 6;
			break;
		default:
			break;
		}
		value += tp*pow(10, len - i - 1);
	}
	return value;
}

void compute(int a[6],int i){
	int sub1 = a[i + 2] - a[i + 1];
	int sub2 = a[i + 1] - a[i];
	if (sub1 == sub2)
	{
		compute(a, i + 1);
	}
	else
	{

	}
}

int foo(vector<int> x, int n)
{
	int a = x[0], b = x[1], c = x[2];
	if (a == b && b == c)
	{
		for (int i = 3; i < n; i++)
		if (x[i] != a)
			return i;
		return -1;
	}
	else{
		if (a == b) return 2;
		if (a == c) return 1;
		return 0;
	}
}

int cpAll(int n){
	if (n <= 3)
		return 1;
	else
	{
		return cpAll(n - 1) + cpAll(n - 4);
	}
}



int main(int argc, char *argv[])
{
	MergeDirFiles("C:\\下载\\绯月\\3424");
	
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
