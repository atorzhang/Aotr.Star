using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    public class DTreeModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string checkArr { get; set; }
        public string parentId { get; set; }
    }

    public class LayUITreeModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool spread { get; set; } = true;
        public List<LayUITreeModel> children { get; set; } = new List<LayUITreeModel>();
    }
}
