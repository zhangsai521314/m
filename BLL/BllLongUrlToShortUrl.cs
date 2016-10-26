using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEl;
using DAL;
using CommonHelp;
using System.Configuration;
namespace BLL
{
    public class BllLongUrlToShortUrl : BaseBll<LongUrlToShorturl>
    {

        private DalLongUrlToShortUrl myDal = null;

        public override void SetDal()
        {
            dal = new DalLongUrlToShortUrl();
            myDal = dal as DalLongUrlToShortUrl;
        }

        #region 获取短链接
        /// <summary>
        /// 获取短链接
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public string GetShortUrlByLongUrl(string longUrl)
        {
            LongUrlToShorturl mode = myDal.GetListByWhere(u => u.LongUrl.Equals(longUrl, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (mode != null && !string.IsNullOrEmpty(mode.ShortUrl))
            {
                return mode.ShortUrl;
            }
            mode = new LongUrlToShorturl() { LongUrl = longUrl, CreateDate = DateTime.Now };
            string shortUrl = string.Empty;
            int id = 0;
            int result = myDal.Add(mode, out  id);
            if (id > 0)
                shortUrl = CommonUrlHelp.GetShortUrlByLongUrlID(id);
            if (!string.IsNullOrEmpty(shortUrl))
            {
                shortUrl = ConfigurationManager.AppSettings["DomainName"] + shortUrl;
            }
            mode.ShortUrl = shortUrl;
            mode.ModifyDate = DateTime.Now;
            myDal.ModifyByIsEfSelect(mode);
            return shortUrl;
        }
        #endregion

        #region 获取长链接
        /// <summary>
        ///  获取长链接
        /// </summary>
        /// <param name="linkModel"></param>
        /// <returns></returns>
        public string GetLongUrlByShortUrl(string shortUrl)
        {
            string longurl = string.Empty;
            LongUrlToShorturl model = new LongUrlToShorturl();
            if (!string.IsNullOrEmpty(shortUrl))
            {
                int start = shortUrl.LastIndexOf('/') + 1;
                int end = shortUrl.Length - start;
                shortUrl = shortUrl.Substring(start, end);
                int id = CommonUrlHelp.GetLongUrlIDByShortUrl(shortUrl);
                model = myDal.GetLongUrlByShortUrlId(id);
            }
            return model == null ? null : model.LongUrl;
        }
        #endregion
    }
}
