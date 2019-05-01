
// SeekurAutoPatrolView.h : CSeekurAutoPatrolView 类的接口
//

#pragma once

#include "resource.h"


class CSeekurAutoPatrolView : public CFormView
{
protected: // 仅从序列化创建
	CSeekurAutoPatrolView();
	DECLARE_DYNCREATE(CSeekurAutoPatrolView)

public:
	enum{ IDD = IDD_SEEKURAUTOPATROL_FORM };

// 特性
public:
	CSeekurAutoPatrolDoc* GetDocument() const;

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
	virtual ~CSeekurAutoPatrolView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// 生成的消息映射函数
protected:
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // SeekurAutoPatrolView.cpp 中的调试版本
inline CSeekurAutoPatrolDoc* CSeekurAutoPatrolView::GetDocument() const
   { return reinterpret_cast<CSeekurAutoPatrolDoc*>(m_pDocument); }
#endif

