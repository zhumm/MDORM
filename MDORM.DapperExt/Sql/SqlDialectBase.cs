using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    /// <summary>
    /// 基础SQL语言
    /// </summary>
    public abstract class SqlDialectBase : ISqlDialect
    {
        /// <summary>
        /// 获取开始引用的字符
        /// </summary>
        /// <value>
        /// The open quote.
        /// </value>
        public virtual char OpenQuote
        {
            get { return '"'; }
        }

        /// <summary>
        /// 获取关闭应用的字符
        /// </summary>
        /// <value>
        /// The close quote.
        /// </value>
        public virtual char CloseQuote
        {
            get { return '"'; }
        }

        /// <summary>
        /// 获取批量时的分割字符
        /// </summary>
        /// <value>
        /// The batch seperator.
        /// </value>
        public virtual string BatchSeperator
        {
            get { return ";" + Environment.NewLine; }
        }

        /// <summary>
        /// 获取是否支持复杂声明
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple statements]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool SupportsMultipleStatements
        {
            get { return true; }
        }

        /// <summary>
        /// 获取参数前缀
        /// </summary>
        /// <value>
        /// The parameter prefix.
        /// </value>
        public virtual char ParameterPrefix
        {
            get
            {
                return '@';
            }
        }

        /// <summary>
        /// 获取控的表达式
        /// </summary>
        /// <value>
        /// The empty expression.
        /// </value>
        public string EmptyExpression
        {
            get
            {
                return "1=1";
            }
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="schemaName">架构名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">TableName;tableName cannot be null or empty.</exception>
        public virtual string GetTableName(string schemaName, string tableName, string alias)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("TableName", "tableName cannot be null or empty.");
            }

            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                result.AppendFormat(QuoteString(schemaName) + ".");
            }

            result.AppendFormat(QuoteString(tableName));

            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }
            return result.ToString();
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="columnName">列的名称</param>
        /// <param name="alias">别名</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">ColumnName;columnName cannot be null or empty.</exception>
        public virtual string GetColumnName(string prefix, string columnName, string alias)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException("ColumnName", "columnName cannot be null or empty.");
            }

            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                result.AppendFormat(QuoteString(prefix) + ".");
            }

            result.AppendFormat(QuoteString(columnName));

            if (!string.IsNullOrWhiteSpace(alias))
            {
                result.AppendFormat(" AS {0}", QuoteString(alias));
            }

            return result.ToString();
        }

        /// <summary>
        /// 获取主键SQL
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public abstract string GetIdentitySql(string tableName);

        /// <summary>
        /// 获取分页SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public abstract string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取Set SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="firstResult">第一个结果索引</param>
        /// <param name="maxResults">最大结果索引</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public abstract string GetSetSql(string sql, int firstResult, int maxResults, IDictionary<string, object> parameters);

        /// <summary>
        /// 是否是应用
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual bool IsQuoted(string value)
        {
            if (value.Trim()[0] == OpenQuote)
            {
                return value.Trim().Last() == CloseQuote;
            }

            return false;
        }

        /// <summary>
        /// 应用字符串
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual string QuoteString(string value)
        {
            return IsQuoted(value) ? value : string.Format("{0}{1}{2}", OpenQuote, value.Trim(), CloseQuote);
        }

        /// <summary>
        /// Uns the quote string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// </returns>
        public virtual string UnQuoteString(string value)
        {
            return IsQuoted(value) ? value.Substring(1, value.Length - 2) : value;
        }
    }
}