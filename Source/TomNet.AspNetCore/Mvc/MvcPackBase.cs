using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TomNet.Core.Packs;


namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// Mvc模块基类
    /// </summary>
    public abstract class MvcPackBase : AspTomNetPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseMvcWithAreaRoute();
            IsEnabled = true;
        }
    }
}