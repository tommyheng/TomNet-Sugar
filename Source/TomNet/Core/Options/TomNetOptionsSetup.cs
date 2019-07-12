using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using TomNet.Exceptions;
using TomNet.Extensions;


namespace TomNet.Core.Options
{
    /// <summary>
    /// TomNet配置选项创建器
    /// </summary>
    public class TomNetOptionsSetup : IConfigureOptions<TomNetOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化一个<see cref="TomNetOptionsSetup"/>类型的新实例
        /// </summary>
        public TomNetOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>Invoked to configure a TOptions instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(TomNetOptions options)
        {
            SetDbContextOptions(options);

            IConfigurationSection section;

            //LocalOption
            section = _configuration.GetSection("TomNet:Local");
            LocalOptions local = section.Get<LocalOptions>();
            if (local != null)
            {
                options.LocalOption = local;
            }

            //MailSender
            section = _configuration.GetSection("TomNet:MailSender");
            MailSenderOptions sender = section.Get<MailSenderOptions>();
            if (sender != null)
            {
                if (sender.Password == null)
                {
                    sender.Password = _configuration["TomNet:MailSender:Password"];
                }
                options.MailSender = sender;
            }

            //JwtOptions
            section = _configuration.GetSection("TomNet:Jwt");
            JwtOptions jwt = section.Get<JwtOptions>();
            if (jwt != null)
            {
                if (jwt.Secret == null)
                {
                    jwt.Secret = _configuration["TomNet:Jwt:Secret"];
                }
                options.Jwt = jwt;
            }

            // RedisOptions
            section = _configuration.GetSection("TomNet:Redis");
            RedisOptions redis = section.Get<RedisOptions>();
            if (redis != null)
            {
                if (redis.Configuration.IsMissing())
                {
                    throw new TomNetException("配置文件中Redis节点的Configuration不能为空");
                }
                options.Redis = redis;
            }

        }

        /// <summary>
        /// 初始化上下文配置信息
        /// </summary>
        /// <param name="options"></param>
        private void SetDbContextOptions(TomNetOptions options)
        {
            IConfigurationSection section = _configuration.GetSection("TomNet:DbContexts");
            IDictionary<string, DbContextOptions> dict = section.Get<Dictionary<string, DbContextOptions>>();

            if (dict == null || dict.Count == 0)
            {
                throw new TomNetException($"配置文件中不存在任何数据库上下文配置信息");
            }

            foreach (KeyValuePair<string, DbContextOptions> pair in dict)
            {
                pair.Value.DatabaseKey = pair.Key;
                options.DbContexts.Add(pair.Key, pair.Value);
            }
        }
    }
}