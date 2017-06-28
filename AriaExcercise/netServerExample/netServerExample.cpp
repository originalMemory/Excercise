#include "Aria.h"

// 使用网络服务时，启动该程序，然后使用“telnet”程序连接本地TCP端口7171，连接方法为命令：telnet localhost 7171
// 当连接提示需要密码时，输入密码，并按回车键。
// 使用help命令可以显示可用命令。
// 使用“test”或“test2”选择是否添加额外参数
// 在不使用ArNetworking库和MobileEyes的情况下，ArNetServer是一个提供通过无线网络简单控制程序在车载电脑上运行的方法。
// 也可以在一个程序内同时使用两种方法

// 本函数只输出客户端输入与返回的数据
void test(char **argv, int argc, ArSocket *socket)
{
	int i;
	printf("Client said: ");
	for (i = 0; i < argc; ++i)
		printf("\t%s\n", argv[i]);
	printf("\n");
	socket->writeString("Thank you, command received.");
}


int main(int argc, char **argv)
{
	Aria::init();
	// 创建ArNetServer对象
	ArNetServer server;
	// 创建回调函数
	ArGlobalFunctor3<char **, int, ArSocket *> testCB(&test);

	// 在不连接机器的情况下连接端口7171，允许多个客户同时连接
	if (!server.open(NULL, 8101, "password", true))
	{
		printf("Could not open server.\n");
		Aria::exit(1);
		return 1;
	}

	// 添加测试命令
	server.addCommand("test", &testCB, "this simply prints out the command given on the server");
	server.addCommand("test2", &testCB, "this simply prints out the command given on the server");

	//server.setLoggingDataSent(true);
	//server.setLoggingDataReceived(true);
	// run while the server is running
	while (server.isOpen() && Aria::getRunning())
	{
		server.runOnce();
		ArUtil::sleep(1);
	}
	server.close();
	Aria::exit(0);
	return 0;  
}