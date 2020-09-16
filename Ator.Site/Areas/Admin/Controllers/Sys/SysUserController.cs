/*****************************************************************
*Module Name:SysUserController
*Module Date:2018-10-30
*Module Auth:Jomzhang
*Description:用户控制器
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
    public class SysUserController : BaseController
    {
        #region Init
        private string _entityName = "用户";
        
        private readonly ILogger _logger;
        private ISysUserService _sysUserService;
        private IMapper _mapper;
        public SysUserController(DbFactory factory, ILogger<SysUserController> logger, ISysUserService sysUserService, IMapper mapper)
        {
             DbContext = factory.GetDbContext();
            _logger = logger;
            _sysUserService = sysUserService;
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
            var model = DbContext.GetById<SysUser>(id);
            return View(model ?? new SysUser() { Status=1});
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
            var data = await DbContext.GetByIdAsync<SysUser>(id);
            var dataDto = _mapper.Map<SysUserSearchDto>(data);
            if (dataDto == null)
                return Error("暂无数据");
            return Ok(dataDto);
        }

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="sysUserSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(SysUserSearchViewModel search)
        {
            var predicate = PredicateBuilder.New<SysUser>(true);//查询条件

            #region 添加条件查询
            if (!string.IsNullOrEmpty(search.UserName))
            {
                predicate = predicate.And(i => i.UserName.Contains(search.UserName));
            }
            if (!string.IsNullOrEmpty(search.NikeName))
            {
                predicate = predicate.And(i => i.NikeName.Contains(search.NikeName));
            }
            if (!string.IsNullOrEmpty(search.Mobile))
            {
                predicate = predicate.And(i => i.Mobile.Contains(search.Mobile));
            } 
            #endregion

            //查询数据
            //var searchData = await DbContext.SysUserRepository.GetPageAsync(predicate.And(o => true) search.Ordering, search.Page, search.Limit);
            //多条件排序
            var searchData = await DbContext.GetPageListAsync<SysUser>(predicate.And(o => true), $"{nameof(SysUser.Mobile)},-{nameof(SysUser.CreateTime)}", search.Page, search.Limit);

            //获得返回集合Dto
            search.ReturnData = searchData.Rows.Select(o => _mapper.Map<SysUserSearchDto>(o)).ToList();
            return Ok(search.ReturnData, searchData.Totals);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(SysUser model,string columns="")
        {
            var errMsg = GetModelErrMsg();
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            model.Status = model.Status ?? 2;
            if (string.IsNullOrEmpty(model.SysUserId))
            {
                if (DbContext.Get<SysUser>(o => o.UserName == model.UserName) != null)
                {
                    return Error($"{_entityName}已存在");
                }
                model.SysUserId = GuidKey;
                model.Password = model.Password.Md532();
                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentLoginUser.Id;
                
                result = await DbContext.InsertAsync<SysUser>(model);
                if (result)
                {
                    _logger.LogInformation($"添加{_entityName}{model.UserName}");
                }
            }
            else
            {
                if(model.SysUserId == "e3a31ac69f7946ca9218f865e8a4b875" || model.UserName == "admin")//
                {
                    return Error("管理员账户不允许修改");
                }
                //定义可以修改的列
                var lstColumn = new List<string>()
                {
                    nameof(SysUser.SysUserId),nameof(SysUser.Email), nameof(SysUser.Mobile), nameof(SysUser.NikeName), nameof(SysUser.QQ), nameof(SysUser.TrueName), nameof(SysUser.Status)
                };
                if (!string.IsNullOrEmpty(columns))//固定过滤只修改某字段
                {
                    lstColumn = lstColumn.Where(o => columns.Split(',').Contains(o)).ToList();
                    lstColumn.Add(nameof(SysUser.SysUserId));
                }
                result = await DbContext.UpdateAsync<SysUser>(model, lstColumn);
                if (result)
                {
                    _logger.LogInformation($"修改{_entityName}{model.UserName}");
                }
            }
            return result?Ok():Error();
        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">数据状态1-正常，2-不通过</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Checks(string ids,int status = 1)
        {
            var lstUpdateModel = await DbContext.GetListAsync<SysUser>(o => ids.TrimEnd(',').Split(',', StringSplitOptions.None).Contains(o.SysUserId));
            bool result = false;
            if (lstUpdateModel.Count > 0)
            {
                for (int i = 0; i < lstUpdateModel.Count; i++)
                {
                    lstUpdateModel[i].Status = status;
                    result = await DbContext.UpdateAsync<SysUser>(lstUpdateModel[i]);
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
            var result = DbContext.DeleteByIds<SysUser>(lstIds);
            if (result)
            {
                _logger.LogInformation($"删除{lstIds.Length}个{_entityName}，{_entityName}编码：{ids}");
            }
            return Result(result);
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> CreateUserTest(int num)
        {
            List<SysUser> lstUser = new List<SysUser>();
            int count = num > 100000 ? 100000 : num;
            for (int i = 0; i < count; i++)
            {
                string mobile = "189" + (new Random()).Next(10000000, 99999999);
                lstUser.Add(new SysUser
                {
                    CreateTime = DateTime.Now,
                    CreateUser = "test",
                    Mobile = mobile,
                    NikeName = mobile,
                    SysUserId = GuidKey,
                    UserName = mobile,
                    Password = "123456".Md532(),
                    TrueName = mobile
                });
            }
            var s = false;
            foreach (var item in lstUser)
            {
               s = await DbContext.InsertAsync<SysUser>(item);
            }
            return s ? Ok() : Error();
        }
    }
}