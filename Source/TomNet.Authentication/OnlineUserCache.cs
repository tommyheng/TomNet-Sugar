using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

using TomNet.Caching;
using TomNet.Dependency;

namespace TomNet.Authentication
{
    /// <summary>
    /// 在线用户缓存，以数据库中最新数据为来源的用户信息缓存
    /// </summary>
    public class OnlineUserCache : IOnlineUserCache

    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 初始化一个<see cref="OnlineUserCache"/>类型的新实例
        /// </summary>
        public OnlineUserCache(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
        }

        /// <summary>
        /// 获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual OnlineUser GetOrRefresh(string userName)
        {
            string key = $"Identity_OnlineUser_{userName}";

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return _cache.Get<OnlineUser>(key,
                () =>
                {
                    return _serviceProvider.ExecuteScopedWork<OnlineUser>(provider =>
                    {
                        IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
                        return onlineUserProvider.CreateAsync(provider, userName).Result;
                    });
                },
                options);
        }

        /// <summary>
        /// 异步获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual async Task<OnlineUser> GetOrRefreshAsync(string userName)
        {
            string key = $"Identity_OnlineUser_{userName}";

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return await _cache.GetAsync<OnlineUser>(key,
                () =>
                {
                    return _serviceProvider.ExecuteScopedWorkAsync<OnlineUser>(async provider =>
                    {
                        IOnlineUserProvider onlineUserProvider = provider.GetService<IOnlineUserProvider>();
                        return await onlineUserProvider.CreateAsync(provider, userName);
                    });
                },
                options);
        }

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        public virtual void Remove(params string[] userNames)
        {
            foreach (string userName in userNames)
            {
                string key = $"Identity_OnlineUser_{userName}";
                _cache.Remove(key);
            }
        }
    }
}