using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace TomNet.AspNetCore
{
    /// <summary>
    /// 服务解析扩展
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取HttpContext实例
        /// </summary>
        public static HttpContext HttpContext(this IServiceProvider provider)
        {
            IHttpContextAccessor accessor = provider.GetService<IHttpContextAccessor>();
            return accessor?.HttpContext;
        }

        /// <summary>
        /// 当前业务是否处于HttpRequest中
        /// </summary>
        public static bool InHttpRequest(this IServiceProvider provider)
        {
            var context = provider.HttpContext();
            return context != null;
        }
    }
}