using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 属性谓词接口
    /// </summary>
    public interface IPropertyPredicate : IComparePredicate
    {
        /// <summary>
        /// 获取或设置属性2
        /// </summary>
        /// <value>
        /// The property name2.
        /// </value>
        string PropertyName2 { get; set; }
    }
}
