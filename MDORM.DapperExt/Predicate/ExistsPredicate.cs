using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 具体的存在谓语
    /// </summary>
    /// <typeparam name="TSub">The type of the sub.</typeparam>
    public class ExistsPredicate<TSub> : IExistsPredicate
        where TSub : class
    {
        /// <summary>
        /// 获取或设置条件谓语
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        public IPredicate Predicate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IExistsPredicate" /> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        public bool Not { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            IClassMapper mapSub = GetClassMapper(typeof(TSub), sqlGenerator.Configuration);
            string sql = string.Format("({0}EXISTS (SELECT 1 FROM {1} WHERE {2}))",
                Not ? "NOT " : string.Empty,
                sqlGenerator.GetTableName(mapSub),
                Predicate.GetSql(sqlGenerator, parameters));
            return sql;
        }

        /// <summary>
        /// 获取类映射
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="configuration">配置</param>
        /// <returns>
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        protected virtual IClassMapper GetClassMapper(Type type, IDapperExtConfiguration configuration)
        {
            IClassMapper map = configuration.GetMap(type);
            if (map == null)
            {
                throw new NullReferenceException(string.Format("Map was not found for {0}", type));
            }

            return map;
        }
    }
}
