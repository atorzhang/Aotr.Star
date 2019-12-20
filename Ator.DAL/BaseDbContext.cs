using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using Ator.DbEntity.SqlSuger;
using Ator.Utility.Ext;
using Ator.Utility.Helper;

namespace Ator.DAL
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class BaseDbContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static List<string> ListConn { get; set; }
        /// <summary>
        /// 获得SqlSugarClient
        /// 注意当前方法的类不能是静态的 public static class这么写是错误的
        /// </summary>
        public static SqlSugarClient Instance 
        {
            get
            {
                var connStr = "";//主库
                var slaveConnectionConfigs = new List<SlaveConnectionConfig>();//从库集合
                for (var i = 0; i < ListConn.Count; i++)
                {
                    if (i == 0)
                    {
                        connStr = ListConn[i];//主数据库连接
                    }
                    else
                    {
                        slaveConnectionConfigs.Add(new SlaveConnectionConfig()
                        {
                            HitRate = i * 2,
                            ConnectionString = ListConn[i]
                        });
                    }
                }

                //如果配置了 SlaveConnectionConfigs那就是主从模式,所有的写入删除更新都走主库，查询走从库，
                //事务内都走主库，HitRate表示权重 值越大执行的次数越高，如果想停掉哪个连接可以把HitRate设为0 
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connStr,
                    DbType = (DbType)(int)SysConfig.Params.DbType,
                    IsAutoCloseConnection = true,
                    SlaveConnectionConfigs = slaveConnectionConfigs,
                    IsShardSameThread = true,
                    InitKeyType = InitKeyType.Attribute
                });
                db.Ado.CommandTimeOut = 30000;//设置超时时间
                db.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
                {
                    LogHelper.WriteLog($"执行时间：{db.Ado.SqlExecutionTime.TotalMilliseconds}毫秒 \r\nSQL如下：{sql} \r\n参数：{GetParams(pars)} ", "SQL执行");
                };
                db.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
                {
                    if (db.TempItems == null) db.TempItems = new Dictionary<string, object>();
                };
                db.Aop.OnError = (exp) =>//执行SQL 错误事件
                {
                    LogHelper.WriteLog($"SQL错误:{exp.Message}\r\nSQL如下：{exp.Sql}", "SQL执行");
                    throw new Exception(exp.Message);
                };
                db.Aop.OnDiffLogEvent = (it) => //可以方便拿到 数据库操作前和操作后的数据变化。
                {
                    var editBeforeData = it.BeforeData; //变化前数据
                    var editAfterData = it.AfterData; //变化后数据
                    var sql = it.Sql; //SQL
                    var parameter = it.Parameters; //参数
                    var data = it.BusinessData; //业务数据
                    var time = it.Time??new TimeSpan(); //时间
                    var diffType = it.DiffType; //枚举值 insert 、update 和 delete 用来作业务区分

                    //你可以在这里面写日志方法
                    var log = $"时间:{time.TotalMilliseconds}\r\n";
                    log += $"类型:{diffType.ToString()}\r\n";
                    log += $"SQL:{sql}\r\n";
                    log += $"参数:{GetParams(parameter)}\r\n";
                    log += $"业务数据:{data.ToJson()}\r\n";
                    log += $"变化前数据:{editBeforeData.ToJson()}\r\n";
                    log += $"变化后数据:{editAfterData.ToJson()}\r\n";
                    LogHelper.WriteLog(log, "数据变化前后");
                };
                return db;
            }
        }
        
        /// <summary>
        ///  设置连接字符串
        /// </summary>
        public static void SetConn()
        {
            var connMain = SysConfig.Params.ConnMain;
            var connFrom = SysConfig.Params.ConnFrom;
            ListConn = connFrom == null
                ? new List<string> {connMain.ToString()}
                : new List<string> {connMain.ToString(), connFrom.ToString()};
        }
        /// <summary>
        ///  设置连接字符串
        /// </summary>
        /// <param name="listKey">数据连接Key</param> 
        public static void SetConn(List<string> listKey)
        {
            ListConn = new List<string>();
            foreach (var t in listKey)
            {
                ListConn.Add((string)SysConfig.Params[t]);
            } 
        }

        /// <summary>
        ///  设置连接字符串
        /// </summary>
        /// <param name="serverIp">服务器IP或文件路径</param>
        /// <param name="user">用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="dataBase">数据库</param>
        public static void SetConn(string serverIp, string user, string pass, string dataBase)
        {
            ListConn = new List<string>();
            switch ((DbType)(int)SysConfig.Params.DbType)
            {
                case DbType.SqlServer:
                    ListConn.Add($"server={serverIp};user id={user};password={pass};persistsecurityinfo=True;database={dataBase}");
                    break;
                case DbType.MySql:
                    ListConn.Add($"Server={serverIp};Database={dataBase};Uid={user};Pwd={pass};");
                    break;
                case DbType.Oracle:
                    ListConn.Add($"Server=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={serverIp})(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={dataBase})));User Id={user};Password={pass};Persist Security Info=True;Enlist=true;Max Pool Size=300;Min Pool Size=0;Connection Lifetime=300");
                    break;
                case DbType.PostgreSQL:
                    ListConn.Add($"PORT=5432;DATABASE={dataBase};HOST={serverIp};PASSWORD={pass};USER ID={user}");
                    break;
                case DbType.Sqlite:
                    ListConn.Add($"Data Source={serverIp};Version=3;Password={pass};");
                    break;
            } 
        }
        
        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        private static string GetParams(SugarParameter[] pars)
        {
            return pars.Aggregate("", (current, p) => current + $"{p.ParameterName}:{p.Value}, ");
        }
    }
}
