using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ator.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Ator.Site
{
    /// <summary>
    /// 控制器基类
    /// </summary>

    public class BaseController : Controller
    {
        public SqlSugarClient DbContext;//注入数据库操作类
        protected LayuiData apiResult;
        protected string resStr = string.Empty;
        protected bool result = false;
        #region 属性

        public string GuidKey
        {
            get
            {
                return Utility.Helper.GuidHelper.NewSequentialGuid().ToString("N");
            }
        }

        //后台登录数据保存
        public UserViewModel CurrentLoginUser
        {
            get
            {
                var principal = HttpContext.User;
                if (principal != null)
                {
                    return new UserViewModel()
                    {
                        UserName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                        Mobile = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value,
                        Id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// 返回实体验证错误信息
        /// </summary>
        /// <returns></returns>
        public string GetModelErrMsg()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelstate = ModelState[key];
                    if (modelstate.Errors.Any())
                    {
                        return modelstate.Errors.FirstOrDefault().ErrorMessage;
                    }
                }
            }
            return string.Empty;
        }

        public UserInfo _UserInfo
        {
            get
            {
                return new UserInfo(HttpContext);
            }
        }
        //前台登录数据保存
        public class UserInfo
        {
            public HttpContext httpContext;
            public string UserName
            {
                get
                {
                    return httpContext.Session.GetString("UserName");
                }
                set
                {
                    httpContext.Session.SetString("UserName", value);
                }
            }
            public string UserId
            {
                get
                {
                    return httpContext.Session.GetString("UserId");
                }
                set
                {
                    httpContext.Session.SetString("UserId", value);
                }
            }
            public string Avatar
            {
                get
                {
                    return httpContext.Session.GetString("Avatar");
                }
                set
                {
                    httpContext.Session.SetString("Avatar", value);
                }
            }
            public string UserUnit
            {
                get
                {
                    return httpContext.Session.GetString("UserUnit");
                }
                set
                {
                    httpContext.Session.SetString("UserUnit", value);
                }
            }
            public string UserType
            {
                get
                {
                    return httpContext.Session.GetString("UserType");
                }
                set
                {
                    httpContext.Session.SetString("UserType", value);
                }
            }
            public UserInfo(HttpContext context)
            {
                httpContext = context;
            }
        }
        #endregion
        /// <summary>
        /// 成功返回单个数据object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public IActionResult Ok(object data, string msg = "", int code = 0)
        {
            apiResult = new LayuiData()
            {
                msg = msg,
                data = data,
                code = code
            };
            return Json(apiResult);
        }
        public IActionResult Ok(string msg = "")
        {
            apiResult = new LayuiData()
            {
                msg = msg
            };
            return Json(apiResult);
        }
        public IActionResult Ok<T>(List<T> data, string msg = "")
        {
            apiResult = new LayuiData()
            {
                data = data,
                msg = msg,
                count = data.LongCount(),
            };
            return Json(apiResult);
        }
        public IActionResult Ok<T>(List<T> data, long count, string msg = "")
        {
            apiResult = new LayuiData()
            {
                data = data,
                msg = msg,
                count = count,
            };
            return Json(apiResult);
        }

        public IActionResult Error(string msg = "", int code = 0)
        {
            apiResult = new LayuiData()
            {
                success = false,
                msg = msg,
                code = code
            };
            return Json(apiResult);
        }

        public IActionResult Result(string msg, string data = "")
        {
            if (string.IsNullOrEmpty(msg))
            {
                return Ok(data, "");
            }
            return Error(msg);
        }
        public IActionResult Result(bool result)
        {
            if (result)
            {
                return Ok();
            }
            return Error();
        }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Id { get; set; }
    }
}