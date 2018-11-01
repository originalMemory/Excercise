
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
#include "ObstacleAvoid.h"
#include <time.h>

#define WM_SEEKUR_RET WM_USER+100		 //Seekur返回消息
#define WM_SEEKUR_MOVE WM_USER+101		//Seekur操作消息（开始）
#define WM_SEEKUR_END WM_USER+102		//Seekur操作消息（结束）
#define WM_SEEKUR_STOP WM_USER+103		//Seekur操作消息（停止）
#define WM_SEEKUR_GET_DATA WM_USER+104		//Seekur返回当前相对位置
#define WM_SEEKUR_GET_SPEED WM_USER+105		//Seekur返回当前相对速度
#define WM_SEEKUR_GET_SPEED_LEFT WM_USER+106		//Seekur返回当前左边相对速度
#define WM_SEEKUR_GET_SPEED_RIGHT WM_USER+107		//Seekur返回当前右边相对速度



#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define wm

#define PI 3.14159265358979323846

const string LASER_HEADER[7] = { "02", "80", "6E", "01", "B0", "B5", "00" };
//const char LASER_HEADER[7] = { 0x02, 0x80, 0x6E, 0x01, 0xB0, 0xB5, 0x00 };

int seekurNo = 1;
CStdioFile seekurRunFile;
MyGPSInfo myGPSInfo;		//我实际使用的GPS信息
SeekurData seekurData;


unsigned long HextoDec(const unsigned char *hex, int length)
{
	int i;
	unsigned long rslt = 0;
	for (i = 0; i < length; i++)
	{
		rslt += (unsigned long)(hex[i]) << (8 * (length - 1 - i));

	}
	return rslt;
}

int HexToDem(string str)
{
	int dem = 0;
	for (int i = 0; i < str.size(); i++)
	{
		dem = dem * 16;
		if ((str[i] <= '9') && (str[i] >= '0'))        //0~9之间的字符
			dem += str[i] - '0';
		else if ((str[i] <= 'F') && (str[i] >= 'A'))   //A~F之间的字符
			dem += str[i] - 'A' + 10;
		else if ((str[i] <= 'f') && (str[i] >= 'a'))   //a~f之间的字符
			dem += str[i] - 'a' + 10;
		else
			return -1;                          //出错时返回-1
	}
	return dem;
}

bool isPointOnLine(IPointPtr pPoint, ILinePtr pPath)
{
	IPointPtr pStartPoint, pEndPoint;
	pPath->get_FromPoint(&pStartPoint);
	pPath->get_ToPoint(&pEndPoint);
	double x, y, startX, startY, endX, endY;
	pPoint->get_X(&x);
	pPoint->get_Y(&y);
	pStartPoint->get_X(&startX);
	pStartPoint->get_Y(&startY);
	pEndPoint->get_X(&endX);
	pEndPoint->get_Y(&endY);
	//(P-P1)*(P2-P1)=0 向量差乘是否为0; 
	double X21, Y21, X10, Y10;
	X21 = endX - startX;
	Y21 = endY - startY;
	X10 = x - startX;
	Y10 = y - startY;

	//向量乘积为0则该点在线上
	double vectorValue = X21 * Y10 - X10 * Y21;
	//if (vectorValue != 0.0)
	if (abs(vectorValue) <= 0.01)
		return true;
	else
	{
		double xMin = min(startX, endX);
		double xMax = max(endX, startX);
		double yMin = min(startY, endY);
		double yMax = max(endY, startY);

		//判断点是否是在该线的延长线上
		if (xMin <= x && x <= xMax && yMin <= y && y <= yMax)
			return true;
		else
			return false;
	}
}

/// <summary>
/// 经纬度转投影坐标
/// </summary>
/// <param name="point">经纬度点</param>
/// <param name="prjType">投影坐标系编号，默认为(2414)esriSRProjCS_Beijing1954_3_Degree_GK_Zone_38</param>
/// <param name="gcsType">经纬为坐标系编号，默认为(4326)esriSRGeoCS_WGS1984</param>
/// <returns>投影坐标点</returns>
IPoint* geoToProj(IPoint* point, long prjType = 2414, long gcsType = 4326)
{
	//构建gcs空间参考关系
	ISpatialReferenceFactoryPtr pgscSRF(CLSID_SpatialReferenceEnvironment);
	IGeographicCoordinateSystemPtr geograpicsys;
	pgscSRF->CreateGeographicCoordinateSystem(gcsType, &geograpicsys);
	ISpatialReferencePtr pgscSR = (ISpatialReferencePtr)geograpicsys;
	
	//构建prj空间参考关系
	point->putref_SpatialReference(pgscSR);
	IProjectedCoordinateSystemPtr prjcoodSys;
	pgscSRF->CreateProjectedCoordinateSystem(prjType, &prjcoodSys);
	ISpatialReferencePtr pprjSR = (ISpatialReferencePtr)prjcoodSys;

	//坐标转换
	point->Project(pprjSR);
	return point;
};

// CMFCTestView

IMPLEMENT_DYNCREATE(CMFCTestView, CFormView)

