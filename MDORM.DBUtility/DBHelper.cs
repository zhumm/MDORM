using System;
using System.Collections.Generic; // 引用
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MDORM.Dapper;
using MDORM.Common;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 数据库操作帮助类
    /// </summary>
    public static class DBHelper
    {
        #region 通用操作
        /// <summary>
        /// 尝试打开链接
        /// </summary>
        /// <param name="DbCon">数据库连接</param>
        public static void TryOpen(IDbConnection DbCon)
        {
            if (DbCon.State == ConnectionState.Closed)
            {
                DbCon.Open();
            }
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="t">发生异常的源类型</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(Type t,Exception ex)
        {
            if (ConfigHelper.WriteLog)
            {
                LogHelper.WriteLog(t,ex);
            }
        }

        #endregion

        #region 常用操作
        //-------------------------------
        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">需要执行的sql语句</param>
        /// <param name="param">参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSql(string sql, object param)
        {
            try
            {
                using (var con = SqlConnectionFactory.CreateSqlCon())
                {
                    int i = con.Execute(sql, param);
                    return i;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 执行SQL，返回第一行第一个元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteSql<T>(string sql, object param)
        {
            try
            {
                using (var con = SqlConnectionFactory.CreateSqlCon())
                {
                    T result = con.Query<T>(sql, param).FirstOrDefault<T>();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 执行SQL，返回数据实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IList<T> ExecuteSql<T>(string sql, object param,out int recordCount)
        {
            recordCount = 0;
            try
            {
                using (var con = SqlConnectionFactory.CreateSqlCon())
                {
                    IEnumerable<T> result = con.Query<T>(sql, param);
                    recordCount = result.Count();
                    return result.ToList<T>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行无返回结果集的存储过程  
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecuteSP(string proName, object param)
        {
            try
            {
                using (var con = SqlConnectionFactory.CreateSqlCon())
                {
                    int result = con.Execute(proName, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 执行存储过程,并返回结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IList<T> ExecuteSP<T>(string proName, object param, out int recordCount)
        {
            recordCount = 0;
            try
            {
                using (var con = SqlConnectionFactory.CreateSqlCon())
                {
                    IEnumerable<T> result = con.Query<T>(proName, param, commandType: CommandType.StoredProcedure);
                    return result.ToList<T>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
