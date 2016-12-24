using MDORM.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MDORM.DapperExt
{
    public static class Lambda2Sql
    {
        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }
        public static string Where<T>(this T entity, Expression<Func<T, bool>> func)
        {
            DynamicParameters parameters = new DynamicParameters();
			var binaryExpression = func.Body as BinaryExpression;
        	if (binaryExpression != null)
            {
                BinaryExpression be = binaryExpression;
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType,parameters);
            }
			else
			{
				return string.Empty;
			}
        }
        
		/// <summary>
		/// 表达式解析器
		/// </summary>
		/// <param name="left">左边表达式</param>
		/// <param name="right">右边表达式</param>
		/// <param name="type">中间的操作类型</param>
		/// <returns></returns>
        static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type,DynamicParameters parameters)
        {
            string sb = "(";
            //先处理左边
            sb += ExpressionRouter(left, parameters);
            //再处理类型
            sb += ExpressionTypeCast(type);
            //最后处理右边
            string tmpStr = ExpressionRouter(right, parameters);
            if (tmpStr == "NULL")
            {
				if (sb.EndsWith("="))
				{	
					sb = sb.Substring(0, sb.Length - 2) + " IS NULL ";
				}
				else if (sb.EndsWith("<>"))
				{
					sb = sb.Substring(0, sb.Length - 2) + " IS NOT NULL ";
				}
            }
            else
            {   
            	sb += tmpStr;
            }
            return sb += ")";
        }

        /// <summary>
        /// 表达式（递归）解析
        /// </summary>
        /// <param name="exp">表达式</param>
        /// <returns></returns>
        static string ExpressionRouter(Expression exp,DynamicParameters parameters)
        {
            string sb = string.Empty;
			var binaryExpression = exp as BinaryExpression;
			if (binaryExpression != null)
			{
				BinaryExpression be = binaryExpression;
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType,parameters);
			} 
			else
			{
				var memberExpression = exp as MemberExpression;
				if (memberExpression != null) 
				{
					MemberExpression me = memberExpression;
					return me.Member.Name;
				} 
				else
				{
					var newArrayExpression = exp as NewArrayExpression;
					if (newArrayExpression != null) 
					{
						NewArrayExpression ae = newArrayExpression;
						StringBuilder tmpstr = new StringBuilder();
						foreach (Expression ex in ae.Expressions)
						{
							tmpstr.Append(ExpressionRouter(ex, parameters));
							tmpstr.Append(",");
						}
						return tmpstr.ToString(0, tmpstr.Length - 1);
					} 
					else
					{
						var methodCallExpression = exp as MethodCallExpression;
						if (methodCallExpression != null) 
						{
							MethodCallExpression mce = methodCallExpression;
                            if (mce.Method.Name == "Like")
                            {
                                return string.Format("({0} like {1})", ExpressionRouter(mce.Arguments[0], parameters), ExpressionRouter(mce.Arguments[1], parameters));
                            }
                            else if (mce.Method.Name == "NotLike")
                            {
                                return string.Format("({0} Not like {1})", ExpressionRouter(mce.Arguments[0], parameters), ExpressionRouter(mce.Arguments[1], parameters));
                            }
                            else if (mce.Method.Name == "In")
                            {
                                return string.Format("{0} In ({1})", ExpressionRouter(mce.Arguments[0], parameters), ExpressionRouter(mce.Arguments[1], parameters));
                            }
                            else if (mce.Method.Name == "NotIn")
                            {
                                return string.Format("{0} Not In ({1})", ExpressionRouter(mce.Arguments[0], parameters), ExpressionRouter(mce.Arguments[1], parameters));
                            }
                        } 
						else
						{
							var constantExpression = exp as ConstantExpression;
							if (constantExpression != null) 
							{
								ConstantExpression ce = constantExpression;
								if (ce.Value == null)
								{
									return "NULL";
								}
								else if (ce.Value is ValueType)
								{
									return ce.Value.ToString();
								}
								else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
								{
									return string.Format("'{0}'", ce.Value.ToString());
								}
							} 
							else
							{
								var unaryExpression = exp as UnaryExpression;
								if (unaryExpression != null)
								{
									UnaryExpression ue = unaryExpression;
                                    return ExpressionRouter(ue.Operand,  parameters);
								}
							}
						}
					}
				}
			}
            return null;
        }

        /// <summary>
        /// 表达式操作类型解析
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <returns></returns>
        static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.NotEqual:
                    return " <> ";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return " + ";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return " - ";
                case ExpressionType.Divide:
                    return " / ";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return " * ";
                default:
                    return null;
            }
        }
    }
}
