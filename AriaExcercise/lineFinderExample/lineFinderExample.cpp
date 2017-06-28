/* Aria线性测量仪实用程序的简单示例，其使用激光测距仪的数据来检测持续的点。 该示例程序将不断地搜索激光测距仪的传感器读数中的线状图案。
如果按“f”键，这些点将保存在“点”和“线”文件中。 使用箭头键或操纵杆遥控机器人。 */
#include "Aria.h"

int main(int argc, char **argv)
{
	Aria::init();

	ArSimpleConnector connector(&argc, argv);
	ArRobot robot;
	ArSick sick;		//控制SICK LMS-2XX系列的激光

	if (!Aria::parseArgs() || argc > 1)
	{
		Aria::logOptions();
		Aria::exit(1);
		return 1;
	}

	ArKeyHandler keyHandler;
	Aria::setKeyHandler(&keyHandler);
	robot.attachKeyHandler(&keyHandler);


	robot.addRangeDevice(&sick);

	// 创建ArLineFinder对象，设置其在运行中输出详细信息
	ArLineFinder lineFinder(&sick);
	lineFinder.setVerbose(true);

	// 为按键绑定回调函数（ArLineFinder::getLinesAndSaveThem()）
	// 搜索在当前激光测距仪读数中的线条，并以“points”和“lines”为文件名存储
	ArFunctorC<ArLineFinder> findLineCB(&lineFinder, 
		&ArLineFinder::getLinesAndSaveThem);
	keyHandler.addKeyHandler('f', &findLineCB);
	keyHandler.addKeyHandler('F', &findLineCB);


	ArLog::log(ArLog::Normal, "lineFinderExample: connecting to robot...");
	if (!connector.connectRobot(&robot))
	{
		printf("Could not connect to robot... exiting\n");
		Aria::exit(1);  // exit program with error code 1
		return 1;
	}
	robot.runAsync(true);

	// 匹配激光
	ArLog::log(ArLog::Normal, "lineFinderExample: connecting to SICK laser...");
	connector.setupLaser(&sick);
	sick.runAsync();
	if (!sick.blockingConnect())
	{
		printf("Could not connect to SICK laser... exiting\n");
		Aria::exit(1);
		return 1;
	}

	printf("If you press the 'f' key the points and lines found will be saved\n");
	printf("Into the 'points' and 'lines' file in the current working directory\n");

	robot.waitForRunExit();
	Aria::exit(0);
	return 0;
}