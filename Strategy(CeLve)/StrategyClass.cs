using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy_CeLv
{

    #region 策略模式
            //策略模式定义了算法族，也就是定义了一系列的算法，并分别对这些算法进行了封装，

            //让这些算法之间可以互换，

            //这个模式的优点在于，当面对算法经常需要改变时，

            //这个模式可以让算法的变化独立于使用算法的客户，

            //也就是说，这个模式可以让算法的变化不会影响到使用算法的客户。

            //同时由于各个算法类是独立的，从而减少了各种算法类与使用算法类（客户）之间的耦合度。
    #endregion

    /// <summary>
    /// 策略模式必须有个策略的抽象类
    /// </summary>
    public abstract class StrategyClass
    {

        /// <summary>
        /// 定义抽象算法供供策略的实现类重写，实现不同的策略得到不同的结果
        /// </summary>
        /// <returns></returns>
        public abstract double GetToSchoolTime();

    }
}
