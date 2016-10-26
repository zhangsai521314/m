using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace ZS.Framework.Cache
{
    public abstract class CacheUtility
    {
        #region 获取缓存依赖
        /// <summary>
        /// 获取缓存依赖
        /// </summary>
        public static CacheDependency GetCacheDependency(string fileName)
        {
            CacheDependency cd = null;
            if (CacheConfiguration.EnableCaching)
                cd = new CacheDependency(fileName);
            return cd;
        }
        #endregion

        #region 获取数据库缓存依赖
        /// <summary>
        /// 获取数据库缓存依赖
        /// </summary>
        public static SqlCacheDependency GetSqlCacheDependency(SqlCommand command)
        {
            SqlCacheDependency cd = null;
            if (CacheConfiguration.EnableCaching)
                cd = new SqlCacheDependency(command);
            return cd;
        }
        #endregion

        #region 添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>
        /// <param name="onRemoveCallback"></param>
        public static void InsertCache(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            if (CacheConfiguration.EnableCaching)
                HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
        #endregion

        #region 添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="absoluteExpHours">固定过期时间(小时)</param>
        /// <param name="slidingExpHours">动态过期时间(小时)</param>
        public static void InsertCache(string key, object value, CacheDependency dependencies, int absoluteExpHours, int slidingExpHours)
        {
            if (absoluteExpHours > 0)
                slidingExpHours = 0;
            else
                slidingExpHours = (slidingExpHours <= 0 ? CacheConfiguration.CacheSlidingExpiration : slidingExpHours);

            InsertCache(key, value, dependencies,
                (absoluteExpHours > 0 ? DateTime.Now.AddHours(absoluteExpHours) : System.Web.Caching.Cache.NoAbsoluteExpiration),
                (slidingExpHours > 0 ? TimeSpan.FromHours(slidingExpHours) : System.Web.Caching.Cache.NoSlidingExpiration), CacheItemPriority.Normal, null);
        }
        #endregion
    }
}
