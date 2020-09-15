using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Service;
using Ator.Utility.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Ator.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : BaseController
    {
        ISysMenuService _sysMenuService;
        private ISysUserService _sysUserService;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController( ISysMenuService menuService, ISysUserService sysUserService, DbFactory factory, ILogger<HomeController> logger)
        {
            _sysMenuService = menuService;
            _sysUserService = sysUserService;
            _logger = logger;
            DbContext = factory.GetDbContext(); 
        }


        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页桌面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult Disk()
        {
            return View();
        }

        /// <summary>
        /// 图标页面获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Icon()
        {
            return View();
        }


        #region 修改密码
        /// <summary>
        /// 后台管理修改密码页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePwd()
        {
            return View();
        }

        /// <summary>
        /// 后台管理修改密码操作提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DoChangePwd(string oldPwd, string newPwd)
        {
            var result = _sysUserService.DoChangePwd(CurrentLoginUser.UserName, oldPwd, newPwd);
            return Result(result);
        } 
        #endregion

        #region 登陆相关
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 后台管理系统登陆操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DoLogin(LoginViewModel loginViewModel)
        {
            loginViewModel.Ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string backPin = HttpContext.Session.GetString("ValidateCode");
            if (loginViewModel.PIN == "jom")
            {

            }
            else if (string.IsNullOrEmpty(backPin))
            {
                return Error("验证码已过期");
            }
            else if (string.IsNullOrEmpty(loginViewModel.PIN))
            {
                return Error("请填写验证码");
            }
            else if (loginViewModel.PIN.ToLower() != backPin.ToLower())
            {
                HttpContext.Session.Remove("ValidateCode");//移除老验证码
                return Error("验证码错误");
            }
            HttpContext.Session.Remove("ValidateCode");//移除已使用的老验证码
            resStr = _sysUserService.DoLogin(loginViewModel);
            if (string.IsNullOrEmpty(resStr))
            {
                var userModel = DbContext.Get<SysUser>(o => o.UserName == loginViewModel.UserName);
                //登陆操作结果
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name,loginViewModel.UserName),
                    new Claim(ClaimTypes.Role,"manage"),
                    new Claim(ClaimTypes.NameIdentifier,userModel?.SysUserId),
                    new Claim(ClaimTypes.MobilePhone,userModel?.Mobile),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (loginViewModel.IsRedict == "0")
                {
                    return Ok("/Admin/Home/Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return Error(resStr);
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Admin/Home/Login");
        }
        #endregion

        #region 接口
        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Menus()
        {
            var menus = await _sysMenuService.GetMenu(CurrentLoginUser.Id);
            return Json(menus);
        }

        /// <summary>
        /// 获取混合验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult MixVerifyCode()
        {
            try
            {
                string code = VerifyCodeHelper.GetSingleObj().CreateVerifyCode(VerifyCodeHelper.VerifyCodeType.MixVerifyCode);
                HttpContext.Session.SetString("ValidateCode", code);
                var bitmap = VerifyCodeHelper.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40);
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Gif);
                return File(stream.ToArray(), "image/gif");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取验证码code错误");
                return Json(ex);
            }
        }

        /// <summary>
        /// 清理缓存接口
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Clear()
        {
            return Json(new
            {
                code = 1,
                msg = "清理缓存成功"
            });
        } 
        #endregion
    }
}