using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton_DanLi
{
    class Program
    {
        static void Main(string[] args)
        {
            LanHanDanLiClass lmo = LanHanDanLiClass.GetInstance();

            EHanDanLiClass emo = EHanDanLiClass.GetInstance();
            
            Console.ReadKey();
        }
    }
}
