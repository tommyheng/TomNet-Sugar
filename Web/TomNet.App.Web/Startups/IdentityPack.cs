using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TomNet.Authentication;
using TomNet.Core;


namespace TomNet.App.Web.Startups
{
    /// <summary>
    /// 身份认证模块，此模块必须在MVC模块之前启动
    /// </summary>
    public class IdentityPack : IdentityPackBase
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 0;

        /// <summary>
        /// 添加Authentication服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected override void AddAuthentication(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;                     // 是否允许客户端Js获取。默认True，不允许。
                    options.Cookie.Name = "tomnet.identity";            // Cookie 名称
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // 过期时间
                    options.SlidingExpiration = true;                   // 是否在过期时间过半的时候，自动延期
                    options.LoginPath = "/Account/SignIn";              // 认证失败，会自动跳转到这个地址                     
                });

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
           
        }
    }
}