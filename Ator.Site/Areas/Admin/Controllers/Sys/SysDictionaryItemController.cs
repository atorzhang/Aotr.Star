/*****************************************************************
*Module Name:SysDictionaryItemTypeController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:字典值控制器
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers.Sys
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SysDictionaryItemController : BaseController
    {
        #region Init
        private string _entityName = "链接";
        private readonly ILogger _logger;
        private IMapper _mapper;

        private ISysDictionaryService _ISysDictionaryService;
        public SysDictionaryItemController(DbFactory factory, ILogger<SysDictionaryItemController> logger, IMapper mapper, ISysDictionaryService SysDictionaryService)
        {
             DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _ISysDictionaryService = SysDictionaryService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index(string SysDictionaryId)
        {
            ViewBag.SysDictionaryId = SysDictionaryId;
            ViewBag.SysDictionarySelect = _ISysDictionaryService.GetDictionaryList();
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id, string SysDictionaryId)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = DbContext.GetById<SysDictionaryItem>(id);
            ViewBag.SysDictionarySelect = _ISysDictionaryService.GetDictionaryList();
            ViewBag.SysDictionaryId = SysDictionaryId;
            return View(model ?? new SysDictionaryItem() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await DbContext.GetByIdAsync<SysDictionaryItem>(id,true);
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
            var data = await DbContext.GetByIdAsync<SysDictionaryItem>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysDictionaryItemSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysDictionaryItem>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.SysDictionaryItemId))
            {
                predicate = predicate.And(i => i.SysDictionaryItemId.Equals(search.SysDictionaryItemId));
            }
            if (!string.IsNullOrEmpty(search.SysDictionaryItemName))
            {
                predicate = predicate.And(i => i.SysDictionaryItemName.Contains(search.SysDictionaryItemName));
            }
            #endregion

            //查询数据
            var searchData = await DbContext.GetPageListAsync<SysDictionaryItem>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.ToList();
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysDictionaryItem model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysDictionaryItemId))
            {
                model.SysDictionaryItemId = GuidKey;

                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysDictionaryItem>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.SysDictionaryItemName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysDictionaryItem.SysDictionaryItemId),nameof(SysDictionaryItem.Remark), nameof(SysDictionaryItem.Sort), nameof(SysDictionaryItem.Status), nameof(SysDictionaryItem.SysDictionaryItemName), nameof(SysDictionaryItem.SysDictionaryId), nameof(SysDictionaryItem.SysDictionaryItemValue)

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
                        lstColumn.Add(nameof(SysDictionaryItem.SysDictionaryItemId));
                    }
                }
                result = await DbContext.UpdateAsync<SysDictionaryItem>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.SysDictionaryItemName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysDictionaryItem>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysDictionaryItemId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysDictionaryItem>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysDictionaryItem>().Where(o => lstIds.Contains(o.SysDictionaryItemId)).Select(o => new
            {
                o.SysDictionaryItemId,
                o.Unchangeable
            }).ToList();
            if (lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result = DbContext.DeleteByIds<SysDictionaryItem>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}