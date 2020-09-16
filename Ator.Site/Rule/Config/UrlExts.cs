using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ator.Site
{
    public static class UrlExts
    {
        public static string GetUrlDecode(this string urlInfo)
        {
            return HttpUtility.UrlDecode(urlInfo);
        }

        public static string GetUrlEncode(this string urlInfo)
        {
            return HttpUtility.UrlEncode(urlInfo);
        }
    }
}
