using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy_CeLv
{
    /// <summary>
    /// 策略实现类（走路）
    /// </summary>
    public class ToSchoolByWalk : StrategyClass
    {
        public override double GetToSchoolTime()
        {
            return 1.0;
        }
    }
}
