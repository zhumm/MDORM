using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 比较谓词接口
    /// </summary>
    public interface IComparePredicate : IBasePredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        Operator Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IComparePredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }
}
