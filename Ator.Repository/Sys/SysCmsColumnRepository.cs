using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysCmsColumnRepository : Repository<DbFactory>,IRepository
    {
        public SysCmsColumnRepository(DbFactory factory) : base(factory)
        {

        }

    }
}
