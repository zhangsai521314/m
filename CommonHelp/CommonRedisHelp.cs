using NServiceKit.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonHelp;

namespace CommonHelp
{
    //客户端方法使用说明：http://www.cnblogs.com/kissdodog/p/3572084.html
    public class RedisCommon
    {
        #region 获取Redis程序池
        /// <summary>
        /// 获取Redis程序池
        /// </summary>
        /// <returns></returns>
        private static PooledRedisClientManager CreateRedisManager(int initialDB = 0)
        {
            string readWriteHosts = CommonConfigHelp.MasterRedisIP;
            string readOnlyHosts = CommonConfigHelp.SlaveRedisIP;
            //建立读写分享的数据缓存,支持读写分离，均衡负载
            return new PooledRedisClientManager(new string[] { readWriteHosts }, new string[] { readOnlyHosts }, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 5,//“写”链接池数
                MaxReadPoolSize = 5,//“读”链接池数
                AutoStart = true
            }, initialDB, 50, 5);


        }
        #endregion

        #region 判断Redis中是否包含Key
        /// <summary>
        /// 判断Redis中是否包含Key
        /// </summary>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetReadOnlyClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.SlavePWD))
                    {
                        rds.Password = CommonConfigHelp.SlavePWD;
                    }
                    return rds.ContainsKey(key);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return false;
            }


        }
        #endregion
        #region 指定的Key，将值加上指定值(仅整型有效)
        /// <summary>
        /// 指定的Key，将值加上指定值(仅整型有效)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int IncrementValueBy(string key, int count)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetReadOnlyClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.SlavePWD))
                    {
                        rds.Password = CommonConfigHelp.SlavePWD;
                    }
                    return (int)rds.IncrementValueBy(key, count);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return 0;
            }

        }
        #endregion
        #region 读取数据
        /// <summary>
        /// 读取数据
        /// </summary>
        public static T Get<T>(string key)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetReadOnlyClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.SlavePWD))
                    {
                        rds.Password = CommonConfigHelp.SlavePWD;
                    }
                    return rds.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return default(T);
            }

        }
        #endregion

        #region 添加数据
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">键的名称</param>
        /// <param name="val">存放的值</param>
        /// <param name="IsCache">是否存入Cache中</param>
        /// <returns></returns>
        public static bool Set<T>(string key, T val, bool IsCache)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.MasterPWD))
                    {
                        rds.Password = CommonConfigHelp.MasterPWD;
                    }
                    return rds.Set<T>(key, val);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return false;
            }


        }
        #endregion
        #region 删除数据根据键
        /// <summary>
        /// 删除数据根据键
        /// </summary>
        public static bool Remove(string key)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.MasterPWD))
                    {
                        rds.Password = CommonConfigHelp.MasterPWD;
                    }
                    return rds.Remove(key);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return false;
            }

        }
        #endregion
        #region 根据传入的多个key移除多条记录
        /// <summary>
        ///  根据传入的多个key移除多条记录
        /// </summary>
        /// <param name="keys"></param>
        public static void RemoveAll(string[] keys)
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.MasterPWD))
                    {
                        rds.Password = CommonConfigHelp.MasterPWD;
                    }
                    rds.RemoveAll(keys);
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
            }



        }
        #endregion
        #region 清除所有数据
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public static void FlushDb()
        {
            try
            {
                using (IRedisClient rds = CreateRedisManager().GetClient())
                {
                    if (!string.IsNullOrEmpty(CommonConfigHelp.MasterPWD))
                    {
                        rds.Password = CommonConfigHelp.MasterPWD;
                    }
                    rds.FlushDb();
                }
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "");
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
            }

        }
        #endregion
    }

}
