using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Filters
{
    //ActionFilterAttribute命名空间using System.Web.Mvc;
    //过滤器是一个特性类（Attribute）,所以在方法的头上加此特性则此执行此方法的时候回执行这个，如果加到类上则执行此类中的任何方法则会执行此方法。如果要全局使用则需配置在FilterConfig.cs下
    //事例：filters.Add(new Filters.MyActionFilterAttribute());
    /// <summary>
    /// 方法过滤器
    /// </summary>
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        #region  控制器中的Action执行之前执行
        /// <summary>
        /// 控制器中的Action执行之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            object studentLoginName = filterContext.HttpContext.Session["StudentLoginName"];

            string sctionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;



            #region http://www.cnblogs.com/wayfarer/archive/2011/10/11/2206816.html
            //利用反射判断请求的方法是否有PhoneAttribute特性
            if (filterContext.ActionDescriptor.IsDefined(typeof(Attributes.PhoneAttribute), false))
            {
                filterContext.Result = new ContentResult() { Content = "<br/> 方法被跳过了 <br/>", };
            }
            #endregion

            filterContext.HttpContext.Response.Write("控制器中的Action执行之前执行" + studentLoginName + "<br/>");

            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region 在控制器中的Action执行之后执行
        /// <summary>
        /// 在控制器中的Action执行之后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            object studentPassWord = filterContext.HttpContext.Session["StudentPassWord"];
            filterContext.HttpContext.Response.Write("控制器中的Action执行之后执行" + studentPassWord + "<br/>");
            base.OnActionExecuted(filterContext);
        }

        #endregion

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }//在Result执行之后

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        } //在Result执行之前

    }
}