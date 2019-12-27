using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysLinkTypeRepository : Repository<DbFactory>, IRepository
    {
        public SysLinkTypeRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
