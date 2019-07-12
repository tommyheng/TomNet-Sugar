using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace TomNet.Core.Options
{
    /// <summary>
    /// TomNet框架配置选项信息
    /// </summary>
    public class TomNetOptions
    {
        /// <summary>
        /// 初始化一个<see cref="TomNetOptions"/>类型的新实例
        /// </summary>
        public TomNetOptions()
        {
            DbContexts = new ConcurrentDictionary<string, DbContextOptions>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取 数据上下文配置信息
        /// </summary>
        public IDictionary<string, DbContextOptions> DbContexts { get; }

        /// <summary>
        /// 获取或设置 本地设置
        /// </summary>
        public LocalOptions LocalOption { get; set; }

        /// <summary>
        /// 获取或设置 邮件发送选项
        /// </summary>
        public MailSenderOptions MailSender { get; set; }

        /// <summary>
        /// 获取或设置 JWT身份认证选项
        /// </summary>
        public JwtOptions Jwt { get; set; }

        /// <summary>
        /// 获取或设置 Redis选项
        /// </summary>
        public RedisOptions Redis { get; set; }
    }
}