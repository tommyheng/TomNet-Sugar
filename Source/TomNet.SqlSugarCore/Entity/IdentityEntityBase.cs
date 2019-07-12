using System;

using SqlSugar;


namespace TomNet.SqlSugarCore.Entity
{
    /// <summary>
    /// 主键可以自增实体类基类
    /// </summary>
    public abstract class IdentityEntityBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public TKey Id { get; set; }
    }
}
