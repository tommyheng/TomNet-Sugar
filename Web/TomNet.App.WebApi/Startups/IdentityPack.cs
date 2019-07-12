using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using TomNet.AspNetCore.Data;
using TomNet.Authentication;
using TomNet.Core;
using TomNet.Exceptions;
using TomNet.Extensions;
using TomNet.Json;


namespace TomNet.App.WebApi.Startups
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
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //在线用户缓存
            services.TryAddSingleton<IOnlineUserCache, OnlineUserCache>();
            services.TryAddSingleton<IOnlineUserProvider, OnlineUserProvider>();
            return base.AddServices(services);
        }

        /// <summary>
        /// 添加Authentication服务
        /// </summary>
        /// <param name="services">服务集合</param>
        protected override void AddAuthentication(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                string secret = configuration["tomnet:Jwt:Secret"];
                if (secret.IsNullOrEmpty())
                {
                    throw new TomNetException("配置文件中Jwt节点的Secret不能为空");
                }
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["tomnet:Jwt:Issuer"],
                    ValidAudience = configuration["tomnet:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.Now,
                    ValidateLifetime = true
                };

                jwt.SecurityTokenValidators.Clear();
                jwt.SecurityTokenValidators.Add(new OnlineUserJwtSecurityTokenHandler());
                jwt.Events = new JwtBearerEvents()
                {
                    // 生成SignalR的用户信息
                    OnMessageReceived = context =>
                    {
                        string token = context.Request.Query["access_token"];
                        string path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.Contains("hub"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseAuthentication();

            //处理异常{此处可以自行编写中间件来代替这部分内容}
            app.UseStatusCodePages(new StatusCodePagesOptions()
            {
                HandleAsync = (context) =>
                {
                    if (context.HttpContext.Response.StatusCode == 401)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(context.HttpContext.Response.Body))
                        {
                            var unAuth = new AjaxResult()
                            {
                                Content = "不允许访问",
                                Type = Data.AjaxResultType.UnAuth
                            };

                            sw.Write(unAuth.ToJsonString(true));
                        }
                    }
                    if (context.HttpContext.Response.StatusCode == 404)
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(context.HttpContext.Response.Body))
                        {
                            var unAuth = new AjaxResult()
                            {
                                Content = "资源未找到",
                                Type = Data.AjaxResultType.UnAuth
                            };

                            sw.Write(unAuth.ToJsonString(true));
                        }
                    }
                    return Task.Delay(0);
                }
            });
            IsEnabled = true;
        }
    }
}