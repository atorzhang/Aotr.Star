using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ator.Common.Web.Handler
{
    /// <summary>
    /// 验证jwtToken有效性
    /// </summary>
    public class ValidJtiHandler : AuthorizationHandler<ValidJtiRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJtiRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;

            // 检查 Jti 是否存在，自定义验证为实现
            var jti = context.User.FindFirst("jti")?.Value;
            //if (jti == null)
            //{
            //    context.Fail(); // 显式的声明验证失败
            //    return Task.CompletedTask;
            //}

            // 检查 jti 是否在黑名单
            var tokenExists = false;
            if (tokenExists)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement); // 显式的声明验证成功
            }
            return Task.CompletedTask;

        }
    }
    public class ValidJtiRequirement : IAuthorizationRequirement
    {
    }
}
