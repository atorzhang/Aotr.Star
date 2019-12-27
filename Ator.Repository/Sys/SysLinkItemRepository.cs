
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysLinkItemRepository : Repository<DbFactory>, IRepository
    {
        public SysLinkItemRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
