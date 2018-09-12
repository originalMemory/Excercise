#include "stdafx.h"
#include <Windows.h>
#include <io.h>

/// <summary>
/// 获取文件及文件夹路径
/// </summary>
/// <param name="dirPath">文件夹路径</param>
/// <param name="files">查找的文件或文件夹路径结果vector</param>
/// <param name="isRecur">是否查找子目录，默认不查找</param>
/// <param name="type">查找类型：0为都查找；1为文件；2为文件夹。默认为文件</param>
void getPath(string dirPath, vector<string>& files, bool isRecur = false, int type = 1) {
	//文件句柄
	long hFile = 0;
	//文件信息
	struct _finddata_t fileinfo;  //很少用的文件信息读取结构
	string p;  //string类很有意思的一个赋值函数:assign()，有很多重载版本
	string st = p.assign(dirPath).append("\\*").c_str();
	if ((hFile = _findfirst(p.assign(dirPath).append("\\*").c_str(), &fileinfo)) != -1) {
		do {
			if (fileinfo.attrib & _A_SUBDIR) {  //比较文件类型是否是文件夹
				if (strcmp(fileinfo.name, ".") != 0 && strcmp(fileinfo.name, "..") != 0)
				{
					if (type != 1)
					{
						files.push_back(p.assign(dirPath).append("\\").append(fileinfo.name));
					}
					if (isRecur)
					{
						getPath(p.assign(dirPath).append("\\").append(fileinfo.name), files, true);
					}
				}
			}
			else if (type != 2) {
				files.push_back(p.assign(dirPath).append("\\").append(fileinfo.name));
			}
		} while (_findnext(hFile, &fileinfo) == 0);  //寻找下一个，成功返回0，否则-1
		_findclose(hFile);

	}

}

/// <summary>
/// 合并不同文件夹，并按序号重命名文件
/// </summary>
/// <param name="dir_path">文件夹路径</param>
void MergeDirFiles(string dir_path){
	vector<string> files;

	getPath(dir_path, files, true);

	char szDrive[_MAX_DRIVE];   //磁盘名
	char szDir[_MAX_DIR];       //路径名
	char szFname[_MAX_FNAME];   //文件名
	char szExt[_MAX_EXT];       //后缀名
	int num = 1;
	int per = 1;
	for (vector<string>::iterator iter = files.begin(); iter != files.end(); iter++)
	{
		cout << *iter << endl;
		string old_path = *iter;

		_splitpath_s(old_path.c_str(), szDrive, szDir, szFname, szExt); //分解路径
		//判断是否是正常图片文件
		if (strcmp(szExt, ".jpg") == 0 || strcmp(szExt, ".JPG") == 0 || strcmp(szExt, ".jpeg") == 0 || strcmp(szExt, ".JPEG") == 0
			|| strcmp(szExt, ".png") == 0 || strcmp(szExt, ".PNG") == 0 || strcmp(szExt, ".gif") == 0 || strcmp(szExt, ".GIF") == 0
			|| strcmp(szExt, ".bmp") == 0 || strcmp(szExt, ".BMP") == 0 || strcmp(szExt, ".tif") == 0 || strcmp(szExt, ".TIF") == 0)
		{
			string newName;

			switch (num)
			{
			case 10:
				per = 2;
				break;
			case 100:
				per = 3;
				break;
			default:
				break;
			}

			switch (per)
			{
			case 1:
				newName = "00" + to_string(num);
				break;
			case 2:
				newName = "0" + to_string(num);
				break;
			case 3:
				newName = to_string(num);
				break;
			default:
				break;
			}

			int j = 0;
			while (szExt[j] != '\0')
			{
				newName += szExt[j];
				j++;
			}
			string new_path = dir_path + "\\" + newName;
			cout << new_path << endl;
			num++;
			MoveFile(old_path.c_str(), new_path.c_str());
			/*LPCWSTR lOld = stringToLPCWSTR(old_path);
			LPCWSTR lNew = stringToLPCWSTR(dir_path + "\\" + newName);*/
			/*int pos = old_path.find_last_of('\\');
			if (pos!=string::npos)
			{
			pos ++;
			string name = old_path.substr(pos, old_path.size() - pos);
			cout << dir_path+"\\"+name << endl;
			}*/
		}
		else
		{
			DeleteFile(old_path.c_str());
			continue;
		}


	}
}