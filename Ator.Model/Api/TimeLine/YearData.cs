using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api.TimeLine
{
    public class YearData
    {
        public string year { get; set; }
        public List<MonthData> months { get; set; } = new List<MonthData>();
    }
}
