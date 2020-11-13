using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Site.Helper
{
    public static class UrlHelper
    {

        /// <summary>
        /// 获取url字符串参数，返回参数值字符串
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="url">url字符串</param>
        /// <returns></returns>
        public static string GetQueryString(this string url,string name)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            var re = new System.Text.RegularExpressions.Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", System.Text.RegularExpressions.RegexOptions.Compiled);
            System.Text.RegularExpressions.MatchCollection mc = re.Matches(url);
            foreach (System.Text.RegularExpressions.Match m in mc)
            {
                if (m.Result("$2").Equals(name))
                {
                    return m.Result("$3");
                }
            }
            return "";
        }
    }
}
