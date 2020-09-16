/*****************************************************************
*Module Name:SysCmsInfoCommentCommentController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:评论控制器
*Others:
*evision History:
*DateRel VerNotes
*Blog:https://www.cnblogs.com/jomzhang/
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model.ViewModel.Sys;
using Ator.Repository;
using Ator.Utility.Ext;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers.Sys
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SysCmsInfoCommentController : BaseController
    {
        #region Init
        private string _entityName = "评论";
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysCmsInfoCommentService _SysCmsInfoCommentService;
        private ISysCmsColumnService _SysCmsColumnService;
        private ISysCmsInfoService _SysCmsInfoService;
        public SysCmsInfoCommentController(DbFactory factory, ILogger<SysCmsInfoCommentController> logger, IMapper mapper, ISysCmsInfoCommentService SysCmsInfoCommentService, ISysCmsColumnService SysCmsColumnService, ISysCmsInfoService SysCmsInfoService)
        {
            DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _SysCmsInfoCommentService = SysCmsInfoCommentService;
            _SysCmsColumnService = SysCmsColumnService;
            _SysCmsInfoService = SysCmsInfoService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index(string SysCmsInfoId,string SysCmsColumnId)
        {
            ViewBag.SysCmsInfoId = SysCmsInfoId;
            ViewBag.SysCmsColumnId = SysCmsColumnId;

            ViewBag.SysCmsColumnSelect = _SysCmsColumnService.GetColumnList();
            ViewBag.SysCmsInfoSelect = _SysCmsInfoService.GetInfoList();
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = DbContext.GetById<SysCmsInfoComment>(id);
            ViewBag.SysCmsColumnSelect = _SysCmsColumnService.GetColumnList();
            ViewBag.SysCmsInfoSelect = _SysCmsInfoService.GetInfoList();
            return View(model ?? new SysCmsInfoComment() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await DbContext.GetByIdAsync<SysCmsInfoComment>(id,true);
            return View(data);
        }
        #endregion

        #region Operate
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string id)
        {
            var data = await DbContext.GetByIdAsync<SysCmsInfoComment>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysCmsInfoCommentSearchViewModel search)
        {
            search.Ordering = "-CommentTime";
            var predicate = PredicateBuilder.New<SysCmsInfoComment>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysCmsInfoId))
            {
                predicate = predicate.And(i => i.SysCmsInfoId.Equals(search.SysCmsInfoId));
            }
            if (!string.IsNullOrEmpty(search.SysCmsColumnId))
            {
                predicate = predicate.And(i => i.SysCmsInfoCommentId.Equals(search.SysCmsColumnId));
            }
            if (!string.IsNullOrEmpty(search.SysUserId))
            {
                predicate = predicate.And(i => i.SysUserId.Equals(search.SysUserId));
            }
            #endregion

            //查询数据
            var searchData = await DbContext.GetPageListAsync<SysCmsInfoComment>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.ToList();
            foreach (var item in searchData.Rows)
            {
                //后台效率无所谓，反正用得不过，一页20条*4也就80次数据库查询而已~_~
                item.SysCmsInfoId = (await DbContext.GetAsync<SysCmsInfo>(o => o.SysCmsInfoId == item.SysCmsInfoId))?.InfoTitle;
                item.SysCmsColumnId = (await DbContext.GetAsync<SysCmsColumn>(o => o.SysCmsColumnId == item.SysCmsColumnId))?.ColumnName;
                //item.SysUserId = (await DbContext.SysUserRepository.LoadAsync(o => o.SysUserId == item.SysUserId)).FirstOrDefault()?.UserName;
                //item.ToUserId = (await DbContext.SysUserRepository.LoadAsync(o => o.SysUserId == item.ToUserId)).FirstOrDefault()?.UserName;
            }
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysCmsInfoComment model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysCmsInfoCommentId))
            {
                model.SysCmsInfoCommentId = GuidKey;
                
                model.CommentTime = DateTime.Now;

                result = await DbContext.InsertAsync<SysCmsInfoComment>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysCmsInfoCommentId}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysCmsInfoComment.SysCmsInfoCommentId),nameof(SysCmsInfoComment.Address), nameof(SysCmsInfoComment.Comment), nameof(SysCmsInfoComment.Status), nameof(SysCmsInfoComment.CommentTime), nameof(SysCmsInfoComment.UserLable),nameof(SysCmsInfoComment.ToUserId),nameof(SysCmsInfoComment.ToUserId)
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    if(lstColumn.Count == 0)
                    {
                        lstColumn = columns.Split(',').ToList();
                    }
                    else
                    {
                        lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                        lstColumn.Add(nameof(SysCmsInfoComment.SysCmsInfoCommentId));
                    }
                }
                result = await DbContext.UpdateAsync<SysCmsInfoComment>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysCmsInfoCommentId}");
                }
            }
            return result ? Ok() : Error();
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">数据状态1-正常，2-不通过</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Checks(string ids, int status = 1)
        {
            var lstUpdateModel = await DbContext.GetListAsync<SysCmsInfoComment>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsInfoCommentId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysCmsInfoComment>(lstUpdateModel[i]);
                }
                
            }
            return Result(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Deletes(string ids)
        {
            var lstIds = ids.Split(',');
            var result = DbContext.DeleteByIds<SysCmsInfoComment>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}