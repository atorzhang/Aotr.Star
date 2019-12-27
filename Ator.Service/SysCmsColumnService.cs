using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Repository.Implement;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Service
{
    public class SysCmsColumnService : Repository<DbFactory, SysCmsColumnRepository>, ISysCmsColumnService
    {
        public SysCmsColumnService(DbFactory factory) : base(factory)
        {

        }
        public List<KeyValuePair<string, string>> GetColumnList(string ColumnParent = "")
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>(); 
            //从上到下的算法。层级越多越麻烦，因此只计算到3级
            if (string.IsNullOrEmpty(ColumnParent))
            {
                var allPage = DbContext.GetList<SysCmsColumn>();
                foreach (var item in allPage.Where(o => string.IsNullOrEmpty(o.ColumnParent)).OrderBy(o => o.Sort))
                {
                    //父级编码为空的为1级列表
                    data.Add(new KeyValuePair<string, string>(item.SysCmsColumnId, item.ColumnName));
                    foreach (var item1 in allPage.Where(o => item.SysCmsColumnId.Equals(o.ColumnParent)).OrderBy(o => o.Sort))
                    {
                        data.Add(new KeyValuePair<string, string>(item1.SysCmsColumnId, "└" + item1.ColumnName));
                        foreach (var item2 in allPage.Where(o => item1.SysCmsColumnId.Equals(o.ColumnParent)).OrderBy(o => o.Sort))
                        {
                            data.Add(new KeyValuePair<string, string>(item2.SysCmsColumnId, "└└" + item2.ColumnName));
                        }
                    }
                }
            }
            else
            {
                var allPage = DbContext.GetList<SysCmsColumn>(o => o.ColumnParent == ColumnParent).OrderBy(o =>o.Sort).ToList();
                foreach (var item in allPage)
                {
                    data.Add(new KeyValuePair<string, string>(item.SysCmsColumnId, item.ColumnName));
                }
            }
            return data;
        }
    }
}
