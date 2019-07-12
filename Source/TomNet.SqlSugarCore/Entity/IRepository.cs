using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SqlSugar;


namespace TomNet.SqlSugarCore.Entity
{
    public interface IRepository<TEntity, TKey>
     where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// 获取 当前单元操作对象
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 获取 当前实体类型的查询数据集
        /// </summary>
        ISugarQueryable<TEntity> Entities { get; }

        //=============================== 实现方法 ===============================
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(TEntity entity);

        Task<int> InsertAsync(TEntity entity);


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(List<TEntity> entities);

        Task<int> InsertAsync(List<TEntity> entities);


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(TEntity[] entities);

        Task<int> InsertAsync(TEntity[] entities);


        /// <summary>
        /// 新增并返回实体
        /// </summary>
        /// <returns>操作影响的行数</returns>
        TEntity InsertReturnEntity(TEntity entity);

        Task<TEntity> InsertReturnEntityAsync(TEntity entity);


        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entity"> 实体对象（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]）</param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        int Update(TEntity entity, string[] ignoreColumns = null);

        Task<int> UpdateAsync(TEntity entity, string[] ignoreColumns = null);


        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entitys">实体对象集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]） </param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        int Update(List<TEntity> entitys, string[] ignoreColumns = null);

        Task<int> UpdateAsync(List<TEntity> entitys, string[] ignoreColumns = null);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="update">实体对象</param> 
        /// <param name="predicate">条件</param>  
        /// <returns>操作影响的行数</returns>
        int Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> predicate = null);

        Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> predicate = null);


        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        int Delete(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);


        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数</returns>
        int Delete(TKey id);

        Task<int> DeleteAsync(TKey id);


        /// <summary>
        /// 根据主键数组删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>操作影响的行数</returns>
        int Delete(TKey[] ids);

        Task<int> DeleteAsync(TKey[] ids);


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>操作影响的行数</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);


        /// <summary>
        /// 根据条件查询第一条符合条件的数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
