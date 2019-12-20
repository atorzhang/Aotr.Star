using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ator.Utility.Helper
{
    public class LogHelper
    {
        private static readonly Object ThisLock = new Object();
        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="ex">消息</param>  
        public static void WriteError(Exception ex)
        {

            AddLog(ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace + "\r\n" + ex.Source, "Error");
        }
        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="message">消息</param>  
        public static void WriteError(string message)
        {

            AddLog(message, "Error");
        }
        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="ex">消息</param> 
        /// <param name="direName">日志存储目录名称</param>
        public static void WriteError(Exception ex, string direName)
        {
            AddLog(ex.Message + "\r\n" + ex.InnerException + "\r\n" + ex.StackTrace + "\r\n" + ex.Source, direName);
        }
        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="message">消息</param>  
        public static void WriteLog(string message)
        {

            AddLog(message, "Log");
        }
        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="message">消息</param> 
        /// <param name="direName">日志存储目录名称</param>
        public static void WriteLog(string message, string direName)
        {

            AddLog(message, direName);
        }

        /// <summary>
        /// 写日志文件数据库日志文件
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="direName">日志存储目录名称</param>
        private static void AddLog(string message, string direName)
        {
            try
            {
                var applicationName = direName;
                //从宿主配置文件中获取日志文件全路径
                //所有的接口日志文件放在一个目录下面，跟web站点的目录分开
                //日志文件一天放一个，按日期分开

                if (string.IsNullOrEmpty(applicationName))
                {
                    applicationName = "Log";
                }
                string logFullPath = AppDomain.CurrentDomain.BaseDirectory + "logs\\" + applicationName;

                if (!Directory.Exists(logFullPath))
                {
                    Directory.CreateDirectory(logFullPath);
                }
                //只保留30天的日志
                var deletePath = $@"{logFullPath}\{DateTime.Now.AddDays(-30):yyyyMMdd}.txt";

                if (File.Exists(deletePath))
                {
                    File.Delete(deletePath);
                }

                logFullPath = $@"{logFullPath}\{DateTime.Now:yyyyMMdd}.txt";
                if (!File.Exists(logFullPath))
                {
                    using (var fs = new FileStream(logFullPath, FileMode.Create, FileAccess.Write))
                    {
                        StreamWriter sw = new StreamWriter(fs);
                        sw.Close();
                        fs.Close();
                    }
                }
                lock (ThisLock)
                {
                    using (StreamWriter writer = new StreamWriter(logFullPath, true))
                    {
                        writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WriteLine(message);
                        writer.WriteLine(Environment.NewLine);

                    }
                }
            }
            catch
            {
                // ignored
            }
        }

    }
}
