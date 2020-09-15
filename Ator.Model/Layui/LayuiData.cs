using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    /// <summary>
    /// layui需要的数据类
    /// </summary>
    public class LayuiData
    {
        public int code { get; set; } = 0;
        public long count { get; set; } = 0;
        public bool success { get; set; } = true;
        public dynamic data { get; set; }
        public string msg { get; set; } = "";
    }
}
