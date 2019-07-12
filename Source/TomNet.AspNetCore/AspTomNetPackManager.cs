using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using TomNet.Core.Packs;
using System;
using TomNet.Core;

namespace TomNet.AspNetCore
{
    /// <summary>
    /// AspNetCore 模块管理器
    /// </summary>
    public class AspTomNetPackManager : TomNetPackManager, IAspUsePack
    {
        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public void UsePack(IApplicationBuilder app)
        {
            ILogger logger = app.ApplicationServices.GetLogger<AspTomNetPackManager>();
            logger.LogInformation("TomNet框架初始化开始1");
            DateTime dtStart = DateTime.Now;

            foreach (TomNetPack pack in LoadedPacks)
            {
                if (pack is AspTomNetPack aspPack)
                {
                    aspPack.UsePack(app);
                }
                else
                {
                    pack.UsePack(app.ApplicationServices);
                }
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"TomNet框架初始化完成，耗时：{ts:g}");
        }
    }
}