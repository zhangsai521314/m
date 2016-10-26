using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MODEl
{
    //因为使用DBFirst生成实体类，实体类上加的特性会在更新模型时删除，所以可用这种方法为实体类加上特性，此外本方法也可以扩展业务所需要而数据库不需要的字段
    //这是为数据库生成的Student的分布类，[MetadataType(typeof(MyStudent))]这句话的意思的 ，这个类引用了MyStudent中字段的特性从而使数据库生成的Student类中的字段有这些特性
    [MetadataType(typeof(MyStudent))]
    public partial class Student 
    {
        //扩展业务字段
        [Display(Name = "再次输入密码:")]
        [System.Web.Mvc.Compare("PassWord")]
        public string CheckPassWord { get; set; }

        /// <summary>
        /// 为自动属性加上默认值
        /// </summary>
        public Student()
        {
            CreateDate = DateTime.Now;
            IsValid = true;
        }

    }
    /// <summary>
    /// 特性类
    /// </summary>
    class MyStudent
    {


        public int ID { get; set; }

        [Display(Name = "学生姓名:")]
        [StringLength(15, ErrorMessage = "{0}必须在{2}和{1}之间", MinimumLength = 1)]
        [Required]//必须特性                   
        [Remote("RemoteYanZheng", "Razor语法事例", ErrorMessage = "此学生姓名已存在")]
        public string StudentName { get; set; }

        [Display(Name = "班级表ID:")]
        [Required]//必须特性
        public int? ClassID { get; set; }

        [Display(Name = "年龄:")]
        [Range(1, 140, ErrorMessage = "{0}必须在{1}和{2}之间")]
        public int? Age { get; set; }

        [Display(Name = "性别:")]
        [Required]//必须特性
        public int? Gender { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public int? IsValid { get; set; }

        [Display(Name = "登录名:")]
        public string LoginName { get; set; }

        [Display(Name = "输入密码:")]
        public string PassWord { get; set; }


    }
}
