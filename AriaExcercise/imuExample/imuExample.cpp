/* 从Seekur示例程序收集IMU数据的示例，演示如何从Seekur级机器人（来自SeekurOS）的IMU获取IMU数据。 只有一些Seekurs有IMU。
[部分先锋机器人也有陀螺仪被集成到机器人中，并且通常会自动校正数据，或者可以选择性地返回数据（在ArAnalogGyro中使用不同的数据包和不同的ARIA接口）。
数据以ID为0x9A的自定义数据包返回。它需客户端命令26。*/
#include "Aria.h"
#include <time.h>

bool imuPacketHandler(ArRobotPacket* packet) {		//ArRobotPacket是机器人与客户端通信时的数据包类型
	//判断是否是IMU的数据
	if (packet->getID() != 0x9a) {
		//ArLog::log(ArLog::Normal, "ignoring packet with id: %d", packet->getID());
		return false;
	}
	//bufToByte()是获取一个有符号的Byte型数据
	int timeSince = packet->bufToByte();		//获取从读数开始后经过了多少毫秒
	int numPerAxis = packet->bufToByte();		//获取每一轴有多少读数

	ArLog::log(ArLog::Normal, "got IMUpac, %dms since readings took place, %d readings per axis", timeSince, numPerAxis);
	int i;
	int range;
	double x = 0.0;
	double y = 0.0;
	double z = 0.0;
	double multiplier = 0.0;
	double offset = 0.0;
	int numGyroAxes = packet->bufToByte();		//陀螺仪读数
	for (i=1; i <= numPerAxis; i++) {
		x = 0.0;
		y = 0.0;
		z = 0.0;
		range = packet->bufToByte();
		if (range == 3) {
			multiplier = 0.07326;
			offset = 0.0;
		}
		else if (range == 2) {
			multiplier = 0.03663;
			offset = 0.0;
		}
		else if (range == 1) {
			multiplier = 0.01832;
			offset = 0.0;
		}
		if (numGyroAxes >= 1) {
			x = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numGyroAxes >= 2) {
			y = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numGyroAxes >= 3) {
			z = (packet->bufToByte2() * multiplier) + offset;
		}
		ArLog::log(ArLog::Normal, "gyro:  reading %d of %d,       x: %10.4f deg/s,   y: %10.4f deg/s,   z: %10.4f deg/s", 
			i, numPerAxis, x, y, z);
	}
	int numAccelAxes = packet->bufToByte();
	for (i=1; i <= numPerAxis; i++) {
		x = 0.0;
		y = 0.0;
		z = 0.0;
		multiplier = 0.002522 * 9806.65;
		offset = 0.0;
		if (numAccelAxes >= 1) {
			x = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numAccelAxes >= 2) {
			y = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numAccelAxes >= 3) {
			z = (packet->bufToByte2() * multiplier) + offset;
		}
		ArLog::log(ArLog::Normal, "accel: reading %d of %d,       x: %10.4f mg,      y: %10.4f mg,      z: %10.4f mg",
			i, numPerAxis, x, y, z);
	}
	int numTemperatureAxes = packet->bufToByte();
	for (i=1; i <= numPerAxis; i++) {
		x = 0.0;
		y = 0.0;
		z = 0.0;
		multiplier = 0.1453;
		offset = 25.0;
		if (numTemperatureAxes >= 1) {
			x = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numTemperatureAxes >= 2) {
			y = (packet->bufToByte2() * multiplier) + offset;
		}
		if (numTemperatureAxes >= 3) {
			z = (packet->bufToByte2() * multiplier) + offset;
		}
		ArLog::log(ArLog::Normal, "temperature: reading %d of %d, x: %10.4f degC,    y: %10.4f degC,    z: %10.4f degC",
			i, numPerAxis, x, y, z);
	}
	ArLog::log(ArLog::Normal, "");
	ArLog::log(ArLog::Normal, "");

	return true;
}

int main(int argc, char **argv) {
	Aria::init();
	ArLog::init(ArLog::StdOut, ArLog::Normal, "", true);
	ArArgumentParser parser(&argc, argv);
	ArRobot *robot = new ArRobot;
	ArSimpleConnector simpleConnector(&parser);
	ArKeyHandler keyHandler;
	Aria::setKeyHandler(&keyHandler);
	robot->attachKeyHandler(&keyHandler);

	parser.loadDefaultArguments();
	if (!simpleConnector.parseArgs() || !parser.checkHelpAndWarnUnparsed(1)) {
		simpleConnector.logOptions();
		keyHandler.restore();
		Aria::exit(1);
	}
	if (!simpleConnector.connectRobot(robot)) {
		ArLog::log(ArLog::Terse, "Error connecting to robot.");
		Aria::shutdown();
		keyHandler.restore();
		Aria::exit(1);
	}
	ArLog::log(ArLog::Normal, "Connected to robot.");
	ArGlobalRetFunctor1<bool, ArRobotPacket *> myImuPacketHandler(&imuPacketHandler);
	robot->addPacketHandler(&myImuPacketHandler, ArListPos::FIRST);
	robot->runAsync(true);
	ArUtil::sleep(500);
	robot->lock();
	robot->comInt(26, 2);  // 获取IMU读数
	robot->unlock();
	robot->waitForRunExit();
	Aria::shutdown();
	exit(0);
}