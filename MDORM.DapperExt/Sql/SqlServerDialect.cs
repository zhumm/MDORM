using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// SqlServer
    /// </summary>
    public class SqlServerDialect : SqlDialectBase
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
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public override string GetIdentitySql(string tableName)
        {
            return string.Format("SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [Id]");
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
            int startValue = (page * resultsPerPage) + 1;
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

            int selectIndex = GetSelectEnd(sql) + 1;
            string orderByClause = GetOrderByClause(sql);
            if (orderByClause == null)
            {
                orderByClause = "ORDER BY CURRENT_TIMESTAMP";
            }


            string projectedColumns = GetColumnNames(sql).Aggregate(new StringBuilder(), (sb, s) => (sb.Length == 0 ? sb : sb.Append(", ")).Append(GetColumnName("_proj", s, null)), sb => sb.ToString());
            string newSql = sql
                .Replace(" " + orderByClause, string.Empty)
                .Insert(selectIndex, string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS {1}, ", orderByClause.Substring(9), GetColumnName(null, "_row_number", null)));

            string result = string.Format("SELECT TOP({0}) {1} FROM ({2}) [_proj] WHERE {3} >= @_pageStartRow ORDER BY {3}",
                maxResults, projectedColumns.Trim(), newSql, GetColumnName("_proj", "_row_number", null));

            parameters.Add("@_pageStartRow", firstResult);
            return result;
        }

        /// <summary>
        /// Gets the order by clause.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// </returns>
        protected string GetOrderByClause(string sql)
        {
            int orderByIndex = sql.LastIndexOf(" ORDER BY ", StringComparison.InvariantCultureIgnoreCase);
            if (orderByIndex == -1)
            {
                return null;
            }

            string result = sql.Substring(orderByIndex).Trim();

            int whereIndex = result.IndexOf(" WHERE ", StringComparison.InvariantCultureIgnoreCase);
            if (whereIndex == -1)
            {
                return result;
            }

            return result.Substring(0, whereIndex).Trim();
        }

        /// <summary>
        /// Gets from start.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// </returns>
        protected int GetFromStart(string sql)
        {
            int selectCount = 0;
            string[] words = sql.Split(' ');
            int fromIndex = 0;
            foreach (var word in words)
            {
                if (word.Equals("SELECT", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectCount++;
                }

                if (word.Equals("FROM", StringComparison.InvariantCultureIgnoreCase))
                {
                    selectCount--;
                    if (selectCount == 0)
                    {
                        break;
                    }
                }

                fromIndex += word.Length + 1;
            }

            return fromIndex;
        }

        /// <summary>
        /// Gets the select end.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">SQL must be a SELECT statement.;sql</exception>
        protected virtual int GetSelectEnd(string sql)
        {
            if (sql.StartsWith("SELECT DISTINCT", StringComparison.InvariantCultureIgnoreCase))
            {
                return 15;
            }

            if (sql.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
            {
                return 6;
            }

            throw new ArgumentException("SQL must be a SELECT statement.", "sql");
        }

        /// <summary>
        /// Gets the column names.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>
        /// </returns>
        protected virtual IList<string> GetColumnNames(string sql)
        {
            int start = GetSelectEnd(sql);
            int stop = GetFromStart(sql);
            string[] columnSql = sql.Substring(start, stop - start).Split(',');
            List<string> result = new List<string>();
            foreach (string c in columnSql)
            {
                int index = c.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                {
                    result.Add(c.Substring(index + 4).Trim());
                    continue;
                }

                string[] colParts = c.Split('.');
                result.Add(colParts[colParts.Length - 1].Trim());
            }

            return result;
        }
    }
}