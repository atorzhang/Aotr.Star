using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysUserRepository : Repository<DbFactory>, IRepository
    {
        public SysUserRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
