using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using TomNet.AspNetCore.Mvc;

using TomNet.App.WebApi.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TomNet.App.WebApi.Startups
{
    /// <summary>
    /// MVC模块，此模块需要在Identity之后启动
    /// </summary>
    public class AspNetCoreMvcPack : MvcPackBase
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动
        /// </summary>
        public override int Order => 1;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    // 全局功能权限过滤器
                    options.Filters.Add(new FunctionAuthorizationFilter());
                })
                .AddJsonOptions(options =>
                {
                    //忽略循环引用
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //这里有3中格式选择，默认是JSON格式，还有驼峰式和标准式
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            base.AddServices(services);

            return services;
        }
    }
}
