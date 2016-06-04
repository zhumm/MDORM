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
}