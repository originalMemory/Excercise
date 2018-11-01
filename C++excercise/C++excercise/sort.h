#include "stdafx.h"

#ifndef SORT
#define SORT

/// <summary>
/// 直接插入排序
/// </summary>
/// <param name="arr">数组</param>
/// <param name="len">数组长度</param>
void DirectInsertSort(int *arr, int len){
	for (int i = 1; i < len; i++)
	{
		int temp = arr[i];
		int j = i - 1;
		while (j>=0&&arr[j]>temp)
		{
			arr[j + 1] = arr[j];
			j--;
		}
		arr[j + 1] = temp;
	}
}

/// <summary>
/// 折半插入排序
/// </summary>
/// <param name="arr">数组</param>
/// <param name="len">数组长度</param>
void HalfInsertSort(int *arr, int len){
	for (int i = 1; i < len; i++)
	{
		int temp = arr[i];	// 临时数据
		int left = 0, right = i - 1;
		while (left<=right)
		{
			int mid = (left + right) / 2;
			if (arr[mid]>temp)
			{
				right = mid - 1;
			} 
			else
			{
				left = mid + 1;
			}
		}
		for (int j = i - 1; j >= left;j--)
		{
			arr[j + 1] = arr[j];
		}
		arr[left] = temp;
		
	}
}

/// <summary>
/// 希尔排序
/// </summary>
/// <param name="arr">数组</param>
/// <param name="len">数组长度</param>
void ShellInsertSort(int *arr, int len){
	int d = len / 2;
	while (d>=1)
	{
		for (int i= d; i < len; i++)
		{
			int temp = arr[i];
			int j = i;
			while (j>=d&&arr[j-d]>temp)
			{
				arr[j] = arr[j-d];
				j -= d;
			}
			arr[j] = temp;
		}
		d /= 2;
	}
}

/// <summary>
/// 冒泡排序
/// </summary>
/// <param name="arr">数组</param>
/// <param name="len">数组长度</param>
void BubbleSort(int *arr, int len){
	int temp;
	for (int i = len-1; i >=1; i--)
	{
		int flag = 0;
		for (int j = 0; j < i; j++)
		{
			if (arr[j]>arr[j+1])
			{
				temp = arr[j];
				arr[j] = arr[j + 1];
				arr[j + 1] = temp;
				flag = 1;
			}
		}
		if (flag==0)
			break;
	}
}

/// <summary>
/// 快速排序分割算法
/// </summary>
/// <param name="array">数组</param>
/// <param name="left">左端位置</param>
/// <param name="right">右端位置</param>
/// <returns>返回轴位置</returns>
int Partition(int *array, int left, int right){
	int pivot = array[left];
	while (left<right)
	{
		while (left<right&&array[right]>pivot)
		{
			right--;
		}
		array[left] = array[right];

		while (left<right&&array[left]<pivot)
		{
			left++;
		}
		array[right] = array[left];
	}
	array[left] = pivot;
	return right;
}

/// <summary>
/// 快速排序主体算法
/// </summary>
/// <param name="array">数组</param>
/// <param name="left">左端位置</param>
/// <param name="right">右端位置</param>
/// <returns>返回轴位置</returns>
void QuickSort(int *array, int left, int right){
	if (left<right)
	{
		int p = Partition(array, left, right);
		QuickSort(array, left, p - 1);
		QuickSort(array, p + 1, right);
	}
}
#endif // !SORT


void sift(int a[], int root, int len)//堆调整函数
{
	int child = 2 * root;
	while (child <= len)//注意该函数的参数是从root到len，所以此处必须写=
	{
		if (child < len&&a[child] < a[child + 1])//注意此处必须是child<len，之所以不取=因为child=child+1会溢出
		{
			child = child + 1;    //得到当前根节点的子节点中值较大的节点
		}
		if (a[root] < a[child])
		{
			swap(a[root], a[child]);
			root = child;
			child = 2 * root;
		}
		else
		{
			break;//这个地方之所以可以直接break是因为堆调整函数的前提它本身就是堆，只不过交换了之后得重新调整，所以如果当前结点不满足，则它的子孙结点一定都不满足。
		}
	}
}
void cre_heap(int a[], int n)
{
	for (int i = n / 2; i >= 1; i--)//从最后一个非叶子结点开始，自底向上倒推，之所以是从非叶子结点开始。是因为该函数是将当前结点与其子结点比较大小，而叶子结点无子结点
	{

		sift(a, i, n);//调整堆函数的参数为，array_name,root,len.
	}
}
void heap_sort(int a[], int n)
{
	cre_heap(a, n);//堆排序的第一步，建初堆
	for (int i = n; i >= 1; i--)
	{
		swap(a[1], a[i]);
		sift(a, 1, i - 1);
	}
}



/// <summary>
/// 使用manacher算法计算最长回文
/// </summary>
/// <param name="str">原字符串</param>
/// <returns></returns>
string manacher(string str){
	string tp("$#");
	for (int i = 0; i < str.length(); i++)
	{
		tp += str[i];
		tp += '#';
	}
	tp += "/";
	int mx = 0, id = 0, max = 0;
	vector<int> p(tp.length(), 1);
	for (int i = 1; i < tp.length() - 1; i++)
	{
		char c = tp[i];
		if (mx>i){
			p[i] = min(p[2 * id - i], mx - i);
		}
		while (tp[i + p[i]] == tp[i - p[i]])
		{
			p[i]++;
		}
		if (i + p[i] > mx){
			mx = i + p[i];
			id = i;
		}
		if (p[max] < p[id]){
			max = id;
		}
	}
	string r = "";
	int i = max - p[max] + 2;
	for (; i < max + p[max]; i += 2){
		r += tp[i];
	}
	return r;
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

