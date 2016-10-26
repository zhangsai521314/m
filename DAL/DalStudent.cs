using MODEl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DalStudent : BaseDAL<Student>
    {
        ZSEntities db = new ZSEntities();

        public Student GetStudentByID(int id)
        {
            return db.Student.Where(s => s.ID == id).FirstOrDefault();
        }

        //#region 测试存储过程
        ///// <summary>
        ///// 1在数据创建存储过程，2在edmx中更新添加存储过程
        ///// </summary>                                                                                 
        ///// <param name="classID"></param>
        ///// <returns></returns>
        //public List<CeShi_Result> Get(int classID)
        //{
        //    List<CeShi_Result> stu = db.CeShi(classID, 1).ToList();
        //    return stu;
        //}


        //#endregion
    }
}
