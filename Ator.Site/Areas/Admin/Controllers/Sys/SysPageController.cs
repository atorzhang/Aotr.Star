﻿/*****************************************************************
*Module Name:SysPageController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:页面控制器
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
    public class SysPageController : BaseController
    {
        #region Init
        private string _entityName = "页面";
        
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysPageService _sysPageService;
        public SysPageController(DbFactory factory, ILogger<SysPageController> logger, IMapper mapper, ISysPageService sysPageService)
        {
             DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _sysPageService = sysPageService;
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
            var model = DbContext.GetById<SysPage>(id);
            ViewBag.SysPageParentSelect = _sysPageService.GetPageList();
            return View(model ?? new SysPage() { Status = 1 });
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
            var data = await DbContext.GetByIdAsync<SysPage>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysPageSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysPage>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysPageName))
            {
                predicate = predicate.And(i => i.SysPageName.Contains(search.SysPageName));
            }
            if (!string.IsNullOrEmpty(search.SysPageNum))
            {
                predicate = predicate.And(i => i.SysPageNum.Contains(search.SysPageNum));
            }
            #endregion

            //查询数据
            var searchData = await DbContext.GetPageListAsync<SysPage>(predicate.And(o => true), nameof(SysPage.SysPageNum)+"," + search.Ordering, search.Page, search.Limit);

            var pagesModels = _sysPageService.GetPageList();
            foreach (var item in searchData.Rows)
            {
                //item.SysPageParent = (await DbContext.GetByIdAsync<SysPage>(item.SysPageParent))?.SysPageName;
                item.SysPageParent = pagesModels.FirstOrDefault(o => o.Key == item.SysPageParent)?.Value;
            }
            //获得返回集合Dto
            search.ReturnData = searchData.Rows.Select(o => _mapper.Map<SysPageSearchDto>(o)).ToList();
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysPage model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysPageId))
            {
                model.SysPageId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysPage>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysPageName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysPage.SysPageId),nameof(SysPage.Remark), nameof(SysPage.Sort), nameof(SysPage.Status), nameof(SysPage.SysPageImg), nameof(SysPage.SysPageName), nameof(SysPage.SysPageNum),nameof(SysPage.SysPageParent),nameof(SysPage.SysPageUrl)
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
                        lstColumn.Add(nameof(SysPage.SysPageId));
                    }
                }
                result = await DbContext.UpdateAsync<SysPage>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysPageName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysPage>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysPageId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysPage>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysPage>().Where(o => lstIds.Contains(o.SysPageId)).Select(o => new
            {
                o.SysPageId,
                o.Unchangeable
            }).ToList();
            if (lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result = DbContext.DeleteByIds<SysPage>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}