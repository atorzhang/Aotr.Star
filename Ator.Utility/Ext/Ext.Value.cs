using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Utility.Ext
{
    public static partial class Ext
    {
        /// <summary>
        /// 根据属性名获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValue(this object obj,string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
