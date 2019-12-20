namespace Ator.DbEntity.SqlSuger
{
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum MsgStatus
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 短信验证码
        /// </summary>
        Dynamic = 2,
        /// <summary>
        /// 其它
        /// </summary>
        OtuOther = 10000,
    }
}
