using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Ator.Common.Web.Helper;
using Ator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Ator.Common.Web.Config;
using Ator.DbEntity.Sys;
using Ator.DbEntity.Factory;
using Microsoft.Extensions.Configuration;

namespace Ator.Site.Controllers
{
    public class QQAuthorController : BaseController
    {
        private readonly ILogger _logger;
        HttpClient httpClient = new HttpClient();
        //qq登录配置
        public QQAuthor qqAuthor;

        public QQAuthorController(DbFactory factory, ILogger<QQAuthorController> logger, IConfiguration configuration)
        {
            DbContext = factory.GetDbContext();
            _logger = logger;

            qqAuthor = configuration.GetSection("QQAuthor").Get<QQAuthor>();
        }
        
        //第一步授权获取code的url,用户跳转到该地址授权
        string authorizeBaseUrl = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}";
        //第二步根据第一部授权成功后获取的code后台去请求得到Access_Token
        string accessBaseUrl = @"https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&state={3}&redirect_uri={4}";
        //第三步根据之前的Access_Token获取OpenId
        string openIdBaseUrl = "https://graph.qq.com/oauth2.0/me?access_token={0}";
        //第四部获取用户数据
        string userInfoBaseUrl = @"https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}";

        public IActionResult Index(string redictUrl="")
        {
            string state = Guid.NewGuid().ToString("N").Substring(0, 8);
            Response.Cookies.Append("state", state);//把状态写进Cookie
            //跳转授权
            return Redirect(string.Format(authorizeBaseUrl, qqAuthor.appId, qqAuthor.returnUrl, state));
        }

        //回调地址，成功授权回调将得到code
        public async Task<IActionResult> Return(string code)
        {
            string state = "";
            Request.Cookies.TryGetValue("state", out state);
            _logger.LogDebug($"进入回调：state:{state}");
            //发起请求
            string accessUrl = string.Format(accessBaseUrl, qqAuthor.appId, qqAuthor.appKey, code, state, qqAuthor.returnUrl);
            string accessStr = await httpClient.GetStringAsync(accessUrl);
            _logger.LogDebug($"获取到accessStr:{accessStr}");
            //解析返回内容
            string accessToken = accessStr.GetQueryString("access_token");
            
            //获取openid
            string openIdUrl = string.Format(openIdBaseUrl, accessToken);
            string openIdJson = await httpClient.GetStringAsync(openIdUrl);
            _logger.LogDebug($"获取到openIdJson:{openIdJson}");
            Match mt = Regex.Match(openIdJson, "{.*?}");
            JObject jObjectOpenId = JObject.Parse(mt.Value);
            string openId = jObjectOpenId["openid"].ToString();
            
            //获取用户数据
            string userInfoUrl = string.Format(userInfoBaseUrl, accessToken, qqAuthor.appId, openId);
            string userInfoJson = await httpClient.GetStringAsync(userInfoUrl);
            _logger.LogInformation($"获取到userInfoJson:{userInfoJson}");
            JObject jObjectuserInfo = JObject.Parse(userInfoJson);

            //判断openid是否在数据库中存在了
            var userModel = DbContext.Get<SysUser>(o => o.QQ == openId);
            if(userModel == null)
            {
                //不存在插入
                userModel = new SysUser
                {
                    SysUserId = openId,
                    UserType = "2",//1-管理用户，2-普通用户，1包含2
                    Status = 1,
                    UserName = openId,
                    NikeName = jObjectuserInfo["nickname"].ToString(),
                    QQ = openId,
                    Sex = jObjectuserInfo["gender"].ToString(),
                    Avatar = jObjectuserInfo["figureurl_qq_2"] == null ? jObjectuserInfo["figureurl_2"].ToString() : jObjectuserInfo["figureurl_qq_2"].ToString()
                };
                DbContext.Insert(userModel);
            }
            else
            {
                //存在更新
                userModel.NikeName = jObjectuserInfo["nickname"].ToString();
                userModel.Sex = jObjectuserInfo["gender"].ToString();
                userModel.Avatar = jObjectuserInfo["figureurl_qq_2"] == null ? jObjectuserInfo["figureurl_2"].ToString() : jObjectuserInfo["figureurl_qq_2"].ToString();
                DbContext.Update(userModel);
            }
            _UserInfo.UserName = userModel.UserName;
            _UserInfo.Avatar = userModel.Avatar;
            _UserInfo.UserId = userModel.SysUserId;
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }
    }
}