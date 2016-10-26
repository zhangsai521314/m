using IDAL;
using DAL;
namespace Factory
{
    /// <summary>
    /// 工厂类
    /// </summary>
    public sealed class DataFactory
    {

        public static IDalClass GetDalClass()
        {
            return new DalClass();
        }
    }
}
