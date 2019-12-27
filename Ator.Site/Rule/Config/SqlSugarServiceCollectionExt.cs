using Ator.DbEntity.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ator.Site
{
    /// <summary>
    ///  SqlSugar 注入Service的扩展方法
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        /// SqlSugar上下文注入
        /// </summary>
        /// <typeparam name="TSugarContext">要注册的上下文的类型</typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="configAction"></param>
        /// <param name="lifetime">用于在容器中注册TSugarClient服务的生命周期</param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarClient<TSugarContext>(this IServiceCollection serviceCollection, Action<IServiceProvider, ConnectionConfig> configAction, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TSugarContext : IDbFactory
        {
            serviceCollection.AddMemoryCache().AddLogging();
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(ConnectionConfig), p => ConnectionConfigFactory(p, configAction), lifetime));
            //serviceCollection.Add(new ServiceDescriptor(typeof(ConnectionConfig), p => ConnectionConfigFactory(p, configAction), lifetime));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TSugarContext), typeof(TSugarContext), lifetime));
            return serviceCollection;
        }

        private static ConnectionConfig ConnectionConfigFactory(IServiceProvider applicationServiceProvider, Action<IServiceProvider, ConnectionConfig> configAction)
        {
            var config = new ConnectionConfig();
            configAction.Invoke(applicationServiceProvider, config);
            return config;
        }
    }
}
