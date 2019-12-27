using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Repository;
using Ator.Repository.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Service
{
    public class SysLinkTypeService : Repository<DbFactory, SysDictionaryRepository>, ISysLinkTypeService
    {
      
        public SysLinkTypeService(DbFactory factory) : base(factory)
        {
           
        }
        public List<KeyValuePair<string, string>> GetLinkTypeList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = DbContext.GetList<SysLinkType>($"{nameof(SysLinkType.Sort)}");
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysLinkTypeId, item.SysLinkTypeName));
            }
            return data;
        }
    }
}
