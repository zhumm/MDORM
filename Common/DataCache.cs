using System;
using System.Web;
using System.Collections;

namespace Common
{
	/// <summary>
	/// 缓存相关的操作类
	/// </summary>
	public class DataCache
	{
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <returns></returns>
		public static object GetCache(string CacheKey)
		{
            if (string.IsNullOrEmpty(CacheKey))
                return null;
            CacheKey = CacheKey.Trim().ToUpper();
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache.Count <= 0)
                return null;

            return objCache.Get(CacheKey);
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject)
		{
            if (string.IsNullOrEmpty(CacheKey))
                return;
            CacheKey = CacheKey.Trim().ToUpper();
            if (objObject == null)
                return;
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            
			objCache.Insert(CacheKey, objObject);
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration,TimeSpan slidingExpiration )
		{
            if (string.IsNullOrEmpty(CacheKey))
                return;
            CacheKey = CacheKey.Trim().ToUpper();
            if (objObject == null)
                return;

			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject,null,absoluteExpiration,slidingExpiration);
		}
	}
}
