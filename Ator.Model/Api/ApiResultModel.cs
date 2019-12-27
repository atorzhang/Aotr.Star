using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model.Api
{
    public class ApiResultModel
    {
        public bool Success { get; set; } = true;
        public string Msg { get; set; } = "";
        public string Type { get; set; } = "";
        public object Data { get; set; } = "";
    }
    public class ListData
    {
        public object ListInfo { get; set; }
        public long Total { get; set; } = 0;
    }
}
