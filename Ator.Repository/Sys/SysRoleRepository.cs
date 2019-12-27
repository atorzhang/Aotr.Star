using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysRoleRepository : Repository<DbFactory>, IRepository
    {
        public SysRoleRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
