using Ator.DbEntity.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysCmsInfoCommentRepository : Repository<DbFactory>, IRepository
    {
        public SysCmsInfoCommentRepository(DbFactory factory) : base(factory)
        {

        }
    }
}
