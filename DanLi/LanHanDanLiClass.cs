using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton_DanLi
{
    public class LanHanDanLiClass 
    {

        //单例1:创建一个对象为null
        private static LanHanDanLiClass lmo = null;

        //2：定义一个私有只读的静态对象

        //1.被锁的类型必须是出string外的引用类型。（因为字符串类型被CLR“暂留”这意味着整个程序中任何给定字符串都只有一个实例，就是这同一个对象表示了所有运行的应用程序域的所有线程中的该文本。因此，只要在应用程序进程中的任何位置处具有相同内容的字符串上放置了锁，就将锁定应用程序中该字符串的所有实例。因此，最好锁定不会被暂留的私有或受保护成员。）

        //2.lock不能锁定空值某一对象可以指向Null，但Null是不需要被释放的。（请参考：认识全面的null）

        //3.lock锁定的对象是一个程序块的内存边界

        //4.lock就避免锁定public 类型或不受程序控制的对象。

        private static readonly object lockmo = new object();

        //2:创建私有的构造函数,是外部不能通过new创建对象
        private LanHanDanLiClass()
        {
            this.name = "懒汉";
        }

        public static LanHanDanLiClass GetInstance()
        {
            //为什么使用二重判断http://www.cnblogs.com/BoyXiao/archive/2010/05/07/1729376.html?login=1
            if (lmo == null)
            {
                lock (lockmo)
                {
                    if (lmo == null)
                    {
                        lmo = new LanHanDanLiClass();
                    }
                }
            }
            return lmo;
        }


        public string name { get; set; }

    }

    public class EHanDanLiClass
    {

        private static EHanDanLiClass emo = new EHanDanLiClass();

        private EHanDanLiClass() { this.name = "饿汉"; }

        public static EHanDanLiClass GetInstance()
        {
            return emo;
        }


        public string name { get; set; }





    }


//    比较:
//         饿汉式是线程安全的,在类创建的同时就已经创建好一个静态的对象供系统使用,以后不在改变
//          懒汉式如果在创建实例对象时不加上 synchronized  则会导致对对象的访问不是线程安全的
//          推荐使用第一种
//从实现方式来讲他们最大的区别就是懒汉式是延时加载,
//他是在需要的时候才创建对象, 而饿汉式在虚拟机启动的时候就会创建,

//饿汉式无需关注多线程问题、写法简单明了、能用则用。但是它是加载类时创建实例（上面有个朋友写错了）、所以如果是一个工厂模式、缓存了很多实例、那么就得考虑效率问题，因为这个类一加载则把所有实例不管用不用一块创建。
//懒汉式的优点是延时加载、缺点是应该用同步（想改进的话现在还是不可能，比如double-check）、其实也可以不用同步、看你的需求了，多创建一两个无引用的废对象其实也没什么大不了。
}
