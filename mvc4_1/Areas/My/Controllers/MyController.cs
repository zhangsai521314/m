using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Areas.My.Controllers
{
    public class MyController : Controller
    {

        #region 验证Global.asax中的去除服务器的查找视图项，不能添加视图
        /// <summary>
        /// 验证Global.asax中的去除服务器的查找视图项，不能添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        public ActionResult f()
        {
            return Content("区域MyController");
        }
    }
}
