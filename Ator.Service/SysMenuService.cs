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
using System.Linq;

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
        public async Task<MenusInfoResultDTO> GetMenu(string userId = "")
        {
            var rootMenu = new MenusInfoResultDTO();
            //LogoInfo初始化
            var imageModel = await DbContext.GetByIdAsync<SysLinkItem>("LogoImgLink");
            rootMenu.LogoInfo.image = imageModel?.SysLinkImg;
            var siteNameModel = await DbContext.GetAsync<SysSetting>(o => o.SysSettingId == "SiteName");
            rootMenu.LogoInfo.title = siteNameModel?.SetValue;

            //HomeInfo初始化
            rootMenu.HomeInfo = new HomeInfo();

            //MenuInfo初始化
            var allSysPage = DbContext.GetList<SysPage>(o => o.Status == 1, "SysPageParent,Sort");
            var allTopPage = allSysPage.Where(o => string.IsNullOrEmpty(o.SysPageParent)).ToList();
            rootMenu.MenuInfo = new List<SystemMenu>();
            foreach (var topPage in allTopPage)
            {
                var subMenu = new SystemMenu
                {
                    Title = topPage.SysPageName,
                    Href = topPage.SysPageUrl,
                    Icon = topPage.SysPageImg,
                    Id = topPage.SysPageId
                };
                GetTreeNodeListByNoLockedDTOArray(allSysPage, subMenu);
                rootMenu.MenuInfo.Add(subMenu);
            }
            //返回实体
            return rootMenu;
        }


        /// <summary>
        /// 递归处理数据
        /// </summary>
        /// <param name="systemMenuEntities"></param>
        /// <param name="rootNode"></param>
        public static void GetTreeNodeListByNoLockedDTOArray(List<SysPage> systemMenuEntities, SystemMenu rootNode)
        {
            if (systemMenuEntities == null || systemMenuEntities.Count <= 0)
            {
                return;
            }
            var childreDataList = systemMenuEntities.Where(p => p.SysPageParent == rootNode.Id);
            if (childreDataList != null && childreDataList.Count() > 0)
            {
                rootNode.Child = new List<SystemMenu>();
                foreach (var item in childreDataList)
                {
                    SystemMenu treeNode = new SystemMenu()
                    {
                        Id = item.SysPageId,
                        Icon = item.SysPageImg,
                        Href = item.SysPageUrl,
                        Title = item.SysPageName,
                    };
                    rootNode.Child.Add(treeNode);
                }

                foreach (var item in rootNode.Child)
                {
                    GetTreeNodeListByNoLockedDTOArray(systemMenuEntities, item);
                }
            }
        }
    }
}
