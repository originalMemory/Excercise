
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

#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define wm

// CMFCTestView

IMPLEMENT_DYNCREATE(CMFCTestView, CFormView)

BEGIN_MESSAGE_MAP(CMFCTestView, CFormView)
	ON_WM_CREATE()
	ON_COMMAND(ID_FILE_OPEN, &CMFCTestView::OnFileOpen)
//	ON_WM_SIZE()
ON_MESSAGE(WM_COMM_RXCHAR, &CMFCTestView::OnCommunication)
ON_BN_CLICKED(IDC_BUTTON1, &CMFCTestView::OnClickedButton1)
ON_COMMAND(ID_FILE_SAVE, &CMFCTestView::OnFileSave)
END_MESSAGE_MAP()


BEGIN_EVENTSINK_MAP(CMFCTestView, CFormView)
	//{{AFX_EVENTSINK_MAP(CFieldNetView_Map)
	ON_EVENT(CMFCTestView, IDC_MAPCONTROL1, 1 /* OnMouseDown */, OnMouseDownMapcontrol1, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8)
	ON_EVENT(CMFCTestView, IDC_MAPCONTROL1, 4 /* OnDoubleClick */, OnDoubleClickMapcontrol1, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_R8 VTS_R8)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()


// CMFCTestView 构造/析构

CMFCTestView::CMFCTestView()
	: CFormView(CMFCTestView::IDD)
{
	// TODO:  在此处添加构造代码

}

CMFCTestView::~CMFCTestView()
{
}

void CMFCTestView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
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
	CWnd* pWndCal=GetDlgItem(IDC_MAPCONTROL1);
	LPUNKNOWN pUnk=pWndCal->GetControlUnknown();
	pUnk->QueryInterface(IID_IMapControl2,(LPVOID *)&m_ipMapControl);

	/*CWnd* pWndCal2 = GetDlgItem(IDC_TOCCONTROL1);
	LPUNKNOWN pUnk2 = pWndCal2->GetControlUnknown();
	pUnk2->QueryInterface(IID_ITOCControl2, (LPVOID *)&m_ipTocControl);
	m_ipTocControl->SetBuddyControl(m_ipMapControl);*/

	m_editLongitude = (CEdit *)GetDlgItem(IDC_EDIT1);
	m_editLatitude = (CEdit *)GetDlgItem(IDC_EDIT2);
	m_editLatitude->SetWindowTextW(_T("sdfefsfe"));
	m_editLongitude->SetWindowTextW(_T("afefsdfsd"));
	testBool = false;

	//调用串口程序，打开与GPS的通信
	CMainFrame *p = (CMainFrame*)AfxGetMainWnd();
	CMFCTestView *m_CView = (CMFCTestView *)p->GetActiveView();//获取View类的指针
	if (serialPort.InitPort(m_CView, 2))
	{
		serialPort.StartMonitoring();
	}
	else
	{
		MessageBox(_T("GPS串口通信失败！"), _T("提示"), MB_OK);
	}
}

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
		if (gpsInfos.size()>0)
		{
			for each (GPSInfo* info in gpsInfos)
			{
				delete info;
			}
			gpsInfos.clear();
		}
		gpsInfos = gpsTran.Tanslate(gpsStr);

		for each (GPSInfo* info in gpsInfos)
		{
			//判断是不是GPS经纬度坐标
			if (info->InfoType == GGA)
			{
				m_editLongitude = (CEdit *)GetDlgItem(IDC_EDIT1);
				m_editLatitude = (CEdit *)GetDlgItem(IDC_EDIT2);
				CString x;
				x.Format(_T("%lf"), ((GGAInfo*)info)->Longitude);
				CString y;
				y.Format(_T("%lf"), ((GGAInfo*)info)->Latitude);
				m_editLongitude->SetWindowText(x);
				m_editLatitude->SetWindowText(y);

				myGPSInfo.Longitude = ((GGAInfo*)info)->Longitude;
				myGPSInfo.Latitude = ((GGAInfo*)info)->Latitude;
			}

			//判断是不是GPS经纬度坐标
			if (info->InfoType == PSAT_HPR)
			{
				myGPSInfo.Heading= ((PSAT_HPRInfo*)info)->Heading;
			}
		}


		//NmeaStr.SetAt(0, str);//将接收的语句作为元素存入字符串数组,但是数组大小始终保持为1
		//ParseNmea();

		//m_map.SetExtent(m_map.GetFullExtent());
		//DisplayCurrentPosition();
		//str = "";
	}

	return 0;
}



void CMFCTestView::OnClickedButton1()
{
	IPointPtr point1;
	HRESULT hr1 = point1.CreateInstance(CLSID_Point);
	point1->PutCoords(114.068188,22.531326);
	//point1->PutCoords(12698012.6530, 2575437.9373);

	point1 = geoToProj(point1);
	double x, y;
	point1->get_X(&x);
	point1->get_Y(&y);
	CString str;
	str.Format(_T("X=%lf,Y=%lf"), x, y);
	MessageBox(str);
	
}

void CMFCTestView::OnMouseDownMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY)
{
	//IActiveViewPtr iActiveView(m_ipMapControl);
	IActiveViewPtr iActiveView;
	m_ipMapControl->get_ActiveView(&iActiveView);
	IPointPtr ipoint(CLSID_Point);
	if (ipoint == NULL) return;
	ipoint->PutCoords(mapX, mapY);
	/*switch (operateStyle)
	{
	case 1:
	{*/
	////点画法
	//IGeometryPtr pGeometry(ipoint);
	//AddCreateElement(pGeometry, iActiveView);
	//iActiveView->Refresh();
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
	/*}
		break;
	}*/
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


	IMarkerElementPtr pmarkerelem(CLSID_MarkerElement);//创建element对象，是element
	if (pmarkerelem == NULL) return;
	IMarkerSymbolPtr imarkerSymbol(m_isymbol);//用m_isymbol初始化imarkerSymbol，是symbol
	pmarkerelem->put_Symbol(imarkerSymbol);//将symbol添加到element
	((IElementPtr)pmarkerelem)->put_Geometry(pgeomln);
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
	points->putref_SpatialReference(spatialRf1);//这句不能要，是设置原始 空间参考的。

	points->Project(spatialRf);
	////测试输出而已////////////////////////
	//double x,y;
	//points->get_X(&x);
	//points->get_Y(&y);
	//printf("x=%lf,y=%lf\n",x,y);
	///////////////////////////////////////
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
			  //m_pNewLineFeedback->Stop(&ipolyline);
			  
			  //m_pgeometry = ipolyline;
			 /* if (ipolyline != NULL)
				  AddCreateElement(m_pgeometry, iActiveView);*/
			  /*m_pNewLineFeedback = NULL;
			  iActiveView->Refresh();*/

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
