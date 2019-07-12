using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TomNet.AspNetCore;
using TomNet.Core.Packs;

namespace TomNet.Authentication
{
    /// <summary>
    /// 身份论证模块基类
    /// </summary>
    public abstract class IdentityPackBase : AspTomNetPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            AddAuthentication(services);
            return services;
        }


        /// <summary>
        /// 添加Authentication服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected virtual void AddAuthentication(IServiceCollection services)
        { }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseAuthentication();

            IsEnabled = true;
        }
    }
}