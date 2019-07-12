using System;

using Microsoft.AspNetCore.Builder;

using TomNet.Core.Packs;


namespace TomNet.AspNetCore
{
    /// <summary>
    ///  ����AspNetCore������Packģ�����
    /// </summary>
    public abstract class AspTomNetPack : TomNetPack
    {
        /// <summary>
        /// Ӧ��AspNetCore�ķ���ҵ��
        /// </summary>
        /// <param name="app">AspӦ�ó��򹹽���</param>
        public virtual void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app.ApplicationServices);
        }
    }
}