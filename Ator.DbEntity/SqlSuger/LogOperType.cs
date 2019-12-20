using System.ComponentModel; 

namespace Ator.DbEntity.SqlSuger
{
    /// <summary>
    /// 系统日志操作类型
    /// </summary>
    public enum LogOperType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Add,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete,
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Release,
        /// <summary>
        /// 废除
        /// </summary>
        [Description("废除")]
        Abolish,
        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        Query,
        /// <summary>
        /// 导出
        /// </summary>
        [Description("导出")]
        Export,
        /// <summary>
        /// 下载
        /// </summary>
        [Description("下载")]
        Download,
        /// <summary>
        /// 导入
        /// </summary>
        [Description("导入")]
        Import,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disable,
        /// <summary>
        /// 打印
        /// </summary>
        [Description("打印")]
        Print,
        /// <summary>
        /// 审核
        /// </summary>
        [Description("审核")]
        Audit,
        /// <summary>
        /// 重置密码
        /// </summary>
        [Description("重置密码")]
        ResetPassword,
        /// <summary>
        /// 修改密码
        /// </summary>
        [Description("修改密码")]
        UpdatePassword,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel,
        /// <summary>
        /// 撤销
        /// </summary>
        [Description("撤销")]
        Revoke,
        /// <summary>
        /// 确定
        /// </summary>
        [Description("确定")]
        Confirm,
        /// <summary>
        /// 打回
        /// </summary>
        [Description("打回")]
        BackTo,
        /// <summary>
        /// 同步数据
        /// </summary>
        [Description("同步数据")]
        Synchrodata,
        /// <summary>
        /// 反馈
        /// </summary>
        [Description("反馈")]
        FeedBack,
        /// <summary>
        /// 退出
        /// </summary>
        [Description("退出")]
        Close,
        /// <summary>
        /// 维护
        /// </summary>
        [Description("维护")]
        Edit,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Lock,
        /// <summary>
        /// 解锁
        /// </summary>
        [Description("解锁")]
        UnLock,
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        Other

    }
}
