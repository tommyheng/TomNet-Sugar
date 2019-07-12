using System;

using Microsoft.Extensions.DependencyInjection;


namespace TomNet.Core.Packs
{
    /// <summary>
    /// TomNet模块基类
    /// </summary>
    public abstract class TomNetPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public virtual PackLevel Level => PackLevel.Business;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，同一级别内部再按此顺序启动
        /// </summary>
        public virtual int Order => 0;

        /// <summary>
        /// 获取 是否已可用
        /// </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection AddServices(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public virtual void UsePack(IServiceProvider provider)
        {
            IsEnabled = true;
        }    
    }
}