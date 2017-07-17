using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VipManager.FormControl;
using VipManager.Helper;

namespace VipManager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //初始化系统环境
            Config.Init();
            //定义系统退出事件, 并启动登录界面.
            Application.ApplicationExit += new EventHandler(Program.Application_ApplicationExit);

            Application.Run(new Mains());
        }

        //当程序退出时
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Config.Dispose();

            try
            {
                ////指定备份文件
                //DirectoryInfo dbBackDir = new DirectoryInfo(Application.StartupPath + "\\BACKUP");

                ////如果目录不存在则创建它.
                //if (!dbBackDir.Exists) dbBackDir.Create();

                ////备份到自动备份文件夹
                //FileInfo file = new FileInfo(Application.StartupPath + "\\Database\\ImgDB.DB");
                //file.CopyTo(dbBackDir.FullName + "\\AutoBak.DB", true);
            }
            catch
            {

            }
        }
    }
}
