using System;

using Microsoft.AspNetCore.Builder;


namespace TomNet.AspNetCore
{
    /// <summary>
    /// ����AspNetCore�����µ�Ӧ��ģ����� 
    /// </summary>
    public interface IAspUsePack
    {
        /// <summary>
        /// Ӧ��ģ����񣬽���AspNetCore�����µ��ã���AspNetCore������ִ�� UsePack(IServiceProvider) ����
        /// </summary>
        /// <param name="app">Ӧ�ó��򹹽���</param>
        void UsePack(IApplicationBuilder app);
    }
}