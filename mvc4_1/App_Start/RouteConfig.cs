using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvc4_1
{
    /// <summary>
    /// 路由控制
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //    routes.MapRoute(
            //    name: "s",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Razor语法事例", action = "TeXing" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AStart", action = "Login", id = UrlParameter.Optional }
                // constraints: new { controller = "AStart" } //注意：这样S不区分大小写
            );

            routes.MapRoute(
                name: "RouteYueShuOne",
                url: "{controller}/{action}/{id}",
                constraints: new { id = @"\d*" }, //使用正则表达式建立路由约束
                defaults: new { controller = "Razor语法事例", action = "RazorAdd", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "RouteYueShuTwo",
               url: "{controller}/{action}/{name}",
               constraints: new { name = "[a-z]+" },  //使用正则表达式建立路由约束
               defaults: new { controller = "Razor语法事例", action = "RazorAdd", name = UrlParameter.Optional }

           );
        }
    }
}