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
    public class SysDictionaryService : Repository<DbFactory, SysDictionaryRepository>, ISysDictionaryService
    {
    
        public SysDictionaryService(DbFactory factory) : base(factory)
        {
            
        }
        public List<KeyValuePair<string, string>> GetDictionaryList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = DbContext.GetList<SysDictionary>("Sort").ToList();
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysDictionaryId, item.SysDictionaryName));
            }
            return data;
        }
    }
}
