using System;
using System.Collections.Generic;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// SqlCe
    /// </summary>
    public class SqlCeDialect : SqlDialectBase
    {
        /// <summary>
        /// 获取开始引用的字符
        /// </summary>
        /// <value>
        /// The open quote.
        /// </value>
        public override char OpenQuote
        {
            get { return '['; }
        }

        /// <summary>
        /// 获取关闭应用的字符
        /// </summary>
        /// <value>
        /// The close quote.
        /// </value>
        public override char CloseQuote
        {
            get { return ']'; }
        }

        /// <summary>
        /// 获取是否支持复杂声明
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple statements]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultipleStatements
        {
            get { return false; }
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="schemaName">架构名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">TableName</exception>
        public override string GetTableName(string schemaName, string tableName, string alias)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("TableName");
            }

            StringBuilder result = new StringBuilder();
            result.Append(OpenQuote);
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                result.AppendFormat("{0}_", schemaName);
            }

            result.AppendFormat("{0}{1}", tableName, CloseQuote);


            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}{1}{2}", OpenQuote, alias, CloseQuote);
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT CAST(@@IDENTITY AS BIGINT) AS [Id]";
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
            int startValue = (page * resultsPerPage);
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
            string result = string.Format("{0} OFFSET @firstResult ROWS FETCH NEXT @maxResults ROWS ONLY", sql);
            parameters.Add("@firstResult", firstResult);
            parameters.Add("@maxResults", maxResults);
            return result;            
        }
    }
}