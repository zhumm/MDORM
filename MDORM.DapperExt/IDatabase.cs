using MDORM.DapperExt.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 数据库接口
    /// </summary>
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// 是否有活动的事务
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has active transaction; otherwise, <c>false</c>.
        /// </value>
        bool HasActiveTransaction { get; }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        IDbConnection Connection { get; }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel">隔离水平（默认ReadCommitted）</param>
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action">The action.</param>
        void RunInTransaction(Action action);

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns>
        /// </returns>
        T RunInTransaction<T>(Func<T> func);

        /// <summary>
        /// 获取指定Id的实体内容
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">指定的Id</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 获取指定Id的实体内容
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">指定的Id</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        T Get<T>(dynamic id, int? commandTimeout = null) where T : class;


        /// <summary>
        /// 批量插入特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 批量插入实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        /// <param name="commandTimeout">超时时间</param>
        void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 插入特定的实体对象并返回对象主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        dynamic Insert<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 插入特定的实体对象并返回对象主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        dynamic Insert<T>(T entity, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 更新特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 更新特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        bool Update<T>(T entity, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 删除特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否操作成功</returns>
        bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 删除特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 删除满足条件的对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 删除满足条件的对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        bool Delete<T>(object predicate, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null, bool buffered = true) where T : class;

        /// <summary>
        /// 根据查询条件分页获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout = null, bool buffered = true) where T : class;

        /// <summary>
        /// 根据查询条件分页获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout = null, bool buffered = true) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="firstResult">第一条结果索引</param>
        /// <param name="maxResults">最大结果所以</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="firstResult">第一条结果索引</param>
        /// <param name="maxResults">最大结果所以</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>
        /// </returns>
        int Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>
        /// </returns>
        int Count<T>(object predicate, int? commandTimeout = null) where T : class;

        /// <summary>
        /// 根据查询条件获取复合结果
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>
        /// </returns>
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout = null);

        /// <summary>
        /// 根据查询条件获取复合结果
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>
        /// </returns>
        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);

        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 获取下一个Guid
        /// </summary>
        /// <returns>
        /// </returns>
        Guid GetNextGuid();

        /// <summary>
        /// 获取类映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>
        /// </returns>
        IClassMapper GetMap<T>() where T : class;
    }
}
