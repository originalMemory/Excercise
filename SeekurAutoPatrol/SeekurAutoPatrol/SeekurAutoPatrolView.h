
// SeekurAutoPatrolView.h : CSeekurAutoPatrolView ��Ľӿ�
//

#pragma once

#include "resource.h"


class CSeekurAutoPatrolView : public CFormView
{
protected: // �������л�����
	CSeekurAutoPatrolView();
	DECLARE_DYNCREATE(CSeekurAutoPatrolView)

public:
	enum{ IDD = IDD_SEEKURAUTOPATROL_FORM };

// ����
public:
	CSeekurAutoPatrolDoc* GetDocument() const;

// ����
public:

// ��д
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��
	virtual void OnInitialUpdate(); // ������һ�ε���

// ʵ��
public:
	virtual ~CSeekurAutoPatrolView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// ���ɵ���Ϣӳ�亯��
protected:
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // SeekurAutoPatrolView.cpp �еĵ��԰汾
inline CSeekurAutoPatrolDoc* CSeekurAutoPatrolView::GetDocument() const
   { return reinterpret_cast<CSeekurAutoPatrolDoc*>(m_pDocument); }
#endif

