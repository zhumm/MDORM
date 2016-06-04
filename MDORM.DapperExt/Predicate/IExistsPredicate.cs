using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 存在谓词接口
    /// </summary>
    public interface IExistsPredicate : IPredicate
    {
        /// <summary>
        /// 获取或设置条件谓语
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        IPredicate Predicate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IExistsPredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }
}
