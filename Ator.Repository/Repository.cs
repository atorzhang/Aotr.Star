using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Ator.DbEntity.Factory;
namespace Ator.Repository
{
    public class Repository<TFactory, TIRepository> : IRepository where TFactory : IDbFactory where TIRepository : IRepository
    {
        protected readonly ILogger Log;
        protected readonly TFactory Factory;
        protected readonly TIRepository DbRepository;
        protected SqlSugarClient DbContext => this.Factory.GetDbContext();

        public Repository(TFactory factory) => Factory = factory;
        public Repository(TFactory factory, ILogger logger) : this(factory) => Log = logger;
        public Repository(TFactory factory, TIRepository repository) : this(factory) => DbRepository = repository;
        public Repository(TFactory factory, TIRepository repository, ILogger logger) : this(factory, repository) => Log = logger;
    }

    public class Repository<TFactory> : IRepository where TFactory : IDbFactory
    {
        protected readonly ILogger Log;
        protected readonly TFactory Factory;
        protected SqlSugarClient DbContext => this.Factory.GetDbContext();

        public Repository(TFactory factory) => Factory = factory;
        public Repository(TFactory factory, ILogger logger) : this(factory) => Log = logger;
    }
}
