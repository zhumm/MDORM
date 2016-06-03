using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        private static List<Type> _simpleTypes = new List<Type>
                               {
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(long),
                                   typeof(ulong),
                                   typeof(float),
                                   typeof(double),
                                   typeof(decimal),
                                   typeof(bool),
                                   typeof(string),
                                   typeof(char),
                                   typeof(Guid),
                                   typeof(DateTime),
                                   typeof(DateTimeOffset),
                                   typeof(byte[])
                               };

        /// <summary>
        /// 获得属性
        /// </summary>
        /// <param name="lambda">lambda表达式</param>
        /// <returns>
        /// </returns>
        public static MemberInfo GetProperty(LambdaExpression lambda)
        {
            Expression expr = lambda;
            for (; ; )
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;
                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        MemberExpression memberExpression = (MemberExpression)expr;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// 原始的。把对象的所有的属性转化成Dictionary形式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public static IDictionary<string, object> GetObjectValues(object obj)
        //{
        //    IDictionary<string, object> result = new Dictionary<string, object>();
        //    if (obj == null)
        //    {
        //        return result;
        //    }
        //    foreach (var propertyInfo in obj.GetType().GetProperties())
        //    {
        //        string name = propertyInfo.Name;
        //        object value = propertyInfo.GetValue(obj, null);
        //        result[name] = value;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 修改的方法。自动去除掉对象中那些值为空的属性并返回一个键值字典
        /// </summary>
        /// <param name="obj">具体的实体对象</param>
        /// <returns></returns>
        public static IDictionary<string, object> GetObjectValues(object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null)
            {
                return result;
            }
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(obj, null);
                if (value != null)
                {
                    if (value is string)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            result[name] = value;
                        }
                    }
                    else
                    {
                        result[name] = value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 把List中的元素按照分割父拼接起来
        /// </summary>
        /// <param name="list">包含元素的List</param>
        /// <param name="seperator">分隔符，默认为，</param>
        /// <returns></returns>
        public static string AppendStrings(this IEnumerable<string> list, string seperator = ", ")
        {
            return list.Aggregate(
                new StringBuilder(),
                (sb, s) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(s),
                sb => sb.ToString());
        }

        /// <summary>
        /// 是否是简单类型（值类型）
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否</returns>
        public static bool IsSimpleType(Type type)
        {
            Type actualType = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                actualType = type.GetGenericArguments()[0];
            }

            return _simpleTypes.Contains(actualType);
        }

        /// <summary>
        /// 获取参数名称
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="parameterPrefix">参数前缀</param>
        /// <returns></returns>
        public static string GetParameterName(this IDictionary<string, object> parameters, string parameterName, char parameterPrefix)
        {
            return string.Format("{0}{1}_{2}", parameterPrefix, parameterName, parameters.Count);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <param name="parameterPrefix">参数前缀</param>
        /// <returns></returns>
        public static string SetParameterName(this IDictionary<string, object> parameters, string parameterName, object value, char parameterPrefix)
        {
            string name = parameters.GetParameterName(parameterName, parameterPrefix);
            parameters.Add(name, value);
            return name;
        }
    }
}