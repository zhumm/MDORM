using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 具体区间谓词类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BetweenPredicate<T> : BasePredicate, IBetweenPredicate
        where T : class
    {
        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            string propertyName1 = parameters.SetParameterName(this.PropertyName, this.Value.Value1, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            string propertyName2 = parameters.SetParameterName(this.PropertyName, this.Value.Value2, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            return string.Format("({0} {1}BETWEEN {2} AND {3})", columnName, Not ? "NOT " : string.Empty, propertyName1, propertyName2);
        }

        /// <summary>
        /// 获取或设置区间值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public BetweenValues Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBetweenPredicate" /> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        public bool Not { get; set; }
    }
}
