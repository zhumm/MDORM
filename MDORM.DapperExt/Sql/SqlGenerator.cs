using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDORM.DapperExt.Mapper;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// SQL生成接口
    /// </summary>
    public interface ISqlGenerator
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <value>
        /// 配置
        /// </value>
        IDapperExtConfiguration Configuration { get; }

        /// <summary>
        /// 生成SELECTSQL语句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters);

        /// <summary>
        /// 生成SelectPagedSQL语句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="parameters">参数</param>
        /// <returns>
        /// </returns>
        string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters);

        /// <summary>
        /// 生成SelectSet的SQL
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="firstResult">第一个结果索引</param>
        /// <param name="maxResults">最大结果</param>
        /// <param name="parameters">参数</param>
        /// <returns>
        /// </returns>
        string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters);

        /// <summary>
        /// 生成Count的SQL語句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        /// <summary>
        /// 生成插入语句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <returns></returns>
        string Insert(IClassMapper classMap);

        /// <summary>
        /// 原始更新方法。不能根据实体动态更新（更新全部列）
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        /// <summary>
        /// Deletes the specified class map.
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        /// <summary>
        /// Identities the SQL.
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <returns>
        /// </returns>
        string IdentitySql(IClassMapper classMap);

        /// <summary>
        /// 获得表名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <returns></returns>
        string GetTableName(IClassMapper map);

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <param name="property">属性.</param>
        /// <param name="includeAlias">if set to <c>true</c> [include alias].</param>
        /// <returns></returns>
        string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias);

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <param name="propertyName">属性的名称</param>
        /// <param name="includeAlias">if set to <c>true</c> [include alias].</param>
        /// <returns></returns>
        string GetColumnName(IClassMapper map, string propertyName, bool includeAlias);

        /// <summary>
        /// Supportses the multiple statements.
        /// </summary>
        /// <returns>
        /// </returns>
        bool SupportsMultipleStatements();

        /// <summary>
        /// 添加的新方法。支持动态更新实体对象
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="updatedColumns">要更新的列</param>
        /// <param name="predicate">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        string Update(IClassMapper classMap, IEnumerable<KeyValuePair<string, object>> updatedColumns, IPredicate predicate, Dictionary<string, object> parameters);
    }


    /// <summary>
    /// 具体的SQL生成类
    /// </summary>
    public class SqlGeneratorImpl : ISqlGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlGeneratorImpl"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SqlGeneratorImpl(IDapperExtConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <value>
        /// 配置
        /// </value>
        public IDapperExtConfiguration Configuration { get; private set; }

        /// <summary>
        /// 生成SELECTSQL语句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Parameters</exception>
        public virtual string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters));
            }

            if (sort != null && sort.Any())
            {
                sql.Append(" ORDER BY ")
                    .Append(sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings());
            }

            return sql.ToString();
        }

        /// <summary>
        /// 生成SelectPagedSQL语句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Sort;Sort cannot be null or empty.
        /// or
        /// Parameters
        /// </exception>
        public virtual string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            if (sort == null || !sort.Any())
            {
                throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters));
            }

            string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
            innerSql.Append(" ORDER BY " + orderBy);

            string sql = Configuration.Dialect.GetPagingSql(innerSql.ToString(), page, resultsPerPage, parameters);
            return sql;
        }

        /// <summary>
        /// 生成SelectSet的SQL
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="firstResult">第一个结果索引</param>
        /// <param name="maxResults">最大结果</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Sort;Sort cannot be null or empty.
        /// or
        /// Parameters
        /// </exception>
        public virtual string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            if (sort == null || !sort.Any())
            {
                throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters));
            }

            string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
            innerSql.Append(" ORDER BY " + orderBy);

            string sql = Configuration.Dialect.GetSetSql(innerSql.ToString(), firstResult, maxResults, parameters);
            return sql;
        }

        /// <summary>
        /// 生成Count的SQL語句
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Parameters</exception>
        public virtual string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT COUNT(*) AS {0}Total{1} FROM {2}",
                                Configuration.Dialect.OpenQuote,
                                Configuration.Dialect.CloseQuote,
                                GetTableName(classMap)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters));
            }

            return sql.ToString();
        }

        /// <summary>
        /// Insert方法。不能根据实体动态添加（添加全部列）
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">No columns were mapped.</exception>
        public virtual string Insert(IClassMapper classMap)
        {
            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var columnNames = columns.Select(p => GetColumnName(classMap, p, false));
            var parameters = columns.Select(p => Configuration.Dialect.ParameterPrefix + p.Name);

            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                                       GetTableName(classMap),
                                       columnNames.AppendStrings(),
                                       parameters.AppendStrings());

            return sql;
        }

        /// <summary>
        /// 原始更新方法。不能根据实体动态更新（更新全部列）
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Predicate
        /// or
        /// Parameters
        /// </exception>
        /// <exception cref="ArgumentException">No columns were mapped.</exception>
        public virtual string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
                columns.Select(
                    p =>
                    string.Format(
                        "{0} = {1}{2}", GetColumnName(classMap, p, false), Configuration.Dialect.ParameterPrefix, p.Name));

            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                GetTableName(classMap),
                setSql.AppendStrings(),
                predicate.GetSql(this, parameters));
        }

        /// <summary>
        /// 添加的新方法。支持动态更新实体对象
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="updatedColumns">要更新的列</param>
        /// <param name="predicate">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Predicate
        /// or
        /// Parameters
        /// </exception>
        /// <exception cref="ArgumentException">No columns were mapped.</exception>
        public virtual string Update(IClassMapper classMap, IEnumerable<KeyValuePair<string, object>> updatedColumns, IPredicate predicate, Dictionary<string, object> parameters)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            if (!updatedColumns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
                updatedColumns.Select(
                    p =>
                    string.Format(
                        "{0} = {1}{2}", p.Key, Configuration.Dialect.ParameterPrefix, p.Key));

            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                GetTableName(classMap),
                setSql.AppendStrings(),
                predicate.GetSql(this, parameters));
        }

        /// <summary>
        /// Deletes the specified class map.
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// Predicate
        /// or
        /// Parameters
        /// </exception>
        public virtual string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("DELETE FROM {0}", GetTableName(classMap)));
            sql.Append(" WHERE ").Append(predicate.GetSql(this, parameters));
            return sql.ToString();
        }

        /// <summary>
        /// Identities the SQL.
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <returns></returns>
        public virtual string IdentitySql(IClassMapper classMap)
        {
            return Configuration.Dialect.GetIdentitySql(GetTableName(classMap));
        }

        /// <summary>
        /// 获得表名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <returns></returns>
        public virtual string GetTableName(IClassMapper map)
        {
            return Configuration.Dialect.GetTableName(map.SchemaName, map.TableName, null);
        }

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <param name="property">属性.</param>
        /// <param name="includeAlias">if set to <c>true</c> [include alias].</param>
        /// <returns></returns>
        public virtual string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias)
        {
            string alias = null;
            if (property.ColumnName != property.Name && includeAlias)
            {
                alias = property.Name;
            }

            return Configuration.Dialect.GetColumnName(GetTableName(map), property.ColumnName, alias);
        }

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="map">类映射</param>
        /// <param name="propertyName">属性的名称</param>
        /// <param name="includeAlias">if set to <c>true</c> [include alias].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public virtual string GetColumnName(IClassMapper map, string propertyName, bool includeAlias)
        {
            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (propertyMap == null)
            {
                throw new ArgumentException(string.Format("Could not find '{0}' in Mapping.", propertyName));
            }

            return GetColumnName(map, propertyMap, includeAlias);
        }

        /// <summary>
        /// Supportses the multiple statements.
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual bool SupportsMultipleStatements()
        {
            return Configuration.Dialect.SupportsMultipleStatements;
        }

        /// <summary>
        /// 创建选择的列
        /// </summary>
        /// <param name="classMap">类映射</param>
        /// <returns>
        /// </returns>
        public virtual string BuildSelectColumns(IClassMapper classMap)
        {
            var columns = classMap.Properties
                .Where(p => !p.Ignored)
                .Select(p => GetColumnName(classMap, p, true));
            return columns.AppendStrings();
        }
    }
}