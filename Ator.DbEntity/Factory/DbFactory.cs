using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.DbEntity.Factory
{
    public class DbFactory : IDbFactory
    {
        private readonly ILogger _logger;
        private readonly ConnectionConfig _config;

        public DbFactory(ConnectionConfig config, ILogger<DbFactory> logger)
        {
            this._logger = logger;
            this._config = config;
        }

        public SqlSugarClient GetDbContext(Action<Exception> onErrorEvent) => GetDbContext(null, null, onErrorEvent);
        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent) => GetDbContext(onExecutedEvent);
        public SqlSugarClient GetDbContext(Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent) => GetDbContext(null, onExecutingChangeSqlEvent);
        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent = null, Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent = null, Action<Exception> onErrorEvent = null)
        {
            SqlSugarClient db = new SqlSugarClient(_config)
            {
                Aop =
                 {
                        OnExecutingChangeSql = onExecutingChangeSqlEvent,
                        OnError = onErrorEvent ?? ((Exception ex) => { this._logger.LogError(ex, "ExecuteSql Error"); }),
                        OnLogExecuted = onExecutedEvent?? ((string sql, SugarParameter[] pars) =>
                        {
                            var keyDic = new KeyValuePair<string, SugarParameter[]>(sql, pars);
                            this._logger.LogDebug($"ExecuteSql：【{Newtonsoft.Json.JsonConvert.SerializeObject(keyDic)}】");
                        })
                 }
            };
            return db;
        }
    }
}
