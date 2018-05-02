
// MFCTestView.cpp : CMFCTestView 类的实现
//

#include "stdafx.h"
// SHARED_HANDLERS 可以在实现预览、缩略图和搜索筛选器句柄的
// ATL 项目中进行定义，并允许与该项目共享文档代码。
#ifndef SHARED_HANDLERS
#include "MFCTest.h"
#endif

#include "MFCTestDoc.h"
#include "MFCTestView.h"
#include <math.h>

#include "MainFrm.h"

#define WM_SEEKUR_RET WM_USER+100		 //Seekur返回消息
#define WM_SEEKUR_MOVE WM_USER+101		//Seekur操作消息（开始）
//#define WM_SEEKUR_SUB WM_USER+102		//Seekur操作消息（负值）
#define WM_SEEKUR_END WM_USER+103		//Seekur操作消息（结束）
#define WM_SEEKUR_STOP WM_USER+104		//Seekur操作消息（停止）

#define WM_TRACK_START WM_USER+104		//开始追踪
#define WM_TRACK_STOP WM_USER+105		//停止追踪
#define WM_TRACK_END WM_USER+106		//结束追踪

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define wm

#define PI 3.1415926
#define EARTH_RADIUS 6378.137

double rad(double d)
{
	return d * PI / 180.0;
}

double GetDistance(double lat1, double lng1, double lat2, double lng2)
{
	double radLat1 = rad(lat1);
	double radLat2 = rad(lat2);
	double a = radLat1 - radLat2;
	double b = rad(lng1) - rad(lng2);
	double s = 2 * asin(sqrt(pow(sin(a / 2), 2) +
		cos(radLat1) * cos(radLat2) * pow(sin(b / 2), 2)));
	s = s * EARTH_RADIUS;
	s = round(s * 10000) / 10000;
	return s;
}

// CMFCTestView

IMPLEMENT_DYNCREATE(CMFCTestView, CFormView)

BEGIN_MESSAGE_MAP(CMFCTestView, CFormView)
	ON_WM_CREATE()
	ON_COMMAND(ID_FILE_OPEN, &CMFCTestView::OnFileOpen)
	//	ON_WM_SIZE()
	ON_MESSAGE(WM_COMM_RXCHAR, &CMFCTestView::OnCommunication)
	ON_BN_CLICKED(IDC_BUTTON1, &CMFCTestView::OnClickedButton1)
	ON_COMMAND(ID_FILE_SAVE, &CMFCTestView::OnFileSave)
	ON_BN_CLICKED(IDC_BUTTON2, &CMFCTestView::OnBnClickedButton2)
	ON_MESSAGE(WM_SEEKUR_RET, &CMFCTestView::OnSeekur)
	ON_BN_CLICKED(IDC_BUTTON3, &CMFCTestView::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, &CMFCTestView::OnBtnPathCreate)
END_MESSAGE_MAP()


BEGIN_EVENTSINK_MAP(CMFCTestView, CFormView)
	//{{AFX_EVENTSINK_MAP(CFieldNetView_Map)
	ON_EVENT(CMFCTestView, IDC_MAPCONTROL1, 1 /* OnMouseDown */, OnMouseDownMapcontrol1, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8)
	//ON_EVENT(CMFCTestView, IDC_MAPCONTROL1, 4 /* OnDoubleClick */, OnDoubleClickMapcontrol1, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()


// CMFCTestView 构造/析构

CMFCTestView::CMFCTestView()
: CFormView(CMFCTestView::IDD)
, m_velocity(0)
{
	// TODO:  在此处添加构造代码

}

CMFCTestView::~CMFCTestView()
{
	PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_END, 0, 0);
	//PostThreadMessage(track_thread->m_nThreadID, WM_TRACK_STOP, 0, 0);
}

void CMFCTestView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT6, m_editHeading);
	DDX_Control(pDX, IDC_EDIT5, m_editDistance);
	DDX_Control(pDX, IDC_EDIT2, m_editLongitude);
	DDX_Control(pDX, IDC_EDIT1, m_editLatitude);
	DDX_Control(pDX, IDC_CHECK1, m_cPointLine);
	DDX_Control(pDX, IDC_EDIT4, m_editSeekurVel);
	DDX_Control(pDX, IDC_EDIT3, m_editSeekurHeading);
	DDX_Control(pDX, IDC_EDIT7, m_editKDis);
	DDX_Control(pDX, IDC_EDIT8, m_editKSubHead);
	DDX_Control(pDX, IDC_BUTTON3, m_btnTrack);
	DDX_Control(pDX, IDC_EDIT10, m_editBJ54_x);
	DDX_Control(pDX, IDC_EDIT9, m_editBJ54_y);
	DDX_Control(pDX, IDC_CHECK2, m_checkShowGPS);
	DDX_Control(pDX, IDC_EDIT11, m_editVelocity);
	DDX_Control(pDX, IDC_BUTTON4, m_btnPathSelect);
	DDX_Control(pDX, IDC_EDIT12, m_editSubHeading);
	DDX_Control(pDX, IDC_EDIT13, m_editDis);
}

BOOL CMFCTestView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO:  在此处通过修改
	//  CREATESTRUCT cs 来修改窗口类或样式

	return CFormView::PreCreateWindow(cs);
}

