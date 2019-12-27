using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISysDictionaryService
    {
        List<KeyValuePair<string, string>> GetDictionaryList();
    }
}
