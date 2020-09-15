/*****************************************************************
*Module Name:SysLinkTypeController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:链接控制器
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
    public class SysLinkItemController : BaseController
    {
        #region Init
        private string _entityName = "链接";
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IMapper _mapper;

        private ISysLinkTypeService _ISysLinkTypeService;
        public SysLinkItemController(UnitOfWork unitOfWork, ILogger<SysLinkItemController> logger, IMapper mapper, ISysLinkTypeService SysLinkTypeService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _ISysLinkTypeService = SysLinkTypeService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index(string SysLinkTypeId)
        {
            ViewBag.SysLinkTypeId = SysLinkTypeId;
            ViewBag.SysLinkTypeParentSelect = _ISysLinkTypeService.GetLinkTypeList();
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id, string SysLinkItemId)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = _unitOfWork.SysLinkItemRepository.Get(id);
            ViewBag.SysLinkTypeParentSelect = _ISysLinkTypeService.GetLinkTypeList();
            ViewBag.SysLinkItemId = SysLinkItemId;
            return View(model ?? new SysLinkItem() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await _unitOfWork.SysLinkItemRepository.GetAsync(id);
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
            var data = await _unitOfWork.SysLinkItemRepository.GetAsync(id);
            return SuccessRes(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysLinkItemSearchViewModel search)
        {
            search.Ordering = "SysLinkTypeId,Sort,-CreateTime";
            var predicate = PredicateBuilder.New<SysLinkItem>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysLinkItemId))
            {
                predicate = predicate.And(i => i.SysLinkItemId.Equals(search.SysLinkItemId));
            }
            if (!string.IsNullOrEmpty(search.SysLinkName))
            {
                predicate = predicate.And(i => i.SysLinkName.Contains(search.SysLinkName));
            }
            if (!string.IsNullOrEmpty(search.SysLinkGroup))
            {
                predicate = predicate.And(i => i.SysLinkGroup.Contains(search.SysLinkGroup));
            }
            if (!string.IsNullOrEmpty(search.SysLinkTypeId))
            {
                predicate = predicate.And(i => i.SysLinkTypeId.Equals(search.SysLinkTypeId));
            }
            #endregion

           //查询数据
           var searchData = await _unitOfWork.SysLinkItemRepository.GetPageAsync(predicate, search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.ToList();
            foreach (var item in search.ReturnData)
            {
                item.SysLinkTypeId = (await _unitOfWork.SysLinkTypeRepository.GetAsync(item.SysLinkTypeId))?.SysLinkTypeName;
            }
            return SuccessRes(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysLinkItem model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return ErrRes(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysLinkItemId))
            {
                model.SysLinkItemId = GuidKey;

                model.CreateTime = DateTime.Now;
                model.CreateUser = Id;

                result = await _unitOfWork.SysLinkItemRepository.InsertAsync(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysLinkName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysLinkItem.Remark), nameof(SysLinkItem.Sort), nameof(SysLinkItem.Status), nameof(SysLinkItem.SysLinkGroup), nameof(SysLinkItem.SysLinkImg), nameof(SysLinkItem.SysLinkName),nameof(SysLinkItem.SysLinkTypeId),nameof(SysLinkItem.SysLinkUrl)
                      
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    if (lstColumn.Count == 0)
                    {
                        lstColumn = columns.Split(',').ToList();
                    }
                    else
                    {
                        lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                    }
                }
                result = await _unitOfWork.SysLinkItemRepository.UpdateAsync(model, true, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysLinkName}");
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
            var lstUpdateModel = await _unitOfWork.SysLinkItemRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkItemId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                }
                result = await _unitOfWork.SysLinkItemRepository.UpdateAsync(lstUpdateModel);
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
            var lstDelModel = await _unitOfWork.SysLinkItemRepository.GetListAsync(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkItemId));
            bool result = false;
            if (lstDelModel.Count > 0)
            {
                result = await _unitOfWork.SysLinkItemRepository.DeleteAsync(lstDelModel);
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