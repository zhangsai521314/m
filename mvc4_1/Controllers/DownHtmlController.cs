using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonHelp;

namespace mvc4_1.Controllers
{
    public class DownHtmlController : Controller
    {
        //
        // GET: /DownHtml/

        public ActionResult Index()
        {
            string postData = string.Format("user_code=11")    ;
            string sendValue = CommonHttpHelp.PostData("http://www.166zw.com/152/152786/index.shtml", postData, null);
            return View();
        }
    }
}
