using System;

using Microsoft.Extensions.DependencyInjection;

using TomNet.AspNetCore;
using TomNet.Core.Packs;
using TomNet.Exceptions;


namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/>辅助扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// TomNet框架初始化，适用于AspNetCore环境
        /// </summary>
        public static IApplicationBuilder UseTomNet(this IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;
            if (!(provider.GetService<ITomNetPackManager>() is IAspUsePack aspPackManager))
            {
                throw new TomNetException("接口 ITomNetPackManager 的注入类型不正确，该类型应同时实现接口 IAspUsePack");
            }
            aspPackManager.UsePack(app);

            return app;
        }

        /// <summary>
        /// 添加MVC并Area路由支持
        /// </summary>
        public static IApplicationBuilder UseMvcWithAreaRoute(this IApplicationBuilder app, bool area = true)
        {
            return app.UseMvc(builder =>
            {
                if (area)
                {
                    builder.MapRoute("area", "{area:exists}/{controller=Default}/{action=Index}/{id?}");
                }
                builder.MapRoute("default", "{controller=Default}/{action=Index}/{id?}");
            });
        }
    }
}