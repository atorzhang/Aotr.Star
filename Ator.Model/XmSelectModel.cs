using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    public class XmSelectModel
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public bool disabled { get; set; }
        public List<XmSelectModel> children { get; set; } = new List<XmSelectModel>();
    }
}
