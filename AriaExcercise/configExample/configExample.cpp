#include "Aria.h"

//演示使用ArConfig的示例程序此程序显示如何使用ArConfig来存储配置参数并从文件加载/保存它们。

class ConfigExample
{
  ArConfig* myConfig;
  int myIntParam;
  double myDoubleParam;
  bool myBoolParam;
  char myStringParam[256];
  ArRetFunctorC<bool, ConfigExample> myProcessConfigCB;

public:
  ConfigExample():
    myIntParam(0),
    myDoubleParam(0.5),
    myBoolParam(false),
    myProcessConfigCB(this, &ConfigExample::processConfigFile)
  {
    // 全局Aria类中包含一个ArConfig对象，可以创建一个新ArConfig对象
	// 但使用默认对象可以在不同程序共享设置
    // 在ArConfig中存储一项设置参数时，首先要将要其添加到ArConfig对象中。
	// 参数是分块存储的，通过一个ArConfigArg对象内的指针影响变量
    ArConfig* config = Aria::getConfig();
	// 为一个块添加评论
    config->setSectionComment("Example Section", "Contains parameters created by the configExample");

    // 添加一个-10到10的整数参数
	// ArConfigArg参数意义分别为：参数名，指针，描述，最小值，最大值
	// addParamc参数为：ArConfigArg对象，块名，优先级
    config->addParam( ArConfigArg("ExampleIntegerParameter", &myIntParam, "Example parameter integer.", -10, 10), "Example Section", ArPriority::NORMAL);
    
    // 添加一个-0.0到1.0的小说参数
    config->addParam( ArConfigArg("ExampleDoubleParameter", &myDoubleParam, "Example double precision floating point number.", 0.0, 1.0), "Example Section", ArPriority::NORMAL);

    // 基本参数可以调高优先级
    config->addParam( ArConfigArg("ExampleBoolParameter", &myBoolParam, "Example boolean parameter."), "Example Section", ArPriority::IMPORTANT);

    // 不重要参数可以调低其优先级
    myStringParam[0] = '\0';  // 字符串置空
    config->addParam( ArConfigArg("ExampleStringParameter", myStringParam, "Example string parameter.", 256), "Example Section", ArPriority::TRIVIAL);

    // 设置配置修改时的回调函数，以防万一需要对参数改变有回应时使用
    config->addProcessFileCB(&myProcessConfigCB, 0);		//第二参数为优先级
  }

  
  // 当载入新文件引起配置改变时调用的方法，返回true或false来指示是否出错
  bool processConfigFile() 
  {
    ArLog::log(ArLog::Normal, "configExample: Config changed. New values: int=%d, float=%f, bool=%s, string=\"%s\".", myIntParam, myDoubleParam, myBoolParam?"true":"false", myStringParam);
    return true;
  }
};
  
int main(int argc, char **argv)
{
  Aria::init();
  ArArgumentParser argParser(&argc, argv);
  argParser.loadDefaultArguments();
  if (argc < 2 || !Aria::parseArgs() || argParser.checkArgument("-help"))
  {
    ArLog::log(ArLog::Terse, "configExample usage: configExample <config file>.\nFor example, \"configExample examples/configExample.cfg\".");
    Aria::logOptions();
    Aria::exit(1);
    return 1;
  }
  
  
  ConfigExample configExample;

  // 通过Aria加载一个配置文件到ArConfig对象中
  // 通常情况下配置位置需在主ARIA目录中（例：/usr/local/Aria或通过$ARIA环境变量指定的目录中）
  char error[512];
  const char* filename = argParser.getArg(1);
  ArConfig* config = Aria::getConfig();
  ArLog::log(ArLog::Normal, "configExample: loading configuration file \"%s\"...", filename);
  if (! config->parseFile(filename, true, false, error, 512) )
  {
    ArLog::log(ArLog::Terse, "configExample: Error loading configuration file \"%s\" %s. Try \"examples/configExample.cfg\".", filename, error);
    Aria::exit(-1);
    return -1;
  }

  ArLog::log(ArLog::Normal, "configExample: Loaded configuration file \"%s\".", filename);
  
  // 在改变一个配置的什后，调用回调函数
  ArConfigSection* section = config->findSection("Example Section");
  if (section)
  {
    ArConfigArg* arg = section->findParam("ExampleBoolParameter");
    if (arg)
    {
      arg->setBool(!arg->getBool());
      if (! config->callProcessFileCallBacks(false, error, 512) )
      {
        ArLog::log(ArLog::Terse, "configExample: Error processing modified config: %s.", error);
      }
      else
      {
        ArLog::log(ArLog::Normal, "configExample: Successfully modified config and invoked callbacks.");
      }
    }
  }

  // 保存配置文件
  ArLog::log(ArLog::Normal, "configExample: Saving configuration...");
  if(!config->writeFile(filename))
  {
    ArLog::log(ArLog::Terse, "configExample: Error saving configuration to file \"%s\"!", filename);
  }

  // 结束程序
  ArLog::log(ArLog::Normal, "configExample: end of program.");
  Aria::exit(0);
  return 0;
}