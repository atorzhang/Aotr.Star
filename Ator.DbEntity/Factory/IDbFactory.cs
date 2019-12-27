using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.DbEntity.Factory
{
    public interface IDbFactory
    {
        SqlSugarClient GetDbContext(Action<Exception> onErrorEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent);
        SqlSugarClient GetDbContext(Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent);
        SqlSugarClient GetDbContext(Action<string, SugarParameter[]> onExecutedEvent = null, Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>> onExecutingChangeSqlEvent = null, Action<Exception> onErrorEvent = null);
    }
}
