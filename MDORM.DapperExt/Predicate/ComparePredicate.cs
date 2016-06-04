using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 比较谓词具体实现
    /// </summary>
    public abstract class ComparePredicate : BasePredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public Operator Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ComparePredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        public bool Not { get; set; }

        /// <summary>
        /// 获得操作字符串
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual string GetOperatorString()
        {
            switch (Operator)
            {
                case Operator.Gt:
                    return Not ? "<=" : ">";
                case Operator.Ge:
                    return Not ? "<" : ">=";
                case Operator.Lt:
                    return Not ? ">=" : "<";
                case Operator.Le:
                    return Not ? ">" : "<=";
                case Operator.Like:
                    return Not ? "NOT LIKE" : "LIKE";
                default:
                    return Not ? "<>" : "=";
            }
        }
    }
}
