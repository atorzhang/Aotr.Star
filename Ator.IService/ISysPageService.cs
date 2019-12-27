using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysPageService
    {
        List<KeyValuePair<string, string>> GetPageList();
        
    }
}
