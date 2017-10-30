#ifndef TESTFUN_H
#define TESTFUN_H

#include "SerialPort.h"  
#include <string>

/*
测试用函数类
*/
class TestFun
{
public:
	/*
	描述：构造函数
	参数：
		@no：端口号
		@baud：波特率
		@path：输出文件路径
	返回值：无
	*/
	void ConnectPort(int no = 3, int baud = 115200, std::string path = "data.txt");
};

#endif // !TESTFUN_H
