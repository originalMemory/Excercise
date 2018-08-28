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
