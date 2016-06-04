using System.Reflection;

namespace MDORM.DapperExt.Mapper
{
    /// <summary>
    /// 实体属性映射到数据库中相应的列
    /// Maps an entity property to its corresponding column in the database.
    /// </summary>
    public interface IPropertyMap
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 列名称
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// 是否忽略
        /// </summary>
        bool Ignored { get; }

        /// <summary>
        /// 是否是只读的
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// 键类型
        /// </summary>
        KeyType KeyType { get; }

        /// <summary>
        /// 属性
        /// </summary>
        PropertyInfo PropertyInfo { get; }
    }
}
