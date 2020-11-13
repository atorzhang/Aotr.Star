using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ator.Utility.Baidu;
using Ator.Repository;
using Ator.IService;
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;

namespace Ator.Site.Controllers
{
    public class CommentController : BaseController
    {
        private IHttpContextAccessor _accessor;
        private ISysCmsInfoCommentService _sysCmsInfoCommentService;
        public CommentController(IHttpContextAccessor accessor, DbFactory factory, ISysCmsInfoCommentService sysCmsInfoCommentService)
        {
            _accessor = accessor;
            DbContext = factory.GetDbContext();
            _sysCmsInfoCommentService = sysCmsInfoCommentService;
        }
        /// <summary>
        /// 留言功能
        /// </summary>
        /// <param name="content">留言内容</param>
        /// <param name="user">留言用户《后面不用，用session中的用户信息》</param>
        /// <param name="comment">回复的id项目</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(string content,string user,string comment,string article)
        {
            string ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            //ip = "106.12.126.88";
            var address = await IpService.GetIpCity(ip);
            user = base._UserInfo.UserId;
            if (string.IsNullOrEmpty(user))
            {
                return Error("请先登陆");
            }

            var commentModel = await DbContext.GetByIdAsync<SysCmsInfoComment>(comment);
            SysCmsInfoComment sysCmsInfoComment = new SysCmsInfoComment
            {
                SysCmsInfoId = article,
                SysCmsInfoCommentId = GuidKey,
                Ip = ip,
                Status = 1,
                Address = address,
                Comment = content,
                CommentTime = DateTime.Now,
                SysUserId = user,
                ToCommentId = comment,
                UserLable = user == "admin"?"1":"2",
            };
            var articleModel = await DbContext.GetByIdAsync<SysCmsInfo>(article);
            
            if(articleModel != null)
            {
                sysCmsInfoComment.SysCmsColumnId = articleModel.SysCmsColumnId;
                articleModel.InfoComments = articleModel.InfoComments + 1;
            } 
            if(commentModel != null)
            {
                sysCmsInfoComment.ToUserId = commentModel.SysUserId;
            }
            var result = DbContext.Insert(sysCmsInfoComment);
            if(articleModel != null)
            {
                result = DbContext.Update(articleModel);
            }
            if (result)
            {
                if (string.IsNullOrEmpty(comment))
                {
                    var formartComment = _sysCmsInfoCommentService.GetComment(sysCmsInfoComment.SysCmsInfoCommentId);
                    return Ok(formartComment);
                }
                else
                {
                    var formartComment = _sysCmsInfoCommentService.GetReply(sysCmsInfoComment.SysCmsInfoCommentId);
                    return Ok(formartComment);
                }
            }
            return Error("添加留言失败失败");
        }
        /// <summary>
        /// 获取评论或者留言
        /// </summary>
        /// <param name="SysCmsInfoId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Get(string SysCmsInfoId, int page)
        {
            int total = 0;
            var commentsData = _sysCmsInfoCommentService.GetComments(SysCmsInfoId, "", page, 10,out total);
            var totalPage = total % 10 == 0 ? total / 10 : total / 10 + 1;
            return Ok(commentsData, totalPage);
        }

    }
}