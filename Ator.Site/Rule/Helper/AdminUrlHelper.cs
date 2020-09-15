using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ator.Site.Rule.Helper
{
    public static class AdminUrlHelper
    {
        public static string ActionAdmin(this IUrlHelper urlHelper, string actionName,string HomeName ="Home", string AreaName = "Admin")
        {
            return $"/{AreaName}/{HomeName}/{actionName}";
        }
    }
}
