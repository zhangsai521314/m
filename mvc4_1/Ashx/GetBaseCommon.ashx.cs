using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc4_1.Ashx
{
    /// <summary>
    /// GetBaseCommon 的摘要说明
    /// </summary>
    public class GetBaseCommon : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string ss = "[{'lng':'121.42','lat':'31.22'},{'lng':'121.45','lat':'31.25'}]";
            context.Response.Write(ss);
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}