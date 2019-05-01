
// MFCTestView.h : CMFCTestView 类的接口
//

#pragma once

#include "resource.h"

#include "GPSTranslate.h"
#include "SerialPort.h"
#include <vector>
//#include "SeekurStatus.h"
#include "afxwin.h"
#include <windows.h>
#include "ObstacleAvoid.h"

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


//seekur控制参数
typedef struct SeekurPara{
	double distance;	//距离
	double heading;		//航向
	double tranVel;	//平移速度
	double rotVel;	//旋转速度
}*SeekurParaPtr;



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
	bool m_bTrack;		//是否正在追踪

//成员对象
protected:
	CSerialPort m_csGps;		//GPS串口通信类
	string m_sTpGps;		//获取到的完整的一句GPS语句
	bool isGPSEnd;		//本轮GPS语句组是否已至最后
	IPolylinePtr pPath;			//创建的路径
	IElementPtr lastSeekurElement;	// GPS最新坐标(地图）
	IElementPtr lastObsElement;	// 障碍物最新坐标(地图）
	IPointPtr pSeekurPoint;	// GPS获取的Seekur最新坐标

	CSerialPort m_csLaser;		//激光串口通信类
	bool m_bLaserStartWrite;	//是否开始写入激光数据
	int m_nLastLaserFirst = -1;	//两个指针的数据中的前半部分
	int m_anLaserData[LEN180X1];	//解析后的激光扫描信息
	int m_nLaserBufIdx;	//当前激光距离数据位置
	int m＿nLaserCheckBufIdx;	//当前激光头部校验数据位置
	int m_nLaserLen = 181;	//激光数据长度，两个8位（高位低位）合成16位包长
	DWORD m_tLastCompute;	//激光上一次计算的时间
	
	CWinThread* seekur_thread;		//Seekur线程句柄
	CWinThread* record_thread;		//记录线程句柄
	CString pathLayerName;		//路径图层名称
	bool isPathExist;	//是否已加载路径图层
	ObstacleAvoid obsAvoid;		//避障行为类
	string testStr;
	double lastClock;		//上次计时时间，用以定时录入文件数据
	CStdioFile dataFile;		//数据文件，记录运行中数据状态
	int no;		//编号，记录当前写入第几个点

private:
	void AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview);
	void OnTestMarkerStyle();
	ISymbolPtr m_isymbol;
	static UINT SeekurFuc(LPVOID lParam);		//Seekur控制线程函数
	static UINT RecordFuc(LPVOID lParam);		//Seekur记录函数
	
	//void CreateShapeFile();
	void ComputeTurnHeading();		//追踪函数

	
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
	CEdit m_editTurnHeading;
	// Seekur当前航向文本框
	CEdit m_editGPSHeading;
	// 追踪开始/停止按钮
	CButton m_btnTrack;
	// Seekur运动速度文本框
	CEdit m_editTranVel;
	// 追踪时的平移速度
	double m_tranVel;
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
	// 距离参数文本框
	CEdit m_editKdis;
	// 航向差对应比例值文本框
	CEdit m_editKheading;
	ILinePtr pNowPath;	//当前路径
	int nowPathPos;		//当前路径在路径集合中的位置，用于跳过已追踪路径
	double lineHeading;		//路径航向
	long segNum;	//路径总数量
	IPolylinePtr m_trackPath;	//追踪路径
	double kdis;	//距离比例系数
	double kheading;	//航向比例系数
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
	返回值：
	*/
	void OnLaserAnalysis();

public:
	afx_msg void OnBtnSeekurQuery();
	afx_msg void OnBtnSeekurStop();
	// Seekur加速度
	CEdit m_editObsPos;
	// gps速度(km/h)
	CEdit m_editGPSSpeedKm;
	// 旋转速度文本控件
	CEdit m_editRotVel;
};

#ifndef _DEBUG  // MFCTestView.cpp 中的调试版本
inline CMFCTestDoc* CMFCTestView::GetDocument() const
   { return reinterpret_cast<CMFCTestDoc*>(m_pDocument); }
#endif

