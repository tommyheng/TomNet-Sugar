using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TomNet.App.Core
{
    public class ServicePackCore : ServicePackBase
    {
        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
          
            return base.AddServices(services);
        }

        public override void UsePack(IServiceProvider provider)
        {

        }
    }
}
