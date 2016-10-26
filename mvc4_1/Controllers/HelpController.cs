using CommonHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc4_1.Controllers
{
    public class HelpController : Controller
    {
        CommonVCodeHelp vcode = new CommonVCodeHelp();

        public ActionResult VCode()
        {
            byte[] arrImg = vcode.GetVCode();

            return File(arrImg, "image/jpeg");
        }

    }
}
