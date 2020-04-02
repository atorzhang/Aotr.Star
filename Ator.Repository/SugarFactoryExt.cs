using Ator.DbEntity;
using Ator.Model;
using Ator.Utility.Ext;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Repository
{
    public static class SugarFactoryExtensions
    {

        #region 根据主键获取实体对象

        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TSource GetById<TSource>(this SqlSugarClient db, dynamic id) where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().InSingle(id);
        }
        /// <summary>
        /// [异步]根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<TSource> GetByIdAsync<TSource>(this SqlSugarClient db, dynamic id) where TSource : EntityDb, new()
        {
            return await Task.Run( () =>
            {
                return db.Queryable<TSource>().InSingle(id);
            });
        }

        /// <summary>
        /// 根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TMap GetById<TSource, TMap>(this SqlSugarClient db, dynamic id) where TSource : EntityDb, new()
        {
            TSource model = db.Queryable<TSource>().InSingle(id);
            return model.Map<TSource, TMap>();
        }

        /// <summary>
        /// [异步]根据主键获取实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<TMap> GetByIdAsync<TSource, TMap>(this SqlSugarClient db, dynamic id) where TSource : EntityDb, new()
        {
            return await Task.Run(() =>
            {
                TSource model = db.Queryable<TSource>().InSingle(id);
                return model.Map<TSource, TMap>();
            });
        }

        #endregion

        #region 根据Linq表达式条件获取单个实体对象

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        public static TSource Get<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().Where(whereExp).Single();
        }

        /// <summary>
        /// [异步]根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp"></param>
        /// <returns></returns>
        public static async Task<TSource> GetAsync<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return await Task.Run(() =>
            {
                return db.Queryable<TSource>().Where(whereExp).Single();
            });
        }

        /// <summary>
        /// 根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static TMap Get<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            TSource model = db.Queryable<TSource>().Where(whereExp).Single();
            return model.Map<TSource, TMap>();
        }

        /// <summary>
        /// [异步]根据条件获取单个实体对象
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static async Task<TMap> GetAsync<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return await Task.Run(() =>
            {
                TSource model = db.Queryable<TSource>().Where(whereExp).Single();
                return model.Map<TSource, TMap>();
            });
        }

        #endregion

        #region 获取所有实体列表

        /// <summary>
        /// 获取所有实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<TSource> GetList<TSource>(this SqlSugarClient db, string orderBy = "") where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToList();
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        public static List<TMap> GetList<TSource, TMap>(this SqlSugarClient db, string orderBy = "") where TSource : EntityDb, new()
        {
            var result = db.Queryable<TSource>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToList();
            return result.Map<List<TSource>, List<TMap>>();
        }

        #endregion

        #region 根据Linq表达式条件获取列表

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static List<TSource> GetList<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp,string orderBy="") where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().Where(whereExp).OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToList();
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static List<TMap> GetList<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp, string orderBy = "") where TSource : EntityDb, new()
        {
            var result = db.Queryable<TSource>().Where(whereExp).OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToList();
            return result.Map<List<TSource>, List<TMap>>();
        }

        #endregion

        #region 根据Sugar条件获取列表

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar调价表达式集合</param>
        /// <returns></returns>
        public static List<TSource> GetList<TSource>(this SqlSugarClient db, List<IConditionalModel> conditionals) where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().Where(conditionals).ToList();
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar调价表达式集合</param>
        /// <returns></returns>
        public static List<TMap> GetList<TSource, TMap>(this SqlSugarClient db, List<IConditionalModel> conditionals) where TSource : EntityDb, new()
        {
            var result = db.Queryable<TSource>().Where(conditionals).ToList();
            return result.Map<List<TSource>, List<TMap>>();
        }

        #endregion

        #region 是否包含某个元素
        /// <summary>
        /// 是否包含某个元素
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static bool Exist<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return db.Queryable<TSource>().Where(whereExp).Any();
        }
        #endregion

        #region 新增实体对象
        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public static bool Insert<TSource>(this SqlSugarClient db, TSource insertObj) where TSource : EntityDb, new()
        {
            return db.Insertable(insertObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertDto"></param>
        /// <returns></returns>
        public static bool Insert<TSource, TMap>(this SqlSugarClient db, TSource insertDto) where TMap : EntityDb, new() where TSource : class, new()
        {
            var entity = insertDto.Map<TSource, TMap>();
            return db.Insertable(entity).ExecuteCommand() > 0;
        }
        #endregion

        #region 批量新增实体对象
        /// <summary>
        /// 批量新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public static bool InsertRange<TSource>(this SqlSugarClient db, List<TSource> insertObjs) where TSource : EntityDb, new()
        {
            return db.Insertable(insertObjs).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 批量新增实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TMap"></typeparam>
        /// <param name="db"></param>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public static bool InsertRange<TSource, TMap>(this SqlSugarClient db, List<TSource> insertObjs) where TMap : EntityDb, new()
        {
            var entitys = insertObjs.Map<List<TSource>, List<TMap>>();
            return db.Insertable(entitys).ExecuteCommand() > 0;
        }
        #endregion

        #region 更新单个实体对象
        /// <summary>
        /// 更新单个实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public static bool Update<TSource>(this SqlSugarClient db, TSource updateObj) where TSource : EntityDb, new()
        {
            return db.Updateable(updateObj).ExecuteCommand() > 0;
        }
        #endregion

        #region 根据条件批量更新实体指定列
        /// <summary>
        /// 根据条件批量更新实体指定列
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="columns">需要更新的列</param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static bool Update<TSource>(this SqlSugarClient db, Expression<Func<TSource, TSource>> columns, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return db.Updateable<TSource>().UpdateColumns(columns).Where(whereExp).ExecuteCommand() > 0;
        }
        #endregion

        #region 物理删除实体对象

        /// <summary>
        /// 物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="deleteObj"></param>
        /// <returns></returns>
        public static bool Delete<TSource>(this SqlSugarClient db, TSource deleteObj) where TSource : EntityDb, new()
        {
            return db.Deleteable<TSource>().Where(deleteObj).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">条件表达式</param>
        /// <returns></returns>
        public static bool Delete<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp) where TSource : EntityDb, new()
        {
            return db.Deleteable<TSource>().Where(whereExp).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键物理删除实体对象
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteById<TSource>(this SqlSugarClient db, dynamic id) where TSource : EntityDb, new()
        {
            return db.Deleteable<TSource>().In(id).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 根据主键批量物理删除实体集合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DeleteByIds<TSource>(this SqlSugarClient db, dynamic[] ids) where TSource : EntityDb, new()
        {
            return db.Deleteable<TSource>().In(ids).ExecuteCommand() > 0;
        }

        #endregion

        #region 分页查询

        /// <summary>
        /// 获取分页列表【页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TSource> GetPageList<TSource>(this SqlSugarClient db, int pageIndex, int pageSize, string orderBy = "") where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToPageList(pageIndex, pageSize, ref count);
            return new PageData<TSource>(result, pageIndex, pageSize, count);
        }

        /// <summary>
        /// 获取分页列表【页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TMap> GetPageList<TSource, TMap>(this SqlSugarClient db, int pageIndex, int pageSize, string orderBy = "") where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToPageList(pageIndex, pageSize, ref count);
            var pageResult = new PageData<TMap>(result.Map<TSource, TMap>(), pageIndex, pageSize, count);
            return pageResult;
        }

        #endregion

        #region 分页查询（排序）

        /// <summary>
        /// 获取分页列表【排序，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="orderExp">排序表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TSource> GetPageList<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp, Expression<Func<TSource, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().WhereIF(whereExp!=null, whereExp).OrderBy(orderExp, orderType).ToPageList(pageIndex, pageSize, ref count);
            return new PageData<TSource>(result, pageIndex, pageSize, count);
        }

        /// <summary>
        /// 获取分页列表【排序，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="orderExp">排序表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TMap> GetPageList<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp, Expression<Func<TSource, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().WhereIF(whereExp != null, whereExp).OrderBy(orderExp, orderType).ToPageList(pageIndex, pageSize, ref count);
            var pageResult = new PageData<TMap>(result.Map<TSource, TMap>(), pageIndex, pageSize, count);
            return pageResult;
        }

        #endregion

        #region 分页查询（Linq表达式条件）

        /// <summary>
        /// 获取分页列表【Linq表达式条件，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">Linq表达式条件</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TSource> GetPageList<TSource>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp,string orderBy, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().WhereIF(whereExp != null, whereExp).OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToPageList(pageIndex, pageSize, ref count);
            return new PageData<TSource>(result, pageIndex, pageSize, count);
        }

        /// <summary>
        /// 获取分页列表【Linq表达式条件，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="whereExp">Linq表达式条件</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TMap> GetPageList<TSource, TMap>(this SqlSugarClient db, Expression<Func<TSource, bool>> whereExp, string orderBy, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().WhereIF(whereExp!=null, whereExp).OrderByIF(!string.IsNullOrEmpty(orderBy), orderBy).ToPageList(pageIndex, pageSize, ref count);
            var pageResult = new PageData<TMap>(result.Map<TSource, TMap>(), pageIndex, pageSize, count);
            return pageResult;
        }

        #endregion

        #region 分页查询（Sugar条件）

        /// <summary>
        /// 获取分页列表【Sugar表达式条件，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar条件表达式集合</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TSource> GetPageList<TSource>(this SqlSugarClient db, List<IConditionalModel> conditionals, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().Where(conditionals).ToPageList(pageIndex, pageSize, ref count);
            return new PageData<TSource>(result, pageIndex, pageSize, count);
        }

        /// <summary>
        /// 获取分页列表【Sugar表达式条件，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource">数据源类型</typeparam>
        /// <typeparam name="TMap">数据源映射类型</typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar条件表达式集合</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TMap> GetPageList<TSource, TMap>(this SqlSugarClient db, List<IConditionalModel> conditionals, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().Where(conditionals).ToPageList(pageIndex, pageSize, ref count);
            var pageResult = new PageData<TMap>(result.Map<TSource, TMap>(), pageIndex, pageSize, count);
            return pageResult;
        }

        #endregion

        #region 分页查询（Sugar条件，排序）

        /// <summary>
        ///  获取分页列表【Sugar表达式条件，排序，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar条件表达式集合</param>
        /// <param name="orderExp">排序表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TSource> GetPageList<TSource>(this SqlSugarClient db, List<IConditionalModel> conditionals, Expression<Func<TSource, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().Where(conditionals).OrderBy(orderExp, orderType).ToPageList(pageIndex, pageSize, ref count);
            return new PageData<TSource>(result, pageIndex, pageSize, count);
        }

        /// <summary>
        ///  获取分页列表【Sugar表达式条件，排序，页码，每页条数】
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="db"></param>
        /// <param name="conditionals">Sugar条件表达式集合</param>
        /// <param name="orderExp">排序表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">页码（从0开始）</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public static PageData<TMap> GetPageList<TSource, TMap>(this SqlSugarClient db, List<IConditionalModel> conditionals, Expression<Func<TSource, object>> orderExp, OrderByType orderType, int pageIndex, int pageSize) where TSource : EntityDb, new()
        {
            int count = 0;
            var result = db.Queryable<TSource>().Where(conditionals).OrderBy(orderExp, orderType).ToPageList(pageIndex, pageSize, ref count);
            var pageResult = new PageData<TMap>(result.Map<TSource, TMap>(), pageIndex, pageSize, count);
            return pageResult;
        }

        #endregion
    }
}
