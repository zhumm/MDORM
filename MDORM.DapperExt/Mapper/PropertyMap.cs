using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 实体属性映射到数据库中相应的列。
    /// Maps an entity property to its corresponding column in the database.
    /// </summary>
    public class PropertyMap : IPropertyMap
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        public PropertyMap(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            ColumnName = PropertyInfo.Name;
        }

        /// <summary>
        /// 获取当前属性的特定（中文）名称
        /// Gets the name of the property by using the specified propertyInfo.
        /// </summary>
        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        /// <summary>
        /// 获取当前属性的列名称
        /// Gets the column name for the current property.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// 获取当前属性的KeyType
        /// Gets the key type for the current property.
        /// </summary>
        public KeyType KeyType { get; private set; }

        /// <summary>
        /// 获取是否忽视当前的属性。如果忽视,当前的属性将不会被包括在查询中。
        /// Gets the ignore status of the current property. If ignored, the current property will not be included in queries.
        /// </summary>
        public bool Ignored { get; private set; }

        /// <summary>
        /// 获取当前的属性是否是只读的，如果是，当前的属性将不会被包括在INSERT和UPDATE查询中。
        /// Gets the read-only status of the current property. If read-only, the current property will not be included in INSERT and UPDATE queries.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// 获取当前属性的属性信息
        /// Gets the property info for the current property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// 设置当前属性的列名称
        /// </summary>
        /// <param name="columnName">数据库中存在的列，The column name as it exists in the database.</param>
        public PropertyMap Column(string columnName)
        {
            ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// 设置当前属性的键类型（当前字段不能设置成忽略的或是只读的）
        /// </summary>
        /// <param name="keyType">键类型枚举</param>
        /// <exception cref="ArgumentException">当前属性不能是忽略的</exception>
        /// /// <exception cref="ArgumentException">当前属性不能是只读的</exception>
        /// <returns></returns>
        public PropertyMap Key(KeyType keyType)
        {
            if (Ignored)
            {
                throw new ArgumentException(string.Format("'{0}' is ignored and cannot be made a key field. ", Name));
            }

            if (IsReadOnly)
            {
                throw new ArgumentException(string.Format("'{0}' is readonly and cannot be made a key field. ", Name));
            }

            KeyType = keyType;
            return this;
        }

        /// <summary>
        /// 设置当前属性为忽略
        /// Fluently sets the ignore status of the property.
        /// </summary>
        /// <exception cref="ArgumentException">当前属性的键类型不能为：NotAKey</exception>
        public PropertyMap Ignore()
        {
            if (KeyType != KeyType.NotAKey)
            {
                throw new ArgumentException(string.Format("'{0}' is a key field and cannot be ignored.", Name));
            }

            Ignored = true;
            return this;
        }

        /// <summary>
        /// 设置当前属性为只读
        /// Fluently sets the read-only status of the property.
        /// </summary>
        /// <exception cref="ArgumentException">当前属性的键类型不能为：NotAKey</exception>
        public PropertyMap ReadOnly()
        {
            if (KeyType != KeyType.NotAKey)
            {
                throw new ArgumentException(string.Format("'{0}' is a key field and cannot be marked readonly.", Name));
            }

            IsReadOnly = true;
            return this;
        }
    }

    /// <summary>
    /// 键类型枚举
    /// ClassMapper用来确定实体哪个属性表示什么类型的键
    /// Used by ClassMapper to determine which entity property represents the key.
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// 属性不是主键,不会自动管理
        /// The property is not a key and is not automatically managed.
        /// </summary>
        NotAKey,

        /// <summary>
        /// 属性是一个基于数据库Int自增长的
        /// The property is an integery-based identity generated from the database.
        /// </summary>
        Identity,

        /// <summary>
        /// 属性是由框架自动管理的Guid主键
        /// The property is a Guid identity which is automatically managed.
        /// </summary>
        Guid,

        /// <summary>
        /// 属性不是自动管理的键
        /// The property is a key that is not automatically managed.
        /// </summary>
        Assigned
    }
}