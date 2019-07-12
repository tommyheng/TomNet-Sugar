using System;

using Microsoft.AspNetCore.Builder;


namespace TomNet.AspNetCore
{
    /// <summary>
    /// 定义AspNetCore环境下的应用模块服务 
    /// </summary>
    public interface IAspUsePack
    {
        /// <summary>
        /// 应用模块服务，仅在AspNetCore环境下调用，非AspNetCore环境请执行 UsePack(IServiceProvider) 功能
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        void UsePack(IApplicationBuilder app);
    }
}