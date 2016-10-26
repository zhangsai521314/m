using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Filters
{
    /// <summary>
    /// 视图过滤器
    /// </summary>
    public class MyResultFilterattribute : ActionFilterAttribute
    {

        #region 视图加载前执行
        /// <summary>
        /// 视图加载前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("加载视图前过滤器<br/>");


            base.OnResultExecuting(filterContext);
        } 
        #endregion

        #region 视图加载后执行
        /// <summary>
        /// 视图加载后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write("加载视图后过滤器<br/>");

            base.OnResultExecuted(filterContext);
        } 
        #endregion

    }
}