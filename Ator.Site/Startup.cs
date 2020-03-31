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
using NLog.Extensions.Logging;
using AutoMapper;
using Ator.Model;
using System.Reflection;
using Ator.DbEntity.Factory;
using SqlSugar;

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

            //通过反射注入各种服务
            services.AddServiceScoped();

            #region 认证配置
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }).AddCookie(options =>
                {
                //Cookie的middleware配置
                options.LoginPath = new PathString("/Admin/Home/Login");
                    options.AccessDeniedPath = new PathString("/Admin/Home/Login");
                //options.ExpireTimeSpan = //有效期
            });
            #endregion

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

            //注册mvc控制器和视图
            services.AddControllersWithViews();

            
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

            app.UseAuthorization();//使用认证

            app.UseAuthentication();//认证配置

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto }); //添加IP获取


            

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
