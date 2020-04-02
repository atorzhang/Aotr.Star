using Ator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.IService
{
    public interface ISelectHelper
    {
        string GenOptions(List<SelectItem> lstItem, string seleted = "", bool addNull = false, string nullTitle = "请选择");
        string GenOptionsByDic(string dicId, string seleted = "", bool addNull = false, string nullTitle = "请选择");
    }
}
