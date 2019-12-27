using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    public class PagingViewModel
    {
        #region Page

        /// <summary>
        /// 每页开始的记录序号
        /// Linq分页查询时Skip的参数
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页显示条数
        /// Linq分页查询时Take的参数
        /// </summary>
        public int Limit { get; set; } = 20;

        /// <summary>
        /// 排序参数名
        /// </summary>
        public virtual string Ordering { get; set; } = "Sort,-CreateTime";//大部分表数据都有CreateTime,默认使用时间倒序排序

        /// <summary>
        /// 排序参数名正序(弃用，改为Ordering前面加负号为正反序，Ordering可以有多个字段用逗号隔开，exp:a,-b)
        /// </summary>
        //public virtual bool IsOrder { get; set; } = false;//排序方式，true正序，false反序

        #endregion
    }
}
