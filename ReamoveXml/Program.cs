using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReamoveXml
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();

            #region 删除普通节点
            doc.Load("CreateXml_1.xml");
            XmlNode book = doc.SelectSingleNode("/Books/book");
            //book.RemoveAll();//删除会留下空节点
            XmlNode books = doc.SelectSingleNode("/Books");
            books.RemoveChild(book);
            doc.Save("CreateXml_1.xml");
            #endregion

            #region 删除属性

            //doc.Load("带属性_1.xml");
            //XmlElement xmle = doc.DocumentElement;
            //XmlNodeList bookList = xmle.SelectNodes("/Books/book");
            //foreach (XmlNode item in bookList)
            //{
            //    if (item.Attributes["ID"] != null && item.Attributes["ID"].Value == "1")
            //    {
            //        XmlElement c = (XmlElement)item;
            //        //c.RemoveAttribute("ID");//删除指定属性
            //        //c.RemoveAllAttributes();//删除全部属性，留下的是一个空节点
            //        //c.RemoveAll();//等价于c.RemoveAllAttributes()
            //        xmle.RemoveChild(c);//把节点彻底删除，没有空节点
            //    }
            //}
            //doc.Save("带属性_1.xml");

            #endregion

            #region 普通修改


            #endregion

            #region 修改属性值

            //doc.Load("带属性_1.xml");
            //XmlElement xmle = doc.DocumentElement;
            //XmlNodeList bookList = xmle.SelectNodes("/Books/book");
            //foreach (XmlNode item in bookList)
            //{
            //    if (item.Attributes["ID"] != null && item.Attributes["ID"].Value == "1")
            //    {
            //        item.Attributes["ID"].Value = "3";
            //    }
            //}
            //doc.Save("带属性_1.xml");


            #endregion

            Console.WriteLine("成功");

        }
    }
}