BEGIN_MESSAGE_MAP(CMFCTestView, CFormView)
	ON_WM_CREATE()
	ON_COMMAND(ID_FILE_OPEN, &CMFCTestView::OnFileOpen)
	//	ON_WM_SIZE()
	ON_MESSAGE(WM_COMM_RXCHAR, &CMFCTestView::OnSerialPortCallback)
	ON_BN_CLICKED(IDC_BUTTON1, &CMFCTestView::OnBtnSavePath)
	ON_COMMAND(ID_FILE_SAVE, &CMFCTestView::OnFileSave)
	ON_BN_CLICKED(IDC_BUTTON2, &CMFCTestView::OnBtnMoveSeekur)
	ON_MESSAGE(WM_SEEKUR_RET, &CMFCTestView::OnSeekurCallback)
	ON_BN_CLICKED(IDC_BUTTON3, &CMFCTestView::OnBtnTrack)
	ON_BN_CLICKED(IDC_BUTTON4, &CMFCTestView::OnBtnPathAdd)
	ON_BN_CLICKED(IDC_BUTTON5, &CMFCTestView::OnBtnSeekurQuery)
	ON_BN_CLICKED(IDC_BUTTON6, &CMFCTestView::OnBtnSeekurStop)
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
, m_tranVel(0)
{
	// TODO:  在此处添加构造代码

}

CMFCTestView::~CMFCTestView()
{
	PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_END, 0, 0);
	Sleep(100);
	//seekurRunFile.Close();
	//dataFile.Close();
	//PostThreadMessage(track_thread->m_nThreadID, WM_TRACK_STOP, 0, 0);
}

