using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace MDORM.DapperExt.Mapper
{
    /// <summary>
    /// 自动将一个实体映射到一个表，使用反射和命名约定的组合键
    /// Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
    /// </summary>
    public class AutoClassMapper<T> : ClassMapper<T> where T : class
    {
        public AutoClassMapper()
        {
            Type type = typeof(T);
            Table(type.Name);
            AutoMap();
        }
    }
}