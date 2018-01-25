#include "stdafx.h"
#include "GPSTranslate.h"

////数据类型转换模板函数   
//template <class Type>
//Type stringToNum(const string str)
//{
//	istringstream iss(str);
//	Type num;
//	iss >> num;
//	return num;
//}

/*
描述：构造函数
参数：
@line：GPS单行语句
返回值：GPS解析语句
*/
vector<GPSInfo*> GPSTranslate::Tanslate(string line)
{
	//将一组语句拆开
	vector<string> arr1;  //定义一个GPS字符串容器    
	int position = 0;
	do
	{
		int lastPos = position;
		string tmp_s;
		position = line.find("$", position + 1); //找到下一句起始的位置
		if (position == -1)
		{
			if (lastPos == 0)
			{
				//仅一句语句
				arr1.push_back(line);
			}
		} 
		else
		{
			tmp_s = line.substr(0, position); //截取需要的字符串
			line.erase(0, position + 1); //将已读取的数据删去       
			arr1.push_back(tmp_s);   //将字符串压入容器中    
		}
		
	} while (position != -1);

	//遍历解析所有语句
	vector<GPSInfo*> infos;
	int num = arr1.size();
	string firstTag;	//第一句的语句类型
	for (int i = 0; i < num; i++)
	{
		vector<string> arr2;  //定义一个GPS字符串解析数据容器    
		position = 0;
		do
		{
			string tmp_s;
			position = arr1[i].find(","); //找到逗号的位置     
			if (position != -1)
			{
				tmp_s = arr1[i].substr(0, position); //截取需要的字符串
				if (i == 0 && position == 6){
					firstTag = tmp_s;
				}
				else if (firstTag == tmp_s)
				{
					break;
				}
				arr1[i].erase(0, position + 1); //将已读取的数据删去       
				arr2.push_back(tmp_s);   //将字符串压入容器中    
			}
			else
			{
				arr2.push_back(arr1[i]);
			}
		} while (position != -1);
		GPSInfo* info = new GPSInfo();
		try{
			info->InfoType = Zero;
			string tag = arr2[0];
			//GGA解析
			if (tag.find("GPGGA") != string::npos)
			{
				GGAInfo* gga = new GGAInfo();
				gga->InfoType = GGA;
				//解析时分秒
				tm utcHms;		//时分秒时间
				string hourStr = arr2[1];
				int hour = atoi(hourStr.substr(0, 2).data());
				int min = atoi(hourStr.substr(2, 2).data());
				int second = atoi(hourStr.substr(4, 2).data());
				utcHms.tm_hour = hour;
				utcHms.tm_min = min;
				utcHms.tm_sec = second;
				gga->UTCHms = utcHms;
				//解析纬度
				gga->Latitude = atof(arr2[2].data()) / 100;
				gga->NSIndicator = arr2[3][0];
				//解析经度
				gga->Longitude = atof(arr2[4].data()) / 100;
				gga->EWIndicator = arr2[5][0];

				gga->GPSStatus = atoi(arr2[6].data());
				gga->UseSatelliteNum = atoi(arr2[7].data());
				gga->HDOP = atof(arr2[8].data());
				gga->Altitude = atof(arr2[9].data());
				gga->Height = atof(arr2[11].data());
				//判断是否有差分时间及ID
				if (gga->GPSStatus == 2)
				{
					gga->DiffSecs = atof(arr2[13].data());
					gga->DiffId = atoi(arr2[14].data());
				}
				info = gga;
			}
			//RMC解析
			else if (tag.find("GPRMC") != string::npos)
			{
				RMCInfo* rmc = new RMCInfo();
				rmc->InfoType = RMC;
				//解析时分秒
				tm utcHms;		//时分秒时间
				string hourStr = arr2[1];
				int hour = atoi(hourStr.substr(0, 2).data());
				int min = atoi(hourStr.substr(2, 2).data());
				int second = atoi(hourStr.substr(4, 2).data());
				utcHms.tm_hour = hour;
				utcHms.tm_min = min;
				utcHms.tm_sec = second;
				rmc->UTCHms = utcHms;
				rmc->PosStatus = arr2[2][0];
				//解析纬度
				rmc->Latitude = atof(arr2[3].data()) / 100;
				rmc->NSIndicator = arr2[4][0];
				//解析经度
				rmc->Longitude = atof(arr2[5].data()) / 100;
				rmc->EWIndicator = arr2[6][0];
				rmc->SpeedKnot = atof(arr2[7].data());
				rmc->Heading = atof(arr2[8].data());
				//解析日期
				tm utcDate;		//日期
				if (arr2.size() >= 12){
					string dateStr = arr2[9];
					int day = atoi(dateStr.substr(0, 2).data());
					int mon = atoi(dateStr.substr(2, 2).data());
					int year = atoi(dateStr.substr(4, 2).data());
					utcDate.tm_mday = day;
					utcDate.tm_mon = mon - 1;
					utcDate.tm_year = 100 + year;

					rmc->MagnDecl = atof(arr2[10].data());
					rmc->MagnDeclDir = arr2[11][0];
					//判断是否有模式指示
					if (arr2[12][0] != '\0')
					{
						rmc->ModeIndication = arr2[12][0];
					}
					info = rmc;
				}
			}
			//VTG解析
			else if (tag.find("GPVTG") != string::npos)
			{
				VTGInfo* vtg = new VTGInfo();
				vtg->InfoType = VTG;
				vtg->TrueHeading = atof(arr2[1].data());
				vtg->MagnHeading = atof(arr2[3].data());
				vtg->SpeedKnot = atof(arr2[5].data());
				vtg->SpeedKm = atof(arr2[7].data());
				//判断是否有模式指示
				if (arr2[9][0] != '\0')
				{
					vtg->ModeIndication = arr2[9][0];
				}
				info = vtg;
			}

			infos.push_back(info);
		}
		catch (int){

		}
	}

	return infos;
}