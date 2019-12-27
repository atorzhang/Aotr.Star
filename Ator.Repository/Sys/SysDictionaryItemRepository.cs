
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.Model.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysDictionaryItemRepository : Repository<DbFactory>, IRepository
    {
        public SysDictionaryItemRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
