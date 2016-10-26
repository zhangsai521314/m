using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MODEl;
using CommonHelp;
using System.Web.Security;

namespace mvc4_1.Controllers
{
    public class AStartController : Controller
    {
        private static BLL.BllStudent bllStudent = new BLL.BllStudent();
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && Session["userInfo"] != null)
            {
                return Redirect("/Student/Index");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Student modelStudent)
        {
            if (User.Identity.IsAuthenticated && Session["userInfo"] != null)
            {
                return Redirect("/Student/Index");
            }
            Student model = bllStudent.GetListByWhere(d => d.LoginName.Equals(modelStudent.LoginName)).FirstOrDefault();
            if (model != null)
            {
                if (model.PassWord.Equals(modelStudent.PassWord))
                {
                    CommonWebHelp.SaveAuthTicket(model, true);
                    Session["userInfo"] = model;
                    return Redirect("/Student/Index");
                }
            }
            return Redirect("/AStart/Login");
        }


        public void Exeit(int id)
        {
            FormsAuthentication.SignOut(); //此句后面不能有User.Identity.IsAuthenticated否则不起作用
            Session.RemoveAll();
            FormsAuthentication.RedirectToLoginPage();
        }



        [HttpPost]
        public ActionResult Logins(JsonMode A)
        {
            string s = HttpContext.Request["A"];
            return null;
        }
    }
    public class JsonMode
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E1 { get; set; }
        public string E2 { get; set; }
        public string E3 { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public string I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string M3 { get; set; }
        public string M4 { get; set; }
        public string N { get; set; }
        public string N1 { get; set; }
        public string O { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string R { get; set; }
        public string S { get; set; }
        public string T { get; set; }
        public string U { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
    }
}
