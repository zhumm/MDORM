using System.Numerics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

    /// <summary>
    /// 通过属性映射的集合将一个实体映射到一个表
    /// Maps an entity to a table through a collection of property maps.
    /// </summary>
    public class ClassMapper<T> : IClassMapper<T> where T : class
    {
        /// <summary>
        /// 获取或设置数据库模式
        /// Gets or sets the schema to use when referring to the corresponding table name in the database.
        /// </summary>
        public string SchemaName { get; protected set; }

        /// <summary>
        /// 获取或设置表名称
        /// Gets or sets the table to use in the database.
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// 映射到数据库表的属性集合
        /// A collection of properties that will map to columns in the database table.
        /// </summary>
        public IList<IPropertyMap> Properties { get; private set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType
        {
            get { return typeof(T); }
        }

        public ClassMapper()
        {
            PropertyTypeKeyTypeMapping = new Dictionary<Type, KeyType>
                                             {
                                                 { typeof(byte), KeyType.Identity }, { typeof(byte?), KeyType.Identity },
                                                 { typeof(sbyte), KeyType.Identity }, { typeof(sbyte?), KeyType.Identity },
                                                 { typeof(short), KeyType.Identity }, { typeof(short?), KeyType.Identity },
                                                 { typeof(ushort), KeyType.Identity }, { typeof(ushort?), KeyType.Identity },
                                                 { typeof(int), KeyType.Identity }, { typeof(int?), KeyType.Identity },
                                                 { typeof(uint), KeyType.Identity}, { typeof(uint?), KeyType.Identity },
                                                 { typeof(long), KeyType.Identity }, { typeof(long?), KeyType.Identity },
                                                 { typeof(ulong), KeyType.Identity }, { typeof(ulong?), KeyType.Identity },
                                                 { typeof(BigInteger), KeyType.Identity }, { typeof(BigInteger?), KeyType.Identity },
                                                 { typeof(Guid), KeyType.Guid }, { typeof(Guid?), KeyType.Guid },
                                             };

            Properties = new List<IPropertyMap>();
            Table(typeof(T).Name);
        }

        /// <summary>
        /// 属性类型和键类型的映射
        /// </summary>
        protected Dictionary<Type, KeyType> PropertyTypeKeyTypeMapping { get; private set; }

        /// <summary>
        /// 模式名称
        /// </summary>
        /// <param name="schemaName"></param>
        public virtual void Schema(string schemaName)
        {
            SchemaName = schemaName;
        }

        /// <summary>
        /// 表名称
        /// </summary>
        /// <param name="tableName"></param>
        public virtual void Table(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 自动映射方法
        /// </summary>
        protected virtual void AutoMap()
        {
            AutoMap(null);
        }

        /// <summary>
        /// 自动映射
        /// </summary>
        /// <param name="canMap"></param>
        protected virtual void AutoMap(Func<Type, PropertyInfo, bool> canMap)
        {
            Type type = typeof(T);
            bool hasDefinedKey = Properties.Any(p => p.KeyType != KeyType.NotAKey);
            PropertyMap keyMap = null;
            foreach (var propertyInfo in type.GetProperties())
            {
                if (Properties.Any(p => p.Name.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                if ((canMap != null && !canMap(type, propertyInfo)))
                {
                    continue;
                }

                PropertyMap map = Map(propertyInfo);
                if (!hasDefinedKey)
                {
                    if (string.Equals(map.PropertyInfo.Name, "id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        keyMap = map;
                    }

                    if (keyMap == null && map.PropertyInfo.Name.EndsWith("id", true, CultureInfo.InvariantCulture))
                    {
                        keyMap = map;
                    }
                }
            }

            if (keyMap != null)
            {
                keyMap.Key(PropertyTypeKeyTypeMapping.ContainsKey(keyMap.PropertyInfo.PropertyType)
                    ? PropertyTypeKeyTypeMapping[keyMap.PropertyInfo.PropertyType]
                    : KeyType.Assigned);
            }
        }

        /// <summary>
        /// 映实体属性射到一数据库表中的某一列
        /// Fluently, maps an entity property to a column
        /// </summary>
        protected PropertyMap Map(Expression<Func<T, object>> expression)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return Map(propertyInfo);
        }

        /// <summary>
        /// 映实体属性射到一数据库表中的某一列
        /// Fluently, maps an entity property to a column
        /// </summary>
        protected PropertyMap Map(PropertyInfo propertyInfo)
        {
            PropertyMap result = new PropertyMap(propertyInfo);
            this.GuardForDuplicatePropertyMap(result);
            Properties.Add(result);
            return result;
        }

        /// <summary>
        /// 判断是否是重复的属性,是就抛出异常
        /// </summary>
        /// <exception cref="ArgumentException">重复的映射属性异常</exception>
        /// <param name="result"></param>
        private void GuardForDuplicatePropertyMap(PropertyMap result)
        {
            if (Properties.Any(p => p.Name.Equals(result.Name)))
            {
                throw new ArgumentException(string.Format("Duplicate mapping for property {0} detected.",result.Name));
            }
        }
    }
}