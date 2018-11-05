#include "stdafx.h"
#include "GPSTranslate.h"

////��������ת��ģ�庯��   
//template <class Type>
//Type stringToNum(const string str)
//{
//	istringstream iss(str);
//	Type num;
//	iss >> num;
//	return num;
//}

/*
���������캯��
������
@line��GPS�������
����ֵ��GPS�������ָ��
*/
GPSInfo* GpsTanslate(string line)
{
	vector<string> arr2;  //����һ��GPS�ַ���������������    
	int	position = 0;
	do
	{
		string tmp_s;
		position = line.find(","); //�ҵ����ŵ�λ��     
		if (position != -1)
		{
			tmp_s = line.substr(0, position); //��ȡ��Ҫ���ַ���
			line.erase(0, position + 1); //���Ѷ�ȡ������ɾȥ       
			arr2.push_back(tmp_s);   //���ַ���ѹ��������    
		}
		else
		{
			arr2.push_back(line);
		}
	} while (position != -1);
	GPSInfo* info = new GPSInfo();
	info->InfoType = Zero;
	string tag = arr2[0];
	//GGA����
	if (tag.find("GPGGA") != string::npos&&arr2.size() >= 14)
	{
		GGAInfo* gga = new GGAInfo();
		gga->InfoType = GGA;
		//����ʱ����
		tm utcHms;		//ʱ����ʱ��
		string hourStr = arr2[1];
		int hour = atoi(hourStr.substr(0, 2).data());
		int min = atoi(hourStr.substr(2, 2).data());
		int second = atoi(hourStr.substr(4, 2).data());
		utcHms.tm_hour = hour;
		utcHms.tm_min = min;
		utcHms.tm_sec = second;
		gga->UTCHms = utcHms;
		//����γ��
		double d1 = atof(arr2[2].data());
		int d2 = d1 / 100;
		double d3 = d2 + (d1 - d2 * 100) / 60;
		gga->Latitude = d3;
		gga->NSIndicator = arr2[3][0];
		//��������
		d1 = atof(arr2[4].data());
		d2 = d1 / 100;
		d3 = d2 + (d1 - d2 * 100) / 60;
		gga->Longitude = d3;
		gga->EWIndicator = arr2[5][0];

		gga->GPSStatus = atoi(arr2[6].data());
		gga->UseSatelliteNum = atoi(arr2[7].data());
		gga->HDOP = atof(arr2[8].data());
		gga->Altitude = atof(arr2[9].data());
		gga->Height = atof(arr2[11].data());
		//�ж��Ƿ��в��ʱ�估ID
		if (gga->GPSStatus == 2)
		{
			gga->DiffSecs = atof(arr2[13].data());
			gga->DiffId = atoi(arr2[14].data());
		}
		delete info;
		info = gga;
	}
	//RMC����
	else if (tag.find("GPRMC") != string::npos&&arr2.size() >= 13)
	{
		RMCInfo* rmc = new RMCInfo();
		rmc->InfoType = RMC;
		//����ʱ����
		tm utcHms;		//ʱ����ʱ��
		string hourStr = arr2[1];
		int hour = atoi(hourStr.substr(0, 2).data());
		int min = atoi(hourStr.substr(2, 2).data());
		int second = atoi(hourStr.substr(4, 2).data());
		utcHms.tm_hour = hour;
		utcHms.tm_min = min;
		utcHms.tm_sec = second;
		rmc->UTCHms = utcHms;
		rmc->PosStatus = arr2[2][0];
		//����γ��
		rmc->Latitude = atof(arr2[3].data()) / 100;
		rmc->NSIndicator = arr2[4][0];
		//��������
		rmc->Longitude = atof(arr2[5].data()) / 100;
		rmc->EWIndicator = arr2[6][0];
		rmc->SpeedKnot = atof(arr2[7].data());
		rmc->Heading = atof(arr2[8].data());
		//��������
		tm utcDate;		//����
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
			//�ж��Ƿ���ģʽָʾ
			if (arr2[12][0] != '\0')
			{
				rmc->ModeIndication = arr2[12][0];
			}
			delete info;
			info = rmc;
		}
	}
	//VTG����
	else if (tag.find("GPVTG") != string::npos&&arr2.size() >= 10)
	{
		VTGInfo* vtg = new VTGInfo();
		vtg->InfoType = VTG;
		vtg->TrueHeading = atof(arr2[1].data());
		vtg->MagnHeading = atof(arr2[3].data());
		vtg->SpeedKnot = atof(arr2[5].data());
		vtg->SpeedKm = atof(arr2[7].data());
		//�ж��Ƿ���ģʽָʾ
		if (arr2[9][0] != '\0')
		{
			vtg->ModeIndication = arr2[9][0];
		}
		delete info;
		info = vtg;
	}
	//$PSAT,HPR,102143.00,356.881,-0.387,-0.4,N*1E
	//PSAT,HPR����
	else if (tag.find("PSAT") != string::npos&&arr2[1] == "HPR"&&arr2.size() >= 7)
	{
		PSAT_HPRInfo* hpr = new PSAT_HPRInfo();
		hpr->InfoType = PSAT_HPR;
		//����ʱ����
		tm utcHms;		//ʱ����ʱ��
		string hourStr = arr2[2];
		int hour = atoi(hourStr.substr(0, 2).data());
		int min = atoi(hourStr.substr(2, 2).data());
		int second = atoi(hourStr.substr(4, 2).data());
		utcHms.tm_hour = hour;
		utcHms.tm_min = min;
		utcHms.tm_sec = second;

		hpr->UTCHms = utcHms;
		//��������
		hpr->Heading = atof(arr2[3].data());
		hpr->member1 = atof(arr2[4].data());
		hpr->member2 = atof(arr2[5].data());
		hpr->member3 = arr2[6];
		delete info;
		info = hpr;
	}

	return info;
}