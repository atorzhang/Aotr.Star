using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysPageRepository : Repository<DbFactory>, IRepository
    {
        public SysPageRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
