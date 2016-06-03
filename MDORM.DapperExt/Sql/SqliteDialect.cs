using System;
using System.Collections.Generic;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// Sqlite语言
    /// </summary>
    public class SqliteDialect : SqlDialectBase
    {
        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LAST_INSERT_ROWID() AS [Id]";
        }

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            int startValue = page * resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

        /// <summary>
        /// 获取Set SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="firstResult">第一个结果索引</param>
        /// <param name="maxResults">最大结果索引</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// SQL
        /// or
        /// Parameters
        /// </exception>
        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("SQL");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            var result = string.Format("{0} LIMIT @Offset, @Count", sql);
            parameters.Add("@Offset", firstResult);
            parameters.Add("@Count", maxResults);
            return result;
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="columnName">列的名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">columnName cannot be null or empty.</exception>
        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(columnName, "columnName cannot be null or empty.");
            }
            var result = new StringBuilder();
            result.AppendFormat(columnName);
            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }
            return result.ToString();
        }
    }
}
