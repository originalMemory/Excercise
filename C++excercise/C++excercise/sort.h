#include "stdafx.h"

#ifndef SORT
#define SORT

/// <summary>
/// ֱ�Ӳ�������
/// </summary>
/// <param name="arr">����</param>
/// <param name="len">���鳤��</param>
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
/// �۰��������
/// </summary>
/// <param name="arr">����</param>
/// <param name="len">���鳤��</param>
void HalfInsertSort(int *arr, int len){
	for (int i = 1; i < len; i++)
	{
		int temp = arr[i];	// ��ʱ����
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
/// ϣ������
/// </summary>
/// <param name="arr">����</param>
/// <param name="len">���鳤��</param>
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
/// ð������
/// </summary>
/// <param name="arr">����</param>
/// <param name="len">���鳤��</param>
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
/// ��������ָ��㷨
/// </summary>
/// <param name="array">����</param>
/// <param name="left">���λ��</param>
/// <param name="right">�Ҷ�λ��</param>
/// <returns>������λ��</returns>
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
/// �������������㷨
/// </summary>
/// <param name="array">����</param>
/// <param name="left">���λ��</param>
/// <param name="right">�Ҷ�λ��</param>
/// <returns>������λ��</returns>
void QuickSort(int *array, int left, int right){
	if (left<right)
	{
		int p = Partition(array, left, right);
		QuickSort(array, left, p - 1);
		QuickSort(array, p + 1, right);
	}
}
#endif // !SORT


void sift(int a[], int root, int len)//�ѵ�������
{
	int child = 2 * root;
	while (child <= len)//ע��ú����Ĳ����Ǵ�root��len�����Դ˴�����д=
	{
		if (child < len&&a[child] < a[child + 1])//ע��˴�������child<len��֮���Բ�ȡ=��Ϊchild=child+1�����
		{
			child = child + 1;    //�õ���ǰ���ڵ���ӽڵ���ֵ�ϴ�Ľڵ�
		}
		if (a[root] < a[child])
		{
			swap(a[root], a[child]);
			root = child;
			child = 2 * root;
		}
		else
		{
			break;//����ط�֮���Կ���ֱ��break����Ϊ�ѵ���������ǰ����������Ƕѣ�ֻ����������֮������µ��������������ǰ��㲻���㣬������������һ���������㡣
		}
	}
}
void cre_heap(int a[], int n)
{
	for (int i = n / 2; i >= 1; i--)//�����һ����Ҷ�ӽ�㿪ʼ���Ե����ϵ��ƣ�֮�����Ǵӷ�Ҷ�ӽ�㿪ʼ������Ϊ�ú����ǽ���ǰ��������ӽ��Ƚϴ�С����Ҷ�ӽ�����ӽ��
	{

		sift(a, i, n);//�����Ѻ����Ĳ���Ϊ��array_name,root,len.
	}
}
void heap_sort(int a[], int n)
{
	cre_heap(a, n);//������ĵ�һ����������
	for (int i = n; i >= 1; i--)
	{
		swap(a[1], a[i]);
		sift(a, 1, i - 1);
	}
}



/// <summary>
/// ʹ��manacher�㷨���������
/// </summary>
/// <param name="str">ԭ�ַ���</param>
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
	//������
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
	//������
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

