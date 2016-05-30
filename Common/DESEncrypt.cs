using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Common
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public static class DESEncrypt
    {
        /// <summary>
        /// 默认密钥
        /// </summary>
        private static string defaultKey = "be@MDORM";

        /// <summary>
        /// DES（使用默认的密钥）解密方法
        /// </summary>
        /// <param name="decryptStr">要解密的字符串</param>
        /// <returns></returns>
        /// 创建人：朱明明
        /// 创建时间：2011-12-8 16:22
        public static string Decrypt(string decryptStr)
        {
            if (decryptStr.Length <= 0)
                return decryptStr;
            else
                return Decrypt(decryptStr, defaultKey);
        }

        /// <summary>
        /// DES(利用默认的密钥)加密方法
        /// </summary>
        /// <param name="encryptStr">要加密的字符串</param>
        /// <returns></returns>
        /// 创建人：朱明明
        /// 创建时间：2011-12-8 16:22
        public static string Encrypt(string encryptStr)
        {
            if (encryptStr.Length <= 0)
                return encryptStr;
            else
                return Encrypt(encryptStr, defaultKey);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符</param>
        /// <param name="sKey">密钥</param>
        /// <returns>
        /// 解密后的字符
        /// </returns>
        public static string Decrypt(string pToDecrypt, string sKey)
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
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToEncrypt))
                return null;
            //pToEncrypt = pToEncrypt.ToLower();
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
    }
}
