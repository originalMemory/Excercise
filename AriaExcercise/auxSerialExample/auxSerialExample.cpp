#include "Aria.h"
//辅助串行端口数据获取实例

// our robot object
ArRobot *robot;

bool getAuxPrinter(ArRobotPacket *packet)
{
  char c;
  
  // 如果这不是一个串行数据包则返回false，允许其他包句柄接收这个包
  // 包的类型ID编号在机器人手册的GETAUX命令描述中
  if (packet->getID() != 0xb0) // 0xB0 is SERAUXpac. SERAUX2pac is 0xB8, SERAUX3pac is 0xC8.
    return false;

  // 从包缓存中获取数据并打印
  while (packet->getReadLength () < packet->getLength() - 2)
  {
    c = packet->bufToUByte();
    if (c == '\r' || c == '\n')
    {
      putchar('\n');

      // 在机器人手册中有如何发送数据给串口
      // (TTY2用于AUX1端口，TTY3用于AUX2)
      robot->comStr(ArCommands::TTY2, "Hello, World!\n\r");
    }
    else
      putchar(c);
    fflush(stdout);
  }

  
  // 请求更多数据
  robot->comInt(ArCommands::GETAUX, 1);

  // 若一次请求12个字节，使用:
  //robot->comInt(ArCommands::GETAUX, 12);

  // 如果想从第二个辅助串口获取数据，使用GETAUX2命令，但是返回的数据包的类型ID也不同
  //robot->comInt(ArCommands::GETAUX2, 1);
  

  // 返回true表示已经处理了这个包
  return true;
}
  

int main(int argc, char **argv) 
{
  Aria::init();

  ArArgumentParser argParser(&argc, argv);
  ArSimpleConnector conn(&argParser);
  argParser.loadDefaultArguments();
  if(!Aria::parseArgs() || !argParser.checkHelpAndWarnUnparsed())
  {
    Aria::logOptions();
    return 1;
  }
  
  // 全局指针
  robot = new ArRobot;

  // 包句柄函数
  ArGlobalRetFunctor1<bool, ArRobotPacket *> getAuxCB(&getAuxPrinter);
  // 将本次数据包句柄添加到机器人的包句柄列表第一位
  robot->addPacketHandler(&getAuxCB, ArListPos::FIRST);

  // 连接机器人
  if(!conn.connectRobot(robot))
  {
      ArLog::log(ArLog::Terse, "getAuxExample: Error connecting to the robot.");
      return 2;
  }

  ArLog::log(ArLog::Normal, "getAuxExample: Connected to the robot. Sending command to change AUX1 baud rate to 9600...");
  robot->comInt(ArCommands::AUX1BAUD, 0); // See robot manual

  // 请求第1个串口
  robot->comInt(ArCommands::GETAUX, 1);

  robot->run(true);
  Aria::exit(0);
  return 0;  
}