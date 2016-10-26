using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEl;
using CommonHelp;
using System.Web.Security;
namespace mvc4_1.Controllers
{
    public class StudentController : Controller
    {

        #region 私有变量
        private BllStudent bllStudent = new BllStudent();
        private BllClass bllClass = new BllClass();
        #endregion

        #region 查询操作

        #region 学生首页
        /// <summary>
        /// 学生首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            #region 测试
            //string d = "3.3";
            //int h = d.ToInt32();
            //List<Student> list = bllStudent.GetUserPageByWhereOrderByDesc(1, 5, u => u.ClassID == 1, c => c.ID);
            //List<CeShi_Result> f = bllStudent.Get(2);
            //List<Student> e = bllStudent.GetListStudentByCache();

            #endregion
            //if (User.Identity.IsAuthenticated && Session["userInfo"] != null)
            //{
            //    Student modelStudent = Session["userInfo"] as Student;
            //    List<Student> listStudent = bllStudent.GetListByWhere(s => s.IsValid == true);
            //    ViewBag.listStudent = listStudent;
            //    ViewBag.UserID = ((Student)Session["userInfo"]).ID;
            //    return View();
            //}

           // FormsAuthentication.RedirectToLoginPage();
            return View();
        }
        #endregion

        #region 进入Modify视图
        /// <summary>
        /// 进入Modify视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Modify(int id)
        {
            Student modelStudent = bllStudent.GetStudentByID(id);
            #region 对应视图中第一种下拉框方法
            //List<Class> listClass = bllClass.GetListByWhere(c => c.IsValid == true).ToList();
            //ViewBag.listClass = listClass; 
            #endregion
            #region 对应视图中第二种下拉框方法
            List<SelectListItem> listClass = bllClass.GetListByWhere(c => c.IsValid == true).Select(c => new SelectListItem() { Text = c.ClassName, Value = c.ID.ToString(), Selected = (modelStudent.ID == id) }).ToList();
            ViewBag.listClass = listClass;
            #endregion
            return View(modelStudent);
        }
        #endregion

        #region  进入Add视图
        /// <summary>
        /// 进入Add视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            List<SelectListItem> listClass = bllClass.GetListByWhere(c => c.IsValid == true).Select(c => new SelectListItem() { Text = c.ClassName, Value = c.ID.ToString() }).ToList();
            ViewBag.listClass = listClass;
            return View();
        }
        #endregion

        #endregion

        #region 修改操作

        #region 执行新增操作
        /// <summary>
        /// 执行新增操作
        /// </summary>
        /// <param name="modelStudent"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(Student modelStudent)
        {
            int id = 0;
            modelStudent.CreateDate = DateTime.Now;
            int result = bllStudent.Add(modelStudent, out id);
            return Redirect("/Student/Index");
        }

        #endregion

        #region 执行修改操作
        /// <summary>
        /// 执行修改操作
        /// </summary>
        /// <param name="modelStudent"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modify(Student modelStudent)
        {
            int result = bllStudent.ModifyByNotEfSelect(new Student() { ID = modelStudent.ID, StudentName = modelStudent.StudentName, ClassID = modelStudent.ClassID, ModifyDate = DateTime.Now }, "ClassID", "StudentName", "ModifyDate");
            return Redirect("/Student/Index");
        }
        #endregion

        #region 执行软删除
        /// <summary>
        /// 执行软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Del(int id)
        {
            int result = bllStudent.ModifyByNotEfSelect(new Student() { ID = id, IsValid = false, ModifyDate = DateTime.Now }, "IsValid", "ModifyDate");
            return Redirect("/Student/Index");
        }
        #endregion

        #endregion
    }
}
