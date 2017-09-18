/******************************************************************* 
 *  文件名称:基础动作类实现文件
 *  简要描述: Seekur机器人相关基础动作的类的实现文件
 *   
 *  创建日期: 2017/09/6
 *  作者: 吴厚波
 *  说明: 无
 *   
 *  修改日期: 
 *  作者: 
 *  说明: 
 ******************************************************************/ 

#include "BaseAction.h"

#pragma region 连接测试类
ConnHandler::ConnHandler(ArRobot *robot) :
myConnectedCB(this, &ConnHandler::connected),  
	myConnFailCB(this, &ConnHandler::connFail),
	myDisconnectedCB(this, &ConnHandler::disconnected)

{
	myRobot = robot;
	myRobot->addConnectCB(&myConnectedCB, ArListPos::FIRST);
	myRobot->addFailedConnectCB(&myConnFailCB, ArListPos::FIRST);
	myRobot->addDisconnectNormallyCB(&myDisconnectedCB, ArListPos::FIRST);
	myRobot->addDisconnectOnErrorCB(&myDisconnectedCB, ArListPos::FIRST);
}

// 连接失败时调用
void ConnHandler::connFail(void)
{
	printf("directMotionDemo connection handler: Failed to connect.\n");
	myRobot->stopRunning();
	Aria::exit(1);
	return;
}

// 启动发动机，关闭声纳和amigobot
void ConnHandler::connected(void)
{
	printf("directMotionDemo connection handler: Connected\n");
	myRobot->comInt(ArCommands::SONAR, 0);
	myRobot->comInt(ArCommands::ENABLE, 1);
	myRobot->comInt(ArCommands::SOUNDTOG, 0);
}

// 连接断开时结束程序
void ConnHandler::disconnected(void)
{
	printf("directMotionDemo connection handler: Lost connection, exiting program.\n");
	Aria::exit(0);
}
#pragma endregion

