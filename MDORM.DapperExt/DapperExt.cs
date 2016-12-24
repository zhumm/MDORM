using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using MDORM.DapperExt.Sql;
using MDORM.DapperExt.Mapper;
using System.Linq.Expressions;
using MDORM.DapperExt.Utility;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 静态Dapper扩展类
    /// </summary>
    public static class DapperExt
    {
        private readonly static object _lock = new object();

        private static Func<IDapperExtConfiguration, IDapperImplementor> _instanceFactory;
        private static IDapperImplementor _instance;
        private static IDapperExtConfiguration _configuration;
        
        /// <summary>
        /// 获取或设置默认的当生成类映时的默认映射。如果不指定,AutoClassMapper < T >将会使用。
        /// DapperExt.Configure(Type, IList<Assembly>, ISqlDialect)将会用来设置所有的值
        /// Gets or sets the default class mapper to use when generating class maps. If not specified, AutoClassMapper<T> is used.
        /// DapperExt.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static Type DefaultMapper
        {
            get
            {
                return _configuration.DefaultMapper;
            }

            set
            {
                Configure(value, _configuration.MappingAssemblies, _configuration.Dialect);
            }
        }

        /// <summary>
        /// 获取或设置生生成Sql的类型
        /// DapperExt.Configure(Type, IList<Assembly>, ISqlDialect) 在一开始会用来替换所有的内容
        /// Gets or sets the type of sql to be generated.
        /// DapperExt.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static ISqlDialect SqlDialect
        {
            get
            {
                return _configuration.Dialect;
            }

            set
            {
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, value);
            }
        }
        
        /// <summary>
        /// 获取或设置Dapper 扩展实现工厂
        /// Get or sets the Dapper Extensions Implementation Factory.
        /// </summary>
        public static Func<IDapperExtConfiguration, IDapperImplementor> InstanceFactory
        {
            get
            {
                if (_instanceFactory == null)
                {
                    _instanceFactory = config => new DapperImplementor(new SqlGeneratorImpl(config));
                }

                return _instanceFactory;
            }
            set
            {
                _instanceFactory = value;
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, _configuration.Dialect);
            }
        }

        /// <summary>
        /// 获取Dapper 扩展实现（使用单例模式）
        /// Gets the Dapper Extensions Implementation
        /// </summary>
        private static IDapperImplementor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = InstanceFactory(_configuration);
                        }
                    }
                }

                return _instance;
            }
        }

        static DapperExt()
        {
            Configure(typeof(AutoClassMapper<>), new List<Assembly>(),SqlDialectFactory.CreateSqlDialect());
        }

        /// <summary>
        /// 添加其他的程序集列表。Dapper扩展将会在这些程序集中查找
        /// Add other assemblies that Dapper Extensions will search if a mapping is not found in the same assembly of the POCO.
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetMappingAssemblies(IList<Assembly> assemblies)
        {
            Configure(_configuration.DefaultMapper, assemblies, _configuration.Dialect);
        }

        /// <summary>
        /// 配置Dapper扩展方法
        /// Configure DapperExt extension methods.
        /// </summary>
        public static void Configure(IDapperExtConfiguration configuration)
        {
            _instance = null;
            _configuration = configuration;
        }

        /// <summary>
        /// 配置Dapper扩展方法
        /// Configure DapperExt extension methods.
        /// </summary>
        /// <param name="defaultMapper">默认的类型</param>
        /// <param name="mappingAssemblies">要映射的程序集</param>
        /// <param name="sqlDialect">sql语言实例</param>
        public static void Configure(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            Configure(new DapperExtConfiguration(defaultMapper, mappingAssemblies, sqlDialect));
        }

        /// <summary>
        /// 按照特定的Id执行查询并返回T类型的对象
        /// Executes a query for the specified id, returning the data typed as per T
        /// </summary>
        /// <typeparam name="T">返回对象的类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="id">Id</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            var result = Instance.Get<T>(connection, id, transaction, commandTimeout);
            return (T)result;
        }

        /// <summary>
        /// 为特定的实体对象执行INSERT方法
        /// Executes an insert query for the specified entity.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="entities">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Instance.Insert<T>(connection, entities, transaction, commandTimeout);
        }

        /// <summary>
        /// 为特定的实体对象执行插入操作并放回这条记录的主键
        /// 如果实体有单个的的主键，仅仅返回主键值
        /// 如果实体有符合的主键。将会返回一个键值字典
        /// 当实体的主键类型是Guid或者Identity时，主键内容会自动被改变
        /// Executes an insert query for the specified entity, returning the primary key.  
        /// If the entity has a single key, just the value is returned.  
        /// If the entity has a composite key, an IDictionary&lt;string, object&gt; is returned with the key values.
        /// The key value for the entity will also be updated if the KeyType is a Guid or Identity.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static dynamic Insert<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Insert<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// 为一个特定的实体对象执行UPDATE操作
        /// Executes an update query for the specified entity.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否执行成功</returns>
        public static bool Update<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Update<T>(connection, entity, transaction, commandTimeout);
        }

        ///// <summary>
        ///// 新添加的。按照条件批量更新
        ///// </summary>
        //public static bool Update<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        //{
        //    return Instance.Update<T>(connection, entity, transaction, commandTimeout);
        //}

        /// <summary>
        /// 对特定的实体执行DELETE操作
        /// Executes a delete query for the specified entity.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="entity">实体对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static bool Delete<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// 删除满足通过查询条件的记录
        /// Executes a delete query using the specified predicate.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>是否执行成功</returns>
        public static bool Delete<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Delete<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 根据查询条件选择满足条件的记录并返回IEnumerable<T>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">是否缓存，默认不缓存</param>
        /// <returns>T类型的枚举器</returns>
        public static IEnumerable<T> GetList<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetList<T>(connection, predicate, sort, transaction, commandTimeout, buffered);
        }

        #region 兼容lambda
        ///// <summary>
        ///// 根据查询条件选择满足条件的记录并返回IEnumerable<T>
        ///// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="connection">数据库连接</param>
        ///// <param name="predicate">查询条件</param>
        ///// <param name="sort">排序列表</param>
        ///// <param name="transaction">事务</param>
        ///// <param name="commandTimeout">超时时间</param>
        ///// <param name="buffered">是否缓存，默认不缓存</param>
        ///// <returns>T类型的枚举器</returns>
        //public static IEnumerable<T> GetList<T>(this IDbConnection connection, Expression func, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        //{
        //    return Instance.GetList<T>(connection, func, transaction, commandTimeout, buffered);
        //}
        #endregion

        /// <summary>
        /// 根据查询条件分页选择满足条件的记录并返回当前页数据IEnumerable<T>
        /// 返回的数据由当前数据页和页大小决定
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">是否缓存，默认为false</param>
        /// <returns>当前页的数据</returns>
        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetPage<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// 根据查询条件选择满足条件的记录并返回IEnumerable<T>
        /// 返回的数据取决于firstResult and maxResults
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified firstResult and maxResults.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="firstResult">第一条数据的索引</param>
        /// <param name="maxResults">最大数据的索引</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="buffered">是否缓存，默认不缓存</param>
        /// <returns>当前记录</returns>
        public static IEnumerable<T> GetSet<T>(this IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
        {
            return Instance.GetSet<T>(connection, predicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

        /// <summary>
        /// 根据查询条件选择满足条件的记录并返回满足添加的记录条数
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>满足条件的记录数</returns>
        public static int Count<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return Instance.Count<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 执行一个查询符合对象的操作并为每个查询返回IMultipleResultReader对象
        /// Executes a select query for multiple objects, returning IMultipleResultReader for each predicate.
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static IMultipleResultReader GetMultiple(this IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Instance.GetMultiple(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// 获取特定类型的映射。
        /// 如果这个映射不存在就使用默认的映射生成一个新的映射
        /// Gets the appropriate mapper for the specified type T. 
        /// If the mapper for the type is not yet created, a new mapper is generated from the mapper type specifed by DefaultMapper.
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public static IClassMapper GetMap<T>() where T : class
        {
            return Instance.SqlGenerator.Configuration.GetMap<T>();
        }

        /// <summary>
        /// 清除sql缓存
        /// Clears the ClassMappers for each type.
        /// </summary>
        public static void ClearCache()
        {
            Instance.SqlGenerator.Configuration.ClearCache();
        }

        /// <summary>
        /// 生成一个COMB Guid,解决了分散指数的问题
        /// Generates a COMB Guid which solves the fragmented index issue.
        /// See: http://davybrion.com/blog/2009/05/using-the-guidcomb-identifier-strategy
        /// </summary>
        public static Guid GetNextGuid()
        {
            return Instance.SqlGenerator.Configuration.GetNextGuid();
        }

        public static ISqlDialect GetSqlDialect 
        { 
            get;
            
            set;
        }
    }
}
