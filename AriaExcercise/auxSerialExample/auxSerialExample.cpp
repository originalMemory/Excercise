#include "Aria.h"

// our robot object
ArRobot *robot;

bool getAuxPrinter(ArRobotPacket *packet)
{
  char c;
  
  // If this is not an aux. serial data packet, then return false to allow other
  // packet handlers to recieve the packet. The packet type ID numbers are found
  // in the description of the GETAUX commands in the robot manual.
  if (packet->getID() != 0xb0) // 0xB0 is SERAUXpac. SERAUX2pac is 0xB8, SERAUX3pac is 0xC8.
    return false;

  // Get bytes out of the packet buffer and print them.
  while (packet->getReadLength () < packet->getLength() - 2)
  {
    c = packet->bufToUByte();
    if (c == '\r' || c == '\n')
    {
      putchar('\n');

      // How to send data to the serial port. See robot manual
      // (but note that TTY2 is for the AUX1 port, TTY3 for AUX2, etc.)
      robot->comStr(ArCommands::TTY2, "Hello, World!\n\r");
    }
    else
      putchar(c);
    fflush(stdout);
  }

  
  // Request more data:
  robot->comInt(ArCommands::GETAUX, 1);

  // To request 12 bytes at a time, specify that instead:
  //robot->comInt(ArCommands::GETAUX, 12);

  // If you wanted to recieve information from the second aux. serial port, use
  // the GETAUX2 command instead; but the packet returned will also have a
  // different type ID.
  //robot->comInt(ArCommands::GETAUX2, 1);
  

  // Return true to indicate to ArRobot that we have handled this packet.
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
  
  // This is a global pointer so the global functions can use it.
  robot = new ArRobot;

  // functor for the packet handler
  ArGlobalRetFunctor1<bool, ArRobotPacket *> getAuxCB(&getAuxPrinter);
  // add our packet handler as the first one in the list
  robot->addPacketHandler(&getAuxCB, ArListPos::FIRST);

  // Connect to the robot
  if(!conn.connectRobot(robot))
  {
      ArLog::log(ArLog::Terse, "getAuxExample: Error connecting to the robot.");
      return 2;
  }

  ArLog::log(ArLog::Normal, "getAuxExample: Connected to the robot. Sending command to change AUX1 baud rate to 9600...");
  robot->comInt(ArCommands::AUX1BAUD, 0); // See robot manual

  // Send the first GETAUX request
  robot->comInt(ArCommands::GETAUX, 1);

  // If you wanted to recieve information from the second aux. serial port, use
  // the GETAUX2 command instead; but the packet returned will also have a
  // different type ID.
  //robot->comInt(ArCommands::GETAUX2, 1);

  // run the robot until disconnect, then shutdown and exit.
  robot->run(true);
  Aria::exit(0);
  return 0;  
}