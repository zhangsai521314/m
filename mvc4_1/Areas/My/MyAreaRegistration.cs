using System.Web.Mvc;

namespace mvc4_1.Areas.My
{
    //区域只是为了更好的把控制器和视图分类
    public class MyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "My";//此名字必须和区域文件夹名称保持一致，一遍MVC根据此名字查找到区域文件夹
            }
        }

        /// <summary>
        /// 区域的路由，和RouteConfig配置一样
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {

            #region 此配置也能放到RouteConfig.cs中
            //  routes.MapRoute(
            //    name: "My_default",
            //    url: "My/{controller}/{action}/{id}",
            //    defaults: new { action = "Index", id = UrlParameter.Optional }
            //); 
            #endregion

            context.MapRoute(
                "My_default",
                "My/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
