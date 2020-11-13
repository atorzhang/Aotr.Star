using Ator.Site.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Common.Web.Filter
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //采用NLog 进行错误日志记录
            NLogHelper.ErrorLog(context.Exception.Message, context.Exception);
        }
    
    }
}
