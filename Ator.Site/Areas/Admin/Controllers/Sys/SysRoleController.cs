/*****************************************************************
*Module Name:SysRoleController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:角色控制器
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
    public class SysRoleController : BaseController
    {
        #region Init
        private string _entityName = "角色";
        
        private readonly ILogger _logger;
        private IMapper _mapper;
        public SysRoleController(DbFactory factory, ILogger<SysRoleController> logger, IMapper mapper)
        {
             DbContext = factory.GetDbContext();
            _logger = logger; 
            _mapper = mapper;
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
            var model = DbContext.GetById<SysRole>(id);
            return View(model ?? new SysRole() { Status = 1 });
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
            var data = await DbContext.GetByIdAsync<SysRole>(id,true);
            return Ok(data);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="SysRoleSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysRoleSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysRole>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.RoleName))
            {
                predicate = predicate.And(i => i.RoleName.Contains(search.RoleName));
            }

            #endregion

            //查询数据
            var searchData = await DbContext.GetPageListAsync<SysRole, SysRoleSearchDto>(predicate.And(o => true), search.Ordering, search.Page, search.Limit);

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
        public async Task<IActionResult> Save(SysRole model, string columns = "")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysRoleId))
            {
                model.SysRoleId = GuidKey;
                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;

                result = await DbContext.InsertAsync<SysRole>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.RoleName}");
                }
            }
            else
            {
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysRole.SysRoleId),nameof(SysRole.RoleName), nameof(SysRole.Remark), nameof(SysRole.Sort), nameof(SysRole.Status)
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
                        lstColumn.Add(nameof(SysRole.SysRoleId));
                    }
                }
                result = await DbContext.UpdateAsync<SysRole>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.RoleName}");
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
            var lstUpdateModel = await DbContext.GetListAsync<SysRole>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysRoleId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysRole>(lstUpdateModel[i]);
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
            var result = DbContext.DeleteByIds<SysRole>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion
    }
}