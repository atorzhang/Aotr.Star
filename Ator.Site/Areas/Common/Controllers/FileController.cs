using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ator.Model.Api;
using Ator.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Ator.Site.Areas.Common.Controllers
{
    [Area("Common")]
    [Route("Common/[controller]/[action]")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public FileController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        //编辑器上传文件地址
        public async Task<IActionResult> KindEditorImgUpload()
        {
            Dictionary<string, string> extTable = new Dictionary<string, string>();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2,apk,ipa,wgt");
            //最大文件大小
            int maxSize = 1024 * 1024 * 10;//10M上传大小限制
            var context = Request.HttpContext;
            var imgFile = Request.Form.Files[0];

            //文件类型
            string dirName = Request.Query["dir"];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return ShowError("目录名不正确。");
            }
            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile == null || imgFile.Length > maxSize)
            {
                return ShowError("上传文件大小超过限制。");
            }
            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }
            string saveDir = Request.Query["saveDir"];
            string saveDirStr = null;
            if (saveDir == null)
            {
                saveDirStr = "tmp";
            }
            else
            {
                saveDirStr = saveDir.ToString();
            }
            //文件保存目录
            if(Request.Query["hasMonth"] == "1")
            {
                saveDirStr += "/" + DateTime.Now.ToString("yyyyMM");
            }
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            string savePath = "/wwwroot/upload/kindeditor/" + dirName + "/" + saveDirStr;
            string dirPath = contentRootPath + savePath;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = DateTime.Now.ToString("_yyyyMMdd_"+GenHelper.GuidTo16String(), DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + "/" + newFileName;
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                await imgFile.CopyToAsync(fs);
                fs.Flush();
            }
            Dictionary<string, object> hash = new Dictionary<string, object>();

            hash["url"] = (savePath + "/" + newFileName).Replace("/wwwroot", "");
            hash["error"] = 0;
            Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
            return Json(hash);
        }

        /// <summary>
        /// 普通文件上传，支持多个文件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ImgUpload()
        {
            Dictionary<string, string> extTable = new Dictionary<string, string>();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2,apk,ipa,wgt");
            //最大文件大小
            int maxSize = 1024 * 1024 * 10;//10M上传大小限制
            var context = Request.HttpContext;

            //文件类型判断
            string dirName = Request.Query["dir"];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return ShowError("文件类型不正确。");
            }

            //保存相对地址构造
            string saveDir = Request.Query["saveDir"];
            string saveDirStr = null;
            if (saveDir == null)
            {
                saveDirStr = "tmp";
            }
            else
            {
                saveDirStr = saveDir.ToString();
            }
            //文件保存目录
            if (Request.Query["hasMonth"] == "1")
            {
                saveDirStr += "/" + DateTime.Now.ToString("yyyyMM");
            }

            //相对程序主目录路径
            string savePath = "/upload/cms/" + dirName + "/" + saveDirStr;
           
            //返回文件集合
            List<string> returnImgs = new List<string>();
            //循环保存文件
            foreach (var imgFile in Request.Form.Files)
            {
                var fileName = imgFile.FileName;
                var fileExt = Path.GetExtension(fileName).ToLower();

                if (imgFile == null || imgFile.Length > maxSize)
                {
                    return ShowError("上传文件大小超过限制。");
                }
                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    return ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
                }

                //文件保存目录
                string dirPath = _hostingEnvironment.WebRootPath + savePath;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                //新文件名称
                var newFileName = DateTime.Now.ToString("_yyyyMMdd_"+GenHelper.GuidTo16String(), DateTimeFormatInfo.InvariantInfo) + fileExt;
                var filePath = dirPath + "/" + newFileName;//新文件物理全路径
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    await imgFile.CopyToAsync(fs);
                    fs.Flush();
                }
                returnImgs.Add(savePath + "/" + newFileName);
            }
            //构造返回数据
            var apiResult = new LayuiDataModel()
            {
                msg = string.Join(",", returnImgs),
                code = "0",
                success = true
            };
            if (returnImgs.Count == 0)
            {
                apiResult.success = false;
            }
            return Json(apiResult);
        }

        private IActionResult ShowError(string message)
        {
            Dictionary<string, object> hash = new Dictionary<string, object>();

            hash["error"] = 1;
            hash["message"] = message;
            Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
            return Json(hash);
        }
    }
}