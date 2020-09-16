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
using Ator.DbEntity.Factory;
using Ator.DbEntity.Sys;
using Ator.IService;
using Ator.Model.ViewModel.Sys;
using Ator.Repository;
using Ator.Utility.Ext;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers.Sys
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize]
    public class SysLinkItemController : BaseController
    {
        #region Init
        private string _entityName = "链接";
        
        private readonly ILogger _logger;
        private IMapper _mapper;

        private ISysLinkTypeService _ISysLinkTypeService;
        public SysLinkItemController(DbFactory factory, ILogger<SysLinkItemController> logger, IMapper mapper, ISysLinkTypeService SysLinkTypeService)
        {
             DbContext = factory.GetDbContext();
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
            var model = DbContext.GetById<SysLinkItem>(id);
            ViewBag.SysLinkTypeParentSelect = _ISysLinkTypeService.GetLinkTypeList();
            ViewBag.SysLinkItemId = SysLinkItemId;
            return View(model ?? new SysLinkItem() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await DbContext.GetByIdAsync<SysLinkItem>(id,true);
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
            var data = await DbContext.GetByIdAsync<SysLinkItem>(id,true);
            return Ok(data);
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
           var searchData = await DbContext.GetPageListAsync<SysLinkItem>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows;
            foreach (var item in searchData.Rows)
            {
                item.SysLinkTypeId = (await DbContext.GetByIdAsync<SysLinkType>(item.SysLinkTypeId))?.SysLinkTypeName;
            }
            return Ok(search.ReturnData, searchData.Totals);
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
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysLinkItemId))
            {
                model.SysLinkItemId = GuidKey;

                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysLinkItem>(model);
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
                    nameof(SysLinkItem.SysLinkItemId),nameof(SysLinkItem.Remark), nameof(SysLinkItem.Sort), nameof(SysLinkItem.Status), nameof(SysLinkItem.SysLinkGroup), nameof(SysLinkItem.SysLinkImg), nameof(SysLinkItem.SysLinkName),nameof(SysLinkItem.SysLinkTypeId),nameof(SysLinkItem.SysLinkUrl) 
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
                        lstColumn.Add(nameof(SysLinkItem.SysLinkItemId));
                    }
                }
                result = await DbContext.UpdateAsync<SysLinkItem>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysLinkName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysLinkItem>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkItemId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysLinkItem>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysLinkItem>().Where(o => lstIds.Contains(o.SysLinkItemId)).Select(o => new
            {
                o.SysLinkItemId,
                o.Unchangeable
            }).ToList();
            if (lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result = DbContext.DeleteByIds<SysLinkItem>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}