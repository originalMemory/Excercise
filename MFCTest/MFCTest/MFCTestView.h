
// MFCTestView.h : CMFCTestView 类的接口
//

#pragma once

#include "resource.h"

#include "GPSTranslate.h"
#include "SerialPort.h"
#include <vector>
#include "BaseAction.h"
#include "afxwin.h"
#include <windows.h>
#include "ObstacleAvoid.h"

#define M_PI       3.14159265358979323846
#define STX 0x02   /*every PC->LMS packet is started by STX*/ 
#define ACKSTX 0x06 /*every LMS->PC packet is started by ACKSTX*/
#define LEN100X1 101
#define LEN100X5 201
#define LEN100X25 401
#define LEN180X1 181
#define LEN180X5 361
#define RANGE100 100//range of LMS200
#define RANGE180 180
#define RES1 1		//resolution
#define RES5 0.5
#define RES25 0.25
#define MAXPACKET 812

const string LASER_HEADER[7] = { "02", "80", "6E", "01", "B0", "B5", "00" };
//const char LASER_HEADER[7] = { 0x02, 0x80, 0x6E, 0x01, 0xB0, 0xB5, 0x00 };


//seekur控制参数
typedef struct seekurPara{
	double distance;	//距离
	double heading;		//航向
	double veloctiy;	//速度
}*seekurParaPtr;

//seekur相关数据
typedef struct seekurData{
	double x;
	double y;
	double heading;		//航向
	double vel;
	double leftVel;
	double rightVel;
	double tranAccel;
	double rotAccel;
}*seekurDataPtr;


class CMFCTestView : public CFormView
{
protected: // 仅从序列化创建
	CMFCTestView();
	DECLARE_DYNCREATE(CMFCTestView)

public:
	enum{ IDD = IDD_MFCTEST_FORM };

// 特性
public:
	CMFCTestDoc* GetDocument() const;

// 操作
public:

// 重写
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual void OnInitialUpdate(); // 构造后第一次调用

// 实现
public:
	virtual ~CMFCTestView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

//控件对象
public:
	IMapControl2Ptr m_ipMapControl;/*指向当前地图文档的指针*/
	ITOCControl2Ptr m_ipTocControl;/*指向当前地图管理的指针*/
	//CEdit m_editLongitude;		//经度文本框
	//CEdit m_editLatitude;		//纬度文本框
	INewLineFeedbackPtr m_pNewLineFeedback;
	bool isTracked;		//是否正在追踪
	MyGPSInfo myGPSInfo;		//我实际使用的GPS信息

//成员对象
protected:
	CSerialPort serialPort_gps;		//GPS串口通信类
	CSerialPort serialPort_laser;		//激光串口通信类
	GPSTranslate gpsTran;	/*GPS语句解析类*/
	string gpsStr;		//获取到的完整的一句GPS语句
	bool isLaserStart;	//是否开始写入激光数据
	string laserBuf[MAXPACKET];	//获取到激光16进制数据缓冲
	string laserStr;
	int laserBufPos;	//当前激光缓冲数据位置
	string laserCheckBuf[7];	//获取到激光16进制数据头缓冲
	int laserCheckBufPos;	//当前激光缓冲数据位置
	int laserData[LEN180X1];	//解析后的激光扫描信息
	int laserDataLength;	//激光数据长度
	double laserRange;		//激光角度范围
	double laserRes;		//激光角度分辨率
	
	CWinThread* seekur_thread;		//Seekur线程句柄
	//CWinThread* track_thread;		//追踪线程句柄
	bool isGPSEnd;		//本轮GPS语句组是否已至最后
	IPolylinePtr pPath;			//创建的路径
	IElementPtr lastPointElement;	// Seekur上一次的坐标
	CString pathLayerName;		//路径图层名称
	bool isPathExist;	//是否已加载路径图层
	ObstacleAvoid obsAvoid;		//避障行为类
	//是否是避障状态
	bool isAvoid;
	string testStr;
	double lastClock;		//上次计时时间，用以定时录入文件数据
	CStdioFile dataFile;		//数据文件，记录运行中数据状态
	int no;		//编号，记录当前写入第几个点

private:
	void AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview);
	void OnTestMarkerStyle();
	IPoint* geoToProj(IPoint* point/*需要更改坐标系的点*/, long fromProjType = 3857, long toGeoType = 4326);	//esriSRGeoCS_Beijing1954
	ISymbolPtr m_isymbol;
	static UINT SeekurFuc(LPVOID lParam);		//Seekur控制线程函数
	//void CreateShapeFile();
	static UINT TrackFuc(LPVOID lParam);		//追踪线程函数

	
