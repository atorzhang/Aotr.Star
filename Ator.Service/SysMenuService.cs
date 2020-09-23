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
            var lstRolePageIds = new List<string>();
            var lstRoles = new List<string>();

            if (!string.IsNullOrEmpty(userId))
            {
                var sql = $@"select b.SysPageId from sys_userrole  a
                            left join sys_rolepage b on a.SysRoleId = b.SysRoleId
                            where a.`Status` = 1 and b.`Status` = 1 and a.SysUserId = '{userId}'";
                lstRolePageIds = await DbContext.Ado.SqlQueryAsync<string>(sql);
                //权限Id列表
                lstRoles = (await DbContext.GetListAsync<SysUserRole>(o => o.SysUserId == userId)).Select(o => o.SysRoleId).ToList();
            }

            var rootMenu = new MenusInfoResultDTO();
            //LogoInfo初始化
            var imageModel = await DbContext.GetByIdAsync<SysLinkItem>("0000_LogoImgLink");
            rootMenu.logoInfo.image = imageModel?.SysLinkImg;
            var siteNameModel = await DbContext.GetAsync<SysSetting>(o => o.SysSettingId == "SiteName");
            rootMenu.logoInfo.title = siteNameModel?.SetValue;

            //HomeInfo初始化
            rootMenu.homeInfo = new HomeInfo();

            //MenuInfo初始化
            var allSysPage = new List<SysPage>();

            //账号不是Admin且用户角色不是Admin,就开启菜单页面过滤
            if (userId != "0000_admin" && !lstRoles.Contains("0000_roleAdmin"))
            {
                allSysPage = DbContext.GetList<SysPage>(o => o.Status == 1 && lstRolePageIds.Contains(o.SysPageId), "SysPageParent,Sort");
            }
            else
            {
                allSysPage = DbContext.GetList<SysPage>(o => o.Status == 1, "SysPageParent,Sort");
            }
            
            var allTopPage = allSysPage.Where(o => string.IsNullOrEmpty(o.SysPageParent)).ToList();
            rootMenu.menuInfo = new List<SystemMenu>();
            foreach (var topPage in allTopPage)
            {
                var subMenu = new SystemMenu
                {
                    title = topPage.SysPageName,
                    href = topPage.SysPageUrl,
                    icon = topPage.SysPageImg,
                    id = topPage.SysPageId
                };
                GetTreeNodeListByNoLockedDTOArray(allSysPage, subMenu);
                rootMenu.menuInfo.Add(subMenu);
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
            var childreDataList = systemMenuEntities.Where(p => p.SysPageParent == rootNode.id);
            if (childreDataList != null && childreDataList.Count() > 0)
            {
                rootNode.child = new List<SystemMenu>();
                foreach (var item in childreDataList)
                {
                    SystemMenu treeNode = new SystemMenu()
                    {
                        id = item.SysPageId,
                        icon = item.SysPageImg,
                        href = item.SysPageUrl,
                        title = item.SysPageName,
                    };
                    rootNode.child.Add(treeNode);
                }

                foreach (var item in rootNode.child)
                {
                    GetTreeNodeListByNoLockedDTOArray(systemMenuEntities, item);
                }
            }
        }
    }
}
