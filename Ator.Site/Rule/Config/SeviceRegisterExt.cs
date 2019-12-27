using Ator.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ator.Site
{
    public static class SeviceRegisterExt
    {
        public static IServiceCollection AddServiceScoped(this IServiceCollection services)
        {
            foreach (var item in ReflectionHelper.GetClassName("Ator.Service"))
            {
                foreach (var typeArray in item.Value)
                {
                    services.AddScoped(typeArray, item.Key);
                }
            }
            return services;
        }
    }
}
