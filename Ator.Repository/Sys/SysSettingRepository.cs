using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysSettingRepository : Repository<DbFactory>, IRepository
    {
        public SysSettingRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
