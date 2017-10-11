// main.cpp : Defines the entry point for the console application.  
//  
#include "SerialPort.h"  
#include <iostream>  
#include <fstream>
//#include "include\proj_api.h";

int main(int argc, char* argv[])
{
	/*projPJ pj_merc, pj_latlong;
	double x, y;

	if (!(pj_merc = pj_init_plus("+proj=merc +lon_0=0 +k=1 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs")))
		exit(1);
	if (!(pj_latlong = pj_init_plus("+proj=longlat +datum=WGS84 +no_defs")))
		exit(1);

	x = -9.866554;
	y = 7.454779;

	x *= DEG_TO_RAD;
	y *= DEG_TO_RAD;

	pj_transform(pj_latlong, pj_merc, 1, 1, &x, &y, NULL);

	std::cout.precision(12);
	std::cout << "(" << x << " , " << y << ")" << std::endl;
	exit(0);*/
	CSerialPort mySerialPort;

	if (!mySerialPort.InitPort(3, 115200))
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
	std::ofstream out("data.txt");
	if (temp == 1){
		mySerialPort.CloseListenTread();
		for (int i = 0; i<mySerialPort.length; i++){
			for (int j = 0; mySerialPort.dataAarry[i][j] != '\n'; j++){
				std::cout << mySerialPort.dataAarry[i][j];
				out << mySerialPort.dataAarry[i][j];
			}
			std::cout << std::endl;
			out << '\n';
		}
	}
	system("pause");
	return 0;
}