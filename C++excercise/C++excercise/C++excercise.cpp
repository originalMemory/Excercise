#include "stdafx.h"
#include "sort.h"
#include "mytools.h"
#include "avoid.h"

#include <bitset>

struct TreeNode {
	int val;
	TreeNode *left;
	TreeNode *right;
	TreeNode(int x) : val(x), left(NULL), right(NULL) {}
};


struct ListNode {
	int val;
	struct ListNode *next;
	ListNode(int x) :
		val(x), next(NULL) {
	}
};

class LRUCache {
private:
	int n;	//����
	list<pair<int, int>> li;	//ʵ�ʼ�ֵ��˳��
	unordered_map<int, list<pair<int, int>>::iterator> m;	//��ֵ��ӳ��˳��
public:
	LRUCache(int capacity) {
		n = capacity;
	}

	int get(int key) {
		//��ȡkey��Ӧ��li�ĵ�����
		auto it = m.find(key);
		int ans = -1;
		if (it != m.end()){
			//ɾ��ԭλ�õ���������ͷ�������µ�����
			ans = it->second->second;
			li.erase(it->second);
			li.push_front(make_pair(it->first, ans));
			it->second = li.begin();
		}
		return ans;
	}

	void put(int key, int value) {
		auto it = m.find(key);
		//��ֵ�Դ���ʱ
		if (it != m.end()){
			//ɾ��ԭλ�õ���������ͷ�������µ�����
			li.erase(it->second);
			li.push_front(make_pair(it->first, value));
			it->second = li.begin();
		}
		//��ֵ�Բ�����������δ��ʱ
		else if (m.size() < n){
			//����¼�ֵ��
			li.push_front(make_pair(key, value));
			m[key] = li.begin();
		}
		//��ֵ�Բ���������������ʱ
		else{
			//ɾ��ĩβ�ļ�ֵ��������µ�
			auto it = li.end();
			it--;
			m.erase(it->first);
			li.erase(it);
			li.push_front(make_pair(key, value));
			m[key] = li.begin();
		}
	}
};

int mon2day[12] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

int date2int(string s){
	int mon = atoi(s.substr(5, 2).c_str()) - 1;
	int day = atoi(s.substr(8, 2).c_str());
	int ret = 0;
	for (int i = 0; i < mon; i++)
		ret += mon2day[i];
	ret += day - 1;
	return ret;
}



vector<string> evaluate(vector<string>&A, vector<string>& B){
	if (A.size() == 0 || B.size() == 0)
		return{};
	string year = A[0].substr(0, 4);
	vector<bool> forecast(365, false);
	vector<bool> rain(365, false);
	for (int i = 0; i < A.size(); i++){
		rain[date2int(A[i])] = true;
	}
	for (int i = 0; i < B.size(); i++){
		forecast[date2int(B[i])] = true;
	}

}


void printByte(int n) {
	cout << n << '\t' << bitset<sizeof(int)*8>(n) << endl;
}

int add(int a, int b) {
	printByte(a);
	printByte(b);
	int c = a ^ b;
	printByte(c);
	int d = a & b<1;
	printByte(d);
	return c + d;
}




int main(int argc, char *argv[])
{
	// MergeDirFiles("D:\\[��ߤ��] �Ѥ��ɥ�ޥ����`! [4K�߈D�M]",3);

	

	//Solution s;
	//vector<int> data = { 100, 4, 200, 1, 3, 2 };
	//vector <pair<int, int>> d2 = { { 5, 4 }, { 6, 4 }, { 6, 7 }, { 2, 3 } };
	//int d = -1;
	////char *a = "ABCESFCSADEE";
	////char str[] = "ABCCED";
	//ListNode *li = new ListNode(4);
	//li->next = new ListNode(2);
	//li->next->next = new ListNode(1);
	//li->next->next->next = new ListNode(3);
	////li->next->next->next->next = new ListNode(5);
	////li->next->next->next->next->next = new ListNode(2);
	//string str = "   a   b ";
	//auto res = s.maxEnvelopes(d2);
	////cout << value[0][0] << endl;

	/*ObstacleAvoid avoid;
	avoid.Initialize(600, 45, 45);
	int angle = -10;
	int dis = 340;
	for (int i = 0; i < 10; i++){
		int rot = avoid.ComputeAvoidHeading(angle, dis);
		cout << "����:" << dis << "\t�Ƕȣ�" << angle << "\tת���:" << rot << endl;
		angle -= rot;
	}*/
	cout << add(10, 1) << endl;
	system("pause");
	return 0;
}

