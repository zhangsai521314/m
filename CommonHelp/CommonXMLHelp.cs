using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace CommonHelp
{
    public class CommonXMLHelp
    {

        #region 将XML转换成DataTable
        /// <summary>
        /// 将XML转换成DataTable
        /// </summary>
        /// <param name="xmlUrl">XML的URL（绝对路径）</param>
        /// <returns></returns>
        public static DataTable ConvertXMLToDataTable(string xmlUrl)
        {
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                reader = new XmlTextReader(xmlUrl);
                xmlDS.ReadXml(reader);
                return xmlDS.Tables[0];
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        #endregion

    }
}
