
// MFCTestView.h : CMFCTestView 类的接口
//

#pragma once

#include "resource.h"

#include "GPSTranslate.h"
#include "SerialPort.h"
#include <vector>


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
protected:
	IMapControl2Ptr m_ipMapControl;/*指向当前地图文档的指针*/
	//ITOCControl2Ptr m_ipTocControl;/*指向当前地图文档的指针*/
	CEdit *m_editLongitude;		//经度文本框
	CEdit *m_editLatitude;		//纬度文本框
	INewLineFeedbackPtr m_pNewLineFeedback;
	IGeometryPtr m_pgeometry;

//成员对象
protected:
	CSerialPort serialPort;		//串口通信类
	GPSTranslate gpsTran;	/*GPS语句解析类*/
	string gpsStr;	//获取到的完整的一句GPS语句
	vector<GPSInfo*> gpsInfos;		//解析后的GPS语句组
	MyGPSInfo myGPSInfo;		//我实际使用的GPS信息
	bool testBool;

private:
	void AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview);
	void OnTestMarkerStyle();
	IPoint* geoToProj(IPoint* point/*需要更改坐标系的点*/, long fromProjType = 3857, long toGeoType = 4326);
	ISymbolPtr m_isymbol;
	
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
};

#ifndef _DEBUG  // MFCTestView.cpp 中的调试版本
inline CMFCTestDoc* CMFCTestView::GetDocument() const
   { return reinterpret_cast<CMFCTestDoc*>(m_pDocument); }
#endif

