using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Repository.Sys;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Service
{
    public class SysPageService : Repository<DbFactory, SysPageRepository> ,ISysPageService
    {
        public SysPageService(DbFactory factory) : base(factory)
        {
         
        }
        public List<KeyValuePair<string, string>> GetPageList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>(); 
            var allPage = DbContext.GetList<SysPage>();
            //从上到下的算法。层级越多越麻烦，因此只计算到3级
            foreach (var item in allPage.Where(o => string.IsNullOrEmpty(o.SysPageParent)).OrderBy(o => o.Sort)) 
            {
                //父级编码为空的为1级列表
                data.Add(new KeyValuePair<string, string>(item.SysPageId, item.SysPageName));
                foreach (var item1 in allPage.Where(o => item.SysPageId.Equals(o.SysPageParent)).OrderBy(o => o.Sort))
                {
                    data.Add(new KeyValuePair<string, string>(item1.SysPageId, "└" + item1.SysPageName));
                    foreach (var item2 in allPage.Where(o => item1.SysPageId.Equals(o.SysPageParent)).OrderBy(o => o.Sort))
                    {
                        data.Add(new KeyValuePair<string, string>(item2.SysPageId, "└└" + item2.SysPageName));
                    }
                }
            }
            return data;
        }
    }
}
