
// SeekurAutoPatrolView.cpp : CSeekurAutoPatrolView ���ʵ��
//

#include "stdafx.h"
// SHARED_HANDLERS ������ʵ��Ԥ��������ͼ������ɸѡ�������
// ATL ��Ŀ�н��ж��壬�����������Ŀ�����ĵ����롣
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

// CSeekurAutoPatrolView ����/����

CSeekurAutoPatrolView::CSeekurAutoPatrolView()
	: CFormView(CSeekurAutoPatrolView::IDD)
{
	// TODO:  �ڴ˴���ӹ������

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
	// TODO:  �ڴ˴�ͨ���޸�
	//  CREATESTRUCT cs ���޸Ĵ��������ʽ

	return CFormView::PreCreateWindow(cs);
}

void CSeekurAutoPatrolView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

}


// CSeekurAutoPatrolView ���

#ifdef _DEBUG
void CSeekurAutoPatrolView::AssertValid() const
{
	CFormView::AssertValid();
}

void CSeekurAutoPatrolView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CSeekurAutoPatrolDoc* CSeekurAutoPatrolView::GetDocument() const // �ǵ��԰汾��������
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CSeekurAutoPatrolDoc)));
	return (CSeekurAutoPatrolDoc*)m_pDocument;
}
#endif //_DEBUG


// CSeekurAutoPatrolView ��Ϣ�������
