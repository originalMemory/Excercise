#include "stdafx.h"

bool isDouble(string str){
	for (size_t i = 0; i < str.length(); i++)
	{
		char c = str[i];
		if (c!='.'&&(c<'0'||c>'9'))
		{
			return false;
		}
	}
	return true;
}

int main(int argc, char *argv[])
{
	string a,b;
	cin >> a >> b;
	if (isDouble(a)&&isDouble(b))
	{
		double x = stod(a), y = stod(b);
		cout << "true " << x + y << endl;
	}
	else
	{
		cout << "false \"\"" << endl;
	}
	system("pause");
	return 0;
}