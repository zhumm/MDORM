using System.Data.Common;
using System.Data;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 提供对数据库的基本操作，连接字符串需要在数据库配置。
    /// </summary>
    public interface IDBHelper
    {

        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="selectSql"></param>
        /// <param name="sqlCount"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        string GetPagingSql(int pageIndex, int pageSize, string selectSql, string sqlCount, string orderBy);


        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTractionand();


        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        DbTransaction BeginTractionand(string connKey);

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="dbTransaction">要回滚的事务</param>
        void RollbackTractionand(DbTransaction dbTransaction);

        /// <summary>
        /// 结束并确认事务
        /// </summary>
        /// <param name="dbTransaction">要结束的事务</param>
        void CommitTractionand(DbTransaction dbTransaction);




        #region DataSet

        /// <summary>
        /// 执行sql语句,ExecuteDataSet 返回DataSet
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        DataSet ExecuteDataSet(string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,ExecuteDataSet 返回DataSet
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        DataSet ExecuteDataSet(string connKey, string commandText, CommandType commandType);


        /// <summary>
        /// 执行sql语句,ExecuteDataSet 返回DataSet
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,ExecuteDataSet 返回DataSet
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        DataSet ExecuteDataSet(string connKey, string commandText, CommandType commandType, params DbParameter[] parameterValues);



        #endregion



        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        int ExecuteNonQuery(string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        int ExecuteNonQuery(string connKey, string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        int ExecuteNonQuery(DbTransaction trans, string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        int ExecuteNonQuery(string connKey, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        int ExecuteNonQuery(DbTransaction trans, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        #endregion





        #region IDataReader

        /// <summary>
        /// 执行sql语句,ExecuteReader 返回IDataReader
        /// </summary>   
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        IDataReader ExecuteReader(string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,ExecuteReader 返回IDataReader
        /// </summary> 
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        IDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,ExecuteReader 返回IDataReader
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>        
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        IDataReader ExecuteReader(string connKey, string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,ExecuteReader 返回IDataReader
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>        
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        IDataReader ExecuteReader(string connKey, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        #endregion


        #region ExecuteScalar

        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        object ExecuteScalar(string commandText, CommandType commandType);


        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        object ExecuteScalar(DbTransaction trans, string commandText, CommandType commandType);

        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        object ExecuteScalar(string connKey, string commandText, CommandType commandType);


        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="connKey">数据库连接字符key</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        object ExecuteScalar(string connKey, string commandText, CommandType commandType, params DbParameter[] parameterValues);

        /// <summary>
        /// 执行sql语句,ExecuteScalar 返回第一行第一列的值
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType"></param>
        /// <param name="parameterValues">参数</param>
        /// <returns></returns>
        object ExecuteScalar(DbTransaction trans, string commandText, CommandType commandType, params DbParameter[] parameterValues);
        #endregion



    }
}