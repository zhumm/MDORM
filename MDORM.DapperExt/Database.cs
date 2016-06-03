using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;

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

    /// <summary>
    /// 数据库
    /// </summary>
    public class Database : IDatabase
    {
        private readonly IDapperImplementor _dapper;

        private IDbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="sqlGenerator">The SQL generator.</param>
        public Database(IDbConnection connection, ISqlGenerator sqlGenerator)
        {
            _dapper = new DapperImplementor(sqlGenerator);
            Connection = connection;
            
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        /// <summary>
        /// 是否有活动的事务
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has active transaction; otherwise, <c>false</c>.
        /// </value>
        public bool HasActiveTransaction
        {
            get
            {
                return _transaction != null;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }

                Connection.Close();
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel">隔离水平（默认ReadCommitted）</param>
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action">The action.</param>
        public void RunInTransaction(Action action)
        {
            BeginTransaction();
            try
            {
                action();
                Commit();
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public T RunInTransaction<T>(Func<T> func)
        {
            BeginTransaction();
            try
            {
                T result = func();
                Commit();
                return result;
            }
            catch (Exception ex)
            {
                if (HasActiveTransaction)
                {
                    Rollback();
                }

                throw ex;
            }
        }

        /// <summary>
        /// 获取指定Id的实体内容
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">指定的Id</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return (T)_dapper.Get<T>(Connection, id, transaction, commandTimeout);
        }

        /// <summary>
        /// 获取指定Id的实体内容
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">指定的Id</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public T Get<T>(dynamic id, int? commandTimeout) where T : class
        {
            return (T)_dapper.Get<T>(Connection, id, _transaction, commandTimeout);
        }

        /// <summary>
        /// 批量插入特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        public void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            _dapper.Insert<T>(Connection, entities, transaction, commandTimeout);
        }

        /// <summary>
        /// 批量插入实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体对象列表</param>
        /// <param name="commandTimeout">超时时间</param>
        public void Insert<T>(IEnumerable<T> entities, int? commandTimeout) where T : class
        {
            _dapper.Insert<T>(Connection, entities, _transaction, commandTimeout);
        }

        /// <summary>
        /// 插入特定的实体对象并返回对象主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public dynamic Insert<T>(T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return _dapper.Insert<T>(Connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// 插入特定的实体对象并返回对象主键
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public dynamic Insert<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Insert<T>(Connection, entity, _transaction, commandTimeout);
        }

        /// <summary>
        /// 更新特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return _dapper.Update<T>(Connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// 更新特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Update<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Update<T>(Connection, entity, _transaction, commandTimeout);
        }

        /// <summary>
        /// 删除特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>
        /// 是否操作成功
        /// </returns>
        public bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return _dapper.Delete(Connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// 删除特定的实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Delete<T>(T entity, int? commandTimeout) where T : class
        {
            return _dapper.Delete(Connection, entity, _transaction, commandTimeout);
        }

        /// <summary>
        /// 删除满足条件的对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return _dapper.Delete<T>(Connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 删除满足条件的对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public bool Delete<T>(object predicate, int? commandTimeout) where T : class
        {
            return _dapper.Delete<T>(Connection, predicate, _transaction, commandTimeout);
        }

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetList<T>(Connection, predicate, sort, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">if set to <c>true</c> [buffered].</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetList<T>(Connection, predicate, sort, _transaction, commandTimeout, buffered);
        }

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
        /// <returns></returns>
        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

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
        /// <returns></returns>
        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetPage<T>(Connection, predicate, sort, page, resultsPerPage, _transaction, commandTimeout, buffered);
        }

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
        /// <returns></returns>
        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

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
        /// <returns></returns>
        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults, int? commandTimeout, bool buffered) where T : class
        {
            return _dapper.GetSet<T>(Connection, predicate, sort, firstResult, maxResults, _transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public int Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout) where T : class
        {
            return _dapper.Count<T>(Connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 根据查询条件获取结果数据集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public int Count<T>(object predicate, int? commandTimeout) where T : class
        {
            return _dapper.Count<T>(Connection, predicate, _transaction, commandTimeout);
        }

        /// <summary>
        /// 根据查询条件获取复合结果
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 根据查询条件获取复合结果
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout)
        {
            return _dapper.GetMultiple(Connection, predicate, _transaction, commandTimeout);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _dapper.SqlGenerator.Configuration.ClearCache();
        }

        /// <summary>
        /// 获取下一个Guid
        /// </summary>
        /// <returns></returns>
        public Guid GetNextGuid()
        {
            return _dapper.SqlGenerator.Configuration.GetNextGuid();
        }

        /// <summary>
        /// 获取类映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public IClassMapper GetMap<T>() where T : class
        {
            return _dapper.SqlGenerator.Configuration.GetMap<T>();
        }
    }
}