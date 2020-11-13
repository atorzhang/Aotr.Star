//-----------------------------------------------------------------------
// <copyright file= "NLogHelper.cs">
//     Copyright (c) Ator. All rights reserved.
// </copyright>
// Author: Ator
// Date Created: 2018/9/8 星期六
// Modified by:
// Description: NLog帮助类
//-----------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Common.Web.Helper
{
    public class NLogHelper
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static void ErrorLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【异常信息】：{0} <br>【异常类型】：{1} <br>【堆栈调用】：{2}",new object[] { throwMsg, ex.GetType().Name, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logger.Error(errorMsg);
        }

        public static void InfoLog(string operateMsg)
        {
            string errorMsg = string.Format("【操作信息】：{0} <br>",new object[] { operateMsg });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            logger.Info(errorMsg);
        }
    }
}
