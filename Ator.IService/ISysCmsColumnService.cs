using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysCmsColumnService
    {
        List<KeyValuePair<string, string>> GetColumnList(string ColumnParent="");
        
    }
}
