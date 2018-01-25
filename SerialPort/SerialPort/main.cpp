// main.cpp : Defines the entry point for the console application.  
//  
#include "SerialPort.h"  
#include <iostream>  
#include <fstream>
#include "GPSTranslate.h"
#include "TestFun.h"

using namespace std;

int main(int argc, char* argv[])
{
	/*GPSTanslate tran;
	ifstream is("data.txt");
	string line;
	getline(is, line);
	getline(is, line);
	getline(is, line);
	GPSInfo* info = tran.Tanslate(line);

	cout << ((RMCInfo*)info)->Longitude << endl;
	system("pause");*/

	TestFun *test = new TestFun();
	test->ConnectPort(2);

	return 0;
}