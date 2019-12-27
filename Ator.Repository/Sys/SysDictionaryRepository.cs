using Ator.DbEntity.Factory;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysDictionaryRepository : Repository<DbFactory>, IRepository
    {
        public SysDictionaryRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
