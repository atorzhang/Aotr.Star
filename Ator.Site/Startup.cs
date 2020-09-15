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
            //ע��SqlSuger����
            SysConfig.InitConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //����ڴ滺��
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            //���Session
            services.AddSession();

            //��ӻ������ע��
            services.AddSingleton(typeof(Utility.CacheService.ICacheService), typeof(MemoryCacheService));

            //ע��AutoMapper����
            services.AddAutoMapper(typeof(AutoMapperProfileConfiguration));

            //ע��sqlSuper���ݲ��������ڴ���Ŀ����2�в������ݷ�ʽ����һ�ַ�װ��Ator.DAL�㣬���������ط�ʹ��var db=SugarHandler.Instance();
            services.AddSqlSugarClient<DbFactory>((sp, op) =>
            {
                op.ConnectionString = sp.GetService<IConfiguration>().GetConnectionString("MySQLConn");
                op.DbType = DbType.MySql;
                op.IsAutoCloseConnection = true;
                op.InitKeyType = InitKeyType.Attribute;
                op.IsShardSameThread = true;
            });

            //ͨ������ע����ַ���
            services.AddServiceScoped();

            #region ��֤����
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/Home/Login");
            });
            #endregion

            //ע��mvc����������ͼ
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddJsonOptions(option =>
                {
                    //ͳһ����JsonResult�е����ڸ�ʽ    
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

            app.UseStaticFiles();//ʹ�þ�̬�ļ�

            app.UseRouting();//ʹ��·��

            app.UseSession();//ʹ��Session

            app.UseAuthentication();//��֤����

            app.UseAuthorization();//ʹ����֤

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto }); //���IP��ȡ

            //codefist��ʼ�����ݿ�
            #region codefist��ʼ�����ݿ�
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
