using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 排序接口
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISort"/> is ascending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ascending; otherwise, <c>false</c>.
        /// </value>
        bool Ascending { get; set; }
    }
}
