using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonHelp
{
    public static class CommonStringHelp
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="def"></param>
        /// <remarks>
        ///  string d = "3";
        ///  int h = d.ToInt32();
        /// </remarks>
        /// <returns></returns>
        public static int ToInt32(this string c, int def = default(Int32))
        {
            Int32 returnDef;
            return int.TryParse(c, out returnDef) ? returnDef : def;
        }
    }


    #region 叶飞
    public static class ObjectExtension
    {
        /// <summary>
        /// 读取首个标记了指定特性的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static V GetPropertyValueMarkedByAttribute<T, V>(this object obj) where T : Attribute
        {
            object propertyValue = null;

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                bool found = false;

                var attributes = property.GetCustomAttributes(typeof(T), false);
                foreach (var attr in attributes)
                {
                    if (attr is T)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (property.CanRead)
                    {
                        propertyValue = property.GetValue(obj, null);
                    }

                    break;
                }
            }

            return propertyValue is V ? (V)propertyValue : default(V);
        }
    }



    public static class TypeExtension
    {
        /// <summary>
        /// 判断当前类型是否实现了指定接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasInterface<T>(this Type type)
        {
            return type.GetInterface(typeof(T).FullName) != null;
        }

        /// <summary>
        /// 尝试从当前类型中读出作用在Class级别的指定特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            object[] attrs = type.GetCustomAttributes(typeof(T), false);
            T targetAttribute = null;

            foreach (var attr in attrs)
            {
                if (attr is T)
                {
                    targetAttribute = attr as T;
                    break;
                }
            }

            return targetAttribute;
        }

        /// <summary>
        /// 尝试从当前类型中读出作用在指定方法的指定特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type, string methodName) where T : Attribute
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                return null;
            }

            T targetAttribute = null;

            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                if (!method.Name.Equals(methodName))
                {
                    continue;
                }

                var attrs = method.GetCustomAttributes(typeof(T), false);
                foreach (var attr in attrs)
                {
                    if (attr is T)
                    {
                        targetAttribute = attr as T;
                        break;
                    }
                }

                if (targetAttribute != null)
                {
                    break;
                }
            }

            return targetAttribute;
        }
    }
    #endregion

}
