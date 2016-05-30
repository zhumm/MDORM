using System;
using System.Configuration;

namespace Common
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        private static bool? _writeLog;

        /// <summary>
        /// 是否写异常日志
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
                    else
                    {
                        _writeLog = false;
                    }
                    return _writeLog.Value;
                }
                return _writeLog.Value;
            }
        }

        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string CacheKey = "AppSettings-" + key;
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetConfig(key);
                    if (objModel != null)
                    {
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
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
