using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 谓词组接口
    /// </summary>
    public interface IPredicateGroup : IPredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        GroupOperator Operator { get; set; }

        /// <summary>
        /// 获取或者设置谓语
        /// </summary>
        /// <value>
        /// The predicates.
        /// </value>
        IList<IPredicate> Predicates { get; set; }
    }
}
