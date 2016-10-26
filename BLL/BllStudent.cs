using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEl;
using DAL;
using System.Linq.Expressions;

namespace BLL
{
    public class BllStudent : BaseBll<Student>
    {
        #region 告诉父级的Bll我要实例化那个Dal类
        private DalStudent myDal = null;
        public override void SetDal()
        {
            dal = new DalStudent();
            myDal = dal as DalStudent;
        }
        #endregion

        public Student GetStudentByID(int id)
        {
            return myDal.GetStudentByID(id);
        }
    }
}
