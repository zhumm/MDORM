using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// PostgreSQL
    /// </summary>
    public class PostgreSqlDialect : SqlDialectBase
    {
        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT LASTVAL() AS Id";
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
        public override string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            string result = string.Format("{0} LIMIT @firstResult OFFSET @pageStartRowNbr", sql);            
            parameters.Add("@firstResult", firstResult);
            parameters.Add("@maxResults", maxResults);
            return result;
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="columnName">列的名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public override string GetColumnName(string prefix, string columnName, string alias)
        {
            return base.GetColumnName(null, columnName, alias).ToLower();
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="schemaName">架构名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        public override string GetTableName(string schemaName, string tableName, string alias)
        {
            return base.GetTableName(schemaName, tableName, alias).ToLower();
        }
    }
}