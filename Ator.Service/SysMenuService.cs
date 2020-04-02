using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Ator.Repository;
using Ator.DbEntity.Sys;
using Ator.DbEntity.Factory;
using Ator.Model;
using System.Threading.Tasks;
using Ator.IService;

namespace Ator.Service
{
    public class SysMenuService : ISysMenuService
    {
        SqlSugarClient DbContext;//注入数据库操作类
        public SysMenuService(DbFactory factory)
        {
            DbContext = factory.GetDbContext();
        }

        /// <summary>
        /// 获取菜单，后期改成根据用户权限获取相应菜单
        /// </summary>
        /// <returns></returns>
        public async Task<RootMenu> GetMenu(string userId = "")
        {
            RootMenu rootMenu = new RootMenu();
            rootMenu.logoInfo.image = (await DbContext.GetByIdAsync<SysLinkItem>("9f17e9bafa1948dda8b7ab1918d18b16"))?.SysLinkImg;
            rootMenu.logoInfo.title = (await DbContext.GetAsync<SysSetting>(o => o.SysSettingId == "SiteName"))?.SetValue;

            rootMenu.menuInfo = new Dictionary<string, Menuinfo>();

            var currencyMenu = new Menuinfo()
            {
                title = "常规管理",
                icon = "fa fa-address-book",
                //child = 
            };

            rootMenu.menuInfo.Add("currency", currencyMenu);
            return rootMenu;
        }

        private List<Menuinfo> GetMainMenes()
        {
            List<Menuinfo> lstData = new List<Menuinfo>();
            var all = DbContext.GetList<SysPage>(o => o.Status == 1, "Sort");
            return lstData;
        }
    }
}
