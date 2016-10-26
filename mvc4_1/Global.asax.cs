using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CommonHelp;
using System;

namespace mvc4_1
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            PureViewEngines();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            SqlDependency.Start(CommonConfigHelp.Config);//用于Sql依赖缓存（   
            //使用sql依赖缓存之前需启用ServiceBroker。1检测是否已启用Select  DATABASEpRoPERTYEX('数据库名称','IsBrokerEnabled')。
            //2: ALTER DATABASE DBname SET NEW_BROKER WITH ROLLBACK IMMEDIATE; ALTER DATABASE DBname SET ENABLE_BROKER;
            // 启用期之前请退出sql重新打开或者关闭SQLServer的Agent代理功能 ）

        }

        #region 只保留以下视图引擎减少服务器查找项
        /// <summary>
        /// 只保留以下视图引擎减少服务器查找项
        /// </summary>
        void PureViewEngines()
        {
            //System.Web.Razor.RazorCodeLanguage.Languages.Remove("vbhtml");
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new RazorViewEngine());


            //移除 集合中 默认添加的 WebFormViewEngine
            ViewEngines.Engines.RemoveAt(0);


            RazorViewEngine razor = ViewEngines.Engines[0] as RazorViewEngine;


            #region 保留的视图
            razor.FileExtensions = new string[] { "cshtml" };
            razor.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.aspx", "~/Areas/{2}/Views/Shared/{0}.aspx" };
            razor.AreaMasterLocationFormats = new string[]{
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml" ,
                 "~/Areas/{2}/Views/{1}/{0}.aspx",
                 "~/Areas/{2}/Views/Shared/{0}.aspx"
            };

            razor.AreaPartialViewLocationFormats = new string[]{
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml",
                 "~/Areas/{2}/Views/{1}/{0}.aspx",
                 "~/Areas/{2}/Views/Shared/{0}.aspx"
            };

            razor.MasterLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml",
                 "~/Views/{1}/{0}.aspx",
                 "~/Views/Shared/{0}.aspx"

            };

            razor.PartialViewLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml",
                 "~/Views/{1}/{0}.aspx",
                 "~/Views/Shared/{0}.aspx"
            };

            razor.ViewLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml",
                 "~/Views/{1}/{0}.aspx",
                 "~/Views/Shared/{0}.aspx"
            };
            #endregion
        }
        #endregion

        #region 为管道的第一个事件注册方法，注册机制的方法名规则： Application_事件名称()
        /// <summary>
        ///  为管道的第一个事件注册方法，注册机制的方法名规则： Application_事件名称()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpApplication obj = sender as HttpApplication;
        //   obj.Context.Response.Write(" 我是在Global.asax中为第一个管道事件(Application_BeginRequest)添加的方法 <br/>");
        //}
        #endregion

        #region 最后一个管道事件
        /// <summary>
        /// 最后一个管道事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Application_EndRequest(object sender, EventArgs e)
        {

        }
        #endregion



    }
}