void CMFCTestView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

	//初始化控件
	//MapControl控件
	CWnd* pWndCal = GetDlgItem(IDC_MAPCONTROL1);
	LPUNKNOWN pUnk = pWndCal->GetControlUnknown();
	pUnk->QueryInterface(IID_IMapControl2, (LPVOID *)&m_ipMapControl);

	CWnd* pWndCal2 = GetDlgItem(IDC_TOCCONTROL1);
	LPUNKNOWN pUnk2 = pWndCal2->GetControlUnknown();
	pUnk2->QueryInterface(IID_ITOCControl2, (LPVOID *)&m_ipTocControl);
	m_ipTocControl->SetBuddyControl(m_ipMapControl);

	//创建控制线程
	//myEvent = ::CreateEvent(
	//	NULL,    // 事件的属性，
	//	FALSE,    // 自动
	//	FALSE,    // 初始化，没有信号的
	//	_T("e_seekur")    // 对象名字
	//	);
	seekur_thread = AfxBeginThread(
		SeekurFuc,
		this->GetSafeHwnd()   // 传递窗体的句柄
		);

	//调用串口程序，打开与GPS的通信
	CMainFrame *p = (CMainFrame*)AfxGetMainWnd();
	CMFCTestView *m_CView = (CMFCTestView *)p->GetActiveView();//获取View类的指针
	if (serialPort.InitPort(m_CView, 6))
	{
		serialPort.StartMonitoring();
	}
	else
	{
		MessageBox(_T("GPS串口通信失败！"), _T("提示"), MB_OK);
	}



	m_editKDis.SetWindowTextW(_T("25"));
	m_editKSubHead.SetWindowTextW(_T("0.5"));
	m_editVelocity.SetWindowTextW(_T("500"));
	m_velocity = 500;
	isTracked = false;
}
//
//HRESULT CMFCTestView::CreateShapeFile(esriGeometryType type, CString layerPath, CString layerName, IFeatureClass** ppFeatureClass)
//{
//	if (!ppFeatureClass)
//		return E_POINTER;
//
//	IWorkspaceFactoryPtr ipWKSPFac(CLSID_ShapefileWorkspaceFactory);
//	IWorkspacePtr ipWKSP;
//	HRESULT hr = ipWKSPFac->OpenFromFile((CComBSTR)layerPath, 0, &ipWKSP);
//	if (FAILED(hr) || ipWKSP == 0)
//	{
//		AfxMessageBox(_T("无法打开目标文件夹！"), MB_ICONINFORMATION);
//		return hr;
//	}
//	IFeatureWorkspacePtr ipFeatureWKSP(ipWKSP);
//
//	// Set up a simple fields collection
//	IFieldsPtr ipFields(CLSID_Fields);
//	IFieldsEditPtr ipFieldsEdit(ipFields);
//
//	// Make and add the shape field. It will need a geometry definition and a spatial reference
//	IFieldPtr ipField(CLSID_Field);
//	IFieldEditPtr ipFieldEdit(ipField);
//	hr = ipFieldEdit->put_Name(CComBSTR(L"Shape"));
//	hr = ipFieldEdit->put_Type(esriFieldTypeGeometry);
//
//	IGeometryDefPtr ipGeomDef(CLSID_GeometryDef);
//	IGeometryDefEditPtr ipGeomDefEdit(ipGeomDef);
//	hr = ipGeomDefEdit->put_GeometryType(type);
//	ISpatialReferencePtr ipUnkSpatial(CLSID_UnknownCoordinateSystem);
//	hr = ipGeomDefEdit->putref_SpatialReference(ipUnkSpatial);
//	ipFieldEdit->putref_GeometryDef(ipGeomDef);
//	ipFieldsEdit->AddField(ipField);
//
//	// Add another miscellanesous text field
//	IFieldPtr ipX(CLSID_Field);
//	IFieldPtr ipY(CLSID_Field);
//	IFieldPtr ipValue(CLSID_Field);
//	long len = 30;
//	ipFieldEdit = ipX;
//	ipFieldEdit->put_Length(len);
//	ipFieldEdit->put_Name(CComBSTR(L"X"));
//	ipFieldEdit->put_Type(esriFieldTypeDouble);
//	ipFieldsEdit->AddField(ipX);
//
//	ipFieldEdit = ipY;
//	ipFieldEdit->put_Length(len);
//	ipFieldEdit->put_Name(CComBSTR(L"Y"));
//	ipFieldEdit->put_Type(esriFieldTypeDouble);
//	ipFieldsEdit->AddField(ipY);
//	ipFieldEdit = ipValue;
//
//	ipFieldEdit->put_Length(len);
//	ipFieldEdit->put_Name(CComBSTR(L"Value"));
//	ipFieldEdit->put_Type(esriFieldTypeDouble);
//	ipFieldsEdit->AddField(ipValue);
//
//	// Create the shapefile (some parameters apply to geodatabase options and can be defaulted as NULL)
//	IFeatureClassPtr ipFeatClass;
//	hr = ipFeatureWKSP->CreateFeatureClass(CComBSTR(layerName), ipFields,
//		0, 0,
//		esriFTSimple,
//		CComBSTR(L"Shape"),
//		CComBSTR(L""),
//		&ipFeatClass);
//	*ppFeatureClass = ipFeatClass;
//	if (*ppFeatureClass)
//		(*ppFeatureClass)->AddRef();
//	else
//		return E_FAIL;
//
//	return hr;
//
//}

// CMFCTestView 诊断

#ifdef _DEBUG
void CMFCTestView::AssertValid() const
{
	CFormView::AssertValid();
}

void CMFCTestView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CMFCTestDoc* CMFCTestView::GetDocument() const // 非调试版本是内联的
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CMFCTestDoc)));
	return (CMFCTestDoc*)m_pDocument;
}
#endif //_DEBUG


// CMFCTestView 消息处理程序


int CMFCTestView::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFormView::OnCreate(lpCreateStruct) == -1)
		return -1;

	// TODO:  在此添加您专用的创建代码

	//初始化许可
	::CoInitialize(NULL);
	ArcGISVersionLib::IArcGISVersionPtr ipVer(__uuidof(ArcGISVersionLib::VersionManager));
	VARIANT_BOOL succeeded;
	if (FAILED(ipVer->LoadVersion(ArcGISVersionLib::esriArcGISEngine, L"10.2", &succeeded))) //10.2 版本 
		return -1;
	IAoInitializePtr m_AoInit;//(CLSID_AoInitialize); 
	m_AoInit.CreateInstance(CLSID_AoInitialize);
	esriLicenseStatus ls;
	HRESULT h = m_AoInit->Initialize(esriLicenseProductCode::esriLicenseProductCodeEngineGeoDB, &ls);



	return 0;
}


#pragma region 成员函数
void CMFCTestView::OnFileOpen()
{
	// TODO:  在此添加命令处理程序代码
	//char sfileter[]="Shape files(*.Shp)|*.Shp|mxd文档(*.mxd)|*.mxd|所有文件(*.*)";  
	CFileDialog dlg(TRUE, //TRUE为OPEN对话框，FALSE为SAVE AS对话框  
		NULL,
		NULL,
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,
		(LPCTSTR)_TEXT("Shape files(*.shp)|*.shp|mxd文档(*.mxd)|*.mxd|"),
		NULL);
	CString m_strFileName;
	if (dlg.DoModal() == IDOK)
	{
		//m_MapControl=new CMapControl2();  


		m_strFileName = dlg.GetPathName();//全路径名  
		//CString filepath=dlg.GetFolderPath();//路径名称，不带文件名  
		//CString filename=dlg.GetFileName();//文件名，不带路径  
		CString strExt = dlg.GetFileExt();//后缀名，不带点  
		if (strExt == "shp")
		{
			CComBSTR MX_DATAFILE;
			//MX_DATAFILE = "F:\\测试mxd文档\\shenzhen.mxd" ;  
			MX_DATAFILE = dlg.GetPathName();
			BSTR filePath = dlg.GetFolderPath().AllocSysString();
			BSTR fileName = dlg.GetFileName().AllocSysString();
			m_ipMapControl->AddShapeFile(filePath, fileName);
		}
		else if (strExt == "mxd")
		{
			CComBSTR MX_DATAFILE;
			//MX_DATAFILE = "F:\\测试mxd文档\\shenzhen.mxd" ;  
			MX_DATAFILE = dlg.GetPathName();
			VARIANT_BOOL bValidDoc;
			//m_MapControlView.CheckMxFile( MX_DATAFILE );  
			m_ipMapControl->CheckMxFile(MX_DATAFILE, &bValidDoc);
			//VARIANT vt = 0;  
			if (bValidDoc)
				m_ipMapControl->LoadMxFile(MX_DATAFILE);
		}
		else
		{
			AfxMessageBox(_T("请选择合适的文件!"));
			return;
		}
		m_ipMapControl->Refresh(esriViewAll);
	}
}


