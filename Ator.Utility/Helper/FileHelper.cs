using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ator.Utility.Helper
{
    /// <summary>  
    /// 文件操作类
    /// </summary>  
    public class FileHelper
    {
        /// <summary>  
        /// 编码方式  
        /// </summary>  
        private static readonly Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return "";
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        }

        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary> 
        /// <returns>绝对路径地址</returns>
        public static string GetRootPath()
        {
            //判断是Web程序还是window程序
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 通过文件Hash 比较两个文件内容是否相同
        /// </summary>
        /// <param name="filePath1">文件1地址</param>
        /// <param name="filePath2">文件2地址</param>
        /// <returns>真假</returns>
        public static bool IsValidFileContent(string filePath1, string filePath2)
        {
            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file1 = new FileStream(filePath1, FileMode.Open), file2 = new FileStream(filePath2, FileMode.Open))
                {
                    byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                    byte[] hashByte2 = hash.ComputeHash(file2);
                    string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                    string str2 = BitConverter.ToString(hashByte2);
                    return (str1 == str2);//比较哈希码 
                }
            }
        }

        /// <summary>
        /// 计算文件的hash值 用于比较两个文件是否相同
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件hash值</returns>
        /// <returns>值</returns>
        public static string GetFileHash(string filePath)
        {
            //创建一个哈希算法对象 
            using (HashAlgorithm hash = HashAlgorithm.Create())
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    //哈希算法根据文本得到哈希码的字节数组 
                    byte[] hashByte = hash.ComputeHash(file);
                    //将字节数组装换为字符串  
                    return BitConverter.ToString(hashByte);
                }
            }
        }

        /// <summary>
        /// 复制一个文件夹下的所有文件到另外一个文件夹
        /// </summary>
        /// <param name="sourceDirName">源文件夹目录</param>
        /// <param name="destDirName">目标文件夹路径</param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

                }

                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                    destDirName = destDirName + Path.DirectorySeparatorChar;

                string[] files = Directory.GetFiles(sourceDirName);
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }

                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 复制一个文件夹下指定的文件到另外一个文件夹
        /// </summary>
        /// <param name="list">指定文件</param>
        /// <param name="destDirName">目标文件夹路径</param>
        public static void CopyFiles(List<string> list, string destDirName)
        {
            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                {
                    destDirName = destDirName + Path.DirectorySeparatorChar;
                }


                string[] files = list.ToArray();
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 复制一个文件
        /// </summary>
        /// <param name="path">指定文件</param>
        /// <param name="copyFolerName">目标文件夹</param>
        /// <param name="webDirPath">相对文件路径</param>
        public static string CopyFile(string path, string copyFolerName, out string webDirPath)
        {
            try
            {
                var directory = GetRootPath();
                path = directory + path;
                var fileName = GetFileName(path);
                var fileExtension = GetFileExtension(path);
                fileName = fileName.Replace(fileExtension, "");
                //复制零时文件 ;
                var tempFoler = $"{copyFolerName}\\";
                var tempDire = directory + tempFoler;
                var tempName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss_ffff") + fileExtension;
                string tempPath = Path.Combine(tempDire, tempName);

                if (!Directory.Exists(tempDire))
                {
                    Directory.CreateDirectory(tempDire);
                }
                File.Copy(path, tempPath);
                webDirPath = tempFoler + tempName;
                return tempPath;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>  
        /// 递归取得文件夹下文件路径  
        /// </summary>  
        /// <param name="dir">目录</param>  
        /// <param name="list">文件集合(文件名称,文件全路径)</param>  
        /// <param name="fileChilds">文件后缀集合</param>
        public static void GetFiles(string dir, Dictionary<string, string> list, List<string> fileChilds = null)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //添加文件   
            DirectoryInfo folder = new DirectoryInfo(dir);
            var files = folder.GetFiles("*.*");
            if (fileChilds != null && fileChilds.Count > 0)
            {
                foreach (FileInfo file in files)
                {
                    string extension = file.Extension;
                    if (fileChilds.Contains(extension))
                    {
                        if (!list.ContainsKey(file.Name))
                            list.Add(file.Name, file.FullName);
                    }
                }
            }
            else
            {
                foreach (var file in files)
                {
                    if (!list.ContainsKey(file.Name))
                        list.Add(file.Name, file.FullName);
                }
            }
            //如果是目录，则递归  
            DirectoryInfo[] directories = new DirectoryInfo(dir).GetDirectories();
            foreach (DirectoryInfo item in directories)
            {
                GetFiles(item.FullName, list, fileChilds);
            }
        }

        /// <summary>  
        /// 写入文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <param name="content">文件内容</param>  
        public static void WriteFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                Encoding encode = Encoding;
                //获得字节数组  
                byte[] data = encode.GetBytes(content);
                //开始写入  
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流  
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns>值</returns>  
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding);
        }

        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <param name="encoding">编码</param>  
        /// <returns>值</returns>  
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>  
        /// 读取文件  
        /// </summary>  
        /// <param name="filePath">文件路径</param>  
        /// <returns>集合</returns>  
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }


        /// <summary>  
        /// 删除文件夹 
        /// </summary>  
        /// <param name="directoryPath">文件目录</param>  
        public static void DeleteFolder(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
            Directory.Delete(directoryPath); //删除空文件夹  
        }

        /// <summary>  
        ///  删除文件   
        /// </summary>  
        /// <param name="filedPath">文件路径</param>  
        public static void Delete(string filedPath)
        {
            if (File.Exists(filedPath))
            {
                var fi = new FileInfo(filedPath);
                if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(filedPath); //删除文件     
            }
        }


        /// <summary>  
        /// 清空文件夹
        /// </summary>  
        /// <param name="directoryPath">文件目录</param>  
        public static void ClearFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件     
                }
                else
                    DeleteFolder(d); //删除文件夹  
            }
        }

        /// <summary>  
        /// 取得文件大小，按适当单位转换  
        /// </summary>  
        /// <param name="filepath">文件路径</param>  
        /// <returns>值</returns>  
        public static string GetFileSize(string filepath)
        {
            string result = "0KB";
            if (File.Exists(filepath))
            {
                var size = new FileInfo(filepath).Length;
                int filelength = size.ToString().Length;
                if (filelength < 4)
                    result = size + "byte";
                else if (filelength < 7)
                    result = Math.Round(Convert.ToDouble(size / 1024d), 2) + "KB";
                else if (filelength < 10)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
                else if (filelength < 13)
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
                else
                    result = Math.Round(Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
                return result;
            }
            return result;
        }


        /// <summary>  
        /// 取得文件名称  
        /// </summary>  
        /// <param name="filepath">文件路径</param>  
        /// <returns>值</returns>  
        public static string GetFileName(string filepath)
        {
            string result = "未定义";
            if (File.Exists(filepath))
            {
                result = Path.GetFileName(filepath);
            }
            return result;
        }
        /// <summary>  
        /// 取得文件扩展  
        /// </summary>  
        /// <param name="filepath">文件路径</param>  
        /// <returns>值</returns>  
        public static string GetFileExtension(string filepath)
        {
            string result = "";
            if (File.Exists(filepath))
            {
                result = Path.GetExtension(filepath);
            }
            return result;
        }

    }
}
