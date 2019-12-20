#region << 版 本 注 释 >>
/*---------------------------------------------------------------- 
* 类 名 称 ：SqlRepository
* 类 描 述 ：    
* 作    者 ：罗泽光
* 创建时间 ：2018/7/23 15:52:47
* 更新时间 ：2018/7/23 15:52:47
* 说    明 ：
using (var db = DbFactory.InstanceSuger())
{
    db.BeginTran(); 
    var userInfo = db.Query<SysUsers>(m => m.UserName == userLogin.UserName); 
    db.CommitTran();
}
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ xnlzg 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions; 
using Ator.DbEntity;
using Ator.DbEntity.SqlSuger;
using SqlSugar; 

namespace Ator.DAL
{
    /// <summary>
    /// SqlSugar调用类
    /// </summary>
    public class SugarHandler
    {
        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>值</returns>
        public static ISugarHandler Instance()
        {
            var suger = new SugarRepository();
            suger.Instance();
            return suger;
        }


        /// <summary>
        ///  初始化
        /// </summary>
        /// <param name="listKey">数据连接Key</param> 
        public static ISugarHandler Instance(List<string> listKey)
        {
            var suger = new SugarRepository();
            suger.Instance(listKey);
            return suger;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="user">用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="dataBase">数据库</param>
        /// <returns>值</returns>
        public static ISugarHandler Instance(string serverIp, string user, string pass, string dataBase)
        {
            var suger = new SugarRepository();
            suger.Instance(serverIp, user, pass, dataBase);
            return suger;
        }

        #endregion
    }
    /// <summary>
    /// SqlSugar操作类型
    /// </summary>
    public class SugarRepository : ISugarHandler
    {

        /// <summary>
        /// 数据库连接对象
        /// </summary> 
        public SqlSugarClient DbContext { get; set; }

        /// <summary>
        /// 锁
        /// </summary>
        private readonly Object _tranLock = new Object();

        /// <summary>
        ///  初始化
        /// </summary>
        public void Instance()
        {
            BaseDbContext.SetConn();
            DbContext = BaseDbContext.Instance;
        }

        /// <summary>
        ///  初始化
        /// </summary>
        /// <param name="listKey">数据连接Key</param> 
        public void Instance(List<string> listKey)
        {
            BaseDbContext.SetConn(listKey);
            DbContext = BaseDbContext.Instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverIp">服务器IP</param>
        /// <param name="user">用户名</param>
        /// <param name="pass">密码</param>
        /// <param name="dataBase">数据库</param>
        /// <returns>值</returns>
        public void Instance(string serverIp, string user, string pass, string dataBase)
        {
            BaseDbContext.SetConn(serverIp, user, pass, dataBase);
            DbContext = BaseDbContext.Instance;
        }

        #region 事务


        /// <summary>
        /// 事务操作
        /// 注意:代码段里面如果调用本身类其它方法或其它类方法必须带着var db = SugerHandler.Instance()这个db走，不带着走事务回滚会不成功
        /// </summary> 
        /// <param name="serviceAction">代码段</param> 
        /// <param name="level">事务级别</param>
        public void InvokeTransactionScope(Action serviceAction, IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            lock (_tranLock)
            {
                try
                {

                    if (DbContext.Ado.Transaction == null)
                    {
                        DbContext.Ado.BeginTran(level);
                    }
                    serviceAction();
                    DbContext.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    DbContext.Ado.RollbackTran();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    DbContext.Ado.Dispose();
                    DbContext.Dispose();
                }
            }
            
        }
        #endregion
         
        #region 数据库管理
        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列信息</param>
       /// <returns>值</returns>
        public bool AddColumn(string tableName, DbColumnInfo column)
        {
            return DbContext.DbMaintenance.AddColumn(tableName, column);
        }
        /// <summary>
        /// 添加主键
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
       /// <returns>值</returns>
        public bool AddPrimaryKey(string tableName, string columnName)
        {
            return DbContext.DbMaintenance.AddPrimaryKey(tableName, columnName);
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="databaseName">数据库名</param>
        /// <param name="fullFileName">文件名</param>
       /// <returns>值</returns>
        public bool BackupDataBase(string databaseName, string fullFileName)
        {
            return DbContext.DbMaintenance.BackupDataBase(databaseName, fullFileName);
        }
        /// <summary>
        /// 备份表
        /// </summary>
        /// <param name="oldTableName">旧表名</param>
        /// <param name="newTableName">行表名</param>
        /// <param name="maxBackupDataRows">行数</param>
       /// <returns>值</returns>
        public bool BackupTable(string oldTableName, string newTableName, int maxBackupDataRows = int.MaxValue)
        {
            return DbContext.DbMaintenance.BackupTable(oldTableName, newTableName, maxBackupDataRows);
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列集合</param>
        /// <param name="isCreatePrimaryKey">是否创建主键</param>
       /// <returns>值</returns>
        public bool CreateTable(string tableName, List<DbColumnInfo> columns, bool isCreatePrimaryKey = true)
        {
            return DbContext.DbMaintenance.CreateTable(tableName, columns, isCreatePrimaryKey);
        }
        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
       /// <returns>值</returns>
        public bool DropColumn(string tableName, string columnName)
        {
            return DbContext.DbMaintenance.DropColumn(tableName, columnName);
        }
        /// <summary>
        /// 删除约束
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="constraintName">约束名</param>
       /// <returns>值</returns>
        public bool DropConstraint(string tableName, string constraintName)
        {
            return DbContext.DbMaintenance.DropConstraint(tableName, constraintName);
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="tableName"></param>
       /// <returns>值</returns>
        public bool DropTable(string tableName)
        {
            return DbContext.DbMaintenance.DropTable(tableName);
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isCache">是否缓存</param>
       /// <returns>值</returns>
        public List<DbColumnInfo> GetColumnInfosByTableName(string tableName, bool isCache = true)
        {
            return DbContext.DbMaintenance.GetColumnInfosByTableName(tableName, isCache);
        }
        /// <summary>
        /// 获取自增列
        /// </summary>
        /// <param name="tableName">表名</param>
       /// <returns>值</returns>
        public List<string> GetIsIdentities(string tableName)
        {
            return DbContext.DbMaintenance.GetIsIdentities(tableName);
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="tableName">表名</param>
       /// <returns>值</returns>
        public List<string> GetPrimaries(string tableName)
        {
            return DbContext.DbMaintenance.GetPrimaries(tableName);
        }
        /// <summary>
        /// 获取表集合
        /// </summary>
        /// <param name="isCache">是否缓存</param>
       /// <returns>值</returns>
        public List<DbTableInfo> GetTableInfoList(bool isCache = true)
        {
            return DbContext.DbMaintenance.GetTableInfoList(isCache);
        }
        /// <summary>
        /// 获取视图集合
        /// </summary>
        /// <param name="isCache">是否缓存</param>
       /// <returns>值</returns>
        public List<DbTableInfo> GetViewInfoList(bool isCache = true)
        {
            return DbContext.DbMaintenance.GetViewInfoList(isCache);
        }
        /// <summary>
        /// 检测列是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列名</param>
       /// <returns>值</returns>
        public bool IsAnyColumn(string tableName, string column)
        {
            return DbContext.DbMaintenance.IsAnyColumn(tableName, column);
        }
        /// <summary>
        /// 检测约束
        /// </summary>
        /// <param name="constraintName">约束名称</param>
       /// <returns>值</returns>
        public bool IsAnyConstraint(string constraintName)
        {
            return DbContext.DbMaintenance.IsAnyConstraint(constraintName);
        }
        /// <summary>
        /// 检测是否有任何系统表权限 
        /// </summary>
       /// <returns>值</returns>
        public bool IsAnySystemTablePermissions()
        {
            return DbContext.DbMaintenance.IsAnySystemTablePermissions();
        }
        /// <summary>
        /// 检测表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isCache">是否缓存</param>
       /// <returns>值</returns>
        public bool IsAnyTable(string tableName, bool isCache = true)
        {
            return DbContext.DbMaintenance.IsAnyTable(tableName, isCache);
        }
        /// <summary>
        /// 检测列是否自增列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列名</param>
       /// <returns>值</returns>
        public bool IsIdentity(string tableName, string column)
        {
            return DbContext.DbMaintenance.IsIdentity(tableName, column);
        }
        /// <summary>
        /// 检测列是否主键
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列名</param>
       /// <returns>值</returns>
        public bool IsPrimaryKey(string tableName, string column)
        {
            return DbContext.DbMaintenance.IsPrimaryKey(tableName, column);
        }
        /// <summary>
        /// 重置列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="oldColumnName">旧列名</param>
        /// <param name="newColumnName">新列名</param>
       /// <returns>值</returns>
        public bool RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            return DbContext.DbMaintenance.RenameColumn(tableName, oldColumnName, newColumnName);
        }
        /// <summary>
        /// 重置表数据
        /// </summary>
        /// <param name="tableName">表名</param>
       /// <returns>值</returns>
        public bool TruncateTable(string tableName)
        {
            return DbContext.DbMaintenance.TruncateTable(tableName);
        }
        /// <summary>
        /// 修改列信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列信息</param>
       /// <returns>值</returns>
        public bool UpdateColumn(string tableName, DbColumnInfo column)
        {
            return DbContext.DbMaintenance.UpdateColumn(tableName, column);
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns>返回值</returns>
        public DateTime GetDataBaseTime()
        {
            return DbContext.GetDate();
        }

        #endregion

        #region 保存或者插入
        /// <summary>
        /// 该功能是根据主键判段是否存在，如果存在则更新，不存在则插入，支持批量操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">插入指定列</param>
        /// <param name="insertIgnoreColumns">不插入指定列</param>
        /// <param name="updateColumns">更新指定列</param>
        /// <param name="updateIgnoreColumns">不更新指定列</param>
        /// <returns></returns>
        public T Saveable<T>(T entity, Expression<Func<T, object>> insertColumns=null, Expression<Func<T, object>> insertIgnoreColumns = null, Expression<Func<T, object>> updateColumns = null, Expression<Func<T, object>> updateIgnoreColumns = null) where T : class, new()
        {

            var up= DbContext.Saveable(entity);
            if (insertColumns != null)
            {
                up = up.InsertColumns(insertColumns);
            }

            if (insertIgnoreColumns != null)
            {
                up = up.InsertIgnoreColumns(insertIgnoreColumns);
            }

            if (updateColumns != null)
            {
                up = up.UpdateColumns(updateColumns);
            }

            if (updateIgnoreColumns != null)
            {
                up = up.UpdateIgnoreColumns(updateIgnoreColumns);
            }
            return up.ExecuteReturnEntity();
        }

        /// <summary>
        /// 该功能是根据主键判段是否存在，如果存在则更新，不存在则插入，支持批量操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="insertColumns">插入指定列</param>
        /// <param name="insertIgnoreColumns">不插入指定列</param>
        /// <param name="updateColumns">更新指定列</param>
        /// <param name="updateIgnoreColumns">不更新指定列</param>
        /// <returns></returns>
        public List<T> Saveable<T>(List<T> entity, Expression<Func<T, object>> insertColumns = null, Expression<Func<T, object>> insertIgnoreColumns = null, Expression<Func<T, object>> updateColumns = null, Expression<Func<T, object>> updateIgnoreColumns = null) where T : class, new()
        {

            var up = DbContext.Saveable(entity);
            if (insertColumns != null)
            {
                up = up.InsertColumns(insertColumns);
            }

            if (insertIgnoreColumns != null)
            {
                up = up.InsertIgnoreColumns(insertIgnoreColumns);
            }

            if (updateColumns != null)
            {
                up = up.UpdateColumns(updateColumns);
            }

            if (updateIgnoreColumns != null)
            {
                up = up.UpdateIgnoreColumns(updateIgnoreColumns);
            }
            return up.ExecuteReturnList();
        }

        #endregion

        #region 新增 
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param>  
        /// <returns>操作影响的行数</returns>
        public int Add<T>(T entity) where T : class, new()
        {
            try
            {
                var result = DbContext.Insertable(entity).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entitys">泛型集合</param> 
        /// <returns>操作影响的行数</returns>
        public int Add<T>(List<T> entitys) where T : class, new()
        {
            try
            {
                if (entitys.Count == 0) return 1;
                var result = DbContext.Insertable(entitys).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="keyValues">字典集合（Key:列名 Value:值）</param> 
        /// <returns>操作影响的行数</returns>
        public int Add<T>(Dictionary<string, object> keyValues) where T : class, new()
        {
            try
            {
                var result = DbContext.Insertable<T>(keyValues).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param>  
        /// <returns>返回实体</returns>
        public T AddReturnEntity<T>(T entity) where T : class, new()
        {
            try
            {
                var result = DbContext.Insertable(entity).ExecuteReturnEntity();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param>  
        /// <returns>返回自增列</returns>
        public int AddReturnIdentity<T>(T entity) where T : class, new()
        {
            try
            {
                var result = DbContext.Insertable(entity).ExecuteReturnIdentity();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 新增
        /// </summary> 
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param>  
        /// <returns>返回bool</returns>
        public bool AddReturnBool<T>(T entity) where T : class, new()
        {
            try
            {
                var result = DbContext.Insertable(entity).ExecuteCommand()>0;
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entitys">泛型集合</param> 
        /// <returns>返回bool</returns>
        public bool AddReturnBool<T>(List<T> entitys) where T : class, new()
        {
            try
            {
                if (entitys.Count == 0) return true;
                var result = DbContext.Insertable(entitys).ExecuteCommand() > 0;
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }  

        #endregion

        #region 修改 
        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件 </param> 
        /// <param name="lstIgnoreColumns">不更新的列</param>
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update<T>(T entity, List<string> lstIgnoreColumns = null, bool isLock = true) where T : class, new()
        {
            try
            {
                IUpdateable<T> up = DbContext.Updateable(entity);
                if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                {
                    up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
                } 
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
         

        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entitys"> 实体对象集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件 </param> 
        /// <param name="lstIgnoreColumns">不更新的列</param>
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update<T>(List<T> entitys, List<string> lstIgnoreColumns = null, bool isLock = true) where T : class, new()
        {
            try
            {
                if (entitys.Count == 0) return 1;
                IUpdateable<T> up = DbContext.Updateable(entitys);
                if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                {
                    up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
                }
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand(); 
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param> 
        /// <param name="where"> 条件 </param>  
        /// <param name="lstIgnoreColumns">不更新的列</param>
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update<T>(T entity, Expression<Func<T, bool>> where, List<string> lstIgnoreColumns = null, bool isLock = true) where T : class, new()
        {
            try
            { 
                IUpdateable<T> up = DbContext.Updateable(entity);
                if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                {
                    up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
                } 
                    up = up.Where(where); 
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        } 

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="update"> 实体对象 </param> 
        /// <param name="where"> 条件 </param>  
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update<T>(Expression<Func<T, T>> update, Expression<Func<T, bool>> where=null, bool isLock = true) where T : class, new()
        {
            try
            {
                IUpdateable<T> up = DbContext.Updateable<T>().SetColumns(update); 
                if (where != null)
                {
                    up = up.Where(where);
                }
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                } 
                var result = up.ExecuteCommand(); 
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="keyValues">字典集合（Key:列名 Value:值）</param> 
        /// <param name="where"> 条件 </param>  
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update<T>(Dictionary<string, object> keyValues, Expression<Func<T, bool>> where = null, bool isLock = true) where T : class, new()
        {
            try
            {
                IUpdateable<T> up = DbContext.Updateable<T>(keyValues);
                if (where != null)
                {
                    up = up.Where(where);
                }
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// 批量修改需要更新的列
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entitys"> 实体对象（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件 </param> 
        /// <param name="updateColumns">更新指定列</param>
        /// <param name="wherecolumns">条件(为空则以主键更新,反之需要把wherecolumns中的列加到UpdateColumns中)</param>
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int UpdateColumns<T>(List<T> entitys, Expression<Func<T, object>> updateColumns, Expression<Func<T, object>> wherecolumns = null, bool isLock = true) where T : class, new()
        {
            try
            {
                if (entitys.Count == 0) return 1;
                IUpdateable<T> up = DbContext.Updateable(entitys).UpdateColumns(updateColumns);
                if (wherecolumns != null)
                {
                    up = up.WhereColumns(wherecolumns);
                }
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// 修改 通过RowVer及主键Code 更新
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param> 
        /// <param name="lstIgnoreColumns">不更新的列</param>
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int UpdateRowVer<T>(T entity, List<string> lstIgnoreColumns = null, bool isLock = true) where T : class, new()
        {
            try
            {
                Type ts = entity.GetType();
                var rowVerProperty = ts.GetProperty("RowVer");
                if (rowVerProperty == null)
                {
                    throw new Exception("Column RowVer Not Exist");
                }
                if (rowVerProperty.GetValue(entity, null) == null)
                {
                    throw new Exception("RowVer Value Is Null");
                }
                var codeProperty = ts.GetProperty("Code");
                if (codeProperty == null)
                {
                    throw new Exception("Column Code Not Exist");
                }
                if (codeProperty.GetValue(entity, null) == null)
                {
                    throw new Exception("Code Value Is Null");
                }
                var rowVerValue = int.Parse(rowVerProperty.GetValue(entity, null).ToString());
                var codeValue = codeProperty.GetValue(entity, null).ToString();
                var sqlWhere = $" RowVer={rowVerValue} AND Code='{codeValue}'";
                rowVerProperty.SetValue(entity, rowVerValue + 1, null);
                IUpdateable<T> up = DbContext.Updateable(entity);
                if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
                {
                    up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
                }
                up = up.Where(sqlWhere);
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }



        /// <summary>
        /// 修改 
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="update"> 实体对象 </param>  
        /// <param name="where"> 更新条件 </param>  
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int UpdateRowVer<T>(Expression<Func<T, T>> update, Dictionary<string, object> where, bool isLock = true) where T : class, new()
        {
            try
            {  
                if (!where.ContainsKey("RowVer"))
                {
                    throw new Exception("Column RowVer Not Exist");
                }
                if (where["RowVer"] == null)
                {
                    throw new Exception("RowVer Value Is Null");
                }
                if (update.Body.ToString().IndexOf("RowVer", StringComparison.Ordinal)==-1)
                {
                    throw new Exception("Column RowVer Update Is Null");
                }
                var sqlWhere = "";
                foreach (var item in where)
                {
                    sqlWhere +=string.IsNullOrWhiteSpace(sqlWhere)? $" {item.Key}='{item.Value}'" : $" and {item.Key}='{item.Value}'";
                }  
                IUpdateable<T> up = DbContext.Updateable<T>().SetColumns(update).Where(sqlWhere); 
                if (isLock)
                {
                    up = up.With(SqlWith.UpdLock);
                }
                var result = up.ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="primaryKeyValues">主键值</param> 
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int DeleteByPrimary<T>(List<object> primaryKeyValues, bool isLock = true) where T : class, new()
        {
            try
            {
                var result = isLock ?
                    DbContext.Deleteable<T>().In(primaryKeyValues).With(SqlWith.RowLock).ExecuteCommand()
                    : DbContext.Deleteable<T>().In(primaryKeyValues).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 （必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param> 
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete<T>(T entity, bool isLock = true) where T : class, new()
        {
            try
            {
                var result = isLock ?
                    DbContext.Deleteable(entity).With(SqlWith.RowLock).ExecuteCommand()
                    : DbContext.Deleteable(entity).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 （必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param> 
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete<T>(List<T> entity, bool isLock = true) where T : class, new()
        {
            try
            {
                var result = isLock ?
                    DbContext.Deleteable(entity).With(SqlWith.RowLock).ExecuteCommand()
                    : DbContext.Deleteable(entity).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="where"> 条件 </param> 
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete<T>(Expression<Func<T, bool>> where, bool isLock = true) where T : class, new()
        {
            try
            {
                var result = isLock ?
                    DbContext.Deleteable<T>().Where(where).With(SqlWith.RowLock).ExecuteCommand()
                    : DbContext.Deleteable<T>().Where(where).ExecuteCommand();
                return result;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        
        /// <summary>
        /// 通过多值(主键)删除数据集
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam> 
        /// <param name="inValues">数据集合</param>
       /// <returns>值</returns>
        public int DeleteIn<T>(List<dynamic> inValues) where T : class, new()
        {
            return DbContext.Deleteable<T>().With(SqlWith.RowLock).In(inValues).ExecuteCommand();
        }
         
        #endregion

        #region 查询

        #region 数据源

        ///// <summary>
        ///// 查询数据源
        ///// </summary>
        ///// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        ///// <returns>数据源</returns>
        //public ISugarQueryable<T> Queryable<T>() where T : class, new()
        //{
        //    return DbContext.Queryable<T>();
        //}

        #endregion

        #region 多表查询  

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (t1,t2) => new object[] {JoinType.Left,t1.UserNo==t2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (t1, t2) => new { Id =t1.UserNo, Id1 = t2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (t1, t2) =>t1.UserNo == "")</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<T, T2, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
       /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }


        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
       /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, TResult>(
                       Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
                       Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
                       Expression<Func<T, T2, T3, T4, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
       /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, T5, TResult>(
            Expression<Func<T, T2, T3, T4, T5,object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5,TResult>> selectExpression,
            Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, T5,T6, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }
        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, T5, T6, T7, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="T8">实体8</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="T8">实体8</typeparam>
        /// <typeparam name="T9">实体9</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuch<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selectExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).ToList();
            }
            return DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Where(whereLambda).Select(selectExpression).ToList();
        } 

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (t1,t2) => new object[] {JoinType.Left,t1.UserNo==t2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (t1, t2) => new { Id =t1.UserNo, Id1 = t2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }


        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }


        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, TResult>(
                       Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
                       Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, T5, TResult>(
            Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, T5, T6, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }
        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, T5, T6, T7, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="T8">实体8</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }

        /// <summary>
        ///查询-多表查询 
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="T4">实体4</typeparam>
        /// <typeparam name="T5">实体5</typeparam>
        /// <typeparam name="T6">实体6</typeparam>
        /// <typeparam name="T7">实体7</typeparam>
        /// <typeparam name="T8">实体8</typeparam>
        /// <typeparam name="T9">实体9</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="query">查询条件</param>  
        /// <param name="totalCount">总行数</param>  
        /// <returns>值</returns>
        public List<TResult> QueryMuchDescriptor<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression,
            Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selectExpression,
            QueryDescriptor query, out int totalCount) where T : class, new()
        {
            var up = DbContext.Queryable(joinExpression).With(SqlWith.NoLock).Select(selectExpression).MergeTable();

            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                up = up.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                up = up.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = up.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = up.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
        }

        #endregion

        #region 查询
        /// <summary>
        /// 查询-返回自定义数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="expression">返回值表达式</param> 
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns>值</returns>
        public TResult QuerySelect<T,TResult>( Expression<Func<T, TResult>> expression,Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        { 
            var datas = DbContext.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).Select(expression).First();
            return datas;
        }
        /// <summary>
        /// 查询-返回自定义数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="expression">返回值表达式</param> 
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns>值</returns>
        public List<TResult> QuerySelectList<T, TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().With(SqlWith.NoLock).Where(whereLambda).Select(expression).ToList();
            return datas;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns>值</returns>
        public T Query<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>().With(SqlWith.NoLock); 
            if (whereLambda != null)
            {
                up = up.Where(whereLambda);
            } 
            return up.First();
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="whereLambda">过滤条件</param> 
        /// <returns>实体</returns>
        public List<T> QueryWhereList<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>().With(SqlWith.NoLock);
            if (whereLambda != null)
            {
                up = up.Where(whereLambda);
            }
            return up.ToList();
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="query">过滤条件</param> 
        /// <returns>实体</returns>
        public List<T> QueryList<T>(QueryDescriptor query=null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>().With(SqlWith.NoLock);
            if (query != null)
            {
                if (query.Conditions != null)
                {
                    var conds = ParseCondition(query.Conditions);
                    up = up.Where(conds);
                }

                if (query.OrderBys != null)
                {
                    var orderBys = ParseOrderBy(query.OrderBys);
                    up = up.OrderBy(orderBys);
                }
            }  
            return up.ToList();
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="sql">sql</param>  
        /// <returns>实体</returns>
        public List<T> QuerySqlList<T>(string sql) where T : class, new()
        {
            return DbContext.SqlQueryable<T>(sql).ToList();
        }
         
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="query">过滤条件</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>实体</returns>
        public List<T> QueryPageList<T>(QueryDescriptor query, out int totalCount) where T : class, new()
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var listDatas = DbContext.Queryable<T>().With(SqlWith.NoLock);
            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                listDatas = listDatas.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                listDatas = listDatas.OrderBy(orderBys);
            }

            totalCount = 0;
            if (query.PageSize == 0)
            {
                var datas = listDatas.ToList();
                totalCount = datas.Count;
                return datas;
            }
            else
            {
                var datas = listDatas.ToPageList(query.PageIndex, query.PageSize, ref totalCount);
                return datas;
            }
           
        }
        /// <summary>
        /// 查询集合-多值
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="inFieldName">字段名</param>
        /// <param name="inValues">数据集合</param>
       /// <returns>值</returns>
        public List<T> In<T>(string inFieldName, List<dynamic> inValues) where T : class, new()
        {
            return DbContext.Queryable<T>().With(SqlWith.NoLock).In(inFieldName, inValues).ToList(); 
        }
        /// <summary>
        /// 查询集合-通过多值(主键)
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="values">主键数据集合</param>
       /// <returns>值</returns>
        public List<T> In<T>(List<dynamic> values) where T : class, new()
        {
            return DbContext.Queryable<T>().With(SqlWith.NoLock).In(values).ToList();
        } 

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="whereLambda">查询表达式</param>  
        /// <returns>实体</returns>
        public DataTable QueryDataTable<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>().With(SqlWith.NoLock);
            if (whereLambda != null)
            {
                up = up.Where(whereLambda);
            } 
            return up.ToDataTable();
        }
         
        /// <summary>
        /// 查询集合
        /// </summary> 
        /// <param name="sql">sql</param> 
        /// <returns>实体</returns>
        public DataTable QueryDataTable(string sql)
        {
            return DbContext.Ado.GetDataTable(sql);
        }

        /// <summary>
        /// 查询单个值
        /// </summary> 
        /// <param name="sql">sql</param> 
        /// <returns>单个值</returns>
        public object QuerySqlScalar(string sql)
        {
            return DbContext.Ado.GetScalar(sql);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="query">过滤条件</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>DataTable</returns>
        public DataTable QueryDataTablePageList<T>(QueryDescriptor query, out int totalCount) where T : class, new()
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            var listDatas = DbContext.Queryable<T>().With(SqlWith.NoLock);
            if (query.Conditions != null)
            {
                var conds = ParseCondition(query.Conditions);
                listDatas = listDatas.Where(conds);
            }

            if (query.OrderBys != null)
            {
                var orderBys = ParseOrderBy(query.OrderBys);
                listDatas = listDatas.OrderBy(orderBys);
            }

            totalCount = 0;
            var datas = listDatas.ToDataTablePage(query.PageIndex, query.PageSize, ref totalCount);
            return datas;
        }
        #endregion

        #region Mapper

        /// <summary>
        /// Mapper查询 一对多和一对一
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="mapperAction">操作(it, cache) =>
        /// { 
        ///     var all = cache.GetListByPrimaryKeys(vmodel => vmodel.Id);  //一次性查询出所要的外键引用数据 
        ///     it.School = all.FirstOrDefault(i => i.Id == it.Id); //一对一 
        ///     it.Schools = all.Where(i => i.Id == it.Id).ToList(); //一对多
        ///     /*用C#处理你想要的结果*/
        ///     it.Name = it.Name == null ? "null" : it.Name;
        ///  }
        /// </param>
        /// <param name="query">过滤条件</param>
        /// <returns></returns>
        public List<T> QueryMapper<T>(Action<T> mapperAction,QueryDescriptor query = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>(); 
            if (query != null)
            {
                if (query.Conditions != null)
                {
                    var conds = ParseCondition(query.Conditions);
                    up = up.Where(conds);
                }

                if (query.OrderBys != null)
                {
                    var orderBys = ParseOrderBy(query.OrderBys);
                    up = up.OrderBy(orderBys);
                }
            }
            var datas = up.Mapper(mapperAction).ToList();
            return datas;
        }

        /// <summary>
        /// Mapper查询 一对多和一对一
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="mapperAction">操作(it, cache) =>
        /// { 
        ///     var all = cache.GetListByPrimaryKeys(vmodel => vmodel.Id);  //一次性查询出所要的外键引用数据 
        ///     it.School = all.FirstOrDefault(i => i.Id == it.Id); //一对一 
        ///     it.Schools = all.Where(i => i.Id == it.Id).ToList(); //一对多
        ///     /*用C#处理你想要的结果*/
        ///     it.Name = it.Name == null ? "null" : it.Name;
        ///  }
        /// </param>
        /// <param name="whereLambda">过滤条件</param>
        /// <returns></returns>
        public List<T> QueryMapper<T>(Action<T> mapperAction, Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>();
            if (whereLambda != null)
            {
                up = up.Where(whereLambda);
            }
            var datas = up.Mapper(mapperAction).ToList();
            return datas;
        }

        #endregion

        #region 分组
        /// <summary>
        /// 分组
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam> 
        /// <param name="groupByLambda">分组表达式</param> 
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns>值</returns>
        public List<T> GroupBy<T>(Expression<Func<T, object>> groupByLambda, Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Where(whereLambda).GroupBy(groupByLambda).ToList();
            return datas;
        }

        /// <summary>
        /// 分组-返回自定义数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <param name="expression">返回值表达式</param> 
        /// <param name="groupByLambda">分组表达式</param> 
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns>值</returns>
        public List<TResult> GroupBy<T, TResult>(Expression<Func<T, TResult>> expression, Expression<Func<T, object>> groupByLambda, Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Where(whereLambda).GroupBy(groupByLambda).Select(expression).ToList();
            return datas;
        } 

        #endregion

        #region 存储过程

        /// <summary>
        /// 查询存储过程
        /// </summary> 
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataSet</returns>
        public DataSet QueryProcedureDataSet(string procedureName, List<SqlParameter> parameters)
        {
            var listParams = ConvetParameter(parameters);
            var datas = DbContext.Ado.UseStoredProcedure().GetDataSetAll(procedureName, listParams);
            return datas;
        }
        /// <summary>
        /// 查询存储过程
        /// </summary> 
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public DataTable QueryProcedure(string procedureName, List<SqlParameter> parameters)
        {
            var listParams = ConvetParameter(parameters);
            var datas = DbContext.Ado.UseStoredProcedure().GetDataTable(procedureName, listParams);
            return datas;
        }

        /// <summary>
        /// 查询存储过程
        /// </summary> 
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>单个值</returns>
        public object QueryProcedureScalar(string procedureName, List<SqlParameter> parameters)
        {
            var listParams = ConvetParameter(parameters);
            var datas = DbContext.Ado.UseStoredProcedure().GetScalar(procedureName, listParams);
            return datas;
        }

        #endregion

        #region Json

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="whereLambda">查询表达式</param>  
        /// <returns>Json</returns>
        public string QueryJson<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
        {
            ISugarQueryable<T> up = DbContext.Queryable<T>();
            if (whereLambda != null)
            {
                up = up.Where(whereLambda);
            } 
            return up.ToJson();
        }

        #endregion

        #region 其它

        /// <summary>
        /// 查询前多少条数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="num">数量</param>
       /// <returns>值</returns>
        public List<T> Take<T>(Expression<Func<T, bool>> whereLambda, int num) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Where(whereLambda).Take(num).ToList();
            return datas;
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
       /// <returns>值</returns>
        public T First<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Where(whereLambda).First();
            return datas;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
       /// <returns>值</returns>
        public bool IsExist<T>(Expression<Func<T, bool>> whereLambda) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Any(whereLambda);
            return datas;
        }
         
        /// <summary>
        /// 合计
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="field">字段</param> 
        /// <returns>值</returns>
        public int Sum<T>(string field) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Sum<int>(field);
            return datas;
        }
        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="field">字段</param> 
        /// <returns>值</returns>
        public TResult Max<T, TResult>(string field) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Max<TResult>(field);
            return datas;
        }
        /// <summary>
        /// 最小值
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="field">字段</param> 
        /// <returns>值</returns>
        public TResult Min<T, TResult>(string field) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Min<TResult>(field);
            return datas;
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="field">字段</param> 
       /// <returns>值</returns>
        public int Avg<T>(string field) where T : class, new()
        {
            var datas = DbContext.Queryable<T>().Avg<int>(field);
            return datas;
        }

        #endregion

        #region 流水号

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="key">列名</param>
        /// <param name="prefix">前缀</param>
        /// <param name="fixedLength">流水号长度</param>
        /// <param name="dateFomart">日期格式(yyyyMMdd) 为空前缀后不加日期,反之加</param>
        /// <returns></returns>
        public string CustomNumber<T>(string key, string prefix = "", int fixedLength = 4, string dateFomart = "") where T : class, new()
        {

            var listNumber = CustomNumber<T>(key, 1, prefix, fixedLength, dateFomart);
            return listNumber[0];
        }
        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="key">列名</param>
        /// <param name="prefix">前缀</param>
        /// <param name="fixedLength">流水号长度</param>
        /// <param name="dateFomart">日期格式(yyyyMMdd) 为空前缀后不加日期,反之加</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public List<string> CustomNumber<T>(string key, int num, string prefix = "", int fixedLength = 4, string dateFomart = "") where T : class, new()
        {

            List<string> numbers = new List<string>();
            var dateValue = dateFomart == "" ? "" : DateTime.Now.ToString(dateFomart);
            var fix = prefix.ToUpper() + dateValue;
            var maxValue = DbContext.Queryable<T>().Where(key + " LIKE '" + fix + "%' AND LEN(" + key + ")=" + (fix.Length + fixedLength)).Select(key).Max<string>(key);

            if (maxValue == null)
            {
                for (var i = 0; i < num; i++)
                {
                    var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
                    numbers.Add(tempNumber);
                }
            }
            else
            {
                if (maxValue.Substring(0, maxValue.Length - fixedLength) == prefix + dateValue)
                {
                    var tempLast = maxValue.Substring(maxValue.Length - fixedLength);
                    for (var i = 0; i < num; i++)
                    {
                        var tempNumber = fix + (int.Parse(tempLast) + i + 1).ToString().PadLeft(fixedLength, '0');
                        numbers.Add(tempNumber);
                    }
                }
                else
                {
                    for (var i = 0; i < num; i++)
                    {
                        var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
                        numbers.Add(tempNumber);
                    }
                }
            }
            return numbers;
        }


        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// SqlParameter转换SugarParameter
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        private List<SugarParameter> ConvetParameter(List<SqlParameter> parameters)
        {  
            var listParams = new List<SugarParameter>();
            foreach (var p in parameters)
            {
                var par = new SugarParameter(p.ParameterName, p.Value)
                {
                    DbType = p.DbType,
                    Direction = p.Direction
                };
                if (!string.IsNullOrWhiteSpace(p.TypeName))
                {
                    par.TypeName = p.TypeName;
                }
                listParams.Add(par);
            }

            return listParams;
        }

        /// <summary>
        /// 过滤条件转换
        /// </summary>
        /// <param name="contitons">过滤条件</param>
        /// <returns>值</returns>
        private List<IConditionalModel> ParseCondition(List<QueryCondition> contitons)
        {
            var conds = new List<IConditionalModel>();
            contitons.Insert(0, new QueryCondition
            {
                Operator = QueryOperator.Equal,
                Key = "1",
                Value = "1"
            });

            #region Or条件组装  
            var orConditional = new ConditionalCollections { ConditionalList = new List<KeyValuePair<WhereType, ConditionalModel>>() };
            foreach (var con in contitons.Where(m => m.Character == QueryCharacter.Or))
            {
                if (orConditional.ConditionalList.Count == 0)
                {
                    var cond = new KeyValuePair<WhereType, ConditionalModel>
                    (WhereType.And, new ConditionalModel()
                    {
                        FieldName = con.Key,
                        ConditionalType = (ConditionalType)(int)con.Operator,
                        FieldValue = con.Value.ToString()
                    });
                    orConditional.ConditionalList.Add(cond);
                }
                else
                {
                    var cond = new KeyValuePair<WhereType, ConditionalModel>
                    (WhereType.Or, new ConditionalModel()
                    {
                        FieldName = con.Key,
                        ConditionalType = (ConditionalType)(int)con.Operator,
                        FieldValue = con.Value.ToString()
                    });
                    orConditional.ConditionalList.Add(cond);
                }
            }
            #endregion

            #region And条件组装
            foreach (var con in contitons.Where(m => m.Character == QueryCharacter.And))
            {
                if (con.Key.Contains(","))
                {
                    ParseKeyOr(con, orConditional); 
                }
                else if (con.Operator == QueryOperator.DateRange)
                {
                    conds.AddRange(ParseRange(con, con.Operator));
                }
                else
                {
                    conds.Add(new ConditionalModel()
                    {
                        FieldName = con.Key,
                        ConditionalType = (ConditionalType)(int)con.Operator,
                        FieldValue = con.Value.ToString()
                    });
                }
            } 

            #endregion 

            //Or条件是否存在
            if (orConditional.ConditionalList.Count > 0)
            {
                conds.Add(orConditional);
            }

            return conds;
        }

        /// <summary>
        /// Key逗号隔开转换Or条件
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="conditionalCollections">条件集合</param>
        /// <returns>值</returns>
        private void ParseKeyOr(QueryCondition condition, ConditionalCollections conditionalCollections)
        {
            var objectKeys = condition.Key.Split(','); 
            foreach (var objKey in objectKeys)
            { 
                if (conditionalCollections.ConditionalList.Count == 0)
                {
                    var cond = new KeyValuePair<WhereType, ConditionalModel>
                    (WhereType.And, new ConditionalModel()
                    {
                        FieldName = objKey,
                        ConditionalType = (ConditionalType)(int)condition.Operator,
                        FieldValue = condition.Value.ToString()
                    });
                    conditionalCollections.ConditionalList.Add(cond);
                }
                else
                {
                    var cond = new KeyValuePair<WhereType, ConditionalModel>
                    (WhereType.Or, new ConditionalModel()
                    {
                        FieldName = objKey,
                        ConditionalType = (ConditionalType)(int)condition.Operator,
                        FieldValue = condition.Value.ToString()
                    });
                    conditionalCollections.ConditionalList.Add(cond);
                } 
            } 
        }

        /// <summary>
        /// 转换区域
        /// </summary>
        /// <param name="condition">过滤条件</param>
        /// <param name="queryOperator">条件类型</param>
        /// <returns>值</returns>
        private List<ConditionalModel> ParseRange(QueryCondition condition, QueryOperator queryOperator)
        {
            var objectValue = condition.Value.ToString().Split('|');
            
            var conditionalList = new List<ConditionalModel>();
            if (objectValue.Length == 2)
            {
                var startValue = objectValue[0];
                var endValue = objectValue[1];
                if (queryOperator == QueryOperator.DateRange)
                {
                    if (startValue.IndexOf(":", StringComparison.Ordinal) == -1)
                    {
                        startValue = startValue + " 00:00:00";
                    }
                    if (endValue.IndexOf(":", StringComparison.Ordinal) == -1)
                    {
                        endValue = endValue + " 23:59:59";
                    }
                }
                if (!string.IsNullOrWhiteSpace(objectValue[0]))
                {
                    conditionalList.Add(new ConditionalModel()
                    {
                        FieldName = condition.Key,
                        ConditionalType = ConditionalType.GreaterThanOrEqual,
                        FieldValue = startValue
                    });
                }
                if (!string.IsNullOrWhiteSpace(objectValue[1]))
                {
                    conditionalList.Add(new ConditionalModel()
                    {
                        FieldName = condition.Key,
                        ConditionalType = ConditionalType.LessThanOrEqual,
                        FieldValue = endValue
                    });
                }
            } 
            return conditionalList;
        }

        /// <summary>
        /// 排序转换
        /// </summary>
        /// <param name="orderBys">排序</param>
        /// <returns>值</returns>
        private string ParseOrderBy(List<OrderByClause> orderBys)
        {
            var conds = "";
            foreach (var con in orderBys)
            {
                switch (con.Order)
                {
                    case OrderSequence.Asc:
                        conds += $"{con.Sort} asc,";
                        break;
                    case OrderSequence.Desc:
                        conds += $"{con.Sort} desc,";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return conds.TrimEnd(',');
        }
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            
            DbContext.Ado.Dispose(); 
            DbContext.Dispose();

        }
    }
}
