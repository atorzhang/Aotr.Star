using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository.Implement
{
    public class SysCmsColumnRepository : Repository<DbFactory>, IRepository
    {
        public SysCmsColumnRepository(DbFactory factory) : base(factory)
        {

        }

        //测试使用
        public async Task<bool> TestAddAsync()
        {
            //这里获取数据库上下文，与业务层一致

            DbContext.Insert<SysCmsColumn>(new SysCmsColumn());
            using (var db = Factory.GetDbContext())
            {
                db.Insert<SysCmsColumn>(new SysCmsColumn());
                db.Update<SysCmsColumn>(new SysCmsColumn());
            }
            return await Task.FromResult(true);
        }
    }
}
