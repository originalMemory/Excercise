#include <string>
#include "Aria.h"


/*
  该类包含一个回调函数。与这些回调函数相关的函子将会被传递给DriverClass
*/
class CallbackContainer
{
public:

  void callback1();
  void callback2(int i);
  bool callback3(const char *str);
};

void CallbackContainer::callback1()
{
  printf("CallbackContainer::callback1 called.\n");
}

void CallbackContainer::callback2(int i)
{
  printf("CallbackContainer::callback2 called with argument of '%d'\n", i);
}

bool CallbackContainer::callback3(const char *str)
{
  printf("CallbackContainer::callback3 called with argument of '%s'.\n", str);
  return(true);
}

/* 
 *函子也可以调用全局函数
 */

void globalCallback()
{
  printf("globalCallback() called.\n");
}


/*
  这是一个“驾驶”类，它有三种不同类型的功能，并会调用三种函子。
  这是函子的典型用例：在松散耦合的对象之间传递信息或事件通知。
*/
class DriverClass
{
public:

  void invokeFunctors();

  void setCallback1(ArFunctor *func) {myFunc1=func;}
  void setCallback2(ArFunctor1<int> *func) {myFunc2=func;}
  void setCallback3(ArRetFunctor1<bool, const char *> *func) {myFunc3=func;}


protected:

  ArFunctor *myFunc1;
  ArFunctor1<int> *myFunc2;
  ArRetFunctor1<bool, const char *> *myFunc3;
};

void DriverClass::invokeFunctors()
{
  bool ret;

  printf("Invoking functor1... ");
  myFunc1->invoke();

  printf("Invoking functor2... ");
  myFunc2->invoke(23);
     

  /*
     对有返回值的函子，使用invorkeR()来获取返回值。
	 invorke()也能调用此类函子，但无法获取返回值。(对返回值是指针的函子可能造成内存泄漏)
  */
  printf("Invoking functor3... ");
  ret=myFunc3->invokeR("This is a string argument");
  if (ret)
    printf("\t-> functor3 returned 'true'\n");
  else
    printf("\t-> functor3 returned 'false'\n");
}

int main()
{
  CallbackContainer cb;
  DriverClass driver;

  ArFunctorC<CallbackContainer> functor1(cb, &CallbackContainer::callback1);
  ArFunctor1C<CallbackContainer, int> functor2(cb, &CallbackContainer::callback2);
  ArRetFunctor1C<bool, CallbackContainer, const char *>
    functor3(cb, &CallbackContainer::callback3);

  driver.setCallback1(&functor1);
  driver.setCallback2(&functor2);
  driver.setCallback3(&functor3);

  driver.invokeFunctors();

  /* 可以将函子指向全局函数 */
  ArGlobalFunctor globalFunctor(&globalCallback);
  printf("Invoking globalFunctor... ");
  globalFunctor.invoke();

  /* 可以在回调时加入参数，使得调用该函子时均为相同值
   */
  ArFunctor1C<CallbackContainer, int> functor4(cb, &CallbackContainer::callback2, 42);
  printf("Invoking functor with constant argument... ");
  functor4.invoke();

  /* 函数可以被父接口类实例化，只要它们的调用不需要参数。
   */
  ArFunctor* baseFunctor = &functor4;
  printf("Invoking downcast functor... ");
  baseFunctor->invoke();


  return(0);
}