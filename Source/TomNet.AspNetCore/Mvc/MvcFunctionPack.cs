using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;
using System.Linq;

using TomNet.Core.Packs;
using TomNet.Dependency;
using TomNet.Reflection;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// MVC功能点模块
    /// </summary>
    [Description("MVC功能点模块")]
    public class MvcFunctionPack : AspTomNetPack
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
            services.GetOrAddTypeFinder<IFunctionTypeFinder>(assemblyFinder => new MvcControllerTypeFinder(assemblyFinder));
            services.AddSingleton<IFunctionHandler, MvcFunctionHandler>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IFunctionHandler functionHandler = app.ApplicationServices.GetServices<IFunctionHandler>().FirstOrDefault(m => m.GetType() == typeof(MvcFunctionHandler));
            if (functionHandler == null)
            {
                return;
            }
            functionHandler.Initialize();
            IsEnabled = true;
        }
    }
}