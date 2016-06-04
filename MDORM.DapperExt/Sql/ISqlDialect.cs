using System.Collections.Generic;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// SQL语言接口
    /// </summary>
    public interface ISqlDialect
    {
        /// <summary>
        /// 获取开始引用的字符
        /// </summary>
        /// <value>
        /// The open quote.
        /// </value>
        char OpenQuote { get; }

        /// <summary>
        /// 获取关闭应用的字符
        /// </summary>
        /// <value>
        /// The close quote.
        /// </value>
        char CloseQuote { get; }

        /// <summary>
        /// 获取批量时的分割字符
        /// </summary>
        /// <value>
        /// The batch seperator.
        /// </value>
        string BatchSeperator { get; }

        /// <summary>
        /// 获取是否支持复杂声明
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple statements]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsMultipleStatements { get; }

        /// <summary>
        /// 获取参数前缀
        /// </summary>
        /// <value>
        /// The parameter prefix.
        /// </value>
        char ParameterPrefix { get; }

        /// <summary>
        /// 获取控的表达式
        /// </summary>
        /// <value>
        /// The empty expression.
        /// </value>
        string EmptyExpression { get; }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="schemaName">架构名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        string GetTableName(string schemaName, string tableName, string alias);

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="columnName">列的名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        string GetColumnName(string prefix, string columnName, string alias);

        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns>
        /// </returns>
        string GetIdentitySql(string tableName);

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="parameters">参数</param>
        /// <returns>
        /// </returns>
        string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取Set SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="firstResult">第一个结果索引</param>
        /// <param name="maxResults">最大结果索引</param>
        /// <param name="parameters">参数</param>
        /// <returns>
        /// </returns>
        string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);

        /// <summary>
        /// 是否是应用
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool IsQuoted(string value);

        /// <summary>
        /// 应用字符串
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// </returns>
        string QuoteString(string value);
    }
}
