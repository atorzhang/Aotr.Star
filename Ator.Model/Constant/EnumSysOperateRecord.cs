using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Constant
{
    public enum EnumSysOperateRecordResult : short
    {
        Success = 1,//成功
        Fail = 2,//失败
    }

    public enum EnumSysOperateRecordType : short
    {
        Add = 1,//添加
        Del = 2,//删除
        Mdf = 3,//修改
        Find = 4,//查询
        Login =5,//登录
        Reg = 6,//注册
    }
}
