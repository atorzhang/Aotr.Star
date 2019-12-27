using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysCmsInfoGoodRepository : Repository<DbFactory>, IRepository
    {
        public SysCmsInfoGoodRepository(DbFactory factory) : base(factory)
        {

        }

    }
}
