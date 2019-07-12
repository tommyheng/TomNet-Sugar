using System;


namespace TomNet.Core.Options
{
    /// <summary>
    /// 数据上下文配置节点
    /// </summary>
    public class DbContextOptions
    {
        /// <summary>
        /// 初始化一个<see cref="TomNetDbContextOptions"/>类型的新实例
        /// </summary>
        public DbContextOptions()
        {
            AutoMigrationEnabled = false;
        }

        /// <summary>
        /// 获取或设置数据库的标识
        /// </summary>
        public string DatabaseKey { get; set; }    

        /// <summary>
        /// 获取或设置 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 获取或设置 是否自动迁移
        /// </summary>
        public bool AutoMigrationEnabled { get; set; }
    }
}