//void CMFCTestView::OnSize(UINT nType, int cx, int cy)
//{
//	CFormView::OnSize(nType, cx, cy);
//
//	// TODO:  在此处添加消息处理程序代码
//	CWnd *pMapCtl = GetDlgItem(IDC_MAPCONTROL1);
//	//CRect MapCtlrect;  
//	if (pMapCtl)
//	{
//		//pMapCtl->GetWindowRect( &MapCtlrect );  
//		pMapCtl->MoveWindow(0, 0, cx, cy);
//	}
//}

/*
描述：GPS串口通信函数
参数：
@ch：串口传递的单个字符
@ch：端口号
返回值：0
*/
afx_msg LRESULT CMFCTestView::OnCommunication(WPARAM ch, LPARAM portnum)
{
	//接收单个字符
	if (ch != 13 && ch != 10)
	{
		gpsStr += (char)ch;
	}
	//接收到完整一句时解析字符串
	else if (gpsStr != "")
	{
		//判断是否需要释放GPS坐标
		/*while (gpsInfos.size()>0)
		{
		GPSInfo *tmp;
		gpsInfos.pop_back(tmp);
		delete tmp;
		}*/
		CString x;
		x.Format(_T("%s"), gpsStr);
		gpsInfo = gpsTran.Tanslate(gpsStr);

		//判断是不是GPS经纬度坐标
		if (gpsInfo->InfoType == GGA)
		{
			/*m_editLongitude = (CEdit *)GetDlgItem(IDC_EDIT1);
			m_editLatitude = (CEdit *)GetDlgItem(IDC_EDIT2);*/
			//将经纬度坐标从度分格式转化为度格式
			double longtitude = ((GGAInfo*)gpsInfo)->Longitude;
			double latitude = ((GGAInfo*)gpsInfo)->Latitude;

			CString x;
			x.Format(_T("%lf"), longtitude);
			CString y;
			y.Format(_T("%lf"), latitude);
			m_editLongitude.SetWindowText(x);
			m_editLatitude.SetWindowTextW(y);

			myGPSInfo.Longitude = longtitude;
			myGPSInfo.Latitude = latitude;

			IPointPtr point1;
			HRESULT hr1 = point1.CreateInstance(CLSID_Point);
			point1->PutCoords(myGPSInfo.Longitude, myGPSInfo.Latitude);
			//point1->PutCoords(114.068188, 22.531326);
			//point1->PutCoords(12698012.6530, 2575437.9373);

			x.Format(_T("%d"), ((GGAInfo*)gpsInfo)->UseSatelliteNum);
			//CString y;
			y.Format(_T("%d"), ((GGAInfo*)gpsInfo)->GPSStatus);
			m_editBJ54_x.SetWindowText(x);
			m_editBJ54_y.SetWindowText(y);

			if (m_checkShowGPS.GetCheck()){
				//point1 = geoToProj(point1);
				//double bj_x, bj_y;
				//point1->get_X(&bj_x);
				//point1->get_Y(&bj_y);
				//myGPSInfo.BJ54_X = bj_x;
				//myGPSInfo.BJ54_Y = bj_y;
				////CString x;
				//x.Format(_T("%lf"), bj_x);
				////CString y;
				//y.Format(_T("%lf"), bj_y);
				//m_editBJ54_x.SetWindowText(x);
				//m_editBJ54_y.SetWindowText(y);

				/*ISpatialReferenceFactory2Ptr ipSpaRefFact2(CLSID_SpatialReferenceEnvironment);
				IGeographicCoordinateSystemPtr ipGeoCoordSys;
				ipSpaRefFact2->CreateGeographicCoordinateSystem(esriSRGeoCS_Beijing1954, &ipGeoCoordSys);

				ISpatialReferencePtr ipSRef;
				ipSRef = ipGeoCoordSys;


				IPointPtr point2(CLSID_Point);
				point1->putref_SpatialReference(ipSRef);
				point2->PutCoords(bj_x, bj_y);*/


			}
			//刷新GPS位置
			IActiveViewPtr iActiveView;
			m_ipMapControl->get_ActiveView(&iActiveView);
			/*IPointPtr ipoint(CLSID_Point);
			if (ipoint == NULL)
			MessageBox(_T("点创建错误"));
			ipoint->PutCoords(myGPSInfo.Longitude*100, myGPSInfo.Latitude*100);*/

			//画点
			//IGeometryPtr pGeometry(ipoint);
			IGeometryPtr pgeomln(point1);
			IGraphicsContainerPtr pgracont(iActiveView);
			//pgracont->DeleteAllElements();

			IMarkerElementPtr pmarkerelem(CLSID_MarkerElement);//创建element对象，是element
			if (pmarkerelem == NULL)
				MessageBox(_T("画图元素创建错误"));
			//IMarkerSymbolPtr imarkerSymbol(m_isymbol);//用m_isymbol初始化imarkerSymbol，是symbol
			//pmarkerelem->put_Symbol(imarkerSymbol);//将symbol添加到element
			((IElementPtr)pmarkerelem)->put_Geometry(pgeomln);
			if (lastPointElement != NULL){
				pgracont->DeleteElement(lastPointElement);
				//lastPointElement.Release();
			}
			lastPointElement = pmarkerelem;
			pgracont->AddElement((IElementPtr)pmarkerelem, 0);
			iActiveView->Refresh();
		}

		if (gpsInfo->InfoType == PSAT_HPR)
		{
			CString heading;
			heading.Format(_T("%lf"), ((PSAT_HPRInfo*)gpsInfo)->Heading);
			m_editSeekurHeading.SetWindowText(heading);
			myGPSInfo.Heading = ((PSAT_HPRInfo*)gpsInfo)->Heading;
		}

		if (gpsInfo->InfoType == VTG)
		{
			/*CString vel;
			vel.Format(_T("%lf"), ((VTGInfo*)gpsInfo)->TrueHeading);
			m_editSeekurVel.SetWindowText(vel);
			myGPSInfo.SpeedKm = ((VTGInfo*)gpsInfo)->TrueHeading;*/

			//CString heading;
			//heading.Format(_T("%lf"), ((VTGInfo*)gpsInfo)->TrueHeading);
			//m_editSeekurHeading.SetWindowText(heading);
			//myGPSInfo.Heading = ((VTGInfo*)gpsInfo)->TrueHeading;
		}
		delete gpsInfo;

		gpsStr = "";
	}

	return 0;
}

