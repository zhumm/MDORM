using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDORM.DapperExt
{
    /// <summary>
    /// Dapper 扩展配置
    /// </summary>
    public class DapperExtConfiguration : IDapperExtConfiguration
    {
        private readonly ConcurrentDictionary<Type, IClassMapper> _classMaps = new ConcurrentDictionary<Type, IClassMapper>();

        public DapperExtConfiguration()
            : this(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect()) { }

        public DapperExtConfiguration(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            DefaultMapper = defaultMapper;
            MappingAssemblies = mappingAssemblies ?? new List<Assembly>();
            Dialect = sqlDialect;
        }

        /// <summary>
        /// 默认映射
        /// </summary>
        public Type DefaultMapper { get; private set; }

        /// <summary>
        /// 映射程序集
        /// </summary>
        public IList<Assembly> MappingAssemblies { get; private set; }

        /// <summary>
        /// SQL语言
        /// </summary>
        public ISqlDialect Dialect { get; private set; }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        public IClassMapper GetMap(Type entityType)
        {
            IClassMapper map;
            if (!_classMaps.TryGetValue(entityType, out map))
            {
                Type mapType = GetMapType(entityType);
                if (mapType == null)
                {
                    mapType = DefaultMapper.MakeGenericType(entityType);
                }

                map = Activator.CreateInstance(mapType) as IClassMapper;
                _classMaps[entityType] = map;
            }

            return map;
        }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public IClassMapper GetMap<T>() where T : class
        {
            return GetMap(typeof(T));
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            _classMaps.Clear();
        }

        /// <summary>
        /// 获取下一个Guid
        /// </summary>
        /// <returns></returns>
        public Guid GetNextGuid()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            DateTime dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }

        /// <summary>
        /// 获取映射类型
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>类型</returns>
        protected virtual Type GetMapType(Type entityType)
        {
            Func<Assembly, Type> getType = a =>
            {
                Type[] types = a.GetTypes();
                return (from type in types
                        let interfaceType = type.GetInterface(typeof(IClassMapper<>).FullName)
                        where
                            interfaceType != null &&
                            interfaceType.GetGenericArguments()[0] == entityType
                        select type).SingleOrDefault();
            };

            Type result = getType(entityType.Assembly);
            if (result != null)
            {
                return result;
            }

            foreach (var mappingAssembly in MappingAssemblies)
            {
                result = getType(mappingAssembly);
                if (result != null)
                {
                    return result;
                }
            }

            return getType(entityType.Assembly);
        }
    }
}