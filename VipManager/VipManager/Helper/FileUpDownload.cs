using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace VipManager.Helper
{
        /// <summary>  
    /// ftp方式文件下载上传  
    /// </summary>  
    public static class FileUpDownload  
    {  
        #region 变量属性  
        /// <summary>  
        /// Ftp服务器ip  
        /// </summary>  
        public static string FtpServerIP = "101.200.43.118";
        /// <summary>  
        /// Ftp 指定用户名  
        /// </summary>  
        public static string FtpUserID = "FTPUser"; 
        /// <summary>  
        /// Ftp 指定用户密码  
        /// </summary>  
        public static string FtpPassword = "OpgEtT9gXLm1n";
 
        #endregion  
 
        #region 从FTP服务器下载文件，指定本地路径和本地文件名  
        /// <summary>  
        /// 从FTP服务器下载文件，指定本地路径和本地文件名  
        /// </summary>  
        /// <param name="remoteFileName">远程文件名</param>  
        /// <param name="localFileName">保存本地的文件名（包含路径）</param>  
        /// <param name="ifCredential">是否启用身份验证（false：表示允许用户匿名下载）</param>  
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <returns>是否下载成功</returns>  
        public static bool FtpDownload(string remoteFileName, string localFileName, bool ifCredential, Action<int, int> updateProgress = null)  
        {  
            FtpWebRequest reqFTP, ftpsize;  
            Stream ftpStream = null;  
            FtpWebResponse response = null;  
            FileStream outputStream = null;  
            try  
            {  
  
                outputStream = new FileStream(localFileName, FileMode.Create);  
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)  
                {  
                    throw new Exception("ftp下载目标服务器地址未设置！");  
                }  
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + remoteFileName);  
                ftpsize = (FtpWebRequest)FtpWebRequest.Create(uri);  
                ftpsize.UseBinary = true;       //以二进制传输数据
  
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);  
                reqFTP.UseBinary = true;  
                reqFTP.KeepAlive = false;       //使用过后销毁与服务器的链接
                if (ifCredential)//使用用户身份认证  
                {  
                    ftpsize.Credentials = new NetworkCredential(FtpUserID, FtpPassword);  
                    reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);  
                }
                //获取文件大小
                ftpsize.Method = WebRequestMethods.Ftp.GetFileSize;  
                FtpWebResponse re = (FtpWebResponse)ftpsize.GetResponse();  
                long totalBytes = re.ContentLength;         //文件大小
                re.Close();  
  
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;  
                response = (FtpWebResponse)reqFTP.GetResponse();  
                ftpStream = response.GetResponseStream();  
  
                //更新进度    
                if (updateProgress != null)  
                {  
                    updateProgress((int)totalBytes, 0);//更新进度条     
                }  
                long totalDownloadedByte = 0;       //已下载文件大小
                int bufferSize = 2048;  
                int readCount;  
                byte[] buffer = new byte[bufferSize];  
                readCount = ftpStream.Read(buffer, 0, bufferSize);  
                while (readCount > 0)  
                {  
                    totalDownloadedByte = readCount + totalDownloadedByte;  
                    outputStream.Write(buffer, 0, readCount);  
                    //更新进度    
                    if (updateProgress != null)  
                    {  
                        updateProgress((int)totalBytes, (int)totalDownloadedByte);//更新进度条     
                    }  
                    readCount = ftpStream.Read(buffer, 0, bufferSize);  
                }  
                ftpStream.Close();  
                outputStream.Close();  
                response.Close();  
                return true;  
            }  
            catch (Exception)  
            {  
                return false;  
                throw;  
            }  
            finally  
            {  
                if (ftpStream != null)  
                {  
                    ftpStream.Close();  
                }  
                if (outputStream != null)  
                {  
                    outputStream.Close();  
                }  
                if (response != null)  
                {  
                    response.Close();  
                }  
            }  
        }  
        /// <summary>  
        /// 从FTP服务器下载文件，指定本地路径和本地文件名（支持断点下载）  
        /// </summary>  
        /// <param name="remoteFileName">远程文件名</param>  
        /// <param name="localFileName">保存本地的文件名（包含路径）</param>  
        /// <param name="ifCredential">是否启用身份验证（false：表示允许用户匿名下载）</param>  
        /// <param name="size">已下载文件流大小</param>  
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <returns>是否下载成功</returns>  
        public static bool FtpBrokenDownload(string remoteFileName, string localFileName, bool ifCredential, long size, Action<int, int> updateProgress = null)  
        {  
            FtpWebRequest reqFTP, ftpsize;  
            Stream ftpStream = null;  
            FtpWebResponse response = null;  
            FileStream outputStream = null;  
            try  
            {  
 
                outputStream = new FileStream(localFileName, FileMode.Append);  
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)  
                {  
                    throw new Exception("ftp下载目标服务器地址未设置！");  
                }  
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + remoteFileName);  
                ftpsize = (FtpWebRequest)FtpWebRequest.Create(uri);  
                ftpsize.UseBinary = true;  
                ftpsize.ContentOffset = size;  
  
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);  
                reqFTP.UseBinary = true;  
                reqFTP.KeepAlive = false;  
                reqFTP.ContentOffset = size;  
                if (ifCredential)//使用用户身份认证  
                {  
                    ftpsize.Credentials = new NetworkCredential(FtpUserID, FtpPassword);  
                    reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);  
                }  
                ftpsize.Method = WebRequestMethods.Ftp.GetFileSize;  
                FtpWebResponse re = (FtpWebResponse)ftpsize.GetResponse();  
                long totalBytes = re.ContentLength;  
                re.Close();  
  
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;  
                response = (FtpWebResponse)reqFTP.GetResponse();  
                ftpStream = response.GetResponseStream();  
  
                //更新进度    
                if (updateProgress != null)  
                {  
                    updateProgress((int)totalBytes, 0);//更新进度条     
                }  
                long totalDownloadedByte = 0;  
                int bufferSize = 2048;  
                int readCount;  
                byte[] buffer = new byte[bufferSize];  
                readCount = ftpStream.Read(buffer, 0, bufferSize);  
                while (readCount > 0)  
                {  
                    totalDownloadedByte = readCount + totalDownloadedByte;  
                    outputStream.Write(buffer, 0, readCount);  
                    //更新进度    
                    if (updateProgress != null)  
                    {  
                        updateProgress((int)totalBytes, (int)totalDownloadedByte);//更新进度条     
                    }  
                    readCount = ftpStream.Read(buffer, 0, bufferSize);  
                }  
                ftpStream.Close();  
                outputStream.Close();  
                response.Close();  
                return true;  
            }  
            catch (Exception)  
            {  
                return false;  
                throw;  
            }  
            finally  
            {  
                if (ftpStream != null)  
                {  
                    ftpStream.Close();  
                }  
                if (outputStream != null)  
                {  
                    outputStream.Close();  
                }  
                if (response != null)  
                {  
                    response.Close();  
                }  
            }  
        }  
  
        /// <summary>  
        /// 从FTP服务器下载文件，指定本地路径和本地文件名  
        /// </summary>  
        /// <param name="remoteFileName">远程文件名</param>  
        /// <param name="localFileName">保存本地的文件名（包含路径）</param>  
        /// <param name="ifCredential">是否启用身份验证（false：表示允许用户匿名下载）</param>  
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <param name="brokenOpen">是否断点下载：true 会在localFileName 找是否存在已经下载的文件，并计算文件流大小</param>  
        /// <returns>是否下载成功</returns>  
        public static bool FtpDownload(string remoteFileName, string localFileName, bool ifCredential, bool brokenOpen, Action<int, int> updateProgress = null)  
        {  
            if (brokenOpen)  
            {  
                try  
                {  
                    long size = 0;  
                    if (File.Exists(localFileName))  
                    {  
                        using (FileStream outputStream = new FileStream(localFileName, FileMode.Open))  
                        {  
                            size = outputStream.Length;  
                        }  
                    }  
                    return FtpBrokenDownload(remoteFileName, localFileName, ifCredential, size, updateProgress);  
                }  
                catch  
                {  
                    throw;  
                }  
            }  
            else  
            {  
                return FtpDownload(remoteFileName, localFileName, ifCredential, updateProgress);  
            }  
        }  
        #endregion  
 
        #region 上传文件到FTP服务器  
        /// <summary>  
        /// 上传文件到FTP服务器  
        /// </summary>  
        /// <param name="localFullPath">本地带有完整路径的文件名</param>  
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <returns>是否下载成功</returns>  
        public static bool FtpUploadFile(string localFullPathName, Action<int, int> updateProgress = null)  
        {  
            FtpWebRequest reqFTP;  
            Stream stream = null;  
            FtpWebResponse response = null;  
            FileStream fs = null;  
            try  
            {  
                FileInfo finfo = new FileInfo(localFullPathName);  
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)  
                {  
                    throw new Exception("ftp上传目标服务器地址未设置！");  
                }  
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + finfo.Name);  
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);  
                reqFTP.KeepAlive = false;  
                reqFTP.UseBinary = true;  
                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);//用户，密码  
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//向服务器发出下载请求命令  
                reqFTP.ContentLength = finfo.Length;//为request指定上传文件的大小  
                response = reqFTP.GetResponse() as FtpWebResponse;  
                reqFTP.ContentLength = finfo.Length;  
                int buffLength = 1024;  
                byte[] buff = new byte[buffLength];  
                int contentLen;  
                fs = finfo.OpenRead();  
                stream = reqFTP.GetRequestStream();  
                contentLen = fs.Read(buff, 0, buffLength);  
                int allbye = (int)finfo.Length;  
                //更新进度    
                if (updateProgress != null)  
                {  
                    updateProgress((int)allbye, 0);//更新进度条     
                }  
                int startbye = 0;  
                while (contentLen != 0)  
                {  
                    startbye = contentLen + startbye;  
                    stream.Write(buff, 0, contentLen);  
                    //更新进度    
                    if (updateProgress != null)  
                    {  
                        updateProgress((int)allbye, (int)startbye);//更新进度条     
                    }  
                    contentLen = fs.Read(buff, 0, buffLength);  
                }  
                stream.Close();  
                fs.Close();  
                response.Close();  
                return true;  
  
            }  
            catch (Exception)  
            {  
                return false;  
                throw;  
            }  
            finally  
            {  
                if (fs != null)  
                {  
                    fs.Close();  
                }  
                if (stream != null)  
                {  
                    stream.Close();  
                }  
                if (response != null)  
                {  
                    response.Close();  
                }  
            }  
        }  
  
        /// <summary>  
        /// 上传文件到FTP服务器(断点续传)  
        /// </summary>  
        /// <param name="localFullPath">本地文件全路径名称：C:\Users\JianKunKing\Desktop\IronPython脚本测试工具</param>  
        /// <param name="remoteFilepath">远程文件所在文件夹路径</param>  
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>  
        /// <returns></returns>         
        public static bool FtpUploadBroken(string localFullPath, string remoteFilepath, Action<int, int> updateProgress = null)  
        {  
            if (remoteFilepath == null)  
            {  
                remoteFilepath = "";  
            }  
            string newFileName = string.Empty;  
            bool success = true;  
            FileInfo fileInf = new FileInfo(localFullPath);  
            long allbye = (long)fileInf.Length;  
            if (fileInf.Name.IndexOf("#") == -1)  
            {  
                newFileName = RemoveSpaces(fileInf.Name);  
            }  
            else  
            {  
                newFileName = fileInf.Name.Replace("#", "＃");  
                newFileName = RemoveSpaces(newFileName);  
            }  
            long startfilesize = GetFileSize(newFileName, remoteFilepath);  
            if (startfilesize >= allbye)  
            {  
                return false;  
            }  
            long startbye = startfilesize;  
            //更新进度    
            if (updateProgress != null)  
            {  
                updateProgress((int)allbye, (int)startfilesize);//更新进度条     
            }  
  
            string uri;  
            if (remoteFilepath.Length == 0)  
            {  
                uri = "ftp://" + FtpServerIP + "/" + newFileName;  
            }  
            else  
            {  
                uri = "ftp://" + FtpServerIP + "/" + remoteFilepath + "/" + newFileName;  
            }  
            FtpWebRequest reqFTP;  
            // 根据uri创建FtpWebRequest对象   
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));  
            // ftp用户名和密码   
            reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);  
            // 默认为true，连接不会被关闭   
            // 在一个命令之后被执行   
            reqFTP.KeepAlive = false;  
            // 指定执行什么命令   
            reqFTP.Method = WebRequestMethods.Ftp.AppendFile;  
            // 指定数据传输类型   
            reqFTP.UseBinary = true;  
            // 上传文件时通知服务器文件的大小   
            reqFTP.ContentLength = fileInf.Length;  
            int buffLength = 2048;// 缓冲大小设置为2kb   
            byte[] buff = new byte[buffLength];  
            // 打开一个文件流 (System.IO.FileStream) 去读上传的文件   
            FileStream fs = fileInf.OpenRead();  
            Stream strm = null;  
            try  
            {  
                // 把上传的文件写入流   
                strm = reqFTP.GetRequestStream();  
                // 每次读文件流的2kb     
                fs.Seek(startfilesize, 0);  
                int contentLen = fs.Read(buff, 0, buffLength);  
                // 流内容没有结束   
                while (contentLen != 0)  
                {  
                    // 把内容从file stream 写入 upload stream   
                    strm.Write(buff, 0, contentLen);  
                    contentLen = fs.Read(buff, 0, buffLength);  
                    startbye += contentLen;  
                    //更新进度    
                    if (updateProgress != null)  
                    {  
                        updateProgress((int)allbye, (int)startbye);//更新进度条     
                    }  
                }  
                // 关闭两个流   
                strm.Close();  
                fs.Close();  
            }  
            catch  
            {  
                success = false;  
                throw;  
            }  
            finally  
            {  
                if (fs != null)  
                {  
                    fs.Close();  
                }  
                if (strm != null)  
                {  
                    strm.Close();  
                }  
            }  
            return success;  
        }  
  
        /// <summary>  
        /// 去除空格  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        private static string RemoveSpaces(string str)  
        {  
            string a = "";  
            CharEnumerator CEnumerator = str.GetEnumerator();  
            while (CEnumerator.MoveNext())  
            {  
                byte[] array = new byte[1];  
                array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());  
                int asciicode = (short)(array[0]);  
                if (asciicode != 32)  
                {  
                    a += CEnumerator.Current.ToString();  
                }  
            }  
            string sdate = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Hour.ToString()  
                + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + System.DateTime.Now.Millisecond.ToString();  
            return a.Split('.')[a.Split('.').Length - 2] + "." + a.Split('.')[a.Split('.').Length - 1];  
        }  
        /// <summary>  
        /// 获取已上传文件大小  
        /// </summary>  
        /// <param name="filename">文件名称</param>  
        /// <param name="path">服务器文件路径</param>  
        /// <returns></returns>  
        public static long GetFileSize(string filename, string remoteFilepath)  
        {  
            long filesize = 0;  
            try  
            {  
                FtpWebRequest reqFTP;  
                FileInfo fi = new FileInfo(filename);  
                string uri;  
                if (remoteFilepath.Length == 0)  
                {  
                    uri = "ftp://" + FtpServerIP + "/" + fi.Name;  
                }  
                else  
                {  
                    uri = "ftp://" + FtpServerIP + "/" + remoteFilepath + "/" + fi.Name;  
                }  
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);  
                reqFTP.KeepAlive = false;  
                reqFTP.UseBinary = true;  
                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);//用户，密码  
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;  
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();  
                filesize = response.ContentLength;  
                return filesize;  
            }  
            catch  
            {  
                return 0;  
            }  
        }  
  
        //public void Connect(String path, string ftpUserID, string ftpPassword)//连接ftp  
        //{  
        //    // 根据uri创建FtpWebRequest对象  
        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));  
        //    // 指定数据传输类型  
        //    reqFTP.UseBinary = true;  
        //    // ftp用户名和密码  
        //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);  
        //}  
 
        #endregion  
  
    }
}
