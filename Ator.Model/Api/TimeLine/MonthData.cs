using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api.TimeLine
{
    public class MonthData
    {
        public string month { get; set; }
        public List<DayData> data { get; set; } = new List<DayData>();
    }
}
