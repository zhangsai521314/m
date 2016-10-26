using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MODEl;
using BLL;

namespace mvc4_1.Controllers
{
    //[Filters.MyActionFilter]
    //[Filters.MyHandleErrorAttibute]
    public class Razor语法事例Controller : Controller
    {
        private BllStudent bllSrudent = new BllStudent();

        #region  razor视图引擎中的一些基本语法
        /// <summary>
        /// razor视图引擎中的一些基本语法
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 模板页事例
        /// <summary>
        /// 模板页事例                                                          
        /// </summary>
        /// <returns></returns>
        public ActionResult ShiYongMoBanYe()
        {
            return View();
        }
        #endregion

        #region RazorView
        /// <summary>
        /// Razor视图事例页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RazorAdd()
        {
            Session["StudentLoginName"] = "007Name";
            Session.Add("StudentPassWord", "007PassWord");
            return View();
        }
        #endregion

        #region 普通表单新增
        /// <summary>
        /// 普通表单新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RazorAdd(Student model)
        {
            int id = 0;
            if (!ModelState.IsValid)
            {
                return Content("验证不通过");
            }
            int result = bllSrudent.Add(model,out id);
            if (result > 0)
            {
                return Redirect("~/Student/Index");
            }
            return Content("新增失败");
        }
        #endregion

        #region 验证特性中的Remote特性
        /// <summary>
        /// 验证特性中的Remote特性
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoteYanZheng()
        {
            string StudentName = Request.Params["StudentName"];
            Student modelStudent = bllSrudent.GetListByWhere(s => s.StudentName == StudentName).FirstOrDefault();
            if (modelStudent != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);     //JsonRequestBehavior.AllowGet解决报System.InvalidOperationException: 此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 验证AjaxBeginForm
        /// <summary>
        /// 验证AjaxBeginForm
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxRazorAdd(Student model)
        {
            int id = 0;
            System.Threading.Thread.Sleep(3000);
            if (!ModelState.IsValid)
            {
                return Content("验证不通过");
            }
            int result = bllSrudent.Add(model,out id);
            if (result > 0)
            {
                return Content("新增成功");
                //return Json(new { id = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Content("新增失败");
        }
        #endregion

        #region 路由约束1
        /// <summary>
        ///RouteConfig中路由约束1 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RouteYueShuOne(int id)
        {

            return Content("ID:" + id);
        }


        #endregion

        #region 路由约束2
        /// <summary>
        /// RouteConfig中路由约束2
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult RouteYueShuTwo(string name)
        {

            return Content("name:" + name);
        }



        #endregion

        #region  验证Global.asax中的去除服务器的查找视图项，不能添加视图
        /// <summary>
        /// 验证Global.asax中的去除服务器的查找视图项，不能添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult my()
        {
            return View();
        }
        #endregion

        #region 过滤器
        /// <summary>
        /// 测试过滤器
        /// </summary>
        /// <returns></returns>
        //[Filters.MyActionFilter]
        public ActionResult GuoLvQi()
        {
            return View();
        }
        #endregion

        #region 验证方法过滤器中的跳过有某特性则不调用方法

        /// <summary>
        /// 验证方法过滤器中的跳过有某特性则不调用方法
        /// </summary>
        /// <returns></returns>
        [Attributes.Phone]
        public ActionResult TeXing()
        {
            return View();
        }

        #endregion

        #region 错误捕捉过滤器HandleErrorAttibute

        public ActionResult GetError()
        {
            int c = Convert.ToInt32("");
            return View();
        }

        #endregion

    }
}
