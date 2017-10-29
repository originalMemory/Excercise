#include "GPSTranslate.h"
#include <fstream>
#include <iostream>
#include <vector>
#include <sstream>

//数据类型转换模板函数   
template <class Type>
Type stringToNum(const string str)
{
	istringstream iss(str);
	Type num;
	iss >> num;
	return num;
}

/*
描述：构造函数
参数：
@line：GPS单行语句
返回值：GPS解析语句
*/
GPSInfo* GPSTanslate::Tanslate(string line)
{
	vector<string> arr1;  //定义一个字符串容器    
	int position = 0;
	do
	{
		string tmp_s;
		position = line.find(","); //找到逗号的位置     
		tmp_s = line.substr(0, position); //截取需要的字符串      
		line.erase(0, position + 1); //将已读取的数据删去       
		arr1.push_back(tmp_s);   //将字符串压入容器中    
	} while (position != -1);
	GPSInfo* info;
	string tag = arr1[0];
	if (strcmp(tag.c_str(), "$GPGGA"))
	{
		info = new GGAInfo();

	} 
	else
	{
	}
}