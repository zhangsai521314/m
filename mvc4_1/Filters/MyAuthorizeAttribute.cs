using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Filters
{
    /// <summary>
    /// 权限验证过滤器 ，在Action过滤器前执行
    /// </summary>
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        #region 权限验证方法
        /// <summary>
        /// 权限验证方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            if (!(controllerName.Equals("Student", StringComparison.OrdinalIgnoreCase) && actionName.Equals("index", StringComparison.OrdinalIgnoreCase)))
            {
                filterContext.Result = new RedirectResult("~/Student/index");
            }


            //屏蔽分类中的OnAuthorization中的ASP.NET的验证机制
            //base.OnAuthorization(filterContext);
        }
        #endregion
    }
}