/*****************************************************************
*Module Name:SysLinkTypeController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:链接类型控制器
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
    public class SysLinkTypeController : BaseController
    {
        #region Init
        private string _entityName = "链接类型";
        
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysLinkTypeService _ISysLinkTypeService;
        public SysLinkTypeController(DbFactory factory, ILogger<SysLinkTypeController> logger, IMapper mapper, ISysLinkTypeService sysLinkTypeService)
        {
             DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _ISysLinkTypeService = sysLinkTypeService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = DbContext.GetById<SysLinkType>(id);
            ViewBag.SysLinkTypeParentSelect = _ISysLinkTypeService.GetLinkTypeList();
            return View(model ?? new SysLinkType() { Status = 1 });
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            return View();
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
            var data = await DbContext.GetByIdAsync<SysLinkType>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysLinkTypeSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysLinkType>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysLinkTypeName))
            {
                predicate = predicate.And(i => i.SysLinkTypeName.Contains(search.SysLinkTypeName));
            }
            if (!string.IsNullOrEmpty(search.Group))
            {
                predicate = predicate.And(i => i.Group.Contains(search.Group));
            }
            if (!string.IsNullOrEmpty(search.SysLinkTypeId))
            {
                predicate = predicate.And(i => i.SysLinkTypeId.Equals(search.SysLinkTypeId));
            }
            #endregion

            //查询数据
            var searchData = await DbContext.GetPageListAsync<SysLinkType>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows;
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysLinkType model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysLinkTypeId))
            {
                model.SysLinkTypeId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysLinkType>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysLinkTypeName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysLinkType.SysLinkTypeId),nameof(SysLinkType.Remark), nameof(SysLinkType.Sort), nameof(SysLinkType.Status), nameof(SysLinkType.SysLinkTypeName), nameof(SysLinkType.Group), nameof(SysLinkType.SysLinkTypeLogo)
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
                        lstColumn.Add(nameof(SysLinkType.SysLinkTypeId));
                    }
                }
                result = await DbContext.UpdateAsync<SysLinkType>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysLinkTypeName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysLinkType>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysLinkTypeId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysLinkType>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysLinkType>().Where(o => lstIds.Contains(o.SysLinkTypeId)).Select(o => new
            {
                o.SysLinkTypeId,
                o.Unchangeable
            }).ToList();
            if (lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result = DbContext.DeleteByIds<SysLinkType>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}