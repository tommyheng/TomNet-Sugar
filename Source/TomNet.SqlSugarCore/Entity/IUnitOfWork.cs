using SqlSugar;


namespace TomNet.SqlSugarCore.Entity
{
    /// <summary>
    /// 业务单元操作接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        SqlSugarClient DbContext { get; }
        /// <summary>
        /// 对数据库连接开启事务
        /// </summary>
        void BeginTran();
        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        void Rollback();
    }
}
