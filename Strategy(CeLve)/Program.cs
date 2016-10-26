using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy_CeLv
{
    class Program
    {
        static void Main(string[] args)
        {
            ShiLi model = new ShiLi(new ToSchoolByWalk());
            // Console.WriteLine($"走路去学校花费时间：{model.GetTotalTime()}");  //netf5的拼接字符串
            model = new ShiLi(new ToSchoolByBus());
            //Console.WriteLine($"坐公交车去学校花费时间：{model.GetTotalTime()}");
            Console.ReadKey();

        }
    }
}
