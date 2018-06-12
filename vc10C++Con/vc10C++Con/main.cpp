//#include "highgui.h" 

#include "gdal_priv.h"  
#include "gdalwarper.h"  
#include "ogr_srs_api.h"
#include <iostream>

int main( int argc,char** argv ) 
{ 
	char** papszWarpOptions = NULL;  
	papszWarpOptions = CSLSetNameValue( papszWarpOptions, "METHOD", "GEOLOC_ARRAY" ); 
	std::cout<<papszWarpOptions;
	system( "PAUSE ");
} 