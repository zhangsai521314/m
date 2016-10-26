using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonHelp
{
    public static class CommonUrlHelp
    {
       
        #region 根据长链接的库存ID生成短链接
        /// <summary>
        /// 根据长链接的库存ID生成短链接
        /// </summary>
        /// <param name="longLinkID">长连接的库存ID</param>
        /// <returns></returns>
        public static string GetShortUrlByLongUrlID(int longLinkID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "A");
            dic.Add(1, "B");
            dic.Add(2, "C");
            dic.Add(3, "D");
            dic.Add(4, "E");
            dic.Add(5, "F");
            dic.Add(6, "G");
            dic.Add(7, "H");
            dic.Add(8, "I");
            dic.Add(9, "J");
            string returnStr = string.Empty;
            if (longLinkID > 9999)
            {
                returnStr = dic[longLinkID.ToString().Length];
                returnStr += Convert.ToString(longLinkID, 16);
            }
            else
            {
                while (longLinkID >= 10)
                {
                    int rest = longLinkID % 10;
                    returnStr = dic[rest] + returnStr;
                    longLinkID /= 10;
                }
                returnStr = dic[longLinkID.ToString().Length] + dic[longLinkID] + returnStr;
            }
            return returnStr;
        }
        #endregion

        #region 把短链接解析成长连接的库存ID
        /// <summary>
        /// 把短链接解析成长连接的库存ID
        /// </summary>
        /// <param name="ShortLink">短链接的url</param>
        /// <returns></returns>
        public static int GetLongUrlIDByShortUrl(string ShortLink)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(0, "A");
            dic.Add(1, "B");
            dic.Add(2, "C");
            dic.Add(3, "D");
            dic.Add(4, "E");
            dic.Add(5, "F");
            dic.Add(6, "G");
            dic.Add(7, "H");
            dic.Add(8, "I");
            dic.Add(9, "J");
            int id = 0;
            string val = ShortLink.Substring(0, 1);
            if (dic.FirstOrDefault(d => d.Value == val).Key > 4)
            {
                val = ShortLink.Substring(1);
                id = Convert.ToInt32(val, 16);
            }
            else
            {
                val = ShortLink.Substring(1);
                char[] chars = val.ToArray();
                StringBuilder sbts = new StringBuilder();
                for (int i = 0; i < chars.Count(); i++)
                {
                    sbts.Append(dic.FirstOrDefault(d => d.Value == chars[i].ToString()).Key);
                }

                int.TryParse(sbts.ToString(), out id);
            }
            return id;
        }






        #endregion
    }
}
