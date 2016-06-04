using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 比较操作枚举
    /// Comparison operator for predicates.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// Equal to
        /// </summary>
        Eq,

        /// <summary>
        /// 大于
        /// Greater than
        /// </summary>
        Gt,

        /// <summary>
        /// 大于或者等于
        /// Greater than or equal to
        /// </summary>
        Ge,

        /// <summary>
        /// 小于
        /// Less than
        /// </summary>
        Lt,

        /// <summary>
        /// 大于或者等于
        /// Less than or equal to
        /// </summary>
        Le,

        /// <summary>
        /// 模糊（你可以使用 %）
        /// Like (You can use % in the value to do wilcard searching)
        /// </summary>
        Like
    }
}
