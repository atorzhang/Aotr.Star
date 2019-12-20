using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.DAL;
using Ator.DbEntity.Sys;
using Microsoft.AspNetCore.Mvc;
using Ator.Utility.Ext;
using SqlSugar;
using Ator.DbEntity.SqlSuger;

namespace Ator.Site.Controllers
{
    public class TestController : BaseController
    {
        public IActionResult Index()
        {
            var db = SugarHandler.Instance();
            var aa = new QueryDescriptor();
            var lst = db.QueryList<SysLinkItem>(); 
            return Json(lst);
        }
    }
}