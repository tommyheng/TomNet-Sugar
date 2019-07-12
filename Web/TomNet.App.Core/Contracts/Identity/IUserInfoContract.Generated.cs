﻿// <autogenerated>
//   This file was generated by T4 code generator ContractCodeScript.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SqlSugar;

using TomNet.SqlSugarCore.Entity;
using TomNet.App.Model.DB.Identity;


namespace TomNet.App.Core.Contracts.Identity
{
    /// <summary>
    /// 契约接口 UserInfo
    /// </summary> 
	public partial interface IUserInfoContract
    { 
		/// <summary>
        /// 获取 当前单元操作对象
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

		/// <summary>
        /// 获取 当前实体类型的查询数据集
        /// </summary>
        ISugarQueryable<UserInfo> Entities { get; }

        //=============================== 实现方法 ===============================
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(UserInfo entity);

        Task<int> InsertAsync(UserInfo entity);


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(List<UserInfo> entities);

        Task<int> InsertAsync(List<UserInfo> entities);


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <returns>操作影响的行数</returns>
        int Insert(UserInfo[] entities);

        Task<int> InsertAsync(UserInfo[] entities);


        /// <summary>
        /// 新增并返回实体
        /// </summary>
        /// <returns>操作影响的行数</returns>
        UserInfo InsertReturnEntity(UserInfo entity);

        Task<UserInfo> InsertReturnEntityAsync(UserInfo entity);


        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entity"> 实体对象（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]）</param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        int Update(UserInfo entity, string[] ignoreColumns = null);

        Task<int> UpdateAsync(UserInfo entity, string[] ignoreColumns = null);


        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entitys">实体对象集合（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]） </param> 
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns>操作影响的行数</returns>
        int Update(List<UserInfo> entitys, string[] ignoreColumns = null);

        Task<int> UpdateAsync(List<UserInfo> entitys, string[] ignoreColumns = null);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="update">实体对象</param> 
        /// <param name="predicate">条件</param>  
        /// <returns>操作影响的行数</returns>
        int Update(Expression<Func<UserInfo, UserInfo>> update, Expression<Func<UserInfo, bool>> predicate = null);

        Task<int> UpdateAsync(Expression<Func<UserInfo, UserInfo>> update, Expression<Func<UserInfo, bool>> predicate = null);


        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        int Delete(UserInfo entity);

        Task<int> DeleteAsync(UserInfo entity);


        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数</returns>
        int Delete(int id);

        Task<int> DeleteAsync(int id);


        /// <summary>
        /// 根据主键数组删除
        /// </summary>
        /// <param name="ids">主键数组</param>
        /// <returns>操作影响的行数</returns>
        int Delete(int[] ids);

        Task<int> DeleteAsync(int[] ids);


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>操作影响的行数</returns>
        int Delete(Expression<Func<UserInfo, bool>> predicate);

        Task<int> DeleteAsync(Expression<Func<UserInfo, bool>> predicate);


        /// <summary>
        /// 根据条件查询第一条符合条件的数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        UserInfo GetFirst(Expression<Func<UserInfo, bool>> predicate);

        Task<UserInfo> GetFirstAsync(Expression<Func<UserInfo, bool>> predicate);

	}
}
