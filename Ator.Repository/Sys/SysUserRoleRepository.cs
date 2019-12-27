using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysUserRoleRepository : Repository<DbFactory>, IRepository
    { 
        public SysUserRoleRepository(DbFactory factory) : base(factory)
        {

        }
    
    }
}
