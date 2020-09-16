/*****************************************************************
*Module Name:SysRoleController
*Module Date:2018-11-21
*Module Auth:Jomzhang
*Description:角色权限控制器
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
    public class SysRolePageController : BaseController
    {
        #region Init
        private string _entityName = "角色权限";
        
        private readonly ILogger _logger;
        private IMapper _mapper;
        public SysRolePageController(DbFactory factory, ILogger<SysRolePageController> logger, IMapper mapper)
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

        #endregion

        #region Operate

        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <param name="SysRoleSearchViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPageData(string role)
        {
            var predicate = PredicateBuilder.New<SysPage>(true);//查询条件

            #region 添加条件查询
            predicate.And(i => i.Status.Equals(1));
            #endregion

            //查询所有页面数据
            var searchData = await DbContext.GetListAsync<SysPage>(predicate.And(o => true), "");
            //查询该角色对应所有页面信息
            var rolePageDatas = await DbContext.GetListAsync<SysRolePage>(o => o.SysRoleId == role, "Sort");

            foreach (var item in searchData)
            {

            }
            //获得返回集合Dto
            
            return Ok(searchData);
        }

        /// <summary>
        /// 保存，修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(string role,string pages)
        {
            //删除该规则原来所有页面

            //添加该规则保存的页面

            return result ? Ok() : Error();
        }
        #endregion
    }
}