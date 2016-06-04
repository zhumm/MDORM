using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 合指定的组操作符一起使用的谓词组
    /// Groups IPredicates together using the specified group operator.
    /// </summary>
    public class PredicateGroup : IPredicateGroup
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public GroupOperator Operator { get; set; }

        /// <summary>
        /// 获取或者设置谓语
        /// </summary>
        /// <value>
        /// The predicates.
        /// </value>
        public IList<IPredicate> Predicates { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string seperator = Operator == GroupOperator.And ? " AND " : " OR ";
            return "(" + Predicates.Aggregate(new StringBuilder(),
                                        (sb, p) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(p.GetSql(sqlGenerator, parameters)),
                sb =>
                {
                    var s = sb.ToString();
                    if (s.Length == 0) return sqlGenerator.Configuration.Dialect.EmptyExpression;
                    return s;
                }
                                        ) + ")";
        }
    }
}