// 生成的消息映射函数
protected:
	DECLARE_MESSAGE_MAP()
	DECLARE_EVENTSINK_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnFileOpen();
protected:
	/*
	描述：串口通信回调函数
	参数：
	@ch：串口传递的单个字符
	@portnum：端口号
	返回值：0
	*/
	afx_msg LRESULT OnSerialPortCallback(WPARAM ch, LPARAM portnum);
public:
//	afx_msg void OnBnClickedButton1();
	afx_msg void OnMouseDownMapcontrol1(long button, long shift, long x, long y, double mapX, double mapY);
	afx_msg void OnDoubleClickMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY);
	afx_msg	void OnMouseMoveMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY);
	afx_msg void OnFileSave();
	afx_msg void OnBtnMoveSeekur();
protected:
	afx_msg LRESULT OnSeekurCallback(WPARAM wParam, LPARAM lParam);
private:
	// 转动相对角度文本框
	CEdit m_editMoveHeading;
	// 移动相对距离文本框
	CEdit m_editMoveDistance;
	// 纬度文本框
	CEdit m_editLongitude;
	// 经度文本框
	CEdit m_editLatitude;
	afx_msg void OnBtnTrack();
	// Seekur当前速度文本框
	CEdit m_editSeekurVel;
	// Seekur当前航向文本框
	CEdit m_editGPSHeading;
	// 追踪开始/停止按钮
	CButton m_btnTrack;
	// GPS卫星数量文本框
	CEdit m_editSatNum;
	// GPS状态指示位文本框
	CEdit m_editGPSStaus;
	// 是否转换北京54坐标
	CButton m_checkShowBJ54;
	// Seekur运动速度文本框
	CEdit m_editVelocity;
	// 追踪时的速度
	double m_velocity;
	// 路径选择按钮
	CButton m_btnPathSelect;
	afx_msg void OnBtnPathAdd();
	afx_msg void OnBtnSavePath();
	// 航向差值
	CEdit m_editSubHeading;
	// 距离差值
	CEdit m_editDis;
	// GPS串口号
	int m_GPSport;
	// 激光串口号
	int m_laserport;
#pragma region PID控制相关控件
	// 距离参数文本框	P参数
	CEdit m_editKp;
	// I参数文本框
	CEdit m_editKi;
	// d参数文本框
	CEdit m_editKd;
	// 航向差对应比例值文本框
	CEdit m_editKheading;
	IPointPtr pStartPoint;		//当前路径起始点
	IPointPtr pEndPoint;		//当前路径结束点
	ILinePtr pNowPath;	//当前路径
	int nowPathPos;		//当前路径在路径集合中的位置，用于跳过已追踪路径
	double lineHeading;		//路径航向
	long segNum;	//路径总数量
	IPolylinePtr m_trackPath;	//追踪路径
	double err;		//当前偏差值
	double err_last;	//上一次的偏差值
	double integral;	//积分值
	double kp;	//P控制比例系数
	double ki;	//I控制比例系数
	double kd;	//D控制比例系数
	double kheading;	//航向比例系数
	// 启用ID控制
	CButton m_cUseID;
#pragma endregion
	// 北京54 x坐标文本框
	CEdit m_editBJ54_x;
	// 北京54 y坐标文本框
	CEdit m_editBJ54_y;
	/*
	描述：GPS串口数据解析
	参数：
	@spData：GPS语句
	返回值：
	*/
	void OnGPSAnalysis(string spData);
	/*
	描述：激光串口数据解析
	参数：
	@buf：16进制检测数据组
	@len：数据长度
	返回值：
	*/
	void OnLaserAnalysis(string *buf, int len);

public:
	// Seekur的位置（x,y）
	CEdit m_editSeekurPose;
	// Seekur自测速度
	CEdit m_editSeekurVal;
	afx_msg void OnBtnSeekurQuery();
	afx_msg void OnBtnSeekurStop();
	// Seekur加速度
	CEdit m_editSeekurAccel;
};

#ifndef _DEBUG  // MFCTestView.cpp 中的调试版本
inline CMFCTestDoc* CMFCTestView::GetDocument() const
   { return reinterpret_cast<CMFCTestDoc*>(m_pDocument); }
#endif

