using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 区间内容
    /// </summary>
    public struct BetweenValues
    {
        /// <summary>
        /// 获取或设置内容1
        /// </summary>
        /// <value>
        /// The value1.
        /// </value>
        public object Value1 { get; set; }

        /// <summary>
        /// 获取或设置内容2
        /// </summary>
        /// <value>
        /// The value2.
        /// </value>
        public object Value2 { get; set; }
    }
}
