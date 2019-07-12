using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using TomNet.Dependency;


namespace TomNet.AspNetCore
{
    /// <summary>
    /// <see cref="IServiceCollection"/>扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 获取<see cref="IHostingEnvironment"/>环境信息
        /// </summary>
        public static IHostingEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IHostingEnvironment>();
        }
    }
}