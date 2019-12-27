using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    public class PageData<T>
    {
        public List<T> Rows { get; set; }
        public long Totals { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public PageData()
        {
        }
        public PageData(List<T> rows,int pageIndex,int pageSize,int count)
        {
            this.Rows = rows;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Totals = count;
        }
    }
}
