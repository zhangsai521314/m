using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MODEl;
using BLL;

namespace mvc4_1.Controllers
{
    public class UrlController : Controller
    {
        private BllLongUrlToShortUrl bllurl = new BllLongUrlToShortUrl();

        public ActionResult Index()
        {
            return View();
        }


        #region 生成短链
        /// <summary>
        /// 生成短链
        /// </summary>
        /// <param name="linkMpodel"></param>
        /// <returns></returns>
        // POST: LongUrlToShorturl/Create
        [HttpPost]
        public string CreateShortLinkByLongLink(LongUrlToShorturl linkMpodel)
        {
            try
            {
                string shortUrl = bllurl.GetShortUrlByLongUrl(linkMpodel.LongUrl);
                return shortUrl;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 根据短链返回长链接
        /// <summary>
        /// 根据短链返回长链接
        /// </summary>
        /// <param name="linkMpodel"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetLongLinkIDByShortLink(LongUrlToShorturl linkMpodel)
        {
            try
            {
                string longUrl = bllurl.GetLongUrlByShortUrl(linkMpodel.ShortUrl);
                return longUrl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
