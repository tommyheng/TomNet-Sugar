using System;

using SqlSugar;


namespace TomNet.SqlSugarCore.Entity
{
    /// <summary>
    /// 主键非自增实体类基类
    /// </summary>
    public abstract class EntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public TKey Id { get; set; }
    }
}
