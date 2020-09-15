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
using System.Linq;
using System.Threading.Tasks;
using Ator.Entity.Sys;
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
    public class SysCmsInfoCommentController : BaseController
    {
        #region Init
        private string _entityName = "评论";
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysCmsInfoCommentService _SysCmsInfoCommentService;
        private ISysCmsColumnService _SysCmsColumnService;
        private ISysCmsInfoService _SysCmsInfoService;
        public SysCmsInfoCommentController(UnitOfWork unitOfWork, ILogger<SysCmsInfoCommentController> logger, IMapper mapper, ISysCmsInfoCommentService SysCmsInfoCommentService, ISysCmsColumnService SysCmsColumnService, ISysCmsInfoService SysCmsInfoService)
        {
            _unitOfWork = unitOfWork;
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
            var model = _unitOfWork.SysCmsInfoCommentRepository.Get(id);
            ViewBag.SysCmsColumnSelect = _SysCmsColumnService.GetColumnList();
            ViewBag.SysCmsInfoSelect = _SysCmsInfoService.GetInfoList();
            return View(model ?? new SysCmsInfoComment() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await _unitOfWork.SysCmsInfoCommentRepository.GetAsync(id);
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
            var data = await _unitOfWork.SysCmsInfoCommentRepository.GetAsync(id);
            return SuccessRes(data);
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
            var searchData = await _unitOfWork.SysCmsInfoCommentRepository.GetPageAsync(predicate, search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.ToList();
            foreach (var item in search.ReturnData)
            {
                //后台效率无所谓，反正用得不过，一页20条*4也就80次数据库查询而已~_~
                item.SysCmsInfoId = (await _unitOfWork.SysCmsInfoRepository.LoadAsync(o => o.SysCmsInfoId == item.SysCmsInfoId)).FirstOrDefault()?.InfoTitle;
                item.SysCmsColumnId = (await _unitOfWork.SysCmsColumnRepository.LoadAsync(o => o.SysCmsColumnId == item.SysCmsColumnId)).FirstOrDefault()?.ColumnName;
                //item.SysUserId = (await _unitOfWork.SysUserRepository.LoadAsync(o => o.SysUserId == item.SysUserId)).FirstOrDefault()?.UserName;
                //item.ToUserId = (await _unitOfWork.SysUserRepository.LoadAsync(o => o.SysUserId == item.ToUserId)).FirstOrDefault()?.UserName;
            }
            return SuccessRes(search.ReturnData, searchData.Totals);
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
                return ErrRes(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysCmsInfoCommentId))
            {
                model.SysCmsInfoCommentId = GuidKey;
                
                model.CommentTime = DateTime.Now;

                result = await _unitOfWork.SysCmsInfoCommentRepository.InsertAsync(model);
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
                    nameof(SysCmsInfoComment.Address), nameof(SysCmsInfoComment.Comment), nameof(SysCmsInfoComment.Status), nameof(SysCmsInfoComment.CommentTime), nameof(SysCmsInfoComment.UserLable),nameof(SysCmsInfoComment.ToUserId),nameof(SysCmsInfoComment.ToUserId)
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
                    }
                }
                result = await _unitOfWork.SysCmsInfoCommentRepository.UpdateAsync(model, true, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysCmsInfoCommentId}");
                }
            }
            return result ? SuccessRes() : ErrRes();
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
            var lstUpdateModel = await _unitOfWork.SysCmsInfoCommentRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsInfoCommentId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                }
                result = await _unitOfWork.SysCmsInfoCommentRepository.UpdateAsync(lstUpdateModel);
            }
            return ResultRes(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Deletes(string ids)
        {
            var lstDelModel = await _unitOfWork.SysCmsInfoCommentRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsInfoCommentId));
            bool result = false;
            if (lstDelModel.Count > 0)
            {
                result = await _unitOfWork.SysCmsInfoCommentRepository.DeleteAsync(lstDelModel);
                if (result)
                {
                    _logger.LogInformation($"删除{lstDelModel.Count}个{_entityName}，{_entityName}编码：{ids}");
                }

            }
            return ResultRes(result);
        }
        #endregion
    }
}