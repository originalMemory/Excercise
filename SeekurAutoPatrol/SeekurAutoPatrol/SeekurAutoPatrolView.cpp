
// SeekurAutoPatrolView.cpp : CSeekurAutoPatrolView 类的实现
//

#include "stdafx.h"
// SHARED_HANDLERS 可以在实现预览、缩略图和搜索筛选器句柄的
// ATL 项目中进行定义，并允许与该项目共享文档代码。
#ifndef SHARED_HANDLERS
#include "SeekurAutoPatrol.h"
#endif

#include "SeekurAutoPatrolDoc.h"
#include "SeekurAutoPatrolView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSeekurAutoPatrolView

IMPLEMENT_DYNCREATE(CSeekurAutoPatrolView, CFormView)

BEGIN_MESSAGE_MAP(CSeekurAutoPatrolView, CFormView)
END_MESSAGE_MAP()

// CSeekurAutoPatrolView 构造/析构

CSeekurAutoPatrolView::CSeekurAutoPatrolView()
	: CFormView(CSeekurAutoPatrolView::IDD)
{
	// TODO:  在此处添加构造代码

}

CSeekurAutoPatrolView::~CSeekurAutoPatrolView()
{
}

void CSeekurAutoPatrolView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
}

BOOL CSeekurAutoPatrolView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO:  在此处通过修改
	//  CREATESTRUCT cs 来修改窗口类或样式

	return CFormView::PreCreateWindow(cs);
}

void CSeekurAutoPatrolView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

}


// CSeekurAutoPatrolView 诊断

#ifdef _DEBUG
void CSeekurAutoPatrolView::AssertValid() const
{
	CFormView::AssertValid();
}

void CSeekurAutoPatrolView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CSeekurAutoPatrolDoc* CSeekurAutoPatrolView::GetDocument() const // 非调试版本是内联的
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CSeekurAutoPatrolDoc)));
	return (CSeekurAutoPatrolDoc*)m_pDocument;
}
#endif //_DEBUG


// CSeekurAutoPatrolView 消息处理程序
