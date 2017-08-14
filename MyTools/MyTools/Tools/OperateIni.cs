using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MyTools.Tools
{
    class OperateIni
    {
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 读Ini文件
        /// <summary>
        /// 读取ini文件
        /// </summary>
        /// <param name="Section">区块名</param>
        /// <param name="Key">属性名</param>
        /// <param name="iniFilePath">ini文件路径，为空时打开根目录Config.ini</param>
        /// <param name="NoText">值为空时的默认值</param>
        /// <returns></returns>
        public static string ReadIniData(string Section, string Key, string iniFilePath = null, string NoText = null)
        {
            if (string.IsNullOrEmpty(iniFilePath))
            {
                iniFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + System.Configuration.ConfigurationManager.AppSettings["config"];
            }
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件
        /// <summary>
        /// 写ini文件
        /// </summary>
        /// <param name="Section">区块名</param>
        /// <param name="Key">属性名</param>
        /// <param name="Value">值</param>
        /// <param name="iniFilePath">ini文件路径，为空时打开根目录Config.ini</param>
        /// <returns></returns>
        public static bool WriteIniData(string Section, string Key, object Value, string iniFilePath = null)
        {
            string strValue = Value.ToString();
            if (string.IsNullOrEmpty(iniFilePath))
            {
                iniFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + System.Configuration.ConfigurationManager.AppSettings["config"];
            }
            if (!File.Exists(iniFilePath))
            {
                File.Create(iniFilePath);
            }
            long OpStation = WritePrivateProfileString(Section, Key, strValue, iniFilePath);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
