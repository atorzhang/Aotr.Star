using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.IService;
using Ator.Service;
using Microsoft.AspNetCore.Mvc;

namespace Ator.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : BaseController
    {
        ISysMenuService _sysMenuService;
        public HomeController(ISysMenuService menuService)
        {
            _sysMenuService = menuService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Icon()
        {
            return View();
        }
        
        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Menus()
        {
            var menus = await _sysMenuService.GetMenu();
            return Json(menus);
        }

    }
}