using System.Collections.Generic;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// Oracle
    /// </summary>
    public class OracleDialect : SqlDialectBase
    {
        /// <summary>
        /// 获取参数前缀
        /// </summary>
        /// <value>
        /// The parameter prefix.
        /// </value>
        public override char ParameterPrefix
        {
            get { return ':'; }
        }

        /// <summary>
        /// 获取开始引用的字符
        /// </summary>
        /// <value>
        /// The open quote.
        /// </value>
        public override char OpenQuote
        {
            get { return '"'; }
        }

        /// <summary>
        /// 获取关闭应用的字符
        /// </summary>
        /// <value>
        /// The close quote.
        /// </value>
        public override char CloseQuote
        {
            get { return '"'; }
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
        /// 获取是否支持复杂声明
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple statements]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultipleStatements
        {
            get { return false; }
        }

        //from Simple.Data.Oracle implementation https://github.com/flq/Simple.Data.Oracle/blob/master/Simple.Data.Oracle/OraclePager.cs
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
            if (page <= 0)
            {
                page = 1;
            }
            int startValue = (page - 1) * resultsPerPage;
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
            var toSkip = firstResult;// page * resultsPerPage;
            var topLimit = firstResult + maxResults;

            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT \"_ss_dapper_1_\".*, ROWNUM RNUM FROM (");
            sb.Append(sql);
            sb.AppendLine(") \"_ss_dapper_1_\"");
            sb.AppendLine("WHERE ROWNUM <= :topLimit) \"_ss_dapper_2_\" ");
            sb.AppendLine("WHERE \"_ss_dapper_2_\".RNUM > :toSkip");
            parameters.Add(":topLimit", topLimit);
            parameters.Add(":toSkip", toSkip);
            return sb.ToString();
        }
    }
}