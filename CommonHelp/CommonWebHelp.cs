using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEl;
using System.Web.Security;
using System.Web;

namespace CommonHelp
{
    public class CommonWebHelp
    {
        #region 生成用户验证ticket
        /// <summary>
        /// 生成用户验证ticket
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="isPersistent"> 如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。如果该票证存储在 URL 中，将忽略此值。</param>
        public static void SaveAuthTicket(Student userInfo, bool isPersistent)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
               1, //票证的版本号
               userInfo.ID.ToString(),  //与票证关联的用户名。
               DateTime.Now,     //票证发出时的本地日期和时间
               DateTime.Now.AddDays(CommonConfigHelp.LoginStateExpires), //票证过期时的本地日期和时间
               isPersistent, //如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。如果该票证存储在 URL 中，将忽略此值。
               userInfo.ID.ToString(), //存储在票证中的用户特定的数据
               FormsAuthentication.FormsCookiePath); //票证存储在 Cookie 中时的路径

            string hash = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion
    }
}
