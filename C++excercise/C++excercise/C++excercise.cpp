// C++excercise.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <string>  
#include <iostream>  
#include <math.h>
#include "gdal_priv.h"  
#include "gdalwarper.h"  
#include "ogr_srs_api.h"

using namespace std;

#define PI 3.1415926  

int _tmain(int argc, _TCHAR* argv[])
{
	GDALAllRegister();
	char** papszWarpOptions = NULL;
	papszWarpOptions = CSLSetNameValue(papszWarpOptions, "METHOD", "GEOLOC_ARRAY");
	std::cout << papszWarpOptions;

	system("pause");

	return 0;
}

