using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using TomNet.Core.Packs;
using System;
using TomNet.Core;

namespace TomNet.AspNetCore
{
    /// <summary>
    /// AspNetCore ģ�������
    /// </summary>
    public class AspTomNetPackManager : TomNetPackManager, IAspUsePack
    {
        /// <summary>
        /// Ӧ��ģ�����
        /// </summary>
        /// <param name="app">Ӧ�ó��򹹽���</param>
        public void UsePack(IApplicationBuilder app)
        {
            ILogger logger = app.ApplicationServices.GetLogger<AspTomNetPackManager>();
            logger.LogInformation("TomNet��ܳ�ʼ����ʼ1");
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
            logger.LogInformation($"TomNet��ܳ�ʼ����ɣ���ʱ��{ts:g}");
        }
    }
}