#include "highgui.h" 
int main( int argc,char** argv ) 
{ 
	//IplImage* img = cvLoadImage( argv[0] ); 
	IplImage* img = cvLoadImage("F:\\QQ图片20180506214207.jpg"); 
	//IplImage *img = cvLoadImage(" F:\opencv_study\test1_imread4\lena.bmp"); 
	//如果用上面这行代码则无法显示，原因是在绝对路径之前出现空格。这个vs要求也太过分了。 
	cvNamedWindow("Example1", CV_WINDOW_AUTOSIZE ); 
	//cvNamedWindow("Example1", 0 ); 
	cvShowImage("Example1", img ); 
	cvWaitKey(0); 
	cvReleaseImage( &img );//后面这两句省去也可以，但是养成习惯每次调用这些函数显示释放资源总是有好处的。 
	cvDestroyWindow("Example1"); 
} 