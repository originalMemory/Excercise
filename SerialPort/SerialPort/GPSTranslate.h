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
	tm UTCTime;				//UTC时间，仅有时分秒
	double Latitude;		//纬度
	char NSIndicator;		//纬度半球指示符
	double Longitude;		//经度
	char EWIndicator;		//经度半球指示符
	int GPSStatus;			// GPS状态：0=未定位，1=非差分定位，2=差分定位，6=正在估算
	int UseSatelliteNum;	//正在使用解算位置的卫星数量（00~12）（前面的0也将被传输）
	double HDOP;			//HDOP水平精度因子（0.5~99.9）
	double Altitude;		//海拔高度（-9999.9~99999.9）
	double Height;			//地球椭球面相对大地水准面的高度
	double DiffTime;		//差分时间（从最近一次接收到差分信号开始的秒数，如果不是差分定位将为空）
	int DiffId;				//差分站ID号0000~1023（前面的0也将被传输，如果不是差分定位将为空）
};

class RMCInfo :GPSInfo
{
public:
	tm UTCTime;				//UTC时间，仅有时分秒
	double Latitude;		//纬度
	char NSIndicator;		//纬度半球指示符
	double Longitude;		//经度
	char EWIndicator;		//经度半球指示符
	int GPSStatus;			// GPS状态：0=未定位，1=非差分定位，2=差分定位，6=正在估算
	int UseSatelliteNum;	//正在使用解算位置的卫星数量（00~12）（前面的0也将被传输）
	double HDOP;			//HDOP水平精度因子（0.5~99.9）
	double Altitude;		//海拔高度（-9999.9~99999.9）
	double Height;			//地球椭球面相对大地水准面的高度
	double DiffTime;		//差分时间（从最近一次接收到差分信号开始的秒数，如果不是差分定位将为空）
	int DiffId;				//差分站ID号0000~1023（前面的0也将被传输，如果不是差分定位将为空）
};

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