void CMFCTestView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT6, m_editMoveHeading);
	DDX_Control(pDX, IDC_EDIT5, m_editMoveDistance);
	DDX_Control(pDX, IDC_EDIT2, m_editLongitude);
	DDX_Control(pDX, IDC_EDIT1, m_editLatitude);
	DDX_Control(pDX, IDC_EDIT4, m_editTurnHeading);
	DDX_Control(pDX, IDC_EDIT3, m_editGPSHeading);
	DDX_Control(pDX, IDC_EDIT7, m_editKdis);
	DDX_Control(pDX, IDC_EDIT8, m_editKheading);
	DDX_Control(pDX, IDC_BUTTON3, m_btnTrack);
	DDX_Control(pDX, IDC_EDIT9, m_editGPSStaus);
	DDX_Control(pDX, IDC_CHECK2, m_checkShowBJ54);
	DDX_Control(pDX, IDC_EDIT11, m_editTranVel);
	DDX_Control(pDX, IDC_BUTTON4, m_btnPathSelect);
	DDX_Control(pDX, IDC_EDIT12, m_editSubHeading);
	DDX_Control(pDX, IDC_EDIT13, m_editDis);
	DDX_Control(pDX, IDC_EDIT17, m_editBJ54_x);
	DDX_Control(pDX, IDC_EDIT16, m_editBJ54_y);
	DDX_Control(pDX, IDC_EDIT18, m_editSeekurPose);
	DDX_Control(pDX, IDC_EDIT19, m_editSeekurVal);
	DDX_Control(pDX, IDC_EDIT20, m_editSeekurAccel);
	DDX_Control(pDX, IDC_EDIT21, m_editGPSSpeedKm);
	DDX_Control(pDX, IDC_EDIT22, m_editRotVel);
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

	m_editKdis.SetWindowTextW(_T("0.025"));
	m_editKheading.SetWindowTextW(_T("0.5"));
	m_checkShowBJ54.SetCheck(1);
	m_editTranVel.SetWindowTextW(_T("500"));
	m_editRotVel.SetWindowTextW(_T("1"));
	isTracked = false;
	isGPSEnd = false;
	isPathExist = false;
	isAvoid = false;
	pathLayerName = _T("path");
	isGPSEnd = false;

	////创建Seekur状态文件
	//time_t timet = time(NULL);
	//tm *tm_ = localtime(&timet);
	//CString name;
	//name.Format(_T("D:\\data\\SeekurTrack %4d-%02d-%02d %02d：%02d：%02d.csv"), tm_->tm_year + 1900, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
	//seekurRunFile.Open(name, CFile::modeWrite | CFile::modeNoTruncate | CFile::modeCreate);
	////写入表头
	//seekurRunFile.WriteString(_T("no,time,x,y,heading,tranVel,tranLeft,tranRight,subTranVel,rotVel\n"));

	//调用串口程序，打开与GPS的通信
	m_GPSport = 6;
	CMainFrame *p = (CMainFrame*)AfxGetMainWnd();
	CMFCTestView *m_CView = (CMFCTestView *)p->GetActiveView();//获取View类的指针
	if (serialPort_gps.InitPort(m_CView, m_GPSport))
	{
		serialPort_gps.StartMonitoring();
	}
	else
	{
		MessageBox(_T("GPS串口通信失败！"), _T("提示"), MB_OK);
	}
	laserBufPos = 0;
	laserCheckBufPos = 0;
	laserStr = "";
	//LASER_HEADER = { 0x02, 0x80, 0x6E, 0x01, 0xB0, 0xB5, 0x00 };
	/*LASER_HEADER[0] = '0x02';
	LASER_HEADER[1] = 0x80;
	LASER_HEADER[2] = 0x6E;
	LASER_HEADER[3] = 0x01;
	LASER_HEADER[4] = 0xB0;
	LASER_HEADER[5] = 0xB5;
	LASER_HEADER[6] = 0x00;*/
	gpsStr = "";
	testStr = "";
	m_laserport = 8;
	if (serialPort_laser.InitPort(m_CView, m_laserport, 9600))
	{
		serialPort_laser.StartMonitoring();
	}
	else
	{
		MessageBox(_T("激光串口通信失败！"), _T("提示"), MB_OK);
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

			//若加载的是路径图层，则获取该图层，并计算路径起始航向
			CString cLayerName = dlg.GetFileTitle();
			if (!pathLayerName.CompareNoCase(cLayerName))
			{
				isPathExist = true;
				//获取路径图层
				ILayerPtr pLayer;
				long num;
				m_ipMapControl->get_LayerCount(&num);
				for (long i = 0; i < num; i++)
				{
					BSTR name;
					m_ipMapControl->get_Layer(i, &pLayer);
					pLayer->get_Name(&name);
					//CString cName(name);
					if (!pathLayerName.CompareNoCase(name))
					{
						//获取路径
						IFeatureLayerPtr pFeatLayer = pLayer;
						IFeatureClassPtr pFeatCla;
						pFeatLayer->get_FeatureClass(&pFeatCla);
						IFeaturePtr pFeature;
						pFeatCla->GetFeature(0, &pFeature);
						IGeometryPtr pGeometry;
						pFeature->get_Shape(&pGeometry);
						m_trackPath = pGeometry;

						//计算起始路径和航向
						ISegmentCollectionPtr pSegCol = (ISegmentCollectionPtr)m_trackPath;	//直线集合
						pSegCol->get_SegmentCount(&segNum);
						ISegmentPtr pSeg;	//当前所在路径
						pSegCol->get_Segment(0, &pSeg);
						pNowPath = pSeg;
						double angle;	//直线与X轴夹角，以弧度为单位
						pNowPath->get_Angle(&angle);
						lineHeading = angle * 180 / PI;
						//因为当前角度是以四象限X为起点的逆时针角度，对应的是地图东部
						//故需要转成以北为起点的顺时针角度
						if (lineHeading <= 90)
						{
							//1、3、4象限
							lineHeading = 90 - lineHeading;
						}
						else
						{
							//2象限
							lineHeading = 360 + (90 - lineHeading);
						}
						break;
					}
				}
				CString cTp;	//临时Cstring类，用于将文本框数据转为其他格式
				m_editKdis.GetWindowTextW(cTp);
				kdis = _ttof(cTp);
				m_editKheading.GetWindowTextW(cTp);
				kheading = _ttof(cTp);
			}
			
			
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


afx_msg LRESULT CMFCTestView::OnSeekurCallback(WPARAM wParam, LPARAM lParam)
{
	SeekurDataPtr pData = (SeekurDataPtr)wParam;
	CString cTp;
	cTp.Format(_T("%.3lf,%.3lf,%.3lf"), pData->x, pData->y, pData->heading);
	m_editSeekurPose.SetWindowTextW(cTp);
	cTp.Format(_T("%.2lf,%.2lf,%.2lf"), pData->vel, pData->rotVel, pData->leftVel);
	m_editSeekurVal.SetWindowTextW(cTp);
	delete pData;
	return 0;
}

/*
描述：GPS串口通信函数
参数：
@ch：串口传递的单个字符
@portnum：端口号
返回值：0
*/
afx_msg LRESULT CMFCTestView::OnSerialPortCallback(WPARAM ch, LPARAM portnum)
{
	//GPS数据
	if (portnum == m_GPSport){
		//接收单个字符
		if (ch != 13 && ch != 10)
		{
			gpsStr += (char)ch;
		}
		//接收到完整一句时解析字符串
		else if (gpsStr != "")
		{
			OnGPSAnalysis(gpsStr);
			gpsStr = "";
		}
	}

	//激光数据
	if (portnum == m_laserport){
		char a = 0x00;
		char b = '0x80';
		unsigned char cha = (unsigned char)ch;
		int i;
		char c16 = ch & 0xFF;
		if (c16==a)
		{
			int dsdf = 324;
		}
		char ctest[3] = { 0 };
		sprintf(ctest, "%02X", cha);
		string cstr = ctest;
		//sscanf(&cha, "%x", ctest);
		//CString str;
		//str.Format(_T("%02X"), cha);//将接收到的BYTE型数据转换为对应的十六进制  
		////char szTemp[4];
		////wchar_t *wChar = str.GetBuffer(str.GetLength());
		////str.ReleaseBuffer();
		////// 将得到的wchar* 类型转为 char*类型
		////size_t len = wcslen(wChar) + 1;
		////size_t converted = 0;
		////char *cChar;
		////cChar = (char*)malloc(len*sizeof(char));
		////wcstombs_s(&converted, cChar, len, wChar, _TRUNCATE);
		//RecText.Append(str);
		//校验是否为激光数据的起始
		if (cstr == "80"){
			int dfe = 214;
		}
		if (LASER_HEADER[laserCheckBufPos] == cstr)
		{
			if (laserCheckBufPos>0)
			{
				int ef = 234;
			}
			laserCheckBuf[laserCheckBufPos] = cstr;
			if (laserCheckBufPos == 6)
			{
				laserDataLength = HexToDem(laserCheckBuf[6] + laserCheckBuf[5]);//两个8位（高位低位）合成16位包长
				isLaserStart = true;
				//当一组语句全部传输完后，对期进行解析
				if (laserBufPos == laserDataLength*2)
				{
					OnLaserAnalysis(laserBuf, laserDataLength);
					laserBufPos = 0;
					laserStr = "";
				}
				return 0;
			}
			laserCheckBufPos++;
		}
		else
		{
			laserCheckBufPos = 0;
			laserCheckBuf[0] = '\0';
		}

		//在是同一句时往缓冲中写入数据
		if (isLaserStart&&laserBufPos<laserDataLength*2)
		{
			laserBuf[laserBufPos] = cstr;
			laserStr += cstr + " ";
			laserBufPos++;
		}
		
	}

	return 0;
}



/*
描述：GPS串口数据解析
参数：
@spData：GPS语句
返回值：
*/
void CMFCTestView::OnGPSAnalysis(string spData)
{
	CString cTp;
	GPSInfo* gpsInfo = gpsTran.Tanslate(spData);

	//判断是不是GPS经纬度坐标
	if (gpsInfo->InfoType == GGA)
	{
		double longtitude = ((GGAInfo*)gpsInfo)->Longitude;
		double latitude = ((GGAInfo*)gpsInfo)->Latitude;
		//刷新窗口显示
		cTp.Format(_T("%lf"), longtitude);
		m_editLongitude.SetWindowText(cTp);
		cTp.Format(_T("%lf"), latitude);
		m_editLatitude.SetWindowTextW(cTp);

		myGPSInfo.Longitude = longtitude;
		myGPSInfo.Latitude = latitude;

		// 建立坐标点
		IPointPtr pShowPoint;
		IPointPtr pGpsPoint;
		HRESULT hr1 = pGpsPoint.CreateInstance(CLSID_Point);
		pGpsPoint->PutCoords(myGPSInfo.Longitude, myGPSInfo.Latitude);

		// 刷新卫星状态
		cTp.Format(_T("%d"), ((GGAInfo*)gpsInfo)->GPSStatus);
		m_editGPSStaus.SetWindowText(cTp);

		// 判断是否要转换为北京54坐标
		if (m_checkShowBJ54.GetCheck()){
			IPointPtr pBJ54Point = geoToProj(pGpsPoint);
			double x, y;
			pGpsPoint->get_X(&x);
			pGpsPoint->get_Y(&y);
			double bj_x, bj_y;
			pBJ54Point->get_X(&bj_x);
			pBJ54Point->get_Y(&bj_y);
			myGPSInfo.BJ54_X = bj_x;
			myGPSInfo.BJ54_Y = bj_y;
			cTp.Format(_T("%lf"), bj_x);
			m_editBJ54_x.SetWindowText(cTp);
			cTp.Format(_T("%lf"), bj_y);
			m_editBJ54_y.SetWindowText(cTp);

			pShowPoint = pBJ54Point;
		}
		else
		{
			pShowPoint = pGpsPoint;
		}
		double tp_x, tp_y;
		pShowPoint->get_X(&tp_x);
		pShowPoint->get_Y(&tp_y);
		//刷新GPS位置
		IActiveViewPtr iActiveView;
		m_ipMapControl->get_ActiveView(&iActiveView);
		pSeekurPoint = pShowPoint;
		//画点
		IGeometryPtr pgeomln(pShowPoint);
		IGraphicsContainerPtr pgracont(iActiveView);

		IMarkerElementPtr pmarkerelem(CLSID_MarkerElement);//创建element对象
		if (pmarkerelem == NULL)
			MessageBox(_T("画图元素创建错误"));
		((IElementPtr)pmarkerelem)->put_Geometry(pgeomln);
		if (lastPointElement != NULL){
			pgracont->DeleteElement(lastPointElement);
		}
		lastPointElement = pmarkerelem;
		pgracont->AddElement((IElementPtr)pmarkerelem, 0);
		iActiveView->Refresh();
		isGPSEnd = false;
		//myGPSInfo.Heading = 80;
	}
	else if (gpsInfo->InfoType == VTG)
	{
		CString speed;
		speed.Format(_T("%lf"), ((VTGInfo*)gpsInfo)->SpeedKm);
		m_editGPSSpeedKm.SetWindowText(speed);
		myGPSInfo.SpeedKm = ((VTGInfo*)gpsInfo)->SpeedKm;
	}
	else if (gpsInfo->InfoType == PSAT_HPR)
	{
		CString heading;
		heading.Format(_T("%lf"), ((PSAT_HPRInfo*)gpsInfo)->Heading);
		m_editGPSHeading.SetWindowText(heading);
		myGPSInfo.Heading = ((PSAT_HPRInfo*)gpsInfo)->Heading;
		//修正误差
		myGPSInfo.Heading += -0.785817526;
		isGPSEnd = true;
	}

	//在路径已加载且更新了一组GPS数据时计算状态
	if (isPathExist&&isGPSEnd)
	{
		//计算当前距离差和航向差
		//::CoInitialize(NULL);

		//获取最近点
		IPointPtr pNearestPoint(CLSID_Point);	//最近点
		double x;
		pSeekurPoint->get_X(&x);
		double y;
		pSeekurPoint->get_Y(&y);

		double distAlongCurveFrom = 0; //曲线其实点到输出点部分的长度
		double distFromCurve = 0;//输入点到曲线的最短距离
		VARIANT_BOOL isRightSide = false;//输入点是否在曲线的右边
		m_trackPath->QueryPointAndDistance(esriNoExtension, pSeekurPoint, false, pNearestPoint, &distAlongCurveFrom, &distFromCurve, &isRightSide);

		//判断最近点在Seekur的左边还是右边，获取
		double nearestX, nearestY, seekurX, seekurY;
		pNearestPoint->get_X(&nearestX);
		pNearestPoint->get_Y(&nearestY);
		pSeekurPoint->get_X(&seekurX);
		pSeekurPoint->get_Y(&seekurY);

		//计算最近点是否在当前路径
#pragma region 废弃代码，曾用于判断GPS坐是否在线上
		//ITopologicalOperatorPtr topo = (ITopologicalOperatorPtr)pNearestPoint;
		///*IGeometryPtr buffer;
		//topo->Buffer(1, &buffer);
		//topo = (ITopologicalOperatorPtr)buffer;*/
		//IGeometryPtr pTpGeo;
		//HRESULT hr = topo->Intersect((IGeometryPtr)pNowPath, esriGeometry0Dimension, &pTpGeo);
#pragma endregion
		bool isOnLine = isPointOnLine(pNearestPoint, pNowPath);	//是否在路径上
		if (!isOnLine)
		{
			MessageBox(_T("交点计算错误"));
		}
		//先判断是否在当前路径上，不是则重新计算现有路径
		if (!isOnLine){
			ISegmentCollectionPtr pSegCol = (ISegmentCollectionPtr)m_trackPath;	//路径集合
			for (long i = nowPathPos + 1; i < segNum; i++)
			{
				ISegmentPtr pSeg;	//当前所在路径
				//ISegmentPtr pSeg(CLSID_Line);	//当前所在路径
				pSegCol->get_Segment(i, &pSeg);

				//计算最近点是否在当前路径
				isOnLine = isPointOnLine(pNearestPoint, (ILinePtr)pSeg);
				if (isOnLine)
				{
					pNowPath = pSeg;
					double angle;	//直线与X轴夹角，以弧度为单位
					pNowPath->get_Angle(&angle);
					lineHeading = angle * 180 / PI;
					//因为当前角度是以四象限X为起点的逆时针角度，对应的是地图东部
					//故需要转成以北为起点的顺时针角度
					if (lineHeading <= 90)
					{
						//1、3、4象限
						lineHeading = 90 - lineHeading;
					}
					else
					{
						//2象限
						lineHeading = 360 + (90 - lineHeading);
					}
					nowPathPos = i;
					break;
				}
			}
		}

		//distFromCurve *= 1000;
		double dis = distFromCurve * 1000;	//单位由m转为mm
		double subHeading = myGPSInfo.Heading - lineHeading;	//Seekur航向路与径航向差值
		//路径航向在1、4象限，Seekur航向在2、3象限的情况
		if (lineHeading<180 && myGPSInfo.Heading>180 + lineHeading)
		{
			subHeading = myGPSInfo.Heading - 360 - lineHeading;
		}
		//Seekur航向在1、4象限，路径航向在2、3象限的情况
		if (lineHeading > 180 && myGPSInfo.Heading < lineHeading - 180)
		{
			subHeading = myGPSInfo.Heading + 360 - lineHeading;
		}
		if (!isRightSide){
			dis = -dis;
		}

		CString cTp;	//临时转换用Cstring

		cTp.Format(_T("%lf"), subHeading);
		m_editSubHeading.SetWindowText(cTp);
		cTp.Format(_T("%lf"), dis);
		m_editDis.SetWindowText(cTp);

		double turnHeading = 0;	//转向角
		turnHeading = kdis*dis + kheading*subHeading;

		double maxTrun = 60;	//最大转向角
		//判断机器车在路径右侧还是左侧
		if (isRightSide)
		{
			//调整转角，使转角不得令转向后的航向差值大于最大转向角
			if (turnHeading - subHeading >= maxTrun)
			{
				turnHeading = maxTrun + subHeading;
			}
		}
		else
		{
			maxTrun = -maxTrun;
			//调整转角，使转角不得令转向后的航向差值大于最大转向角
			if (turnHeading - subHeading <= maxTrun)
			{
				turnHeading = maxTrun + subHeading;
			}
		}
		cTp.Format(_T("%lf"), turnHeading);
		m_editTurnHeading.SetWindowText(cTp);

		//在追踪状态且不为避障状态时发送运动指令
		if (isTracked&&!isAvoid)
		{
			time_t timet = time(NULL);
			tm *tm_ = localtime(&timet);
			CString cTime;
			cTime.Format(_T("%4d-%02d-%02d %02d:%02d:%02d"), tm_->tm_year + 1990, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
			//保存时间，GPS坐标、BJ54坐标、距离偏差、航向和航向偏差
			//no,time,lon,lat,bj54_x,bj54_y,speed,dis,heading,subheading
			cTp.Format(_T("%d,%s,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf,,"), no, cTime, myGPSInfo.Longitude, myGPSInfo.Latitude, myGPSInfo.BJ54_X, myGPSInfo.BJ54_Y,
				myGPSInfo.SpeedKm, dis, myGPSInfo.Heading, subHeading,turnHeading);
			dataFile.WriteString(cTp);
			//seekur_x,seekur_y,seekur_heading,seekur_tranLeft,seekur_tranRight,seekur_subTranVel,seekur_rotVel
			cTp.Format(_T("%lf,%lf,%lf,%lf,%lf,%lf,%lf\n"), seekurData.x,seekurData.y,seekurData.heading,seekurData.leftVel,seekurData.rightVel,
				seekurData.leftVel-seekurData.rightVel,seekurData.rotVel);
			dataFile.WriteString(cTp);

			//发送控制指令
			SeekurParaPtr pPara = new SeekurPara();
			pPara->distance = 0;
			pPara->tranVel = 0;
			//判断距离是否在500mm内，同时转向角在1度以内
			if (abs((dis) <= 500) && abs(turnHeading) <= 1){
				pPara->heading = 0;
				if (abs(turnHeading) <= 0.2&&abs(subHeading) <= 0.2)
					pPara->rotVel = 0;
				else
				{
					m_editRotVel.GetWindowTextW(cTp);
					double rotVel = _ttof(cTp);
					if (turnHeading > 0)
						pPara->rotVel = rotVel;
					else
						pPara->rotVel = -rotVel;
				}
			}
			else
			{
				pPara->heading = turnHeading;
				pPara->rotVel = 0;
			}
			/*pPara->heading = turnHeading;
			pPara->rotVel = 0;*/
			PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, 0);
			no++;
		}
	}

	delete gpsInfo;
}


/*
描述：激光串口数据解析
参数：
@buf：16进制检测数据组
@len：数据长度
返回值：
*/
void CMFCTestView::OnLaserAnalysis(string *buf, int len){
	for (int i = 0; i < len; i++)
	{
		//only upper 12 bits of upper byte are used
		laserData[i] = HexToDem(buf[2 * i + 1] + buf[2 * i]);
		//intBuf[i] = buf[2 * i] | ((buf[2 * i + 1] & 0x1f) << 8);
	}
	//解析数据，判断是否需要避障
	if (isTracked)
	{
		obsAvoid.Initialize(10 * 100, 90, 90);
		bool isObs = obsAvoid.SetObstaclePosture(laserData, len);
		if (isObs)
		{
			isAvoid = true;
			float turnHeading = obsAvoid.ComputeAvoidHeading();
			//发送控制指令
			SeekurParaPtr pPara = new SeekurPara();
			pPara->distance = 0;
			pPara->tranVel = 0;
			pPara->heading = turnHeading;

			PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, 0);
		}
		else
		{
			isAvoid = false;
		}
		
	}
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
	//robot->setVel2(100, -100);
	robot->runAsync(true);
	BaseAction action(robot);

	MSG msg;
	int no = 1;	//序号，第几次操作
	CStdioFile seekurFile;		//数据文件，记录seekur运行中数据状态
	/*time_t timet = time(NULL);
	tm *tm_ = localtime(&timet);
	CString name;
	name.Format(_T("D:\\data\\Seekur %4d-%02d-%02d %02d：%02d：%02d.csv"), tm_->tm_year + 1900, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
	seekurFile.Open(name, CFile::modeWrite | CFile::modeNoTruncate | CFile::modeCreate);*/
	no = 1;
	while (true)
	{
		GetMessage(&msg, NULL, 0, 0);
		SeekurParaPtr pPara = NULL;
		if (msg.message == WM_SEEKUR_MOVE)
		{
			pPara = (SeekurParaPtr)msg.wParam;
			//直线移动距离
			if (pPara->distance != 0)
			{
				//robot->setVel2(300, 500);
				action.Move(pPara->distance);
			}
			//转动角度
			if (pPara->heading != 0)
			{
				action.SetDeltaHeading(pPara->heading);
			}
			if (pPara->rotVel != 0)
				robot->setRotVel(pPara->rotVel);

			if (pPara->tranVel != 0)
			{
				robot->setVel(pPara->tranVel);
			}

			seekurData.x = robot->getX();
			seekurData.y = robot->getY();
			seekurData.heading = robot->getTh();
			seekurData.vel = robot->getVel();
			seekurData.leftVel = robot->getLeftVel();
			seekurData.rightVel = robot->getRightVel();
			seekurData.rotVel = robot->getRotVel();
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
		if (msg.message == WM_SEEKUR_GET_DATA){
			SeekurDataPtr pData = new SeekurData();
			seekurData.x = robot->getX();
			seekurData.y = robot->getY();
			seekurData.heading = robot->getTh();
			seekurData.vel = robot->getVel();
			seekurData.leftVel = robot->getLeftVel();
			seekurData.rightVel = robot->getRightVel();
			seekurData.rotVel = robot->getRotVel();

			pData->x = robot->getX();
			pData->y = robot->getY();
			pData->heading = robot->getTh();
			pData->vel = robot->getRotVelMax();
			pData->leftVel = robot->getLeftVel();
			pData->rightVel = robot->getRightVel();
			pData->rotVel = robot->getRotVel();

			/*CString cTp;
			double d = 200;
			cTp.Format(_T("%d,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf\n"), 200, d, robot->getX(), robot->getY(), robot->getTh(), robot->getVel(),
				robot->getLeftVel(), robot->getRightVel(), robot->getLeftVel() - robot->getRightVel(), robot->getRotVel());
			seekurFile.WriteString(cTp);*/
			::PostMessage((HWND)lParam, WM_SEEKUR_RET, (UINT)pData, 0);	//发出自定义消息
			continue;
		}
		//记录数据
		if (false&&pPara != NULL&&pPara->distance==0)
		{
			//初始化
			ArPose pose;
			pose.setPose(0, 0);
			robot->moveTo(pose);

			#pragma region 速度模式数据记录
			//bool isEnd = robot->isMoveDone() && robot->isHeadingDone() ? true : false;	//判断运动是否完成
			//写入目标信息
			seekurFile.WriteString(_T("targetDis,targetHeading,targetVel\n"));	//列表头
			CString cTp;
			cTp.Format(_T("%lf,%lf,%lf\n"), pPara->distance, pPara->heading, pPara->tranVel);
			seekurFile.WriteString(cTp);
			seekurFile.WriteString(_T("no,time,x,y,heading,tranVel,tranLeft,tranRight,subTranVel,rotVel\n"));
			//double lastCl = clock();
			int num = 0;
			//CString cTime;
			double lastCl = clock();
			double nowCl, sub;
			while (num<200)
			{
				nowCl = clock();
				sub = (nowCl - lastCl) / CLK_TCK;
				cTp.Format(_T("%d,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf\n"), num, sub, robot->getX(), robot->getY(), robot->getTh(), robot->getVel(),
					robot->getLeftVel(), robot->getRightVel(), robot->getLeftVel()-robot->getRightVel(), robot->getRotVel());
				seekurFile.WriteString(cTp);
				Sleep(100);
				num++;
			}
			robot->stop();
			/*bool isEnd;
			do
			{
				Sleep(200);
				isEnd = robot->isMoveDone() && robot->isHeadingDone() ? true : false;
			}while (!isEnd);*/
			nowCl = clock();
			sub = (nowCl - lastCl) / CLK_TCK;
			cTp.Format(_T("%d,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf,%lf\n"), num, sub, robot->getX(), robot->getY(), robot->getTh(), robot->getVel(),
				robot->getLeftVel(), robot->getRightVel(), robot->getLeftVel() - robot->getRightVel(), robot->getRotVel());
			seekurFile.WriteString(cTp);
			delete pPara;
			//seekurFile.WriteString(_T("\n"));
			#pragma endregion


		}
	}
	//seekurFile.Close();
	//seekurRunFile.Close();
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
		pView->m_editKdis.GetWindowTextW(cKDis);
		CString cKSubHead;
		pView->m_editKheading.GetWindowTextW(cKSubHead);
		double kDis = _ttof(cKDis);
		double kSubHead = _ttof(cKSubHead);

		distFromCurve *= 100000;
		//double dis = GetDistance(seekurY, seekurX, nearestY, nearestX) * 1000;	//Seekur到路径距离（米）
		double dis = distFromCurve;
		double subHeading = lineHeading - myGPSInfo.Heading;	//路径航向与Seekur航向差值

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
		pView->m_editTurnHeading.SetWindowText(vel);

		SeekurParaPtr pPara = new SeekurPara();
		pPara->distance = 0;
		pPara->heading = turnHeading;

		if (isFirst)
		{
			CString cVel;
			pView->m_editTranVel.GetWindowTextW(cVel);
			double vel = _ttof(cVel);
			pPara->tranVel = pView->m_tranVel;
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

void CMFCTestView::OnBtnSavePath()
{
	//分解文件路径和名称
	CString strFolder = _T("D:\\arcMap");
	CString strFile = _T("path.shp");

	//如果路径已加载，删除加载图层和源文件
	long num;
	m_ipMapControl->get_LayerCount(&num);
	ILayerPtr pLayer;
	for (int i = 0; i < num; i++)
	{
		BSTR name;
		m_ipMapControl->get_Layer(i, &pLayer);
		pLayer->get_Name(&name);
		CString cName(name);
		if (!pathLayerName.CompareNoCase(cName))
		{
			m_ipMapControl->DeleteLayer(i);
			CFileFind finder;
			BOOL bWorking = finder.FindFile(_T("D:\\ArcMap\\path.*"));
			while (bWorking)
			{
				bWorking = finder.FindNextFile();
				if (bWorking)
				{
					CString filePath = finder.GetFilePath();
					DeleteFile(filePath);
				}
			}
			break;
		}
	}
	isPathExist = false;

	CFileFind finder;
	BOOL bWorking = finder.FindFile(_T("D:\\ArcMap\\path.*"));
	while (bWorking)
	{
		bWorking = finder.FindNextFile();
		if (bWorking)
		{
			CString filePath = finder.GetFilePath();
			DeleteFile(filePath);
		}
	}

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
	IProjectedCoordinateSystemPtr ipGeoCoordSys;
	ipSpaRefFact2->CreateProjectedCoordinateSystem(esriSRProjCS_Beijing1954_3_Degree_GK_Zone_38, &ipGeoCoordSys);
	//ipSpaRefFact2->CreateGeographicCoordinateSystem(esriSRGeoCS_WGS1984, &ipGeoCoordSys);

	ISpatialReferencePtr ipSRef;
	ipSRef = ipGeoCoordSys;
	ipGeoDefEdit->putref_SpatialReference(ipSRef);
	ipFieldEdit->putref_GeometryDef(ipGeoDef);
	ipFieldsEdit->AddField(ipField);

	//创建shp文件
	IFeatureClassPtr pFeatureClass;
	hr = ipFeatWork->CreateFeatureClass(CComBSTR(strFile), ipFields, 0, 0,
		esriFTSimple, CComBSTR("Shape"), 0, &pFeatureClass);

	if (FAILED(hr))
		AfxMessageBox(_T("创建出错！"), MB_ICONINFORMATION);

	IFeaturePtr pFeature;
	pFeatureClass->CreateFeature(&pFeature);



	pFeature->putref_Shape(pPath);
	pFeature->Store();

	CString str(_T("路径起始点"));
	m_btnPathSelect.SetWindowTextW(str);
	pPath->Release();
	//dataFile.Close();
	AfxMessageBox(_T("创建成功！"), MB_ICONINFORMATION);

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
	AddCreateElement((IGeometryPtr)ipoint, iActiveView);
	iActiveView->Refresh();

	//m_cUseID.SetCheck(1);

	//if (m_cUseID.GetCheck())
	//{
	//	//画点
	//	//IGeometryPtr pGeometry(ipoint);
	//	AddCreateElement((IGeometryPtr)ipoint, iActiveView);
	//	iActiveView->Refresh();
	//}
	//else{
	//	//画线
	//	HRESULT hr;
	//	if (m_pNewLineFeedback == NULL)
	//	{
	//		CoCreateInstance(CLSID_NewLineFeedback,
	//			NULL, CLSCTX_ALL, IID_INewLineFeedback,
	//			(void**)&m_pNewLineFeedback);
	//		ISymbolPtr isymbol;
	//		if (m_pNewLineFeedback == NULL) return;
	//		m_pNewLineFeedback->get_Symbol(&isymbol);
	//		ISimpleLineSymbolPtr pslnsym(isymbol);
	//		if (pslnsym == NULL) return;
	//		IRgbColorPtr prgb(CLSID_RgbColor);
	//		prgb->put_Red(0);
	//		prgb->put_Green(205);
	//		prgb->put_Blue(0);
	//		IColorPtr icolor(prgb);

	//		pslnsym->put_Color(icolor);
	//		pslnsym->put_Style(esriSLSDot);

	//		IScreenDisplayPtr pScrD;
	//		iActiveView->get_ScreenDisplay(&pScrD);

	//		m_pNewLineFeedback->putref_Display(pScrD);
	//		m_pNewLineFeedback->Start(ipoint);
	//	}
	//	else
	//		hr = m_pNewLineFeedback->AddPoint(ipoint);
	//}



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
void CMFCTestView::OnBtnMoveSeekur()
{
	// TODO:  在此添加控件通知处理程序代码
	//获取要移动的距离
	CString cTp;
	m_editMoveDistance.GetWindowTextW(cTp);
	double dis = _ttof(cTp) * 1000;
	m_editMoveHeading.GetWindowTextW(cTp);
	double head = _ttof(cTp);
	m_editTranVel.GetWindowTextW(cTp);
	double vel = _ttof(cTp);
	m_editRotVel.GetWindowTextW(cTp);
	double rotVel = _ttof(cTp);

	//弹出窗口确认参数是否无误
	CString alert;
	alert.Format(_T("确定为以下参数？\n距离：%.2lfmm\n角度：%.2lfdeg\n速度：%.2lfmm/s\n角度：%.2lfdeg/s"), dis, head, vel, rotVel);
	int result = AfxMessageBox(alert, MB_OKCANCEL);
	if (result==IDOK)
	{
		SeekurParaPtr pPara = new SeekurPara();
		pPara->distance = dis;
		pPara->heading = head;
		if (dis == 0 && head == 0)
		{
			pPara->tranVel = vel;
			pPara->rotVel = rotVel;
		}
		else
		{
			pPara->tranVel = 0;
			pPara->rotVel = 0;
		}

		PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, NULL);

		//PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, dis, head);

		m_editMoveDistance.SetWindowTextW(_T(""));
		m_editMoveHeading.SetWindowTextW(_T(""));
	}

}



//测试追踪
void CMFCTestView::OnBtnTrack()
{
	if (!isPathExist){
		MessageBox(_T("无路径！"));
	}
	else{
		if (!isTracked)
		{
			//初始化参数
			nowPathPos = 0;
			CString cTp;	//临时Cstring类，用于将文本框数据转为其他格式
			m_editKdis.GetWindowTextW(cTp);
			kdis = _ttof(cTp);
			m_editKheading.GetWindowTextW(cTp);
			kheading = _ttof(cTp);

			//开始记录数据，以年月日分秒为文件名
			time_t timet = time(NULL);
			tm *tm_ = localtime(&timet);
			CString fileName;
			fileName.Format(_T("D:\\data\\Track(%.4lf-%.4lf) %4d-%02d-%02d %02d：%02d：%02d.csv"), kdis, kheading, tm_->tm_year + 1900, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
			dataFile.Open(fileName, CFile::modeCreate | CFile::modeWrite);
			//列表头
			dataFile.WriteString(_T("no,time,lon,lat,bj54_x,bj54_y,speed,dis,heading,subheading,turnHeading,,seekur_x,seekur_y,seekur_heading,seekur_tranLeft,seekur_tranRight,seekur_subTranVel,seekur_rotVel\n"));
			no = 1;
			lastClock = clock();

			//启动追踪
			isTracked = true;

			

			//令Seekur开始运动
			SeekurParaPtr pPara = new SeekurPara();
			pPara->distance = 0;
			pPara->heading = 0;
			m_editTranVel.GetWindowTextW(cTp);
			double vel = _ttof(cTp);
			pPara->tranVel = vel;
			PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_MOVE, (UINT)pPara, 0);
			m_btnTrack.SetWindowTextW(_T("停止追踪"));
		}
		else{
			//PostThreadMessage(track_thread->m_nThreadID, WM_TRACK_STOP, 0, 0);
			isTracked = false;
			PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_STOP, 0, 0);
			m_btnTrack.SetWindowTextW(_T("开始追踪"));
			no = 1;
			dataFile.Close();
		}
	}

}

/*
向路径中添加点，无路径时则创建路径
*/
void CMFCTestView::OnBtnPathAdd()
{
	// TODO:  在此添加控件通知处理程序代码if (!pPath)
	double lon = myGPSInfo.Longitude, lat = myGPSInfo.Latitude, hd = myGPSInfo.Heading, bjx = myGPSInfo.BJ54_X, bjy = myGPSInfo.BJ54_Y, speed = myGPSInfo.SpeedKm;
	CString cTp;
	if (!pPath)
	{
		//路径为空时创建路径
		HRESULT hr1 = pPath.CreateInstance(CLSID_Polyline);
		CString str(_T("下一点"));
		m_btnPathSelect.SetWindowTextW(str);

		//no = 1;
		//time_t timet = time(NULL);
		//tm *tm_ = localtime(&timet);
		//CString fileName;
		//fileName.Format(_T("D:\\data\\GPS %4d-%02d-%02d %02d：%02d：%02d.csv"), tm_->tm_year + 1900, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
		//dataFile.Open(fileName, CFile::modeCreate | CFile::modeWrite);
		////列表头
		//dataFile.WriteString(_T("no,time,lon,lat,x,y,speed,heading\n"));
	}

	IPointCollectionPtr pPtclo = (IPointCollectionPtr)pPath;
	IPointPtr point(CLSID_Point);
	point->PutCoords(bjx,bjy);
	pPtclo->AddPoint(point);
	//point->putref_SpatialReference()

	/*time_t timet = time(NULL);
	tm *tm_ = localtime(&timet);
	CString cTime;
	cTime.Format(_T("%4d-%02d-%02d %02d:%02d:%02d"), tm_->tm_year + 1990, tm_->tm_mon + 1, tm_->tm_mday, tm_->tm_hour, tm_->tm_min, tm_->tm_sec);
	cTp.Format(_T("%d,%s,%lf,%lf,%lf,%lf,%lf,%lf\n"), no, cTime, lon,lat, bjx,bjy,speed,hd);
	dataFile.WriteString(cTp);
	no++;*/
}

void CMFCTestView::OnBtnSeekurQuery()
{
	// TODO:  在此添加控件通知处理程序代码
	PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_GET_DATA, 0, 0);
}


void CMFCTestView::OnBtnSeekurStop()
{
	// TODO:  在此添加控件通知处理程序代码
	isTracked = false;
	isAvoid = false;
	PostThreadMessage(seekur_thread->m_nThreadID, WM_SEEKUR_STOP, 0, 0);
}