UINT CMFCTestView::SeekurFuc(LPVOID lParam){
	//获取exe执行文件路径
	HMODULE module = GetModuleHandle(0);
	TCHAR exeFullPath[MAX_PATH]; // MAX_PATH
	GetModuleFileName(NULL, exeFullPath, MAX_PATH);
	CString fileName(exeFullPath);
	int argc = 3;
	char rp[MAX_PATH];
#if defined(UNICODE) 
	WideCharToMultiByte(CP_ACP, 0, exeFullPath, -1, rp, wcslen(exeFullPath), 0, 0);
	rp[wcslen(exeFullPath)] = 0;
#else 
	lstrcpy((PTSTR)rp, (PTSTR)pFileName);
#endif 
	char *argv[3];
	/*string path1 = rp;*/
	argv[0] = rp;
	argv[1] = "-rp";
	argv[2] = "com3";

	Aria::init();
	ArArgumentParser argParser(&argc, argv);
	argParser.loadDefaultArguments();
	ArRobot* robot = new ArRobot();
	ArRobotConnector con(&argParser, robot);

	//解析来自命令行的参数
	if (!Aria::parseArgs()){
		Aria::logOptions();
	}
	//连接机器人
	if (!con.connectRobot()){
		ArLog::log(ArLog::Normal, "无法连接机器人");
		if (argParser.checkHelpAndWarnUnparsed()){
			ArLog::log(ArLog::Terse, "无法连接机器人");
		}
	}

	//robot->com2Bytes(116, 6, 1);
	//robot->com2Bytes(116, 7, 1);
	//robot->com2Bytes(116, 28, 1);
	//robot->com2Bytes(116, 12, 1);


	// 异步运行机器人处理循环
	robot->enableMotors();
	robot->runAsync(true);
	BaseAction action(robot);

	MSG msg;
	while (true)
	{
		GetMessage(&msg, NULL, 0, 0);
		if (msg.message == WM_SEEKUR_MOVE)
		{
			seekurParaPtr pPara = (seekurParaPtr)msg.wParam;
			//直线移动距离
			if (pPara->distance != 0)
				action.Move(pPara->distance);
			//转动角度
			action.SetDeltaHeading(pPara->heading);
			//直线移动距离
			if (pPara->veloctiy != 0)
				action.SetVelocity(pPara->veloctiy);
			delete pPara;
		}

		if (msg.message == WM_SEEKUR_STOP)
		{
			action.Stop();
		}
		if (msg.message == WM_SEEKUR_END)
		{
			action.Stop();
			break;
		}
		//::PostMessage((HWND)lParam,WM_SEEKUR_RET, 0, 0);//发出自定义消息
	}
	//robot->comInt(ArCommands::ENABLE, 1);
	robot->disconnect();
	Aria::exit();

	return 0;
}

