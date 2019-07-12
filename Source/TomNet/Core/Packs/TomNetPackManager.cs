using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TomNet.Dependency;


namespace TomNet.Core.Packs
{
    /// <summary>
    /// TomNet模块管理器
    /// </summary>
    public class TomNetPackManager : ITomNetPackManager
    {
        private readonly List<TomNetPack> _sourcePacks;

        /// <summary>
        /// 初始化一个<see cref="TomNetPackManager"/>类型的新实例
        /// </summary>
        public TomNetPackManager()
        {
            _sourcePacks = new List<TomNetPack>();
            LoadedPacks = new List<TomNetPack>();
        }

        /// <summary>
        /// 获取 自动检索到的所有模块信息
        /// </summary>
        public IEnumerable<TomNetPack> SourcePacks => _sourcePacks;

        /// <summary>
        /// 获取 最终加载的模块信息集合
        /// </summary>
        public IEnumerable<TomNetPack> LoadedPacks { get; private set; }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection LoadPacks(IServiceCollection services)
        {
            ITomNetPackTypeFinder packTypeFinder =
                services.GetOrAddTypeFinder<ITomNetPackTypeFinder>(assemblyFinder => new TomNetPackTypeFinder(assemblyFinder));
            Type[] packTypes = packTypeFinder.FindAll();
            _sourcePacks.Clear();
            _sourcePacks.AddRange(packTypes.Select(m => (TomNetPack)Activator.CreateInstance(m)));


            List<TomNetPack> packs = _sourcePacks.Distinct().ToList();

            // 按先层级后顺序的规则进行排序
            packs = packs.OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();
            LoadedPacks = packs;
            foreach (TomNetPack pack in LoadedPacks)
            {
                services = pack.AddServices(services);
            }

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public virtual void UsePack(IServiceProvider provider)
        {
            ILogger logger = provider.GetLogger<TomNetPackManager>();
            logger.LogInformation("TomNet框架初始化开始");
            DateTime dtStart = DateTime.Now;

            foreach (TomNetPack pack in LoadedPacks)
            {
                pack.UsePack(provider);
                logger.LogInformation($"模块{pack.GetType()}加载成功");
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"TomNet框架初始化完成，耗时：{ts:g}");
        }
    }
}