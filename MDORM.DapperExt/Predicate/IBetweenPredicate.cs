using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 区间谓词接口
    /// </summary>
    public interface IBetweenPredicate : IPredicate
    {
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string PropertyName { get; set; }

        /// <summary>
        /// 获取或设置区间值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        BetweenValues Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBetweenPredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }
}
