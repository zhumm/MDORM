using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 数据库连接工厂
    /// </summary>
    public class SqlConnectionFactory
    {
        private DBType _dbType = DBType.SqlServer;

        private string _conStr = string.Empty;
        private SqlConnectionFactory()
        {
            string tempDbType = PubConstant.DbType;
            switch (tempDbType)
            {
                case "mssql":
                    _dbType = DBType.SqlServer;
                    break;
                case "mysql":
                    _dbType = DBType.MySql;
                    break;
                case "oracle":
                    _dbType = DBType.Oracle;
                    break;
                case "sqlce":
                    _dbType = DBType.SqlCe;
                    break;
                case "sqlite":
                    _dbType = DBType.SQLite;
                    break;
            }
            _conStr = PubConstant.ConnectionString;
        }

        /// <summary>
        /// 根据配置创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateSqlConnection()
        {
            IDbConnection connection = null;
            switch (_dbType)
            {
                case DBType.SqlServer:
                    connection = new System.Data.SqlClient.SqlConnection(_conStr);
                    break;
                case DBType.MySql:
                    connection = new MySql.Data.MySqlClient.MySqlConnection(_conStr);
                    break;
                case DBType.Oracle:
                    connection = new Oracle.DataAccess.Client.OracleConnection(_conStr);
                    break;
                case DBType.SqlCe:
                    connection = new System.Data.SqlServerCe.SqlCeConnection(_conStr);
                    break;
                case DBType.SQLite:
                    connection = new System.Data.SQLite.SQLiteConnection(_conStr);
                    break;
            }
            return connection;
        }

        /// <summary>
        /// 工厂模式 根据配置创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateSqlCon()
        {
            IDbConnection connection = null;
            string conStr = PubConstant.ConnectionString;
            switch (PubConstant.DbType)
            {
                case "mssql":
                    connection = new System.Data.SqlClient.SqlConnection(conStr);
                    break;
                case "mysql":
                    connection = new MySql.Data.MySqlClient.MySqlConnection(conStr);
                    break;
                case "oracle":
                    //connection = new System.Data.OracleConnection(_conStr);
                    break;
                case "sqlce":
                    connection = new System.Data.SqlServerCe.SqlCeConnection(conStr);
                    break;
                case "sqlite":
                    connection = new System.Data.SQLite.SQLiteConnection(conStr);
                    break;
            }
            return connection;
        }
    }
}
