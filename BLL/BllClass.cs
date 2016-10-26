using MODEl;
using DAL;
using Factory;
using IDAL;
namespace BLL
{
    public class BllClass : BaseBll<Class>
    {
        private IDalClass myDalClass = DataFactory.GetDalClass();

        /// <summary>
        ///重写父类的方法确定掉用是自己的底层
        /// </summary>
        public override void SetDal()
        {
            dal = new DalClass();
            myDalClass = dal as DalClass;
        }

    }
}