UINT CMFCTestView::TrackFuc(LPVOID lParam){

	MSG msg;
	//bool isStart = false;
	bool isFirst = true;	//是否为刚开始追踪
	GetMessage(&msg, NULL, 0, 0);
	CMFCTestView* pView = (CMFCTestView*)msg.wParam;

	::CoInitialize(NULL);
	//获取路径内所有直线
	IPolylinePtr pPolyline;
	pPolyline = pView->m_trackPath;
	ISegmentCollectionPtr pSegCol = (ISegmentCollectionPtr)pPolyline;	//直线集合
	long num;
	pSegCol->get_SegmentCount(&num);
	IPointPtr pStartPoint(CLSID_Point);		//当前路径起始点
	IPointPtr pEndPoint(CLSID_Point);		//当前路径结束点
	IPolylinePtr pNowPath(CLSID_Polyline);	//当前路径
	int nowPathPos = -1;		//当前路径在直线集合中的位置
	double lineHeading;		//路径航向
	//pNowPath = pPolyline;

	while (pView->isTracked){
		//获取最近点
		IPointPtr pNearestPoint(CLSID_Point);	//最近点
		IPointPtr pSeekurPoint(CLSID_Point);		//Seekur所在点
		IGeometryPtr pGeo;
		pView->lastPointElement->get_Geometry(&pGeo);
		pSeekurPoint = (IPointPtr)pGeo;
		double x;
		pSeekurPoint->get_X(&x);
		double y;
		pSeekurPoint->get_Y(&y);
		//pSeekurPoint->PutCoords(12943887.463977, 4857805.079523);

		double distAlongCurveFrom = 0; //曲线其实点到输出点部分的长度
		double distFromCurve = 0;//输入点到曲线的最短距离
		VARIANT_BOOL isRightSide = false;//输入点是否在曲线的右边
		pPolyline->QueryPointAndDistance(esriNoExtension, pSeekurPoint, false, pNearestPoint, &distAlongCurveFrom, &distFromCurve, &isRightSide);

		//判断最近点在Seekur的左边还是右边，获取
		double nearestX, nearestY, seekurX, seekurY;
		pNearestPoint->get_X(&nearestX);
		pNearestPoint->get_Y(&nearestY);
		pSeekurPoint->get_X(&seekurX);
		pSeekurPoint->get_Y(&seekurY);

		//计算最近点是否在当前路径
		ITopologicalOperatorPtr topo = (ITopologicalOperatorPtr)pNearestPoint;
		/*IGeometryPtr buffer;
		topo->Buffer(0.001, &buffer);
		topo = (ITopologicalOperatorPtr)buffer;*/
		IGeometryPtr pTpGeo;
		HRESULT hr = topo->Intersect((IGeometryPtr)pNowPath, esriGeometry0Dimension, &pTpGeo);
		if (FAILED(hr))
		{
			pView->MessageBox(_T("交点计算错误"));
		}
		VARIANT_BOOL isEmpty;	//交点是否为空
		pTpGeo->get_IsEmpty(&isEmpty);
		//先判断是否在当前路径上，不是则重新计算现有路径
		if (isEmpty){
			for (long i = nowPathPos + 1; i < num; i++)
			{
				ISegmentPtr pSeg;	//当前所在路径
				//ISegmentPtr pSeg(CLSID_Line);	//当前所在路径
				pSegCol->get_Segment(i, &pSeg);

				//构造polyline路径
				IPolylinePtr ptpPath(CLSID_Polyline);
				ISegmentCollectionPtr ptpSegCol = (ISegmentCollectionPtr)ptpPath;
				ptpSegCol->AddSegment(pSeg);


				//计算最近点是否在当前路径
				hr = topo->Intersect((IGeometryPtr)ptpPath, esriGeometry0Dimension, &pTpGeo);
				if (FAILED(hr))
				{
					pView->MessageBox(_T("交点计算错误"));
				}
				pTpGeo->get_IsEmpty(&isEmpty);
				if (!isEmpty)
				{
					pNowPath = ptpPath;
					pSeg->get_FromPoint(&pStartPoint);
					pSeg->get_ToPoint(&pEndPoint);
					//计算路径航向
					double startX, startY;
					pStartPoint->get_X(&startX);
					pStartPoint->get_Y(&startY);

					double endX, endY;
					pEndPoint->get_X(&endX);
					pEndPoint->get_Y(&endY);

					lineHeading = atan2(endY - startY, endX - startX) * 180 / PI;
					if (lineHeading > 0)
					{
						lineHeading = 90 - lineHeading;
					}
					else
					{
						lineHeading = -lineHeading;
					}
					nowPathPos = i;
					break;
				}
			}
		}

		CString cKDis;
		pView->m_editKDis.GetWindowTextW(cKDis);
		CString cKSubHead;
		pView->m_editKSubHead.GetWindowTextW(cKSubHead);
		double kDis = _ttof(cKDis);
		double kSubHead = _ttof(cKSubHead);

		distFromCurve *= 100000;
		double dis = GetDistance(seekurY, seekurX, nearestY, nearestX) * 1000;	//Seekur到路径距离（米）
		double subHeading = lineHeading - pView->myGPSInfo.Heading;	//路径航向与Seekur航向差值

		double turnHeading = 0;	//转向角
		double maxTrun = 45;	//最大转向角
		if (dis > 2){
			maxTrun = 45;
		}
		else if (dis > 1){
			maxTrun = 30;
		}
		else if (dis>0.5)
		{
			maxTrun = 20;
		}
		else
		{
			maxTrun = 10;
		}


		/*if (turnHeading > maxTrun){
			int dfsds = 23;
		}*/
		//判断机器车在路径右侧还是左侧
		if (isRightSide)
		{
			//dis += 0.1;
			turnHeading = kDis*dis - kSubHead*subHeading;
			//调整转角，使转角不得令转向后的航向差值大于最大转向角
			if ((subHeading + turnHeading) >= maxTrun)
			{
				turnHeading = maxTrun - subHeading;
			}
			//防止在右侧时外偏
			if ((subHeading + turnHeading) <= -maxTrun)
			{
				turnHeading = -maxTrun - subHeading;
			}

			////在右侧时航向差值大于等于最大转向角时不再转角
			//if (subHeading >= maxTrun)
			//{
			//	turnHeading= 0;
			//}
			/*if (dis <= diffDis&&subHeading >= (maxTrun / 2))
			{
			turnHeading = turnHeading/ - 2;
			}*/
		}
		else
		{
			dis = -dis;
			//dis += 0.1;
			turnHeading = kDis*dis + kSubHead*subHeading;
			//调整转角，使转角不得令转向后的航向差值大于最大转向角
			if ((subHeading + turnHeading) <= -maxTrun)
			{
				turnHeading = -maxTrun - subHeading;
			}


			//防止在右侧时外偏
			if ((subHeading + turnHeading) >= maxTrun)
			{
				turnHeading = maxTrun - subHeading;
			}
			////在左侧时航向差值小于等于负的最大转向角时不再转角
			//if (subHeading <= -maxTrun)
			//{
			//	turnHeading = 0;
			//}
			//if (dis <= diffDis&&subHeading <=( -maxTrun / 2))
			//{
			//	turnHeading = turnHeading /- 2;
			//}
			//turnHeading = -turnHeading;
		}

		CString str;
		str.Format(_T("%lf"), subHeading);
		pView->m_editSubHeading.SetWindowText(str);
		str.Format(_T("%lf"), dis);
		pView->m_editDis.SetWindowText(str);
		CString vel;
		vel.Format(_T("%lf"), turnHeading);
		pView->m_editSeekurVel.SetWindowText(vel);

		seekurParaPtr pPara = new seekurPara();
		pPara->distance = 0;
		pPara->heading = turnHeading;

		if (isFirst)
		{
			CString cVel;
			pView->m_editVelocity.GetWindowTextW(cVel);
			double vel = _ttof(cVel);
			pPara->veloctiy = pView->m_velocity;
			isFirst = false;
		}

		PostThreadMessage(pView->seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, 0);

		//::PostMessage((HWND)lParam, WM_SEEKUR_RET, 0, 0);//发出自定义消息
		Sleep(20);
	}
	PostThreadMessage(pView->seekur_thread->m_nThreadID, WM_SEEKUR_STOP, 0, 0);
	return 0;
}
#pragma endregion

void CMFCTestView::OnClickedButton1()
{
	/*CString x;
	x.Format(_T("%lf"), myGPSInfo.Longitude++);
	CString y;
	y.Format(_T("%lf"), myGPSInfo.Latitude);
	m_editLongitude.SetWindowText(x);
	m_editLatitude.SetWindowText(y);*/


	//CString distance;
	//m_editDistance.GetWindowTextW(distance);
	//CString heading;
	//m_editHeading.GetWindowTextW(heading);
	//double dis = _ttof(distance);
	//double head = _ttof(heading);

	//IPointPtr point1;
	//HRESULT hr1 = point1.CreateInstance(CLSID_Point);
	//point1->PutCoords(dis, head);
	////point1->PutCoords(114.068188, 22.531326);
	////point1->PutCoords(12698012.6530, 2575437.9373);

	//point1 = geoToProj(point1);
	//double x, y;
	//point1->get_X(&x);
	//point1->get_Y(&y);
	//CString str;
	//str.Format(_T("X=%lf,Y=%lf"), x, y);
	//MessageBox(str);


	//分解文件路径和名称
	CString strFolder = _T("D:\\arcMap");
	CString strFile = _T("path.shp");

	//为输出SHP文件创建特征工厂
	IWorkspaceFactoryPtr ipWorkFact(CLSID_ShapefileWorkspaceFactory);
	IWorkspacePtr ipWork;
	HRESULT hr = ipWorkFact->OpenFromFile(CComBSTR(strFolder), 0, &ipWork);
	if (FAILED(hr) || ipWork == 0)
	{
		AfxMessageBox(_T("无法打开目标文件夹！"), MB_ICONINFORMATION);
		/*if (FAILED(hr))
		return hr;
		else
		return E_FAIL;*/
	}
	IFeatureWorkspacePtr ipFeatWork(ipWork);

	// Set up fields
	IFieldsPtr ipFields(CLSID_Fields);
	IFieldsEditPtr ipFieldsEdit;
	ipFieldsEdit = ipFields; //QI
	// Geometry and spatial reference
	IFieldPtr ipField(CLSID_Field);
	IFieldEditPtr ipFieldEdit;
	ipFieldEdit = ipField; //QI
	ipFieldEdit->put_Name(CComBSTR("Shape"));
	ipFieldEdit->put_Type(esriFieldTypeGeometry);

	//创建空间数据类型
	IGeometryDefPtr ipGeoDef(CLSID_GeometryDef);
	IGeometryDefEditPtr ipGeoDefEdit;
	ipGeoDefEdit = ipGeoDef; //QI
	ipGeoDefEdit->put_GeometryType(esriGeometryPolyline);

	//创建坐标系
	ISpatialReferenceFactory2Ptr ipSpaRefFact2(CLSID_SpatialReferenceEnvironment);
	IGeographicCoordinateSystemPtr ipGeoCoordSys;
	ipSpaRefFact2->CreateGeographicCoordinateSystem(esriSRGeoCS_WGS1984, &ipGeoCoordSys);

	ISpatialReferencePtr ipSRef;
	ipSRef = ipGeoCoordSys;
	ipGeoDefEdit->putref_SpatialReference(ipSRef);
	ipFieldEdit->putref_GeometryDef(ipGeoDef);
	ipFieldsEdit->AddField(ipField);

	// Create the shapefile
	IFeatureClassPtr pFeatureClass;
	hr = ipFeatWork->CreateFeatureClass(CComBSTR(strFile), ipFields, 0, 0,
		esriFTSimple, CComBSTR("Shape"), 0, &pFeatureClass);

	IFeaturePtr pFeature;
	pFeatureClass->CreateFeature(&pFeature);



	pFeature->putref_Shape(pPath);
	pFeature->Store();

	CString str(_T("路径起始点"));
	m_btnPathSelect.SetWindowTextW(str);
	pPath->Release();
}

