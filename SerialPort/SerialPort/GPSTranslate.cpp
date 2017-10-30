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
	GPSInfo* info = new GPSInfo();
	string tag = arr1[0];
	//GGA解析
	if (strcmp(tag.c_str(), "$GPGGA") == 0)
	{
		GGAInfo* gga = new GGAInfo();
		gga->InfoType = GGA;
		//解析时分秒
		tm utcHms;		//时分秒时间
		string hourStr = arr1[1];
		int hour = atoi(hourStr.substr(0, 2).data());
		int min = atoi(hourStr.substr(2, 2).data());
		int second = atoi(hourStr.substr(4, 2).data());
		utcHms.tm_hour = hour;
		utcHms.tm_min = min;
		utcHms.tm_sec = second;
		gga->UTCHms = utcHms;
		//解析纬度
		gga->Latitude = atof(arr1[2].data());
		gga->NSIndicator = arr1[3][0];
		//解析经度
		gga->Longitude = atof(arr1[4].data());
		gga->EWIndicator = arr1[5][0];

		gga->GPSStatus = atoi(arr1[6].data());
		gga->UseSatelliteNum = atoi(arr1[7].data());
		gga->HDOP = atof(arr1[8].data());
		gga->Altitude = atof(arr1[9].data());
		gga->Height = atof(arr1[11].data());
		//判断是否有差分时间及ID
		if (gga->GPSStatus==2)
		{
			gga->DiffSecs = atof(arr1[13].data());
			gga->DiffId = atoi(arr1[14].data());
		}
		info = gga;
	} 
	//RMC解析
	else if (strcmp(tag.c_str(), "$GPRMC") == 0)
	{
		RMCInfo* rmc = new RMCInfo();
		rmc->InfoType = RMC;
		//解析时分秒
		tm utcHms;		//时分秒时间
		string hourStr = arr1[1];
		int hour = atoi(hourStr.substr(0, 2).data());
		int min = atoi(hourStr.substr(2, 2).data());
		int second = atoi(hourStr.substr(4, 2).data());
		utcHms.tm_hour = hour;
		utcHms.tm_min = min;
		utcHms.tm_sec = second;
		rmc->UTCHms = utcHms;
		rmc->PosStatus = arr1[2][0];
		//解析纬度
		rmc->Latitude = atof(arr1[3].data());
		rmc->NSIndicator = arr1[4][0];
		//解析经度
		rmc->Longitude = atof(arr1[5].data());
		rmc->EWIndicator = arr1[6][0];
		rmc->SpeedKnot = atof(arr1[7].data());
		rmc->Heading = atof(arr1[8].data());
		//解析日期
		tm utcDate;		//日期
		string dateStr = arr1[9];
		int day = atoi(dateStr.substr(0, 2).data());
		int mon = atoi(dateStr.substr(2, 2).data());
		int year = atoi(dateStr.substr(4, 2).data());
		utcDate.tm_mday = day;
		utcDate.tm_mon = mon - 1;
		utcDate.tm_year = 100 + year;

		rmc->MagnDecl = atof(arr1[10].data());
		rmc->MagnDeclDir = arr1[11][0];
		//判断是否有模式指示
		if (arr1[12][0]!='\0')
		{
			rmc->ModeIndication = arr1[12][0];
		} 
		info = rmc;
	}
	//VTG解析
	else if (strcmp(tag.c_str(), "$GPVTG") == 0)
	{
		VTGInfo* vtg = new VTGInfo();
		vtg->InfoType = VTG;
		vtg->TrueHeading = atof(arr1[1].data());
		vtg->MagnHeading = atof(arr1[3].data());
		vtg->SpeedKnot = atof(arr1[5].data());
		vtg->SpeedKm = atof(arr1[7].data());
		//判断是否有模式指示
		if (arr1[9][0] != '\0')
		{
			vtg->ModeIndication = arr1[9][0];
		}
		info = vtg;
	}

	return info;
}