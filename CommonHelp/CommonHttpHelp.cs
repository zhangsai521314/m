using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using CommonHelp;

namespace CommonHelp
{
    public class CommonHttpHelp
    {
        #region Post访问外部地址
        /// <summary>
        /// Post访问外部地址
        /// </summary>
        /// <param name="url">调用的URL</param>
        /// <param name="paraData">参数</param>
        /// <param name="encoding">编码格式（传空为默认的编码）</param>
        /// <returns></returns>
        public static string PostData(string url, string paraData, Encoding encoding)
        {
            if (string.IsNullOrEmpty(paraData))
            {
                return string.Empty;
            }
            try
            {
                byte[] dataPara = UTF8Encoding.UTF8.GetBytes(paraData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = dataPara.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataPara, 0, dataPara.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader strRead = null;
                if (encoding != null)
                {
                    strRead = new StreamReader(response.GetResponseStream(), encoding);
                }
                else
                {
                    strRead = new StreamReader(response.GetResponseStream());
                }
                string result = strRead.ReadToEnd();
                newStream.Close();
                strRead.Close();
                return result;
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return string.Empty;
            }

        }
        #endregion

        #region Get访问外部地址
        /// <summary>
        /// Get访问外部地址
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetData(string uri)
        {
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(uri);
            using (StreamReader sr = new StreamReader(stream))
            {
                try
                {
                    return sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                    return null;
                }
            }
        }
        #endregion   
    }
}