void CMFCTestView::OnMouseDownMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY)
{
	//IActiveViewPtr iActiveView(m_ipMapControl);

	//获取当前活动页面及鼠标所在点
	IActiveViewPtr iActiveView;
	m_ipMapControl->get_ActiveView(&iActiveView);
	IPointPtr ipoint(CLSID_Point);

	if (ipoint == NULL) return;
	ipoint->PutCoords(mapX, mapY);
	m_cPointLine.SetCheck(1);

	if (m_cPointLine.GetCheck())
	{
		//画点
		//IGeometryPtr pGeometry(ipoint);
		AddCreateElement((IGeometryPtr)ipoint, iActiveView);
		iActiveView->Refresh();
	}
	else{
		//画线
		HRESULT hr;
		if (m_pNewLineFeedback == NULL)
		{
			CoCreateInstance(CLSID_NewLineFeedback,
				NULL, CLSCTX_ALL, IID_INewLineFeedback,
				(void**)&m_pNewLineFeedback);
			ISymbolPtr isymbol;
			if (m_pNewLineFeedback == NULL) return;
			m_pNewLineFeedback->get_Symbol(&isymbol);
			ISimpleLineSymbolPtr pslnsym(isymbol);
			if (pslnsym == NULL) return;
			IRgbColorPtr prgb(CLSID_RgbColor);
			prgb->put_Red(0);
			prgb->put_Green(205);
			prgb->put_Blue(0);
			IColorPtr icolor(prgb);

			pslnsym->put_Color(icolor);
			pslnsym->put_Style(esriSLSDot);

			IScreenDisplayPtr pScrD;
			iActiveView->get_ScreenDisplay(&pScrD);

			m_pNewLineFeedback->putref_Display(pScrD);
			m_pNewLineFeedback->Start(ipoint);
		}
		else
			hr = m_pNewLineFeedback->AddPoint(ipoint);
	}



}

void CMFCTestView::OnTestMarkerStyle()
{
	ISymbolSelectorPtr pSymbolSelector(CLSID_SymbolSelector);//symbol select
	ISimpleMarkerSymbolPtr psimpleMarksb(CLSID_SimpleMarkerSymbol);//simple marker
	VARIANT_BOOL bOK;
	if (pSymbolSelector == NULL) return;
	pSymbolSelector->AddSymbol((ISymbolPtr)psimpleMarksb, &bOK);//将simple marker添加到symbol select中
	HRESULT hr;
	hr = pSymbolSelector->SelectSymbol(0, &bOK);
	if (FAILED(hr)) return;
	if (bOK)
		pSymbolSelector->GetSymbolAt(0, &m_isymbol);

}

void CMFCTestView::AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview)
{
	IGraphicsContainerPtr pgracont(iactiveview);
	//pgracont->DeleteAllElements();

	IMarkerElementPtr pmarkerelem(CLSID_MarkerElement);//创建element对象，是element
	if (pmarkerelem == NULL) return;
	IMarkerSymbolPtr imarkerSymbol(m_isymbol);//用m_isymbol初始化imarkerSymbol，是symbol
	pmarkerelem->put_Symbol(imarkerSymbol);//将symbol添加到element
	((IElementPtr)pmarkerelem)->put_Geometry(pgeomln);
	if (lastPointElement != NULL){
		pgracont->DeleteElement(lastPointElement);
	}
	lastPointElement = pmarkerelem;
	pgracont->AddElement((IElementPtr)pmarkerelem, 0);
}


IPoint* CMFCTestView::geoToProj(IPoint* point/*需要更改坐标系的点*/, long fromProjType, long toGeoType)
{
	long geoType = toGeoType;//4326;
	IPoint* points = point;
	ISpatialReference* spatialRf;
	ISpatialReference* spatialRf1;
	IGeographicCoordinateSystem* geograpicsys;
	IProjectedCoordinateSystem*projCoordSystem;
	ISpatialReferenceFactoryPtr originalSpecialReference;
	ISpatialReferenceFactoryPtr newReferenceSystem;

	HRESULT hr = originalSpecialReference.CreateInstance(CLSID_SpatialReferenceEnvironment);

	HRESULT hr1 = originalSpecialReference->CreateProjectedCoordinateSystem(fromProjType, &projCoordSystem);
	spatialRf = (ISpatialReference*)projCoordSystem;
	//HRESULT hr2 = points->putref_SpatialReference(spatialRf);


	newReferenceSystem.CreateInstance(CLSID_SpatialReferenceEnvironment);
	newReferenceSystem->CreateGeographicCoordinateSystem(geoType, &geograpicsys);
	spatialRf1 = (ISpatialReference*)geograpicsys;
	points->putref_SpatialReference(spatialRf1);//设置原始空间参考

	points->Project(spatialRf);
	return points;
};

void CMFCTestView::OnDoubleClickMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY)
{
	// TODO: 在此处添加消息处理程序代码
	IActiveViewPtr iActiveView;
	m_ipMapControl->get_ActiveView(&iActiveView);
	IPolylinePtr ipolyline;
	if (m_pNewLineFeedback == NULL) return;
	if (m_isymbol != NULL) m_pNewLineFeedback->putref_Symbol(m_isymbol);
	m_pNewLineFeedback->Stop(&ipolyline);

	//m_pgeometry = ipolyline;
	if (ipolyline != NULL)
	{
		//AddCreateElement(m_pgeometry, iActiveView);


		IGraphicsContainerPtr pgracont(iActiveView);
		ILineElementPtr pLineElement(CLSID_LineElement);
		//pLineElement->put_Symbol(m_isymbol);

		IElementPtr pElement = (IElementPtr)pLineElement;
		pElement->put_Geometry(ipolyline);
		pgracont->AddElement((IElementPtr)pElement, 0);
	}
	m_pNewLineFeedback = NULL;
	iActiveView->Refresh();

}

