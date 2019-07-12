using System;
using System.Collections.Concurrent;
using System.Security.Claims;

using Microsoft.Extensions.DependencyInjection;


namespace TomNet.Dependency
{
    /// <summary>
    /// 基于Scoped生命周期的数据字典，可用于在Scoped环境中传递数据
    /// </summary>
    [Dependency(ServiceLifetime.Scoped, AddSelf = true)]
    public class ScopedDictionary : ConcurrentDictionary<string, object>, IDisposable
    {     
        /// <summary>
        /// 获取或设置 当前用户
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

        /// <summary>释放资源.</summary>
        public void Dispose()
        {
            Identity = null;
            this.Clear();
        }
    }
}