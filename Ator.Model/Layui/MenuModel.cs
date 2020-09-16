using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Model
{
    /// <summary>
    /// 菜单跟节点
    /// </summary>
    public class MenusInfoResultDTO
    {
        /// <summary>
        /// 权限菜单树
        /// </summary>
        public List<SystemMenu> menuInfo { get; set; } = new List<SystemMenu>();

        /// <summary>
        /// logo
        /// </summary>
        public LogoInfo logoInfo { get; set; } = new LogoInfo();

        /// <summary>
        /// Home
        /// </summary>
        public HomeInfo homeInfo { get; set; } = new HomeInfo();

        public Clearinfo clearinfo { get; set; } = new Clearinfo();

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
    public class HomeInfo
    {
        public string title { get; set; } = "首页";
        public string href { get; set; } = "/Admin/Home/Disk";
    }

    /// <summary>
    /// Logo信息
    /// </summary>
    public class LogoInfo
    {
        public string title { get; set; } = "sdsdsdsff";
        public string image { get; set; } = "images/logo.png";
        public string href { get; set; } = "";
    }

    /// <summary>
    /// 树结构对象
    /// </summary>
    public class SystemMenu
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string pId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 节点地址
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 新开Tab方式
        /// </summary>
        public string target { get; set; } = "_self";

        /// <summary>
        /// 菜单图标样式
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<SystemMenu> child { get; set; }
    }
 
}
