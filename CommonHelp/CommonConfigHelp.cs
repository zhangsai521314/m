using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CommonHelp
{
    public class CommonConfigHelp
    {

        #region  私有变量

        static string _config;
        static string _domainName;
        static string _masterRedisIP;
        static string _masterPWD;
        static string _slaveRedisIP;
        static string _slavePWD;


        #endregion


        static CommonConfigHelp()
        {
            if (string.IsNullOrEmpty(_config))
            {
                _config = ConfigurationManager.ConnectionStrings["config"].ToString();
            }
            if (string.IsNullOrEmpty(_domainName))
            {
                _domainName = ConfigurationManager.AppSettings["DomainName"].ToString();
            }
            if (string.IsNullOrEmpty(_masterRedisIP))
            {
                _masterRedisIP = ConfigurationManager.AppSettings["MasterRedisIP"].ToString();
            }
            if (string.IsNullOrEmpty(_slaveRedisIP))
            {
                _slaveRedisIP = ConfigurationManager.AppSettings["MasterPWD"].ToString();
            } if (string.IsNullOrEmpty(_slaveRedisIP))
            {
                _slaveRedisIP = ConfigurationManager.AppSettings["SlaveRedisIP"].ToString();
            }
            if (string.IsNullOrEmpty(_slavePWD))
            {
                _slavePWD = ConfigurationManager.AppSettings["SlavePWD"].ToString();
            }
        }


        public static string Config
        {
            get { return _config; }
        }

        /// <summary>
        /// 短链接域名
        /// </summary>
        public static string DomainName
        {
            get { return _domainName; }
        }


        public static string MasterRedisIP
        {
            get { return _masterRedisIP; }
            set { _masterRedisIP = value; }
        }


        public static string MasterPWD
        {
            get { return _masterPWD; }
            set { _masterPWD = value; }
        }


        public static string SlaveRedisIP
        {
            get { return _slaveRedisIP; }
            set { _slaveRedisIP = value; }
        }

        public static string SlavePWD
        {
            get { return _slavePWD; }
            set { _slavePWD = value; }
        }


        #region 用户登录票证过期时间
        /// <summary>
        /// 用户登录票证过期时间(天)
        /// </summary>
        public static int LoginStateExpires
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["LoginStateExpires"]); }
        }
        #endregion


    }
}
