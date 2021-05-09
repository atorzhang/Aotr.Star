using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Repository.Sys;
using Ator.Utility.Ext;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ator.Service
{
    public class SysRolePageService : Repository<DbFactory, SysPageRepository> , ISysRolePageService
    {
        public SysRolePageService(DbFactory factory) : base(factory)
        {
         
        }

        public List<LayUITreeModel> GetTreeList()
        {
            List<LayUITreeModel> data = new List<LayUITreeModel>();
            var allRoles = DbContext.GetList<SysRole>(o => o.Status == 1, "Sort");
            //从上到下的算法。层级越多越麻烦，因此只计算到3级
            foreach (var item in allRoles)
            {
                data.Add(new LayUITreeModel
                {
                    id = item.SysRoleId,
                    name = item.RoleName,
                });
            }
            return data;
        }

        public List<DTreeModel> GetDTreeList()
        {
            List<DTreeModel> data = new List<DTreeModel>(); 
            var allRoles = DbContext.GetList<SysRole>(o => o.Status == 1,"Sort");
            //从上到下的算法。层级越多越麻烦，因此只计算到3级
            foreach (var item in allRoles) 
            {
                data.Add(new DTreeModel
                {
                    id = item.SysRoleId,
                    checkArr = "0",
                    parentId = "0",
                    title = item.RoleName
                });
            }
            return data;
        }

        /// <summary>
        /// 获取xmselect角色授权页面的下拉框数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<XmSelectModel> GetAuthXmSelectList(string roleId)
        {
            //已有权限
            var rolePageIds = DbContext.GetList<SysRolePage>(o => o.Status == 1 && o.SysRoleId == roleId, "Sort").Select(o => o.SysPageId).ToList();

            //所有页面信息
            var allPages = DbContext.GetList<SysPage>(o => o.Status == 1, "Sort");
            var allTopPage = allPages.Where(o => string.IsNullOrEmpty(o.SysPageParent)).ToList();
            var res = new List<XmSelectModel>();
            foreach (var topPage in allTopPage)
            {
                var subXmMenu = new XmSelectModel
                {
                    value = topPage.SysPageId,
                    name = topPage.SysPageName,
                    children = new List<XmSelectModel>(),
                };
                GenXmSelect(allPages, subXmMenu, rolePageIds);
                res.Add(subXmMenu);
            }
            return res;
        }

        /// <summary>
        /// 递归处理数据
        /// </summary>
        /// <param name="systemMenuEntities"></param>
        /// <param name="rootNode"></param>
        public static void GenXmSelect(List<SysPage> systemMenuEntities, XmSelectModel rootNode,List<string> rolePageIds)
        {
            if (systemMenuEntities == null || systemMenuEntities.Count <= 0)
            {
                return;
            }
            var childreDataList = systemMenuEntities.Where(p => p.SysPageParent == rootNode.value);
            if (childreDataList != null && childreDataList.Count() > 0)
            {
                rootNode.children = new List<XmSelectModel>();
                foreach (var item in childreDataList)
                {
                    XmSelectModel treeNode = new XmSelectModel()
                    {
                        value = item.SysPageId,
                        name = item.SysPageName,
                    };
                    if (rolePageIds.Contains(item.SysPageId))
                    {
                        treeNode.selected = true;
                    }
                    rootNode.children.Add(treeNode);
                }

                foreach (var item in rootNode.children)
                {
                    GenXmSelect(systemMenuEntities, item, rolePageIds);
                }
            }
        }

        /// <summary>
        /// 设置角色页面权限
        /// </summary>
        /// <param name="SysRoleId"></param>
        /// <param name="AuthPages"></param>
        /// <returns></returns>
        public async Task<int> SetAuthPage(string SysRoleId, string AuthPages)
        {
            //该角色已有权限
            var rolePages = DbContext.GetList<SysRolePage>(o => o.SysRoleId == SysRoleId, "Sort");
            var roleEnablePageIds = rolePages.Where(o => o.Status == 1).Select(o => o.SysPageId).ToList();
            var roleDisablePageIds = rolePages.Where(o => o.Status != 1).Select(o => o.SysPageId).ToList();
            var ct = 0;

            //删除权限
            if (string.IsNullOrEmpty(AuthPages))
            {
                //删除所有权限
                var sql = $@"update sys_rolepage set Status = 2 where SysRoleId =@SysRoleId";
                ct += await DbContext.Ado.ExecuteCommandAsync(sql,new SugarParameter("@SysRoleId", SysRoleId));
            }
            else
            {
                var newAuths = AuthPages.Split(',');
                foreach (var item in rolePages.Select(o => o.SysPageId))
                {
                    if(!newAuths.Contains(item))
                    {
                        var sql = $@"update sys_rolepage set Status = 2 where SysPageId =@SysPageId and SysRoleId=@SysRoleId";
                        ct += await DbContext.Ado.ExecuteCommandAsync(sql, new SugarParameter("@SysPageId", item), new SugarParameter("@SysRoleId", SysRoleId));
                    }
                }
            }

            //新增权限
            if (!string.IsNullOrEmpty(AuthPages))
            {
                foreach (var pageId in AuthPages.Split(','))
                {
                    //如果新增的权限是禁用的，将其启用
                    if (roleDisablePageIds.Contains(pageId))
                    {
                        var sql = $@"update sys_rolepage set Status = 1 where SysPageId =@SysPageId and SysRoleId=@SysRoleId";
                        ct += await DbContext.Ado.ExecuteCommandAsync(sql, new SugarParameter("@SysPageId", pageId),new SugarParameter("@SysRoleId", SysRoleId));
                    }
                    //如果启用的不包括就新增
                    else if (!roleEnablePageIds.Contains(pageId))
                    {
                        SysRolePage sysRolePage = new SysRolePage
                        {
                            SysRolePageId = Guid.NewGuid().ToString("n"),
                            CreateTime = DateTime.Now,
                            CreateUser ="",
                            Sort=0,
                            Status = 1,
                            SysPageId = pageId,
                            SysRoleId = SysRoleId,
                            Unchangeable = false,
                            Remark=""
                        };
                        ct += DbContext.Insert<SysRolePage>(sysRolePage) ? 1:0;
                    }
                }
            }
            
            return ct;
        }
    }
}
