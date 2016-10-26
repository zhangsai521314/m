using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZS.Framework.Cache
{
    public abstract class CacheConfiguration
    {
        #region 获取是否启用数据库依赖缓存
        /// <summary>
        /// 获取是否启用数据库依赖缓存
        /// </summary>
        public static bool EnableCaching
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaching"]); }
        }
        #endregion

        #region 缓存自动过期时间
        /// <summary>
        /// 缓存自动过期时间
        /// </summary>
        public static int CacheSlidingExpiration
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["CacheSlidingExpiration"]); }
        }
        #endregion

        #region 缓存的失效时间(小时)
        /// <summary>
        /// 基础数据信息缓存过期时间
        /// </summary>
        public enum BaseDataCacheTime
        {
            BaseDataTime = 72,
        }
        #endregion

        #region 缓存key集合
        /// <summary>
        /// Framework基础数据模版key
        /// </summary>
        public const string CHY_BASEDATA_KEY = "chy_basedata_{0}";
        /// <summary>
        /// 基础数据模版key
        /// </summary>
        public const string BS_BASEDATA_KEY = "bs_basedata_{0}";
        /// <summary>
        /// 第3方数据模板key
        /// </summary>
        public const string THIRD_DATA_KEY = "third_data_{0}";
        #endregion
    }
}