void CMFCTestView::OnMouseMoveMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY)
{
	//MapComposer mapComposer;
	/*mapComposer.m_ipMap = m_ipMapControl->get_Map();
	mapComposer.ShowMouseCoord(mapX, mapY);
	IPointPtr pPoint(CLSID_Point);
	pPoint->PutCoords(mapX, mapY);*/
	//switch (m_operateType)
	//{
	//case 1:
	//	m_ctrlMap.put_MousePointer(2);//esriPointerCrosshair == 2
	//	if (m_pNewLineFeedback != NULL)
	//		m_pNewLineFeedback->MoveTo(pPoint);
	//	break;
	//}
	//// TODO: 在此处添加消息处理程序代码
}


void CMFCTestView::OnFileSave()
{
	// TODO:  在此添加命令处理程序代码
	CFileDialog dlg(FALSE, //TRUE为OPEN对话框，FALSE为SAVE AS对话框  
		_T("shp"),
		_T("newFile"),
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,
		(LPCTSTR)_TEXT("Shape files(*.shp)|*.shp|mxd文档(*.mxd)|*.mxd|"),
		NULL);
	CString m_strFileName;
	if (dlg.DoModal() == IDOK)
	{
		//m_MapControl=new CMapControl2();  


		m_strFileName = dlg.GetPathName();//全路径名  
		//CString filepath=dlg.GetFolderPath();//路径名称，不带文件名  
		//CString filename=dlg.GetFileName();//文件名，不带路径  
		CString strExt = dlg.GetFileExt();//后缀名，不带点  
		if (strExt == "shp")
		{
			CComBSTR MX_DATAFILE;
			MX_DATAFILE = dlg.GetPathName();
			BSTR filePath = dlg.GetFolderPath().AllocSysString();
			BSTR fileName = dlg.GetFileName().AllocSysString();
			m_ipMapControl->AddShapeFile(filePath, fileName);
		}
		else if (strExt == "mxd")
		{
			CComBSTR MX_DATAFILE;
			MX_DATAFILE = dlg.GetPathName();
			VARIANT_BOOL bValidDoc;
			//m_MapControlView.CheckMxFile( MX_DATAFILE );  
			m_ipMapControl->CheckMxFile(MX_DATAFILE, &bValidDoc);
			//VARIANT vt = 0;  
			if (bValidDoc)
				m_ipMapControl->LoadMxFile(MX_DATAFILE);
		}
		else
		{
			AfxMessageBox(_T("请选择合适的文件!"));
			return;
		}
		m_ipMapControl->Refresh(esriViewAll);
	}
}

//测试运动按钮
void CMFCTestView::OnBnClickedButton2()
{
	// TODO:  在此添加控件通知处理程序代码
	//获取要移动的距离
	CString distance;
	m_editDistance.GetWindowTextW(distance);
	CString heading;
	m_editHeading.GetWindowTextW(heading);
	double dis = _ttof(distance);
	double head = _ttof(heading);
	seekurParaPtr pPara = new seekurPara();
	pPara->distance = dis;
	pPara->heading = head;

	PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, NULL);
	//PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, dis, head);

}


afx_msg LRESULT CMFCTestView::OnSeekur(WPARAM wParam, LPARAM lParam)
{
	//MessageBox(_T("测试通信"));
	/*m_editLongitude = (CEdit *)GetDlgItem(IDC_EDIT1);
	m_editLongitude->SetWindowTextW(_T("测试消息"));*/
	return 0;
}

//测试追踪
void CMFCTestView::OnBnClickedButton3()
{
	//获取路径图层
	ILayerPtr pLayer;
	IFeatureLayerPtr pFeatLayer;
	long num;
	m_ipMapControl->get_LayerCount(&num);
	//BSTR pathName = ::SysAllocString(L"Path");	//路径所在图层名称
	CString pathName = _T("path");
	for (long i = 0; i < num; i++)
	{
		BSTR name;
		m_ipMapControl->get_Layer(i, &pLayer);
		pLayer->get_Name(&name);
		CString cName(name);
		if (!pathName.CompareNoCase(cName))
		{
			pFeatLayer = pLayer;
			break;
		}
	}
	if (!pFeatLayer){
		MessageBox(_T("无路径！"));
	}
	else{
		//获取路径
		IFeatureClassPtr pFeatCla;
		pFeatLayer->get_FeatureClass(&pFeatCla);
		IFeaturePtr pFeature;
		pFeatCla->GetFeature(0, &pFeature);
		IGeometryPtr pGeometry;
		pFeature->get_Shape(&pGeometry);
		m_trackPath = pGeometry;


		if (!isTracked)
		{
			//启动追踪线程
			isTracked = true;
			track_thread = AfxBeginThread(
				TrackFuc,
				this->GetSafeHwnd()   // 传递窗体的句柄
				);
			CMainFrame*   pFrame = (CMainFrame*)AfxGetMainWnd();
			CMFCTestView *pView = (CMFCTestView *)pFrame->GetActiveView();//获取View类的指针
			PostThreadMessage(track_thread->m_nThreadID, WM_TRACK_START, (UINT)pView, NULL);

			m_btnTrack.SetWindowTextW(_T("停止追踪"));
		}
		else{
			//PostThreadMessage(track_thread->m_nThreadID, WM_TRACK_STOP, 0, 0);
			isTracked = false;
			m_btnTrack.SetWindowTextW(_T("开始追踪"));
		}
	}
	//// TODO:  在此添加控件通知处理程序代码
	//IFeatureLayer pFeatLayer = m_ipMapControl->get_Layer(0) as IFeatureLayer;
	//IFeatureClass pFeatCla = pFeatLayer.FeatureClass;

	//IPoint pnt = new PointClass();
	//pnt.X = 113.301;
	//pnt.Y = 23.113;
	//GISTool.CreateTimePoint(pFeatCla, pnt, 100.1);

	//axMapControl1.ActiveView.Refresh();

	/*ILayerPtr pLayer;
	IFeatureLayerPtr pFeatLayer;
	m_ipMapControl->get_Layer(0, &pLayer);
	pFeatLayer = pLayer;
	IFeatureClassPtr pFeatCla;
	pFeatLayer->get_FeatureClass(&pFeatCla);
	IPointPtr ipoint(CLSID_Point);
	ipoint->PutCoords(290, 480);*/


}

