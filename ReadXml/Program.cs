using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace ReadXml
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();

            #region 读取普通的xml
            //doc.Load("CreateXml_1.xml");//根据路径读取xml
            //XmlElement books = doc.DocumentElement;
            //XmlNodeList bookList = books.ChildNodes;//获取第一个节点下的所有节点
            //XmlNodeList book = null;
            //foreach (XmlNode item in bookList)
            //{
            //    book = item.ChildNodes;
            //    foreach (XmlNode c in book)
            //    {
            //        Console.WriteLine($"节点名称：{ c.Name}，节点内容:{c.InnerText}\r");
            //    }
            //} 
            #endregion

            #region 读取带属性的xml
            doc.Load("带属性_1.xml");
            XmlElement xmle = doc.DocumentElement;
            XmlNodeList books = xmle.SelectNodes("/Books/book");//根据节点路径获取节点
            foreach (XmlNode item in books)
            {
                //Console.WriteLine($"ID:{item.Attributes["ID"].Value},booknam:{item.Attributes["bookName"].Value}");
            }
            #endregion
            Console.ReadKey();
        }
    }
}
