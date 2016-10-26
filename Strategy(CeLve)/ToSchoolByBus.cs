using System;

namespace Strategy_CeLv
{
    /// <summary>
    /// 策略实现类（坐公交）
    /// </summary>
    public class ToSchoolByBus : StrategyClass
    {

        public override double GetToSchoolTime()
        {
            return 0.3;
        }
    }
}