void CreateShapeFile(){
	//分解文件路径和名称
	CString strFolder = _T("F:\\arcMap");
	CString strFile = _T("path.shp");

	//为输出SHP文件创建特征工厂
	IWorkspaceFactoryPtr ipWorkFact(CLSID_ShapefileWorkspaceFactory);
	IWorkspacePtr ipWork;
	HRESULT hr = ipWorkFact->OpenFromFile(CComBSTR(strFolder), 0, &ipWork);
	if (FAILED(hr) || ipWork == 0)
	{
		AfxMessageBox(_T("无法打开目标文件夹！"), MB_ICONINFORMATION);
		/*if (FAILED(hr))
		return hr;
		else
		return E_FAIL;*/
	}
	IFeatureWorkspacePtr ipFeatWork(ipWork);

	// Set up fields
	IFieldsPtr ipFields(CLSID_Fields);
	IFieldsEditPtr ipFieldsEdit;
	ipFieldsEdit = ipFields; //QI
	// Geometry and spatial reference
	IFieldPtr ipField(CLSID_Field);
	IFieldEditPtr ipFieldEdit;
	ipFieldEdit = ipField; //QI
	ipFieldEdit->put_Name(CComBSTR("Shape"));
	ipFieldEdit->put_Type(esriFieldTypeGeometry);

	//创建空间数据类型
	IGeometryDefPtr ipGeoDef(CLSID_GeometryDef);
	IGeometryDefEditPtr ipGeoDefEdit;
	ipGeoDefEdit = ipGeoDef; //QI
	ipGeoDefEdit->put_GeometryType(esriGeometryPolyline);

	//创建坐标系
	ISpatialReferenceFactory2Ptr ipSpaRefFact2(CLSID_SpatialReferenceEnvironment);
	IGeographicCoordinateSystemPtr ipGeoCoordSys;
	ipSpaRefFact2->CreateGeographicCoordinateSystem(esriSRGeoCS_Beijing1954, &ipGeoCoordSys);

	ISpatialReferencePtr ipSRef;
	ipSRef = ipGeoCoordSys;
	ipGeoDefEdit->putref_SpatialReference(ipSRef);
	ipFieldEdit->putref_GeometryDef(ipGeoDef);
	ipFieldsEdit->AddField(ipField);

	// Create the shapefile
	IFeatureClassPtr pFeatureClass;
	hr = ipFeatWork->CreateFeatureClass(CComBSTR(strFile), ipFields, 0, 0,
		esriFTSimple, CComBSTR("Shape"), 0, &pFeatureClass);

	IFeaturePtr pFeature;
	pFeatureClass->CreateFeature(&pFeature);

	IPolylinePtr pPolyline(CLSID_Polyline);
	IPointCollectionPtr pPtclo = (IPointCollectionPtr)pPolyline;

	IPointPtr point(CLSID_Point);
	point->PutCoords(12943884.706671, 4857804.266050);
	pPtclo->AddPoint(point);

	point = new IPointPtr(CLSID_Point);
	point->PutCoords(12943885.463977, 4857805.079523);
	pPtclo->AddPoint(point);


	pFeature->putref_Shape(pPolyline);
	pFeature->Store();


#pragma region 废弃创建方式
	////创建图层
	//IFeatureClassPtr pFeatClass;
	//IFeatureLayerPtr pFeatLayer(CLSID_FeatureLayer);
	//ISpatialReferencePtr ipSpatialReference;

	////设置坐标系并定义几何类型
	//ISpatialReferenceFactoryPtr spatialReferenceFactory;
	//HRESULT hr = spatialReferenceFactory.CreateInstance(CLSID_SpatialReferenceEnvironment);

	//IGeographicCoordinateSystem* pGCS;
	//pGCS = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

	//HRESULT hr = ipSpatialReference.CreateInstance(CLSID_UnknownCoordinateSystem);
	//if (FAILED(hr)) return;
	//hr = m_ipMapControl->get_SpatialReference(&ipSpatialReference);
	//if (FAILED(hr))
	//	return;
	//hr = pFeatLayer->putref_SpatialReference(ipSpatialReference);
	//if (FAILED(hr))
	//	return;
	//hr = pFeatLayer->put_Name((CComBSTR)(L"SeekurGPS"));
	//if (FAILED(hr))
	//	return;
	//


	//m_currentLayer = pFeatLayer;
	//m_ipMapControl->get_Map(&m_map);
	////获取编辑的要素类
	//IDatasetPtr pDataset;
	//pDataset = pFeatClass;
	//IWorkspaceEditPtr pWorkSpaceEdit;
	//IWorkspacePtr pWorkspace;
	//pDataset->get_Workspace(&pWorkspace);
	//pWorkSpaceEdit = pWorkspace;
	//VARIANT_BOOL isStart;
	//pWorkSpaceEdit->IsBeingEdited(&isStart);
	//if (!isStart)
	//{
	//	pWorkSpaceEdit->StartEditing(true);
	//}

	//IFeaturePtr pFeature;
	//pFeatClass->CreateFeature(&pFeature);
	//IPointPtr point(CLSID_Point);
	//point->PutCoords(452499, 4450252);
	//pFeature->putref_Shape(point);
	//pFeature->Store();
	////m_ipTocControl->Update();
	//pWorkSpaceEdit->StopEditing(isStart);

	//hr = pFeatLayer->putref_FeatureClass(pFeatClass);
	//if (FAILED(hr))
	//	return;

	//hr = m_ipMapControl->AddLayer(pFeatLayer, 1);
	//if (FAILED(hr))
	//	return;

	//IActiveViewPtr iActiveView;
	//m_ipMapControl->get_ActiveView(&iActiveView);
	//IPointPtr ipoint(CLSID_Point);

	////点画法
	//IGeometryPtr pGeometry;
	//pFeature->get_Shape(&pGeometry);
	//AddCreateElement(pGeometry, iActiveView);
	//iActiveView->Refresh();
	//
	////INewMultiPointFeedbackPtr pMultiPointFeed;
	////CoCreateInstance(CLSID_NewMultiPointFeedback,
	////	NULL, CLSCTX_ALL, IID_INewMultiPointFeedback,
	////	(void**)&pMultiPointFeed);
	////IActiveViewPtr pActiveView;
	////pActiveView = m_map;

	////IPointPtr point(CLSID_Point);
	////m_ipMapControl->ToMapPoint(290, 680, &point);
	////CreateFeature(point);
#pragma endregion
}



void CMFCTestView::OnBtnPathCreate()
{
	// TODO:  在此添加控件通知处理程序代码
	//路径为空时创建路径
	if (!pPath)
	{
		HRESULT hr1 = pPath.CreateInstance(CLSID_Polyline);
		CString str(_T("下一点"));
		m_btnPathSelect.SetWindowTextW(str);
	}

	IPointCollectionPtr pPtclo = (IPointCollectionPtr)pPath;
	IPointPtr point(CLSID_Point);
	point->PutCoords(myGPSInfo.Longitude, myGPSInfo.Latitude);
	pPtclo->AddPoint(point);
}
