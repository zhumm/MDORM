using System;
using System.Configuration;

namespace MDORM.DapperExt.Utility
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        private static bool? _writeLog;

        /// <summary>
        /// 是否写异常日志，默认false
        /// </summary>
        public static bool WriteLog
        {
            get
            {
                if (!_writeLog.HasValue)
                {
                    string tempValue = GetConfig("WriteLog");
                    bool tempConfig = false;
                    if (bool.TryParse(tempValue, out tempConfig))
                    {
                        _writeLog = tempConfig;
                    }
                    return _writeLog.Value;
                }
                return _writeLog.Value;
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
            {
                return ConfigurationManager.AppSettings[key].Trim();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
