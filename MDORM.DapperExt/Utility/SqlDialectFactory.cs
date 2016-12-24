using MDORM.DapperExt.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt.Utility
{
    public class SqlDialectFactory
    {
        private DBType _dbType = DBType.SqlServer;

        private string _conStr = string.Empty;
        private SqlDialectFactory()
        {
            string tempDbType = PubConstant.DbType;
            switch (tempDbType.ToLower())
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
        public ISqlDialect CreateDialect()
        {
            ISqlDialect sqlDialect = null;
            switch (_dbType)
            {
                case DBType.SqlServer:
                    sqlDialect = new SqlServerDialect();
                    break;
                case DBType.MySql:
                    sqlDialect = new MySqlDialect();
                    break;
                case DBType.Oracle:
                    sqlDialect = new OracleDialect();
                    break;
                case DBType.SqlCe:
                    sqlDialect = new SqlCeDialect();
                    break;
                case DBType.SQLite:
                    sqlDialect = new SqliteDialect();
                    break;
            }
            return sqlDialect;
        }

        /// <summary>
        /// 工厂模式 根据配置创建数据库
        /// </summary>
        /// <returns></returns>
        public static ISqlDialect CreateSqlDialect()
        {
            ISqlDialect sqlDialect = null;
            switch (PubConstant.DbType.ToLower())
            {
                case "mssql":
                    sqlDialect = new SqlServerDialect();
                    break;
                case "mysql":
                    sqlDialect = new MySqlDialect();
                    break;
                case "oracle":
                    sqlDialect = new OracleDialect();
                    break;
                case "sqlce":
                    sqlDialect = new SqlCeDialect();
                    break;
                case "sqlite":
                    sqlDialect = new SqliteDialect();
                    break;
            }
            return sqlDialect;
        }
    }
}
