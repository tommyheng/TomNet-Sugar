using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TomNet.Core.Packs;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.SqlSugarCore
{
    public class SqlSugarCorePack : TomNetPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IDbFactory), typeof(DbFactory));
            services.TryAddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.TryAddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }
    }
}
