using System.Collections.Generic;
using System.Text;

namespace MDORM.DapperExt.Sql
{
    public class OracleDialect : SqlDialectBase
    {
        public override char ParameterPrefix
        {
            get { return ':'; }
        }

        public override char OpenQuote
        {
            get { return '"'; }
        }

        public override char CloseQuote
        {
            get { return '"'; }
        }

        public override string GetIdentitySql(string tableName)
        {
            return "SELECT CONVERT(LAST_INSERT_ID(), SIGNED INTEGER) AS ID";
        }


        public override bool SupportsMultipleStatements
        {
            get { return false; }
        }

        //from Simple.Data.Oracle implementation https://github.com/flq/Simple.Data.Oracle/blob/master/Simple.Data.Oracle/OraclePager.cs
        public override string GetPagingSql(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            if (page <= 0)
            {
                page = 1;
            }
            int startValue = (page - 1) * resultsPerPage;
            return GetSetSql(sql, startValue, resultsPerPage, parameters);
        }

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