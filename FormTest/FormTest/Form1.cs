using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormTest
{
    public partial class Form1 : Form
    {
        private Form2 progressForm = new Form2();
        //代理定义，可以在Invoke时传入相应参数
        private delegate void funHandle(int nValue);
        private funHandle myHandle = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //启动线程
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadFun));  
            thread.Start();
        }

        /// <summary>
        /// 线程函数中调用的函数
        /// </summary>
        private void ShowProgressBar()
        {
            myHandle = new funHandle(progressForm.SetProgressValue);
            progressForm.Show();
        }

        /// <summary>
        /// 线程函数，用于处理调用
        /// </summary>
        private void ThreadFun()
        {
            MethodInvoker mi = new MethodInvoker(ShowProgressBar);
            this.BeginInvoke(mi);
            System.Threading.Thread.Sleep(1000);
            for (int i = 0; i < 1000; i++)
            {
                System.Threading.Thread.Sleep(5);
                //这里直接调用代理
                this.Invoke(this.myHandle, new object[] { i / 10 });
            }
        }
    }
}
