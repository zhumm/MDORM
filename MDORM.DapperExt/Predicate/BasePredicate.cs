using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 具体的基本谓词类
    /// </summary>
    public abstract class BasePredicate : IBasePredicate
    {
        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public abstract string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// 属性名称
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="sqlGenerator">SQL语句生成</param>
        /// <param name="propertyName">属性的名称</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">
        /// </exception>
        protected virtual string GetColumnName(Type entityType, ISqlGenerator sqlGenerator, string propertyName)
        {
            IClassMapper map = sqlGenerator.Configuration.GetMap(entityType);
            if (map == null)
            {
                throw new NullReferenceException(string.Format("Map was not found for {0}", entityType));
            }

            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name == propertyName);
            if (propertyMap == null)
            {
                throw new NullReferenceException(string.Format("{0} was not found for {1}", propertyName, entityType));
            }

            return sqlGenerator.GetColumnName(map, propertyMap, false);
        }
    }
}
