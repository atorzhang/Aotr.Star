using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    public class SelectItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class KeyValueItem
    {
        public KeyValueItem()
        {

        }
        public KeyValueItem(string _key,string _value)
        {
            this.Key = _key;
            this.Value = _value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
