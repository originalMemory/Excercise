
// MFCTestView.h : CMFCTestView ��Ľӿ�
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


//seekur���Ʋ���
typedef struct SeekurPara{
	double distance;	//����
	double heading;		//����
	double tranVel;	//ƽ���ٶ�
	double rotVel;	//��ת�ٶ�
}*SeekurParaPtr;



class CMFCTestView : public CFormView
{
protected: // �������л�����
	CMFCTestView();
	DECLARE_DYNCREATE(CMFCTestView)

public:
	enum{ IDD = IDD_MFCTEST_FORM };

// ����
public:
	CMFCTestDoc* GetDocument() const;

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
	virtual ~CMFCTestView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

//�ؼ�����
public:
	IMapControl2Ptr m_ipMapControl;/*ָ��ǰ��ͼ�ĵ���ָ��*/
	ITOCControl2Ptr m_ipTocControl;/*ָ��ǰ��ͼ�����ָ��*/
	//CEdit m_editLongitude;		//�����ı���
	//CEdit m_editLatitude;		//γ���ı���
	INewLineFeedbackPtr m_pNewLineFeedback;
	bool m_bTrack;		//�Ƿ�����׷��

//��Ա����
protected:
	CSerialPort m_csGps;		//GPS����ͨ����
	string m_sTpGps;		//��ȡ����������һ��GPS���
	bool isGPSEnd;		//����GPS������Ƿ��������
	IPolylinePtr pPath;			//������·��
	IElementPtr lastSeekurElement;	// GPS��������(��ͼ��
	IElementPtr lastObsElement;	// �ϰ�����������(��ͼ��
	IPointPtr pSeekurPoint;	// GPS��ȡ��Seekur��������

	CSerialPort m_csLaser;		//���⴮��ͨ����
	bool m_bLaserStartWrite;	//�Ƿ�ʼд�뼤������
	int m_nLastLaserFirst = -1;	//����ָ��������е�ǰ�벿��
	int m_anLaserData[LEN180X1];	//������ļ���ɨ����Ϣ
	int m_nLaserBufIdx;	//��ǰ�����������λ��
	int m��nLaserCheckBufIdx;	//��ǰ����ͷ��У������λ��
	int m_nLaserLen = 181;	//�������ݳ��ȣ�����8λ����λ��λ���ϳ�16λ����
	DWORD m_tLastCompute;	//������һ�μ����ʱ��
	
	CWinThread* seekur_thread;		//Seekur�߳̾��
	CWinThread* record_thread;		//��¼�߳̾��
	CString pathLayerName;		//·��ͼ������
	bool isPathExist;	//�Ƿ��Ѽ���·��ͼ��
	ObstacleAvoid obsAvoid;		//������Ϊ��
	string testStr;
	double lastClock;		//�ϴμ�ʱʱ�䣬���Զ�ʱ¼���ļ�����
	CStdioFile dataFile;		//�����ļ�����¼����������״̬
	int no;		//��ţ���¼��ǰд��ڼ�����

private:
	void AddCreateElement(IGeometryPtr pgeomln, IActiveViewPtr iactiveview);
	void OnTestMarkerStyle();
	ISymbolPtr m_isymbol;
	static UINT SeekurFuc(LPVOID lParam);		//Seekur�����̺߳���
	static UINT RecordFuc(LPVOID lParam);		//Seekur��¼����
	
	//void CreateShapeFile();
	void ComputeTurnHeading();		//׷�ٺ���

	
// ���ɵ���Ϣӳ�亯��
protected:
	DECLARE_MESSAGE_MAP()
	DECLARE_EVENTSINK_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnFileOpen();
protected:
	/*
	����������ͨ�Żص�����
	������
	@ch�����ڴ��ݵĵ����ַ�
	@portnum���˿ں�
	����ֵ��0
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
	// ת����ԽǶ��ı���
	CEdit m_editMoveHeading;
	// �ƶ���Ծ����ı���
	CEdit m_editMoveDistance;
	// γ���ı���
	CEdit m_editLongitude;
	// �����ı���
	CEdit m_editLatitude;
	afx_msg void OnBtnTrack();
	// Seekur��ǰ�ٶ��ı���
	CEdit m_editTurnHeading;
	// Seekur��ǰ�����ı���
	CEdit m_editGPSHeading;
	// ׷�ٿ�ʼ/ֹͣ��ť
	CButton m_btnTrack;
	// Seekur�˶��ٶ��ı���
	CEdit m_editTranVel;
	// ׷��ʱ��ƽ���ٶ�
	double m_tranVel;
	// ·��ѡ��ť
	CButton m_btnPathSelect;
	afx_msg void OnBtnPathAdd();
	afx_msg void OnBtnSavePath();
	// �����ֵ
	CEdit m_editSubHeading;
	// �����ֵ
	CEdit m_editDis;
	// GPS���ں�
	int m_GPSport;
	// ���⴮�ں�
	int m_laserport;
#pragma region PID������ؿؼ�
	// ��������ı���
	CEdit m_editKdis;
	// ������Ӧ����ֵ�ı���
	CEdit m_editKheading;
	ILinePtr pNowPath;	//��ǰ·��
	int nowPathPos;		//��ǰ·����·�������е�λ�ã�����������׷��·��
	double lineHeading;		//·������
	long segNum;	//·��������
	IPolylinePtr m_trackPath;	//׷��·��
	double kdis;	//�������ϵ��
	double kheading;	//�������ϵ��
#pragma endregion
	// ����54 x�����ı���
	CEdit m_editBJ54_x;
	// ����54 y�����ı���
	CEdit m_editBJ54_y;
	/*
	������GPS�������ݽ���
	������
	@spData��GPS���
	����ֵ��
	*/
	void OnGPSAnalysis(string spData);
	/*
	���������⴮�����ݽ���
	����ֵ��
	*/
	void OnLaserAnalysis();

public:
	afx_msg void OnBtnSeekurQuery();
	afx_msg void OnBtnSeekurStop();
	// Seekur���ٶ�
	CEdit m_editObsPos;
	// gps�ٶ�(km/h)
	CEdit m_editGPSSpeedKm;
	// ��ת�ٶ��ı��ؼ�
	CEdit m_editRotVel;
};

#ifndef _DEBUG  // MFCTestView.cpp �еĵ��԰汾
inline CMFCTestDoc* CMFCTestView::GetDocument() const
   { return reinterpret_cast<CMFCTestDoc*>(m_pDocument); }
#endif

