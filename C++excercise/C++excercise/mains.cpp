#include <proj_api.h>  
#include <iostream>  


int main(int argc, char **argv)
{
	projPJ pj_merc, pj_latlong;
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
	system("pause");
	exit(0);
}