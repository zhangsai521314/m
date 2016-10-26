using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Filters
{
    /// <summary>
    /// 所有程序没有try的错误，不包括404错误。此方法优先于配置中的customErrors节点配置的服务器内部的错误页
    /// </summary>
    public class MyHandleErrorAttibute : HandleErrorAttribute
    {

        #region 错误处理
        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.Result = new RedirectResult("~/Htmls/AttributeError.html");

            //标记异常处理完毕，屏蔽程序集的处理
            filterContext.ExceptionHandled = true;

            //base.OnException(filterContext);
        }
        #endregion

    }
}