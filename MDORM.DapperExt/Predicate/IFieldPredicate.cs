using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 字段谓词接口
    /// </summary>
    public interface IFieldPredicate : IComparePredicate
    {
        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object Value { get; set; }
    }
}
