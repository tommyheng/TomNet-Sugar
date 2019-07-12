using System;
using Microsoft.Extensions.DependencyInjection;

using TomNet.SqlSugarCore.Entity;

namespace TomNet.SqlSugarCore
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 从服务提供者中获取DbFactory
        /// </summary>
        /// <returns></returns>
        public static IDbFactory GetDbFactory(this IServiceProvider provider)
        {
            return provider.GetService<IDbFactory>();
        }

        /// <summary>
        /// 从服务提供者中获取UnitOfWork
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork GetUnitOfWork(this IServiceProvider provider)
        {
            return provider.GetService<IUnitOfWork>();
        }
    }
}
