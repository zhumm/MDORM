using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// Dapper扩展配置接口
    /// </summary>
    public interface IDapperExtConfiguration
    {
        /// <summary>
        /// 默认映射
        /// </summary>
        Type DefaultMapper { get; }

        /// <summary>
        /// 映射程序集
        /// </summary>
        IList<Assembly> MappingAssemblies { get; }

        /// <summary>
        /// SQL语言
        /// </summary>
        ISqlDialect Dialect { get; }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        IClassMapper GetMap(Type entityType);

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        IClassMapper GetMap<T>() where T : class;

        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 获得下一个Guid
        /// </summary>
        /// <returns></returns>
        Guid GetNextGuid();
    }
}
