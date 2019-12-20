namespace Ator.DbEntity.SqlSuger
{
    /// <summary>
    /// 查询条件枚举
    /// </summary>
    public enum QueryOperator
    {
        /// <summary>
        /// 相等
        /// </summary>
        Equal,
        /// <summary>
        /// 匹配
        /// </summary>
        Like,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 大于或等于
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 小于或等于
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// 等于集合
        /// </summary>
        In,
        /// <summary>
        /// 不等于集合
        /// </summary>
        NotIn,
        /// <summary>
        /// 左边匹配
        /// </summary>
        LikeLeft,
        /// <summary>
        /// 右边匹配
        /// </summary>
        LikeRight,
        /// <summary>
        /// 不相等
        /// </summary>
        NoEqual,
        /// <summary>
        /// 为空或空
        /// </summary>
        IsNullOrEmpty,
        /// <summary>
        /// 不为空
        /// </summary>
        IsNot,
        /// <summary>
        /// 不匹配
        /// </summary>
        NoLike,
        /// <summary>
        /// 时间段 值用 "|" 隔开
        /// </summary>
        DateRange
    }

    /// <summary>
    /// 查询条件运算符
    /// </summary>
    public enum QueryCharacter
    {
        /// <summary>
        /// 且
        /// </summary>
        And,
        /// <summary>
        /// 或
        /// </summary>
        Or,
    }
}
