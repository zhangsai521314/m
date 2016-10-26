using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SuanFa
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 纯翻转元素1
            string str = "acbdfe";
            char[] cha = str.ToCharArray();
            Array.Reverse(cha);
            Console.WriteLine(cha);
            Array.Sort(cha);//正序
            Console.WriteLine(cha);

            #endregion                                                                                              

            #region 纯翻转元素2
            string str2 = "acbdfe";
            string resule = null;
            for (int i = str2.Length - 1; i >= 0; i--)
            {
                resule += str2[i];
            }
            Console.WriteLine(resule);

            #endregion

            #region 考虑大小的翻转，特殊符号不动.a,gcd.welalket,b"

            string str3 = "a,1gcd.welAlket,b";
            char[] char2 = str3.ToCharArray();
            Regex reg = new Regex("[a-zA-Z]");
            char ch = new char();
            for (int i = 0; i < char2.Length; i++)
            {
                if (reg.IsMatch(char2[i].ToString()))
                {
                    for (int j = 0; j < char2.Length-2; j++)
                    {
                        if (reg.IsMatch(char2[i].ToString()))
                        {
                            #region 从小到大
                            if (char2[i] < char2[j])
                            {
                                ch = char2[i];
                                char2[i] = char2[j];
                                char2[j] = ch;
                            }
                            #endregion
                        }
                    }
                }
            }
            string s = new String(char2);
            Console.WriteLine(char2);
            Console.ReadKey();
            #endregion


        }
    }
}
