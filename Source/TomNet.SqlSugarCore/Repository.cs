using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using SqlSugar;

using TomNet.Core;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.SqlSugarCore
{
    /// <summary>
    /// 实体数据存储操作类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
           where TEntity : class, IEntity<TKey>, new()
           where TKey : IEquatable<TKey>
    {
        private readonly SqlSugarClient _dbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// 获取 当前单元操作对象
        /// </summary>
        public IUnitOfWork UnitOfWork { get; }

        public Repository(IServiceProvider serviceProvider)
        {
            UnitOfWork = serviceProvider.GetUnitOfWork();
            _dbContext = UnitOfWork.DbContext;
            _logger = serviceProvider.GetLogger<Repository<TEntity, TKey>>();
        }

        public ISugarQueryable<TEntity> Entities => _dbContext.Queryable<TEntity>();


        //=============================== 实现方法 ===============================
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public int Insert(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteCommand();
        }

        public Task<int> InsertAsync(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public int Insert(List<TEntity> entities)
        {
            return _dbContext.Insertable(entities).ExecuteCommand();
        }

        public Task<int> InsertAsync(List<TEntity> entities)
        {
            return _dbContext.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public int Insert(TEntity[] entities)
        {
            return _dbContext.Insertable(entities).ExecuteCommand();
        }

        public Task<int> InsertAsync(TEntity[] entities)
        {
            return _dbContext.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 新增并返回实体
        /// </summary>
        /// <returns>操作影响的行数</returns>
        public TEntity InsertReturnEntity(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteReturnEntity();
        }

        public Task<TEntity> InsertReturnEntityAsync(TEntity entity)
        {
            return _dbContext.Insertable(entity).ExecuteReturnEntityAsync();
        }


        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entity"> 实体对象（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]）</param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        public int Update(TEntity entity, string[] ignoreColumns = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(entity);
            if (ignoreColumns != null && ignoreColumns.Length > 0)
            {
                up = up.IgnoreColumns(ignoreColumns);
            }
            return up.ExecuteCommand();
        }

        public Task<int> UpdateAsync(TEntity entity, string[] ignoreColumns = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(entity);
            if (ignoreColumns != null && ignoreColumns.Length > 0)
            {
                up = up.IgnoreColumns(ignoreColumns);
            }
            return up.ExecuteCommandAsync();
        }

        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entitys">实体对象集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]） </param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        public int Update(List<TEntity> entitys, string[] ignoreColumns = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(entitys);
            if (ignoreColumns != null && ignoreColumns.Length > 0)
            {
                up = up.IgnoreColumns(ignoreColumns);
            }
            return up.ExecuteCommand();
        }


        public Task<int> UpdateAsync(List<TEntity> entitys, string[] ignoreColumns = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(entitys);
            if (ignoreColumns != null && ignoreColumns.Length > 0)
            {
                up = up.IgnoreColumns(ignoreColumns);
            }
            return up.ExecuteCommandAsync();
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="update">实体对象</param> 
        /// <param name="predicate">条件</param>  
        /// <returns>操作影响的行数</returns>
        public int Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> predicate = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(update);
            if (predicate != null)
            {
                up = up.Where(predicate);
            }
            return up.ExecuteCommand();
        }

        public Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> predicate = null)
        {
            IUpdateable<TEntity> up = _dbContext.Updateable(update);
            if (predicate != null)
            {
                up = up.Where(predicate);
            }
            return up.ExecuteCommandAsync();
        }


        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(TEntity entity)
        {
            return _dbContext.Deleteable<TEntity>().Where(entity).ExecuteCommand();
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            return _dbContext.Deleteable<TEntity>().Where(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(TKey id)
        {
            return _dbContext.Deleteable<TEntity>().In(id).ExecuteCommand();
        }

        public Task<int> DeleteAsync(TKey id)
        {
            return _dbContext.Deleteable<TEntity>().In(id).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据主键数组删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(TKey[] ids)
        {
            return _dbContext.Deleteable<TEntity>().In(ids).ExecuteCommand();
        }

        public Task<int> DeleteAsync(TKey[] ids)
        {
            return _dbContext.Deleteable<TEntity>().In(ids).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Deleteable<TEntity>().Where(predicate).ExecuteCommand();
        }

        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Deleteable<TEntity>().Where(predicate).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据条件查询第一条符合条件的数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Queryable<TEntity>().Where(predicate).First();
        }

        public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Queryable<TEntity>().Where(predicate).FirstAsync();
        }

        /// <summary>
        /// 获取符合条件的数据列表（未完成复杂排序）
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">是否正序</param>
        //public List<TEntity> GetList(
        //    Expression<Func<TEntity, bool>> predicate,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.ToList();
        //}

        //public Task<List<TEntity>> GetListAsync(
        //    Expression<Func<TEntity, bool>> predicate,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.ToListAsync();
        //}

        /// <summary>
        /// 获取符合条件的前N条数据
        /// </summary>
        /// <param name="num">数据条数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">是否正序</param>
        //public List<TEntity> GetTop(
        //    int num,
        //    Expression<Func<TEntity, bool>> predicate = null,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.Take(num).ToList();
        //}

        //public Task<List<TEntity>> GetTopAsync(
        //    int num,
        //    Expression<Func<TEntity, bool>> predicate = null,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.Take(num).ToListAsync();
        //}

        /// <summary>
        /// 获取指定条件的
        /// </summary>
        /// <param name="count">数据总数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页行数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns></returns>
        //public List<TEntity> GetPageList(
        //    ref int count,
        //    int pageIndex,
        //    int pageSize,
        //    Expression<Func<TEntity, bool>> predicate = null,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.ToPageList(pageIndex, pageSize, ref count);
        //}


        //public Task<List<TEntity>> GetPageListAsync(
        //    RefAsync<int> count,
        //    int pageIndex,
        //    int pageSize,
        //    Expression<Func<TEntity, bool>> predicate = null,
        //    Expression<Func<TEntity, object>> orderBy = null,
        //    bool isAsc = true)
        //{
        //    var lst = _dbContext.Queryable<TEntity>();
        //    if (predicate != null)
        //    {
        //        lst = lst.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        var obt = isAsc ? OrderByType.Asc : OrderByType.Desc;
        //        lst = lst.OrderBy(orderBy, obt);
        //    }
        //    return lst.ToPageListAsync(pageIndex, pageSize, count);
        //}
    }
}
