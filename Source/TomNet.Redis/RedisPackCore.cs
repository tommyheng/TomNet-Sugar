using System;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TomNet.Core;
using TomNet.Core.Packs;
using TomNet.Exceptions;
using TomNet.Extensions;


namespace TomNet.Redis
{
    /// <summary>
    /// Redis模块基类
    /// </summary>
    public abstract class RedisPackCore : TomNetPack
    {
        private bool _enabled = false;

        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Framework;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            _enabled = configuration["TomNet:Redis:Enabled"].CastTo(false);
            if (!_enabled)
            {
                return services;
            }

            string config = configuration["TomNet:Redis:Configuration"];
            if (config.IsNullOrEmpty())
            {
                throw new TomNetException("配置文件中Redis节点的Configuration不能为空");
            }
            string name = configuration["TomNet:Redis:InstanceName"].CastTo("RedisName");

            services.RemoveAll(typeof(IDistributedCache));
            services.AddDistributedRedisCache(opts =>
            {
                opts.Configuration = config;
                opts.InstanceName = name;
            });

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IsEnabled = _enabled;
        }
    }
}