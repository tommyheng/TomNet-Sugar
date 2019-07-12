using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using SqlSugar;
using TomNet.Core;
using TomNet.Core.Options;
using TomNet.Exceptions;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.SqlSugarCore
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class DbFactory : IDbFactory
    {
        // 配置信息
        private TomNetOptions options;
        // 日志工具
        private readonly ILogger logger;
        // 配置集合
        private IDictionary<string, ConnectionConfig> connectionConfigs { get; }
        public DbFactory(IServiceProvider serviceProvider)
        {
            connectionConfigs = new ConcurrentDictionary<string, ConnectionConfig>(StringComparer.OrdinalIgnoreCase);
            options = serviceProvider.GetTomNetOptions();
            logger = serviceProvider.GetLogger<DbFactory>();
            GetDbConfigs();
        }
        public SqlSugarClient GetDbContext() => GetDbContext("Default");
        public SqlSugarClient GetDbContext(string dbContextKey) => GetDbContext(dbContextKey, null, null);
        public SqlSugarClient GetDbContext(Action<Exception> onErrorEvent) => GetDbContext(null, onErrorEvent);
        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> OnLogExecutingEvent) => GetDbContext(OnLogExecutingEvent, null);
        public SqlSugarClient GetDbContext(Action<string, SugarParameter[]> OnLogExecutingEvent, Action<Exception> onErrorEvent) => GetDbContext("Default", OnLogExecutingEvent, onErrorEvent);
        public SqlSugarClient GetDbContext(
            string dbContextKey = "Default",
            Action<string, SugarParameter[]> OnLogExecutingEvent = null,
            Action<Exception> onErrorEvent = null)
        {
            var db = new SqlSugarClient(connectionConfigs[dbContextKey]);

            db.Aop.OnLogExecuted = OnLogExecutingEvent ?? ((sql, pars) =>
            {
                logger.LogInformation(sql + "\r\n" +
                    db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            });

            db.Aop.OnError = onErrorEvent ?? ((Exception ex) =>
            {
                logger.LogError(ex.Message);
            });

            return db;
        }

        private void GetDbConfigs()
        {
            foreach (var item in options.DbContexts.Values)
            {
                var cfg = new ConnectionConfig()
                {
                    ConnectionString = item.ConnectionString,
                    DbType = DatabaseTypeToDbType(item.DatabaseType),
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = true
                };
                connectionConfigs.Add(item.DatabaseKey, cfg);
            }
        }
        private DbType DatabaseTypeToDbType(DatabaseType dbtype)
        {
            switch (dbtype)
            {
                case DatabaseType.SqlServer:
                    return DbType.SqlServer;
                case DatabaseType.Sqlite:
                    return DbType.Sqlite;
                case DatabaseType.MySql:
                    return DbType.MySql;
                case DatabaseType.Oracle:
                    return DbType.Oracle;
                case DatabaseType.PostgreSql:
                    return DbType.PostgreSQL;
                default:
                    throw new TomNetException($"无法识别DbType类型");
            }
        }
    }
}
