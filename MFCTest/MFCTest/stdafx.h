
// stdafx.h : ��׼ϵͳ�����ļ��İ����ļ���
// ���Ǿ���ʹ�õ��������ĵ�
// �ض�����Ŀ�İ����ļ�

#pragma once

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // �� Windows ͷ���ų�����ʹ�õ�����
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // ĳЩ CString ���캯��������ʽ��

// �ر� MFC ��ĳЩ�����������ɷ��ĺ��Եľ�����Ϣ������
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC ��������ͱ�׼���
#include <afxext.h>         // MFC ��չ


#include <afxdisp.h>        // MFC �Զ�����



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC �� Internet Explorer 4 �����ؼ���֧��
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>             // MFC �� Windows �����ؼ���֧��
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxcontrolbars.h>     // �������Ϳؼ����� MFC ֧��








#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif


#pragma region ArcEngine ͷ�ļ� 
#pragma warning(push) 
#pragma warning(disable : 4192) 
#pragma warning(disable : 4146) 
#import "libid:6FCCEDE0-179D-4D12-B586-58C88D26CA78" raw_interfaces_only no_implementation 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriSystem.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids, exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE") rename("min", "esriMin") rename("max", "esriMax") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriSystemUI.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR","IProgressDialog") rename("ICommand", "esriICommand") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriControls.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriGeometry.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriDisplay.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR") rename("RGB", "esriRGB") rename("CMYK", "esriCMYK") rename("ResetDC", "esriResetDC") rename("GetMessage", "esriGetMessage") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriOutput.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriGeoDatabase.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR"), rename("GetMessage", "esriGetMessage") ,rename("ICursor", "esriICursor"),rename("IRow", "esriIRow") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriDataSourcesFile.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriDataSourcesRaster.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR") 
#import "D:\Program Files (x86)\ArcGIS\Engine10.2\com\esriCarto.olb" raw_interfaces_only, raw_native_types, no_namespace, named_guids,exclude("OLE_COLOR","UINT_PTR") rename("ITableDefinition","esriITableDefinition") 

#import "D:\Program Files (x86)\ArcGIS\Desktop10.2\com\esriDisplayUI.olb" raw_interfaces_only raw_native_types no_namespace named_guids exclude("OLE_COLOR", "OLE_HANDLE", "VARTYPE")
#pragma warning(pop)

#pragma endregion

#include <thread>
#include <mutex>

////Seekur ����
#include <Aria.h>