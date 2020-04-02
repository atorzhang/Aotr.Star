using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    /// <summary>
    /// 菜单跟节点
    /// </summary>
    public class RootMenu
    {
        public Clearinfo clearInfo { get; set; } = new Clearinfo();
        public Homeinfo homeInfo { get; set; } = new Homeinfo();
        public Logoinfo logoInfo { get; set; } = new Logoinfo();
        public Dictionary<string, Menuinfo> menuInfo { get; set; } = new Dictionary<string, Menuinfo>();
    }

    /// <summary>
    /// 清除操作地址
    /// </summary>
    public class Clearinfo
    {
        public string clearUrl { get; set; } = "/api/clear.json";
    }

    /// <summary>
    /// 主页信息
    /// </summary>
    public class Homeinfo
    {
        public string title { get; set; } = "首页";
        public string icon { get; set; } = "fa fa-home";
        public string href { get; set; } = "/Admin/WelCome/Index";
    }

    /// <summary>
    /// Logo信息
    /// </summary>
    public class Logoinfo
    {
        public string title { get; set; }
        public string image { get; set; }
        public string href { get; set; }
    }

    /// <summary>
    /// 菜单信息
    /// </summary>
    public class Menuinfo
    {
        public string title { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
        public string target { get; set; }
        public List<Menuinfo> child { get; set; }
    }

}
