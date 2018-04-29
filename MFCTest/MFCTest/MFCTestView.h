
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

//seekur控制参数
typedef struct seekurPara{
	double distance;	//距离
	double heading;		//航向
	double veloctiy;	//速度
}*seekurParaPtr;


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
	IPolylinePtr m_trackPath;	//追踪路径
	bool isTracked;		//是否正在追踪
	MyGPSInfo myGPSInfo;		//我实际使用的GPS信息

//成员对象
protected:
	CSerialPort serialPort;		//串口通信类
	GPSTranslate gpsTran;	/*GPS语句解析类*/
	string gpsStr;	//获取到的完整的一句GPS语句
	GPSInfo* gpsInfo;		//解析后的GPS语句指针
	CWinThread* seekur_thread;		//Seekur线程句柄
	CWinThread* track_thread;		//追踪线程句柄

	//ILayerPtr m_currentLayer;		//当前图层
	//IMapPtr m_map;		//地图控件中的地图
	//IFeaturePtr m_editFeature;	//编辑中的要素
	//IDisplayFeedbackPtr m_feedback;		//用于地图显示
	//bool m_bInUse;		//判断是否正在使用
	//IPointCollectionPtr m_pointCollection;	//当前要素的点集
	IPolylinePtr pPath;			//创建的路径

	IElementPtr lastPointElement;

private:
	void AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview);
	void OnTestMarkerStyle();
	IPoint* geoToProj(IPoint* point/*需要更改坐标系的点*/, long fromProjType = 3857, long toGeoType = 4326);
	ISymbolPtr m_isymbol;
	static UINT SeekurFuc(LPVOID lParam);		//Seekur控制线程函数
	//HRESULT CreateShapeFile(esriGeometryType type, CString layerPath, CString layerName, IFeatureClass** ppFeatureClass);
	void CreateShapeFile();
	static UINT TrackFuc(LPVOID lParam);		//追踪线程函数

	
// 生成的消息映射函数
protected:
	DECLARE_MESSAGE_MAP()
	DECLARE_EVENTSINK_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnFileOpen();
//	afx_msg void OnSize(UINT nType, int cx, int cy);
protected:
	/*
	描述：GPS串口通信函数
	参数：
	@ch：串口传递的单个字符
	@ch：端口号
	返回值：0
	*/
	afx_msg LRESULT OnCommunication(WPARAM ch, LPARAM portnum);
public:
//	afx_msg void OnBnClickedButton1();
	afx_msg void OnClickedButton1();
	afx_msg void OnMouseDownMapcontrol1(long button, long shift, long x, long y, double mapX, double mapY);
	afx_msg void OnDoubleClickMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY);
	afx_msg	void OnMouseMoveMapcontrol1(long button, long shift, long X, long Y, double mapX, double mapY);
	afx_msg void OnFileSave();
	afx_msg void OnBnClickedButton2();
private:
	// 控制线程的结束
protected:
	afx_msg LRESULT OnSeekur(WPARAM wParam, LPARAM lParam);
public:
	// 航向文本框
	CEdit m_editHeading;
	// 距离文本框
	CEdit m_editDistance;
	// 纬度文本框
	CEdit m_editLongitude;
	// 经度文本框
	CEdit m_editLatitude;
	afx_msg void OnBnClickedButton3();
	// 点线切换绘制
	CButton m_cPointLine;
	// Seekur当前速度
	CEdit m_editSeekurVel;
	// Seekur当前航向
	CEdit m_editSeekurHeading;
	// P控制法K1参数，与距离有关
	CEdit m_editKDis;
	// P控制法K2参数，与航向差值相关
	CEdit m_editKSubHead;
	// 追踪开始/停止按钮
	CButton m_btnTrack;
	// 北京54，X坐标
	CEdit m_editBJ54_x;
	// 北京54，Y坐标
	CEdit m_editBJ54_y;
	// 是否在地图上显示GPS坐标
	CButton m_checkShowGPS;
	// Seekur运动速度
	CEdit m_editVelocity;
	// 追踪时的速度
	double m_velocity;
	// 路径选择按钮
	CButton m_btnPathSelect;
	afx_msg void OnBtnPathCreate();
	// 航向差值
	CEdit m_editSubHeading;
	// 距离差值
	CEdit m_editDis;
};

#ifndef _DEBUG  // MFCTestView.cpp 中的调试版本
inline CMFCTestDoc* CMFCTestView::GetDocument() const
   { return reinterpret_cast<CMFCTestDoc*>(m_pDocument); }
#endif

