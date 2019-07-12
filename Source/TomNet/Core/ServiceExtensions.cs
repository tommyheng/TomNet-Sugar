using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TomNet.Core.Options;
using TomNet.Core.Packs;
using TomNet.Data;
using TomNet.Dependency;
using TomNet.Reflection;


namespace TomNet.Core
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 将TomNet服务，各个<see cref="TomNetPack"/>模块的服务添加到服务容器中
        /// </summary>
        public static IServiceCollection AddTomNet<TTomNetPackManager>(this IServiceCollection services)
            where TTomNetPackManager : ITomNetPackManager, new()
        {
            Check.NotNull(services, nameof(services));

            IConfiguration configuration = services.GetConfiguration();
            Singleton<IConfiguration>.Instance = configuration;

            //初始化所有程序集查找器
            services.TryAddSingleton<IAllAssemblyFinder>(new AppDomainAllAssemblyFinder());

            TTomNetPackManager manager = new TTomNetPackManager();
            services.AddSingleton<ITomNetPackManager>(manager);
            manager.LoadPacks(services);
            return services;
        }

        /// <summary>
        /// 获取<see cref="IConfiguration"/>配置信息
        /// </summary>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstanceOrNull<IConfiguration>();
        }

        /// <summary>
        /// 从服务提供者中获取TomNetOptions
        /// </summary>
        public static TomNetOptions GetTomNetOptions(this IServiceProvider provider)
        {
            return provider.GetService<IOptions<TomNetOptions>>()?.Value;
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type">指定类型</param>
        /// <returns>日志对象</returns>
        public static ILogger GetLogger(this IServiceProvider provider, Type type)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }
    }
}