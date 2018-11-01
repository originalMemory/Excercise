#include "stdafx.h"
#include "sort.h"
#include "mytools.h"


struct TreeNode {
	int val;
	TreeNode *left;
	TreeNode *right;
	TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};

class Solution {
public:
	int MoreThanHalfNum_Solution(vector<int> numbers) {
		if (numbers.size() == 0)
			return 0;
		int num = numbers[0];
		int count = 1;
		for (int i = 1; i < numbers.size(); i++){
			int n = numbers[i];
			if (n == num){
				count++;
			}
			else{
				count--;
				if (count == 0){
					num = n;
					count = 1;
				}
			}
		}
		if (count > 0){
			count = 0;
			for (int i = 0; i<numbers.size(); i++){
				if (numbers[i] == num){
					count++;
				}
			}
		}
		return count>numbers.size() / 2 ? num : 0;
	}

	//大顶堆调整
	void adjust(vector<int> &a, int root,int len){
		int child = root * 2;
		while (child<len)
		{
			if (child<len-1 && a[child] < a[child + 1]){
				child++;
			}
			if (a[child]>a[root])
			{
				swap(a[child], a[root]);
				root = child;
				child = root * 2;
			}
			else
			{
				break;
			}
		}
	}
};
int main(int argc, char *argv[])
{
	MergeDirFiles("C:\\下载\\绯月\\[無邪気漢化組][愛上陸] イジラレ ～復讐催眠 1-5 [無修正]");
	/*vector<int> nums = { 4, 5, 1, 6, 2, 7, 3, 8 };
	Solution s;
	for (int i = nums.size() / 2; i >= 0;i--)
	{
		s.adjust(nums, i, nums.size());
	}*/
	system("pause");;
	return 0;
}

