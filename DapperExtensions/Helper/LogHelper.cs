using System;
using System.Configuration;

namespace DapperExtensions.Helper
{
    /// <summary>
    /// 配置文件操作类
    /// </summary>
    internal sealed class LogHelper
    {
        private static bool? _sqlLog;

        /// <summary>
        /// 获取是否记录操作sql
        /// </summary>
        public static bool SqlLog
        {
            get
            {
                if (!_sqlLog.HasValue)
                {
                    string tempValue = GetConfig("sqlLog");
                    bool tempConfig = false;
                    if (bool.TryParse(tempValue, out tempConfig))
                    {
                        _sqlLog = tempConfig;
                    }
                    else
                    {
                        _sqlLog = false;
                    }
                    return _sqlLog.Value;
                }
                return _sqlLog.Value;
            }
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
