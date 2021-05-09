using Ator.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ator.IService
{
    public interface ISysRolePageService
    {
        List<LayUITreeModel> GetTreeList();
        List<DTreeModel> GetDTreeList();
        List<XmSelectModel> GetAuthXmSelectList(string roleId);

        Task<int> SetAuthPage(string SysRoleId, string AuthPages);
    }
}
