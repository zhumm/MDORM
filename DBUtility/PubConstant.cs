using System;
using System.Configuration;
namespace DBUtility
{
    /// <summary>
    /// 公共常量
    /// </summary>
    public class PubConstant
    {
        private static string _connectionString = string.Empty;

        private static string _dbType = string.Empty;

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static string DbType
        {
            get
            {
                if (string.IsNullOrEmpty(_dbType))
                {
                    _dbType = GetConfig("DBType");
                }
                return _dbType;
            }
        }
        /// <summary>
        /// 获取默认连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = GetConfig("ConnectionString");
                    string ConStringEncrypt = GetConfig("ConStringEncrypt");
                    if (ConStringEncrypt == "true")
                    {
                        _connectionString = DESEncrypt.Decrypt(_connectionString);
                    }
                }

                return _connectionString;
            }
        }

        /// <summary>
        /// 得到config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns></returns>
        public static string GetCustomConStr(string configName)
        {
            string connectionString = GetConfig(configName);
            string ConStringEncrypt = GetConfig("ConStringEncrypt");
            if (ConStringEncrypt == "true")
            {
                connectionString = DESEncrypt.Decrypt(connectionString);
            }

            return connectionString;
        }

        /// <summary>
        /// 获取Config配置的值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// 创建人：朱明明
        /// 创建时间：2014-6-16 9:37
        private static string GetConfig(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            key = key.Trim();
            string[] temp = ConfigurationManager.AppSettings.AllKeys;
            bool isContain = false;
            foreach (string one in temp)
            {
                if (one.Equals(key, StringComparison.OrdinalIgnoreCase))
                    isContain = true;
            }
            if (isContain)
                return ConfigurationManager.AppSettings[key].Trim();
            else
                return string.Empty;
        }
    }
}
