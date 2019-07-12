using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using TomNet.Core.Options;

namespace TomNet.Core.Packs
{
    /// <summary>
    /// TomNet核心模块
    /// </summary>
    [Description("TomNet核心模块")]
    public class TomNetCorePack : TomNetPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Core;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IConfigureOptions<TomNetOptions>, TomNetOptionsSetup>();
            return services;
        }
    }
}