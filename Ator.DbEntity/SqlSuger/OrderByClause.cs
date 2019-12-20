using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.DbEntity.SqlSuger
{
    public class OrderByClause
    {
        public string Sort { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public OrderSequence Order { get; set; }
    }
}
