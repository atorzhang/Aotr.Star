using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ator.Site.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}