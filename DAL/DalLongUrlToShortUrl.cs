using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEl;

namespace DAL
{
    public class DalLongUrlToShortUrl :BaseDAL<LongUrlToShorturl>
    {

        ZSEntities db = new ZSEntities();

        public LongUrlToShorturl GetLongUrlByShortUrlId(int id)
        {
            return db.LongUrlToShorturl.Where(l => l.ID == id).FirstOrDefault();
        }

    }
}
