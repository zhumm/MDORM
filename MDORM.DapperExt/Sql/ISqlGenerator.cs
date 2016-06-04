using MDORM.DapperExt.Mapper;
using System.Collections.Generic;

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

}
