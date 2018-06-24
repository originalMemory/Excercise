#include "TestFun.h"
#include <iostream>  
#include <fstream>

/*
描述：构造函数
参数：
@no：端口号
@baud：波特率
@path：输出文件路径
返回值：无
*/
void TestFun::ConnectPort(int no, int baud, std::string path){
	CSerialPort mySerialPort;

	if (!mySerialPort.InitPort(no, baud))
	{
		std::cout << "初始化失败！" << std::endl;
	}
	else
	{
		std::cout << "初始化成功！" << std::endl;
	}

	if (!mySerialPort.OpenListenThread())
	{
		std::cout << "监听线程打开失败！" << std::endl;
	}
	else
	{
		std::cout << "监听线程打开成功！" << std::endl;
	}

	int temp;
	std::cin >> temp;
	//std::ofstream out(path);
	//if (temp == 1){
		mySerialPort.CloseListenTread();
		for (int i = 0; i < mySerialPort.length; i++){
			for (int j = 0; mySerialPort.dataAarry[i][j] != '\n'; j++){
				std::cout << mySerialPort.dataAarry[i][j];
				//out << mySerialPort.dataAarry[i][j];
			}
			std::cout << std::endl;
			//out << '\n';
		}
	//}
}