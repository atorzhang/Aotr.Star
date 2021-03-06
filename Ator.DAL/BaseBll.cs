﻿
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Ator.DbEntity;
using Ator.DbEntity.SqlSuger;

namespace Ator.DAL
{
    /// <summary>
    /// 业务基类
    /// </summary>
    public class BaseBll<T, TResult> where T : class, new() where TResult : class
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static TResult Instance()
        {
            return new T() as TResult;
        }

        #region 查询条件
        /// <summary>
        /// 查询条件是否存在
        /// </summary>
        /// <param name="conditions">条件集合</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsExistCondition(List<QueryCondition> conditions, string key)
        {
            var cond = conditions.Find(m => m.Key == key);
            if (cond == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取查询条件值
        /// </summary>
        /// <param name="conditions">条件集合</param>
        /// <param name="key">键</param>
        /// <param name="isRemove">是否移除</param>
        /// <returns></returns>
        public string FindConditionValue(List<QueryCondition> conditions, string key, bool isRemove = true)
        {
            var value = "";
            var cond = conditions.Find(m => m.Key == key);
            if (cond != null)
            {
                value = cond.Value.ToString();
                if (isRemove)
                {
                    conditions.Remove(cond);
                }
            }
            return value;
        }

       
        #endregion
      

        }
}
