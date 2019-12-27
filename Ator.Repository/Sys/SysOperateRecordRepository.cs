
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.Model.Constant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysOperateRecordRepository : Repository<DbFactory>, IRepository
    {
        public SysOperateRecordRepository(DbFactory factory) : base(factory)
        {

        }
        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="SysUserId">操作用户ID</param>
        /// <param name="UserName">用户名</param>
        /// <param name="ClassName">操作类名</param>
        /// <param name="MethodName">操作方法名</param>
        /// <param name="Operate">操作内容</param>
        /// <param name="TableName">操作表名</param>
        /// <param name="Type">操作类型</param>
        /// <param name="Result">操作结果</param>
        /// <returns></returns>
        public bool InsertOperate(string ClassName,string MethodName,string Operate,string TableName, short Type = (short)EnumSysOperateRecordType.Find, short Result = (short)EnumSysOperateRecordResult.Success, string SysUserId = null, string UserName="")
        {
            return  DbContext.Insert<SysOperateRecord>(new SysOperateRecord
            {
                ClassName = ClassName,
                MethodName = MethodName,
                Operate = Operate,
                CreateTime = DateTime.Now,
                TableName = TableName,
                Type = Type,
                Result = Result,
                SysUserId = SysUserId,
                UserName = UserName,
                SysOperateRecordId = Guid.NewGuid().ToString("N"),
            });
        }
    }
}
