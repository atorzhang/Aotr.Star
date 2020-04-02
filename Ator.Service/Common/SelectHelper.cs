using Ator.DAL;
using Ator.DbEntity.Factory;
using Ator.Model;
using Ator.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Ator.DbEntity.Sys;
using Ator.IService;

namespace Ator.Service
{
    public class SelectHelper : ISelectHelper
    {
        SqlSugarClient DbContext;//注入数据库操作类
        public SelectHelper(DbFactory factory)
        {
            DbContext = factory.GetDbContext();
        }
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
                if(item.Id == seleted)
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

        /// <summary>
        /// 根据字典生成下拉框option
        /// </summary>
        /// <param name="dicId"></param>
        /// <param name="seleted"></param>
        /// <param name="addNull"></param>
        /// <param name="nullTitle"></param>
        /// <returns></returns>
        public string GenOptionsByDic(string dicId, string seleted = "", bool addNull = false, string nullTitle = "请选择")
        {
            StringBuilder sb = new StringBuilder();
            var lstDic = DbContext.GetList<SysDictionaryItem>(o => o.SysDictionaryId == dicId && o.Status == 1,"Sort");
            if (addNull)
            {
                sb.Append($"<option value=\"\">{nullTitle}</option>");
            }
            foreach (var item in lstDic)
            {
                if (item.SysDictionaryItemValue == seleted)
                {
                    sb.Append($"<option value=\"{item.SysDictionaryItemValue}\">{item.SysDictionaryItemName}</option>");
                }
                else
                {
                    sb.Append($"<option value=\"{item.SysDictionaryItemValue}\">{item.SysDictionaryItemName}</option>");
                }
            }
            return sb.ToString();
        }
    }
}
