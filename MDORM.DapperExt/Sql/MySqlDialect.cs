using System;
using System.Collections.Generic;
using System.Linq;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// MySql
    /// </summary>
    public class MySqlDialect : SqlDialectBase
    {
        /// <summary>
        /// 获取开始引用的字符
        /// </summary>
        /// <value>
        /// The open quote.
        /// </value>
        public override char OpenQuote
        {
            get { return '`'; }
        }

        /// <summary>
        /// 获取关闭应用的字符
        /// </summary>
        /// <value>
        /// The close quote.
        /// </value>
        public override char CloseQuote
        {
            get { return '`'; }
        }

        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public override string GetIdentitySql(string tableName)
        {
            return "SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS ID";
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
            string result = string.Format("{0} LIMIT @firstResult, @maxResults", sql);
            parameters.Add("@firstResult", firstResult);
            parameters.Add("@maxResults", maxResults);
            return result;
        }
    }
}