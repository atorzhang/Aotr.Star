using Ator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Service.Common
{
    public class SelectHelper
    {
        /// <summary>
        /// 生成下拉框选项
        /// </summary>
        /// <param name="lstItem"></param>
        /// <param name="seleted"></param>
        /// <param name="addNull"></param>
        /// <param name="nullTitle"></param>
        /// <returns></returns>
        public string GenOptions(List<SelectItem> lstItem,string seleted = "",bool addNull = false,string nullTitle = "请选择")
        {
            StringBuilder sb = new StringBuilder();
            if (addNull)
            {
                sb.Append($"<option value=\"\">{nullTitle}</option>");
            }
            foreach (var item in lstItem)
            {
                if(item.Name == seleted)
                {
                    sb.Append($"<option value=\"{item.Id}\">{item.Name}</option>");
                }
                else
                {
                    sb.Append($"<option value=\"{item.Id}\">{item.Name}</option>");
                }
            }
            return sb.ToString();
        }
    }
}
