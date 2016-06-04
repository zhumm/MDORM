using System;
using System.Collections.Generic;

namespace MDORM.DapperExt.Mapper
{
    /// <summary>
    /// IClassMapper接口
    /// </summary>
    public interface IClassMapper
    {
        /// <summary>
        /// 模式名称
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// 表名称
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// 属性列表
        /// </summary>
        IList<IPropertyMap> Properties { get; }

        /// <summary>
        /// 类型
        /// </summary>
        Type EntityType { get; }
    }

    /// <summary>
    /// IClassMapper接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IClassMapper<T> : IClassMapper where T : class
    {
    }
}
