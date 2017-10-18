//////////////////////////////////////////////////////////////////////////
///   
/// @file    GPSTanslate.h    
/// @brief   GPS文件解析 
///  
/// 本文件完成串口通信类的声明  
///  
/// @version 1.0     
/// @author  卢俊   
/// @E-mail：lujun.hust@gmail.com  
/// @date    2010/03/19  
///  
///  修订说明：  
//////////////////////////////////////////////////////////////////////////


#ifndef GPSTRANSLATE_H
#define GPSTRANSLATE_H

#include <time.h>

/*
GPS语句类型
*/
enum GPSType
{
	GGA,
	RMC,
	VTG
};

/*
解析后的GPS语句，基类
*/
class GPSInfo
{
public:
	GPSType InfoType;		//语句类型
private:

};

class GGAInfo:GPSInfo
{
public:
	GGAInfo();
	~GGAInfo();
	tm UTCTime;
	double Latitude;
	char NSIndicator;
	double Longitude;
	char EWIndicator;
	int GPSStatus;
	int UseSatelliteNum;
	double HDOP;
	double Altitude;


private:

};

GGAInfo::GGAInfo()
{
}

GGAInfo::~GGAInfo()
{
}

class GPSTanslate
{
public:
	GPSTanslate();
	~GPSTanslate();

private:

};

GPSTanslate::GPSTanslate()
{
}

GPSTanslate::~GPSTanslate()
{
}
#endif // !GPSTRANSLATE_H
