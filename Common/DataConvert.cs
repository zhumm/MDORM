using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace Common
{
    /// <summary>
    /// 提供数据类型转换
    /// </summary>
    public static class DataConvert
    {
        /// <summary>
        /// 默认加密密钥
        /// </summary>
        /// 创建人：朱明明
        /// 创建时间：2011-12-8 16:19
        private static string DefaultKey = "&MJEmail";

        /// <summary>
        /// 获取配置信息。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AppSetting(string key)
        {
            if (key == string.Empty)
                return string.Empty;
            return ConfigurationManager.AppSettings[key] == null ? string.Empty : ConfigurationManager.AppSettings[key];
        }

        public static DateTime ConvertDate(object @value)
        {
            if (@value == null)
                return DateTime.MinValue;
            DateTime t;
            if (DateTime.TryParse(@value.ToString().Trim(), out t))
                return t;
            else
                return DateTime.MinValue;
        }

        public static byte ConvertByte(object @value)
        {
            if (@value == null)
                return 0;
            byte b;
            if (byte.TryParse(@value.ToString().Trim(), out b))
                return b;
            else
                return 0;
        }

        public static bool ConvertBool(object @value)
        {
            if (@value == null || @value == DBNull.Value)
                return false;
            bool b;
            if (bool.TryParse(@value.ToString().Trim(), out b))
                return b;
            else
                return false;
        }

        public static int ConvertInt(object @value)
        {
            if (@value == null)
                return 0;
            int i;
            if (int.TryParse(@value.ToString().Trim(), out i))
                return i;
            else
                return 0;
        }

        public static decimal ConvertDecimal(object @value)
        {
            if (@value == null)
                return decimal.Zero;
            decimal d;
            if (decimal.TryParse(@value.ToString().Trim(), out d))
                return d;
            else
                return decimal.Zero;
        }

        public static double ConvertDouble(object @value)
        {
            if (@value == null)
                return 0D;
            double d;
            if (double.TryParse(@value.ToString().Trim(), out d))
                return d;
            else
                return 0D;
        }

        public static string ConvertStr(object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString().Trim();
        }

        public static Guid ConvertGuid(object obj)
        {
            if (obj == null)
                return Guid.Empty;
            else
            {
                string str = ConvertStr(obj);
                if (System.Text.RegularExpressions.Regex.IsMatch(str, "^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    return new Guid(str);
                else
                    return Guid.Empty;
            }
        }

        public static string StrRandom(int length)
        {
            System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] buffer1 = new byte[length];
            rng.GetBytes(buffer1);
            rng = null;

            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random rnd = new Random(buffer1[i]);
                int w = rnd.Next(0, str.Length);
                s.Append(str.Substring(w, 1));
            }
            return s.ToString();
        }

        public static string StrRandomNumber(int length)
        {
            System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] buffer1 = new byte[length];
            rng.GetBytes(buffer1);
            rng = null;

            string str = "0123456789";
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                Random rnd = new Random(buffer1[i]);
                int w = rnd.Next(0, str.Length);
                s.Append(str.Substring(w, 1));
            }
            return s.ToString();
        }

        public static string StrLeft(string str, int length)
        {
            if (str == null || str == string.Empty)
                return string.Empty;

            if (length >= str.Length)
                return str;
            return str.Substring(0, length);
        }

        public static string StrTrimLeft(string str, int length)
        {
            if (str == null || str == string.Empty)
                return string.Empty;

            return StrLeft(str.Trim(), length);
        }

        /// <summary>
        /// DES（使用默认的密钥）解密方法
        /// </summary>
        /// <param name="decryptStr">要解密的字符串</param>
        /// <returns></returns>
        /// 创建人：朱明明
        /// 创建时间：2011-12-8 16:22
        public static string DESDeCode(string decryptStr)
        {
            if (decryptStr.Length <= 0)
                return decryptStr;
            else
                return DESDeCode(decryptStr, DefaultKey);
        }

        /// <summary>
        /// DES(利用默认的密钥)加密方法
        /// </summary>
        /// <param name="encryptStr">要加密的字符串</param>
        /// <returns></returns>
        /// 创建人：朱明明
        /// 创建时间：2011-12-8 16:22
        public static string DESEnCode(string encryptStr)
        {
            if (encryptStr.Length <= 0)
                return encryptStr;
            else
                return DESEnCode(encryptStr, DefaultKey);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符</param>
        /// <param name="sKey">密钥</param>
        /// <returns>
        /// 解密后的字符
        /// </returns>
        public static string DESDeCode(string pToDecrypt, string sKey)
        {
            try
            {
                if (string.IsNullOrEmpty(pToDecrypt))
                    return null;
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = System.Convert.FromBase64String(pToDecrypt);
                des.Key = Encoding.ASCII.GetBytes(sKey);
                des.IV = Encoding.ASCII.GetBytes(sKey);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                //if (Encoding.UTF8.GetString(ms.ToArray()).Length >= 2)
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch
            {
                return pToDecrypt;
            }
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt">待加密字符</param>
        /// <param name="sKey">密钥</param>
        /// <returns>
        /// 加密后的字符
        /// </returns>
        public static string DESEnCode(string pToEncrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToEncrypt))
                return null;
            pToEncrypt = pToEncrypt.ToLower();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            des.Key = Encoding.ASCII.GetBytes(sKey);
            des.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return System.Convert.ToBase64String(ms.ToArray());
        }

        //#region 不用的
        ///// <summary>
        ///// 解密方法
        ///// </summary>
        ///// <param name="pToDecrypt"></param>
        ///// <param name="sKey"></param>
        ///// <returns></returns>
        //private static string DESDeCode(string pToDecrypt, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
        //    for (int x = 0; x < (pToDecrypt.Length / 2); x++)
        //    {
        //        int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 0x10);
        //        inputByteArray[x] = (byte)i;
        //    }
        //    des.Key = Encoding.ASCII.GetBytes(sKey);
        //    des.IV = Encoding.ASCII.GetBytes(sKey);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    return Encoding.Default.GetString(ms.ToArray());
        //}

        ///// <summary>
        ///// 加密方法
        ///// </summary>
        ///// <param name="pToEncrypt"></param>
        ///// <param name="sKey"></param>
        ///// <returns></returns>
        //private static string DESEnCode(string pToEncrypt, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray = Encoding.GetEncoding("UTF-8").GetBytes(pToEncrypt);
        //    des.Key = Encoding.ASCII.GetBytes(sKey);
        //    des.IV = Encoding.ASCII.GetBytes(sKey);
        //    MemoryStream ms = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    foreach (byte b in ms.ToArray())
        //        ret.AppendFormat("{0:X2}", b);
        //    ret.ToString();
        //    return ret.ToString();
        //}
        //#endregion

        /// <summary>
        /// MD5 加密类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncryptByMD5(string data)
        {
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] temp = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                string crydata = string.Empty;
                foreach (byte b in temp)
                    crydata += b.ToString("X2");
                return crydata;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int CompareDateTime(DateTime dtStart, DateTime dtEnd, int hour)
        {
            DateTime dt1 = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, dtStart.Hour, dtStart.Minute, 0);
            DateTime dt2 = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, dtEnd.Hour, dtEnd.Minute, 0);

            return dt1.AddHours(hour).CompareTo(dt2);
        }

        public static bool FileIsImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return false; }
            if (!Path.HasExtension(fileName)) { return false; }
            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext)) { return false; }
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^(.bmp|.dib|.jpg|.jpeg|.jpe|.jfif|.gif|.tiff|.tif|.png|.ico)$");

            return reg.IsMatch(ext);
        }
    }
}
