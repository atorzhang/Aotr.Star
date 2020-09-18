/*****************************************************************
*Module Name:SysCmsColumnController
*Module Date:2020-09-15
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
    public class SysCmsColumnController : BaseController
    {
        #region Init
        private string _entityName = "页面";
        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysCmsColumnService _SysCmsColumnService;
        public SysCmsColumnController(DbFactory factory, ILogger<SysCmsColumnController> logger, IMapper mapper, ISysCmsColumnService SysCmsColumnService)
        {
            DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _SysCmsColumnService = SysCmsColumnService;
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
            var model = DbContext.GetById<SysCmsColumn>(id);
            ViewBag.SysCmsColumnParentSelect = _SysCmsColumnService.GetColumnList();
            return View(model ?? new SysCmsColumn() { Status = 1 });
        }

        [HttpGet]
        public IActionResult TestV(string id)
        {
            return View();
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
            var data = await DbContext.GetByIdAsync<SysCmsColumn>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysCmsColumnSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysCmsColumn>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.ColumnName))
            {
                predicate = predicate.And(i => i.ColumnName.Contains(search.ColumnName));
            }

            #endregion

            //查询数据
            var searchData = DbContext.GetPageList<SysCmsColumn>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows;
            foreach (var item in searchData.Rows)
            {
                item.ColumnParent = (await DbContext.GetAsync<SysCmsColumn>(o => o.SysCmsColumnId == item.ColumnParent))?.ColumnName;
            }
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysCmsColumn model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysCmsColumnId))
            {
                model.SysCmsColumnId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = GuidKey;

                result = await DbContext.InsertAsync<SysCmsColumn>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.ColumnName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysCmsColumn.SysCmsColumnId),nameof(SysCmsColumn.Remark), nameof(SysCmsColumn.Sort), nameof(SysCmsColumn.Status), nameof(SysCmsColumn.ColumnName), nameof(SysCmsColumn.ColumnLogo), nameof(SysCmsColumn.ColumnDescript),nameof(SysCmsColumn.ColumnParent)
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
                        lstColumn.Add(nameof(SysCmsColumn.SysCmsColumnId));
                    }
                }
                result = await DbContext.UpdateAsync<SysCmsColumn>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.ColumnName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysCmsColumn>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsColumnId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysCmsColumn>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysCmsColumn>().Where(o => lstIds.Contains(o.SysCmsColumnId)).Select(o => new
            {
                o.SysCmsColumnId,
                o.Unchangeable
            }).ToList();
            if(lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result =  DbContext.DeleteByIds<SysCmsColumn>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}