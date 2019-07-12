using System;
using System.Threading.Tasks;


namespace TomNet.Authentication
{
    /// <summary>
    /// 在线用户提供者
    /// </summary>
    public interface IOnlineUserProvider
    {
        /// <summary>
        /// 创建在线用户信息
        /// </summary>
        /// <param name="provider">服务提供器</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        Task<OnlineUser> CreateAsync(IServiceProvider provider, string userName);
    }
}
