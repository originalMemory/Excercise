
// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件

#pragma once

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // 从 Windows 头中排除极少使用的资料
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // 某些 CString 构造函数将是显式的

// 关闭 MFC 对某些常见但经常可放心忽略的警告消息的隐藏
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC 核心组件和标准组件
#include <afxext.h>         // MFC 扩展


#include <afxdisp.h>        // MFC 自动化类



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC 对 Internet Explorer 4 公共控件的支持
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>             // MFC 对 Windows 公共控件的支持
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxcontrolbars.h>     // 功能区和控件条的 MFC 支持









#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif

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
