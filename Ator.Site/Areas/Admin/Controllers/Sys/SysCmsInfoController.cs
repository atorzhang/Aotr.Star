/*****************************************************************
*Module Name:SysCmsInfoController
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.Site.Areas.Admin.Controllers.Sys
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class SysCmsInfoController : BaseController
    {
        #region Init
        private string _entityName = "页面";

        private readonly ILogger _logger;
        private IMapper _mapper;
        private ISysCmsInfoService _SysCmsInfoService;
        private ISysCmsColumnService _SysCmsColumnService;
        public SysCmsInfoController(DbFactory factory, ILogger<SysCmsInfoController> logger, IMapper mapper, ISysCmsInfoService SysCmsInfoService, ISysCmsColumnService SysCmsColumnService)
        {
            DbContext = factory.GetDbContext();
            _logger = logger;
            _mapper = mapper;
            _SysCmsInfoService = SysCmsInfoService;
            _SysCmsColumnService = SysCmsColumnService;
        }
        #endregion

        #region Page
        [HttpGet]
        public IActionResult Index(string SysCmsColumnId)
        {
            ViewBag.SysCmsColumnParentSelect = _SysCmsColumnService.GetColumnList();
            ViewBag.SysCmsColumnId = SysCmsColumnId;
            return View();
        }

        [HttpGet]
        public IActionResult Form(string id,string SysCmsColumnId)
        {
            ViewBag.id = id;
            ViewBag.isCreate = string.IsNullOrEmpty(id);
            var model = DbContext.GetById<SysCmsInfo>(id);
            ViewBag.SysCmsColumnParentSelect = _SysCmsColumnService.GetColumnList();
            ViewBag.SysCmsColumnId = SysCmsColumnId;
            return View(model ?? new SysCmsInfo() { Status = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var data = await DbContext.GetByIdAsync<SysCmsInfo>(id,true);
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
            var data = await DbContext.GetByIdAsync<SysCmsInfo>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysCmsInfoSearchViewModel search)
        {
            //获得返回集合Dto
            var reData = _SysCmsInfoService.GetInfoPage(search.SysCmsColumnId, search.InfoTitle, "", "", $"-{nameof(SysCmsInfo.InfoTop)},-{nameof(SysCmsInfo.InfoPublishTime)}", search.Page, search.Limit);
            search.ReturnData = reData.Rows;
            foreach (var item in reData.Rows)
            {
                item.InfoType = (await DbContext.GetByIdAsync<SysCmsColumn>(item.SysCmsColumnId))?.ColumnName;
            }
            return Ok(search.ReturnData, reData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysCmsInfo model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            var columnModel = await DbContext.GetByIdAsync<SysCmsColumn>(model.SysCmsColumnId);
            if(columnModel != null && columnModel.ColumnParent == "87cb34a9bb084d229f137d4f09b2d742")
            {
                model.InfoType = "2";//这种为前台展示信息类型
            }
            if (string.IsNullOrEmpty(model.SysCmsInfoId))
            {
                model.SysCmsInfoId = GuidKey;
                
                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysCmsInfo>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.InfoTitle}");
                }
            }
            else
            {
                model.InfoEditUser = CurrentLoginUser.Id;
                model.InfoEditTime = DateTime.Now;
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysCmsInfo.SysCmsInfoId),nameof(SysCmsInfo.Remark), nameof(SysCmsInfo.Sort), nameof(SysCmsInfo.Status), nameof(SysCmsInfo.InfoTitle), nameof(SysCmsInfo.InfoAbstract), nameof(SysCmsInfo.InfoCheckTime),nameof(SysCmsInfo.InfoCheckUser),nameof(SysCmsInfo.InfoContent),nameof(SysCmsInfo.InfoEditTime),nameof(SysCmsInfo.InfoEditUser),nameof(SysCmsInfo.InfoLable),nameof(SysCmsInfo.InfoPublishTime),nameof(SysCmsInfo.InfoSecTitle),nameof(SysCmsInfo.InfoSource),nameof(SysCmsInfo.InfoTop),nameof(SysCmsInfo.InfoType),nameof(SysCmsInfo.InfoImage)
                      ,nameof(SysCmsInfo.InfoAuthor)
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
                        lstColumn.Add(nameof(SysCmsInfo.SysCmsInfoId));
                    }
                }
                result = await DbContext.UpdateAsync<SysCmsInfo>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.InfoTitle}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysCmsInfo>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysCmsInfoId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysCmsInfo>(lstUpdateModel[i]);
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
            var lstModel = DbContext.Queryable<SysCmsInfo>().Where(o => lstIds.Contains(o.SysCmsInfoId)).Select(o => new
            {
                o.SysCmsInfoId,
                o.Unchangeable
            }).ToList();
            if (lstModel.Any(o => o.Unchangeable))
            {
                return Error("存在不可删除的数据");
            }
            var result = DbContext.DeleteByIds<SysCmsInfo>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}