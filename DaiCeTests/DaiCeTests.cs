using Microsoft.VisualStudio.TestTools.UnitTesting;
using DaiCe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiCe.Tests
{
    [TestClass()]
    public class DaiCeTests
    {
        [TestMethod()]
        public void AddTestAreEqual()
        {
            int sum = DaiCe.add(1, 2);
            Assert.AreEqual(sum, 3);//验证值是否和给定的值相等，相等测试通过
        }

        [TestMethod]
        public void AddTestAreNoEqual()                     
        {                                                               
            int sum = DaiCe.add(1, 2);
            Assert.AreNotEqual(sum, 1);//验证值是否和给定的值不相等，不相等则通过
        }
    }
}