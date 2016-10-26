using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy_CeLv
{
    public class ShiLi
    {

        //在这个客户类中必须维护一个算法策略对象
        private StrategyClass strategy;

        public ShiLi(StrategyClass strategy)
        {
            this.strategy = strategy;
        }

        public double GetTotalTime()
        {
            //利用多态
            return this.strategy.GetToSchoolTime();
        }
    }
}
