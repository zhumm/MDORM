using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MDORM.DapperExt.Mapper;
using MDORM.DapperExt.Sql;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 条件谓词
    /// </summary>
    public static class Predicates
    {
        /// <summary>
        /// 创建一个新的IFieldPrediate的工厂方法
        /// 比如WHERE FistName = 'Foo'
        /// Factory method that creates a new IFieldPredicate predicate: [FieldName] [Operator] [Value]. 
        /// Example: WHERE FirstName = 'Foo'
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">An expression that returns the left operand [FieldName].</param>
        /// <param name="op">比较操作</param>
        /// <param name="value">查询谓词内容</param>
        /// <param name="not">Effectively inverts the comparison operator. Example: WHERE FirstName &lt;&gt; 'Foo'.</param>
        /// <returns>An instance of IFieldPredicate.</returns>
        public static IFieldPredicate Field<T>(Expression<Func<T, object>> expression, Operator op, object value, bool not = false) where T : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new FieldPredicate<T>
                       {
                           PropertyName = propertyInfo.Name,
                           Operator = op,
                           Value = value,
                           Not = not
                       };
        }

        /// <summary>
        /// Factory method that creates a new IPropertyPredicate predicate: [FieldName1] [Operator] [FieldName2]
        /// Example: WHERE FirstName = LastName
        /// </summary>
        /// <typeparam name="T">The type of the entity for the left operand.</typeparam>
        /// <typeparam name="T2">The type of the entity for the right operand.</typeparam>
        /// <param name="expression">An expression that returns the left operand [FieldName1].</param>
        /// <param name="op">The comparison operator.</param>
        /// <param name="expression2">An expression that returns the right operand [FieldName2].</param>
        /// <param name="not">Effectively inverts the comparison operator. Example: WHERE FirstName &lt;&gt; LastName </param>
        /// <returns>An instance of IPropertyPredicate.</returns>
        public static IPropertyPredicate Property<T, T2>(Expression<Func<T, object>> expression, Operator op, Expression<Func<T2, object>> expression2, bool not = false)
            where T : class
            where T2 : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            PropertyInfo propertyInfo2 = ReflectionHelper.GetProperty(expression2) as PropertyInfo;
            return new PropertyPredicate<T, T2>
                       {
                           PropertyName = propertyInfo.Name,
                           PropertyName2 = propertyInfo2.Name,
                           Operator = op,
                           Not = not
                       };
        }

        /// <summary>
        /// 创建IPredicateGroup 谓词的工厂方法
        /// 谓词组可以和其他的谓词组连接
        /// Factory method that creates a new IPredicateGroup predicate.
        /// Predicate groups can be joined together with other predicate groups.
        /// </summary>
        /// <param name="op">The grouping operator to use when joining the predicates (AND / OR).</param>
        /// <param name="predicate">A list of predicates to group.</param>
        /// <returns>An instance of IPredicateGroup.</returns>
        public static IPredicateGroup Group(GroupOperator op, params IPredicate[] predicate)
        {
            return new PredicateGroup
                       {
                           Operator = op,
                           Predicates = predicate
                       };
        }

        /// <summary>
        /// 创建IExistsPredicate谓词的工厂方法
        /// Factory method that creates a new IExistsPredicate predicate.
        /// </summary>
        /// <typeparam name="TSub">类型</typeparam>
        /// <param name="predicate">添加谓语</param>
        /// <param name="not">if set to <c>true</c> [not].</param>
        /// <returns></returns>
        public static IExistsPredicate Exists<TSub>(IPredicate predicate, bool not = false)
            where TSub : class
        {
            return new ExistsPredicate<TSub>
                       {
                           Not = not,
                           Predicate = predicate
                       };
        }

        /// <summary>
        /// 创建IBetweenPredicate的工厂方法
        /// Factory method that creates a new IBetweenPredicate predicate. 
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <param name="values">值</param>
        /// <param name="not">if set to <c>true</c> [not].</param>
        /// <returns></returns>
        public static IBetweenPredicate Between<T>(Expression<Func<T, object>> expression, BetweenValues values, bool not = false)
            where T : class
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new BetweenPredicate<T>
                       {
                           Not = not,
                           PropertyName = propertyInfo.Name,
                           Value = values
                       };
        }

        /// <summary>
        /// 创建排序的工厂方法
        /// Factory method that creates a new Sort which controls how the results will be sorted.
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns></returns>
        public static ISort Sort<T>(Expression<Func<T, object>> expression, bool ascending = true)
        {
            PropertyInfo propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return new Sort
                       {
                           PropertyName = propertyInfo.Name,
                           Ascending = ascending
                       };
        }
    }

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


    /// <summary>
    /// 基础谓词接口
    /// </summary>
    public interface IBasePredicate : IPredicate
    {

        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// 属性名称
        /// </value>
        string PropertyName { get; set; }
    }


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


    /// <summary>
    /// 比较谓词接口
    /// </summary>
    public interface IComparePredicate : IBasePredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        Operator Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IComparePredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }

    /// <summary>
    /// 比较谓词具体实现
    /// </summary>
    public abstract class ComparePredicate : BasePredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public Operator Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ComparePredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        public bool Not { get; set; }

        /// <summary>
        /// 获得操作字符串
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual string GetOperatorString()
        {
            switch (Operator)
            {
                case Operator.Gt:
                    return Not ? "<=" : ">";
                case Operator.Ge:
                    return Not ? "<" : ">=";
                case Operator.Lt:
                    return Not ? ">=" : "<";
                case Operator.Le:
                    return Not ? ">" : "<=";
                case Operator.Like:
                    return Not ? "NOT LIKE" : "LIKE";
                default:
                    return Not ? "<>" : "=";
            }
        }
    }

    /// <summary>
    /// 字段谓词接口
    /// </summary>
    public interface IFieldPredicate : IComparePredicate
    {
        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        object Value { get; set; }
    }

    /// <summary>
    /// 字段谓词的具体实现
    /// </summary>
    /// <typeparam name="T">类型名称</typeparam>
    public class FieldPredicate<T> : ComparePredicate, IFieldPredicate
        where T : class
    {
        /// <summary>
        /// 获取或设置值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Operator must be set to Eq for Enumerable types</exception>
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            if (Value == null)
            {
                return string.Format("({0} IS {1}NULL)", columnName, Not ? "NOT " : string.Empty);
            }

            if (Value is IEnumerable && !(Value is string))
            {
                if (Operator != Operator.Eq)
                {
                    throw new ArgumentException("Operator must be set to Eq for Enumerable types");
                }

                List<string> @params = new List<string>();
                foreach (var value in (IEnumerable)Value)
                {
                    string valueParameterName = parameters.SetParameterName(this.PropertyName, value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
                    @params.Add(valueParameterName);
                }

                string paramStrings = @params.Aggregate(new StringBuilder(), (sb, s) => sb.Append((sb.Length != 0 ? ", " : string.Empty) + s), sb => sb.ToString());
                return string.Format("({0} {1}IN ({2}))", columnName, Not ? "NOT " : string.Empty, paramStrings);
            }

            string parameterName = parameters.SetParameterName(this.PropertyName, this.Value, sqlGenerator.Configuration.Dialect.ParameterPrefix);
            return string.Format("({0} {1} {2})", columnName, GetOperatorString(), parameterName);
        }
    }

    /// <summary>
    /// 属性谓词几口
    /// </summary>
    public interface IPropertyPredicate : IComparePredicate
    {
        /// <summary>
        /// 获取或设置属性2
        /// </summary>
        /// <value>
        /// The property name2.
        /// </value>
        string PropertyName2 { get; set; }
    }

    /// <summary>
    /// 具体属性谓词类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    public class PropertyPredicate<T, T2> : ComparePredicate, IPropertyPredicate
        where T : class
        where T2 : class
    {
        /// <summary>
        /// 获取或设置属性2
        /// </summary>
        /// <value>
        /// The property name2.
        /// </value>
        public string PropertyName2 { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public override string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string columnName = GetColumnName(typeof(T), sqlGenerator, PropertyName);
            string columnName2 = GetColumnName(typeof(T2), sqlGenerator, PropertyName2);
            return string.Format("({0} {1} {2})", columnName, GetOperatorString(), columnName2);
        }
    }

    /// <summary>
    /// 区间内容
    /// </summary>
    public struct BetweenValues
    {
        /// <summary>
        /// 获取或设置内容1
        /// </summary>
        /// <value>
        /// The value1.
        /// </value>
        public object Value1 { get; set; }

        /// <summary>
        /// 获取或设置内容2
        /// </summary>
        /// <value>
        /// The value2.
        /// </value>
        public object Value2 { get; set; }
    }

    /// <summary>
    /// 区间谓词接口
    /// </summary>
    public interface IBetweenPredicate : IPredicate
    {
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string PropertyName { get; set; }

        /// <summary>
        /// 获取或设置区间值
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        BetweenValues Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IBetweenPredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }

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

    /// <summary>
    /// 比较操作枚举
    /// Comparison operator for predicates.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// Equal to
        /// </summary>
        Eq,

        /// <summary>
        /// 大于
        /// Greater than
        /// </summary>
        Gt,

        /// <summary>
        /// 大于或者等于
        /// Greater than or equal to
        /// </summary>
        Ge,

        /// <summary>
        /// 小于
        /// Less than
        /// </summary>
        Lt,

        /// <summary>
        /// 大于或者等于
        /// Less than or equal to
        /// </summary>
        Le,

        /// <summary>
        /// 模糊（你可以使用 %）
        /// Like (You can use % in the value to do wilcard searching)
        /// </summary>
        Like
    }

    /// <summary>
    /// 谓词组接口
    /// </summary>
    public interface IPredicateGroup : IPredicate
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        GroupOperator Operator { get; set; }

        /// <summary>
        /// 获取或者设置谓语
        /// </summary>
        /// <value>
        /// The predicates.
        /// </value>
        IList<IPredicate> Predicates { get; set; }
    }

    /// <summary>
    /// 合指定的组操作符一起使用的谓词组
    /// Groups IPredicates together using the specified group operator.
    /// </summary>
    public class PredicateGroup : IPredicateGroup
    {
        /// <summary>
        /// 获取或设置操作类型
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public GroupOperator Operator { get; set; }

        /// <summary>
        /// 获取或者设置谓语
        /// </summary>
        /// <value>
        /// The predicates.
        /// </value>
        public IList<IPredicate> Predicates { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            string seperator = Operator == GroupOperator.And ? " AND " : " OR ";
            return "(" + Predicates.Aggregate(new StringBuilder(),
                                        (sb, p) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(p.GetSql(sqlGenerator, parameters)),
                sb =>
                {
                    var s = sb.ToString();
                    if (s.Length == 0) return sqlGenerator.Configuration.Dialect.EmptyExpression; 
                    return s;
                }
                                        ) + ")";
        }
    }

    /// <summary>
    /// 存在谓词接口
    /// </summary>
    public interface IExistsPredicate : IPredicate
    {
        /// <summary>
        /// 获取或设置条件谓语
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        IPredicate Predicate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IExistsPredicate"/> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        bool Not { get; set; }
    }

    /// <summary>
    /// 具体的存在谓语
    /// </summary>
    /// <typeparam name="TSub">The type of the sub.</typeparam>
    public class ExistsPredicate<TSub> : IExistsPredicate
        where TSub : class
    {
        /// <summary>
        /// 获取或设置条件谓语
        /// </summary>
        /// <value>
        /// The predicate.
        /// </value>
        public IPredicate Predicate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IExistsPredicate" /> is not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if not; otherwise, <c>false</c>.
        /// </value>
        public bool Not { get; set; }

        /// <summary>
        /// 获取SQL语句
        /// </summary>
        /// <param name="sqlGenerator">SQL生成接口</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public string GetSql(ISqlGenerator sqlGenerator, IDictionary<string, object> parameters)
        {
            IClassMapper mapSub = GetClassMapper(typeof(TSub), sqlGenerator.Configuration);
            string sql = string.Format("({0}EXISTS (SELECT 1 FROM {1} WHERE {2}))",
                Not ? "NOT " : string.Empty,
                sqlGenerator.GetTableName(mapSub),
                Predicate.GetSql(sqlGenerator, parameters));
            return sql;
        }

        /// <summary>
        /// 获取类映射
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="configuration">配置</param>
        /// <returns>
        /// </returns>
        /// <exception cref="NullReferenceException"></exception>
        protected virtual IClassMapper GetClassMapper(Type type, IDapperExtConfiguration configuration)
        {
            IClassMapper map = configuration.GetMap(type);
            if (map == null)
            {
                throw new NullReferenceException(string.Format("Map was not found for {0}", type));
            }

            return map;
        }
    }

    /// <summary>
    /// 排序接口
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISort"/> is ascending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ascending; otherwise, <c>false</c>.
        /// </value>
        bool Ascending { get; set; }
    }

    /// <summary>
    /// 具体排序类
    /// </summary>
    public class Sort : ISort
    {
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISort" /> is ascending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ascending; otherwise, <c>false</c>.
        /// </value>
        public bool Ascending { get; set; }
    }

    /// <summary>
    /// 用来和其他谓词组连接的操作枚举
    /// Operator to use when joining predicates in a PredicateGroup.
    /// </summary>
    public enum GroupOperator
    {
        /// <summary>
        /// 并且
        /// </summary>
        And,
        /// <summary>
        /// 或者
        /// </summary>
        Or
    }
}