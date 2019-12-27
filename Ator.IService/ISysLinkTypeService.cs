using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysLinkTypeService
    {
        List<KeyValuePair<string, string>> GetLinkTypeList();
    }
}
