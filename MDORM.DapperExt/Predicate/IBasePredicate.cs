using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 基础谓词接口
    /// </summary>
    public interface IBasePredicate : IPredicate
    {

        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// 属性名称
        /// </value>
        string PropertyName { get; set; }
    }
}
