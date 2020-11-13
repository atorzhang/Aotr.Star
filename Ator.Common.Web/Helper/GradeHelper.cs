using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Common.Web.Helper
{
    public class GradeHelper
    {
        public static string GetGrade(int comments)
        {
            if(comments < 10)
            {
                return "1";
            }
            if (comments < 50)
            {
                return "2";
            }
            if (comments < 100)
            {
                return "3";
            }
            if (comments < 200)
            {
                return "4";
            }
            return "5";
        }

        public static string GetGradeName(int comments)
        {
            if (comments < 10)
            {
                return "潜水";
            }
            if (comments < 50)
            {
                return "冒泡";
            }
            if (comments < 100)
            {
                return "小牛";
            }
            if (comments < 200)
            {
                return "项目经理";
            }
            return "CTO";
        }
    }
}
