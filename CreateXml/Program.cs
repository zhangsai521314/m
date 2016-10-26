using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CreateXml
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dc = doc.CreateXmlDeclaration("1.0", "utf-8", null);//创建xml的描述信息
            doc.AppendChild(dc);//将描述信息加入到文档中
            XmlElement books = doc.CreateElement("Books");//创建一个根节点
            doc.AppendChild(books);
            #region 创建简单不带属性的xml
            //XmlElement book = doc.CreateElement("book");
            //books.AppendChild(book);
            //XmlElement bookName = doc.CreateElement("bookName");
            //bookName.InnerText = "第一本书";
            //book.AppendChild(bookName);
            //XmlElement bookPrice = doc.CreateElement("bookPrice");
            //bookPrice.InnerText = "10.2";
            //book.AppendChild(bookPrice);
            //doc.Save("CreateXml_1.xml");
            #endregion

            #region 创建带属性的xml
            XmlElement book = doc.CreateElement("book");
            book.SetAttribute("ID", "1");
            book.SetAttribute("bookName", "带属性1");
            books.AppendChild(book);
            doc.Save("带属性_1.xml");

            #endregion

            Console.WriteLine("创建成功");
            Console.ReadKey();



        }
    }
}
