#ifndef _PID_H_  
#define _PID_H_  

typedef struct _pid{
	float SetSpeed;		//理想速度
	float ActualSpeed;	//当前速度
	float err;		//当前偏差值
	float err_last;	//上一次的偏差值
	float Kp;	//P控制比例系数
	float Ki;	//I控制比例系数
	float Kd;	//D控制比例系数
	float voltage;
	float integral;	//积分
	float umax;		//阈值上边界
	float umin;		//阈值下边界
}Pid;


class Pid_control
{
public:

	void PID_init();
	float PID_realize(float speed);

private:
	int index;
	Pid pid;
};
#endif
