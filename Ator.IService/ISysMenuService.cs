using Ator.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ator.IService
{
    public interface ISysMenuService
    {
        Task<MenusInfoResultDTO> GetMenu(string userId = "");
    }
}
