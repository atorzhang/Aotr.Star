using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.DbEntity.SqlSuger;
using Ator.Utility.CacheService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Ator.Model;
using System.Reflection;
using Ator.DbEntity.Factory;
using SqlSugar;
using Ator.DbEntity.Sys;

namespace Ator.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //注册SqlSuger服务
            SysConfig.InitConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加内存缓存
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            //添加Session
            services.AddSession();

            //添加缓存服务注册
            services.AddSingleton(typeof(Utility.CacheService.ICacheService), typeof(MemoryCacheService));

            //注入AutoMapper服务
            services.AddAutoMapper(typeof(AutoMapperProfileConfiguration));

            //注入sqlSuper数据操作服务，在此项目中有2中操作数据方式，另一种封装在Ator.DAL层，可在其他地方使用var db=SugarHandler.Instance();
            services.AddSqlSugarClient<DbFactory>((sp, op) =>
            {
                op.ConnectionString = sp.GetService<IConfiguration>().GetConnectionString("MySQLConn");
                op.DbType = DbType.MySql;
                op.IsAutoCloseConnection = true;
                op.InitKeyType = InitKeyType.Attribute;
                op.IsShardSameThread = true;
            });

            //通过反射注入各种服务
            services.AddServiceScoped();

            #region 认证配置
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/Home/Login");
            });
            #endregion

            //注册mvc控制器和视图
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(option =>
                {
                    //统一设置JsonResult中的日期格式    
                    option.JsonSerializerOptions.Converters.Add(new Rule.Config.DateTimeConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();//使用静态文件

            app.UseRouting();//使用路由

            app.UseSession();//使用Session

            app.UseAuthentication();//认证配置

            app.UseAuthorization();//使用认证

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto }); //添加IP获取

            //codefist初始化数据库
            #region codefist初始化数据库
            //SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            //{
            //    ConnectionString = "server=127.0.0.1;database=dbmjyj;userid=root;pwd=123456;port=3306;sslmode=none;",
            //    DbType = DbType.MySql,
            //    IsAutoCloseConnection = true,
            //    InitKeyType = InitKeyType.Attribute
            //});
            //db.CodeFirst.InitTables(typeof(SysCmsColumn), typeof(SysCmsInfo), typeof(SysCmsInfoComment), typeof(SysCmsInfoGood), typeof(SysDictionary), typeof(SysDictionaryItem), typeof(SysLinkItem), typeof(SysLinkType), typeof(SysLog), typeof(SysOperateRecord), typeof(SysPage), typeof(SysRole), typeof(SysRolePage), typeof(SysSetting), typeof(SysUser), typeof(SysUserRole));
            //db.CodeFirst.InitTables(typeof(SysLog));
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "areas", "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
