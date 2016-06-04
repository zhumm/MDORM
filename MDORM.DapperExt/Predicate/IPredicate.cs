using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 谓词接口
    /// </summary>
    public interface IPredicate
    {
        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns>
        /// </returns>
        string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters);
    }
}
