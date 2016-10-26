using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Reflection;
using MODEl;

namespace DAL
{
    public class DalUser
    {
        ZSEntities db = new ZSEntities();

        #region 查询相关

        #region 根据ID查询
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUserByUserID(int userID)
        {
            User modelUser = db.User.Include("UserRole").Where(d => d.ID == userID).FirstOrDefault();
            if (modelUser == null)
            {
                modelUser = new User();
            }
            return modelUser;
        }
        #endregion

        #region 根条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        public List<User> GetUserByWhere(Expression<Func<User, bool>> whereLamda)
        {
            List<User> listUser = db.User.Include("UserRole").Where(whereLamda).ToList();
            if (listUser == null)
            {
                listUser = new List<User>();
            }
            return listUser;
        }
        #endregion

        #region 根条件查询并正序排序
        /// <summary>
        /// 根条件查询并正序排序
        /// </summary>
        /// <typeparam name="Order"></typeparam>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <param name="orderColumn">排序列lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<User> GetUserByWhereOrderBbyAsc<Order>(Expression<Func<User, bool>> whereLamda, Expression<Func<User, Order>> orderColumn)
        {
            List<User> listUser = db.User.Include("UserRole").Where(whereLamda).OrderBy(orderColumn).ToList();
            if (listUser == null)
            {
                listUser = new List<User>();
            }
            return listUser;
        }
        #endregion

        #region 根条件查询并倒序排序
        /// <summary>
        /// 根条件查询并倒序排序
        /// </summary>
        /// <typeparam name="Order"></typeparam>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <param name="orderDescColumn">排序列lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<User> GetUserByWhereOrderByDesc<Order>(Expression<Func<User, bool>> whereLamda, Expression<Func<User, Order>> orderDescColumn)
        {
            List<User> listUser = db.User.Include("UserRole").Where(whereLamda).OrderByDescending(orderDescColumn).ToList();
            if (listUser == null)
            {
                listUser = new List<User>();
            }
            return listUser;
        }
        #endregion

        #region 根条件查询并正序排序分页
        /// <summary>
        ///   根条件查询并正序排序分页
        /// </summary>
        /// <typeparam name="Order"></typeparam>
        /// <param name="index">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <param name="orderAscColumn">排序列lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<User> GetUserPageByWhereOrderByAsc<Order>(int index, int pageSize, Expression<Func<User, bool>> whereLamda, Expression<Func<User, Order>> orderAscColumn)
        {
            List<User> listUser = db.User.Include("UserRole").Where(whereLamda).OrderBy(orderAscColumn).Skip((index - 1) * pageSize).Take(pageSize).ToList();
            if (listUser == null)
            {
                listUser = new List<User>();
            }
            return listUser;
        }
        #endregion

        #region 根条件查询并倒序排序分页
        /// <summary>
        ///   根条件查询并倒序排序分页
        /// </summary>
        /// <typeparam name="Order"></typeparam>
        /// <param name="index">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <param name="orderDescColumn">排序列lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<User> GetUserPageByWhereOrderByDesc<Order>(int index, int pageSize, Expression<Func<User, bool>> whereLamda, Expression<Func<User, Order>> orderDescColumn)
        {
            //skip应该在orderby之后，使之生成rowNamBer排序列
            List<User> listUser = db.User.Include("UserRole").Where(whereLamda).OrderByDescending(orderDescColumn).Skip((index - 1) * pageSize).Take(pageSize).ToList();
            if (listUser == null)
            {
                listUser = new List<User>();
            }
            return listUser;
        }
        #endregion

        #endregion

        #region 更改相关
        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="modelUser"></param>
        /// <returns></returns>
        public int AddUser(User modelUser)
        {
            db.User.Add(modelUser);
            db.SaveChanges();
            return modelUser.ID;
        }
        #endregion

        #region 根据ID删除
        /// <summary>
        ///  根据ID删除
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int DeleteUserByUserID(int userID)
        {
            User modelUser = new User() { ID = userID };
            db.User.Attach(modelUser);
            db.User.Remove(modelUser);
            return db.SaveChanges();
        }
        #endregion

        #region 根据条件删除
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="deleteWhere">删除lamda条件</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        public int DeleteUserByWhere(Expression<Func<User, bool>> deleteWhere)
        {
            List<User> listUser = db.User.Where(deleteWhere).ToList();
            listUser.ForEach(u => db.User.Remove(u));
            return db.SaveChanges();
        }
        #endregion

        #region 更改销售信息，销售的信息必须是通过ef查询出来的
        /// <summary>
        /// 更改销售信息，销售的信息必须是通过ef查询出来的
        /// </summary>
        /// <param name="modelUser"></param>
        /// <returns></returns>
        public int ModifyUserByUserIsEfSelect(User modelUser)
        {
            return db.SaveChanges();
        }
        #endregion

        #region 更改销售信息，销售的信息不是经过EF查询出来的
        /// <summary>
        /// 更改销售信息，销售的信息不是经过EF查询出来的
        /// </summary>
        /// <param name="modelUser"></param>
        /// <param name="column">要更改的列</param>
        /// <returns></returns>
        public int ModifyByUserNotEfSelect(User modelUser, params string[] column)
        {
            DbEntityEntry entry = db.Entry<User>(modelUser);
            entry.State = EntityState.Unchanged;
            foreach (var item in column)
            {
                entry.Property(item).IsModified = true;
            }
            return db.SaveChanges();
        }
        #endregion

        #region 根据ID批量更改
        /// <summary>
        /// 根据ID批量更改
        /// </summary>
        /// <param name="listUserID">要修改的ID</param>
        /// <param name="modelUser">修改成的值</param>
        /// <param name="column">要修改的列</param>
        /// <returns></returns>
        public int BatchModifyByUserID(int[] listUserID, User modelUser, params string[] column)
        {
            List<User> listBeiModifyUser = db.User.Where(d => listUserID.Contains(d.ID)).ToList();
            if (listBeiModifyUser != null && listBeiModifyUser.Count > 0)
            {
                //获取实体类的类型对象
                Type t = typeof(User);
                //获取实体类的Instance属性和Public属性的所有内容
                List<PropertyInfo> listropertyInso = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //创建实体属性的字典集合
                Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
                //将实体属性装入字典集合
                listropertyInso.ForEach(p => dic.Add(p.Name, p));
                foreach (var myModelUser in listBeiModifyUser)
                {
                    foreach (var param in column)
                    {
                        if (dic.ContainsKey(param))
                        {
                            //因为字典中存的值是属性对象，所以根据键取出该对象
                            PropertyInfo propertyInfo = dic[param];
                            //从修改值类中取出param对应的值
                            object modifyValue = propertyInfo.GetValue(modelUser, null);
                            //把值设置给要修改的类中的属性
                            propertyInfo.SetValue(myModelUser, modifyValue, null);
                        }
                    }
                }
                return db.SaveChanges();
            }
            return 0;
        }


        #endregion

        #region 根据条件更改
        /// <summary>
        /// 根据条件更改
        /// </summary>
        /// <param name="listBeiModifyUser">等待修改的对象</param>
        /// <param name="modelUser">修改成的值</param>
        /// <param name="column">要修改的列</param>
        /// <returns></returns>
        public int BatchModifyByWhere(User modelUser, Expression<Func<User, bool>> whereLamda, params string[] column)
        {
            List<User> listBeiModifyUser = db.User.Where(whereLamda).ToList();
            if (listBeiModifyUser != null && listBeiModifyUser.Count > 0)
            {
                //获取实体类的类型对象
                Type t = typeof(User);
                //获取实体类的Instance属性和Public属性的所有内容
                List<PropertyInfo> listropertyInso = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //创建实体属性的字典集合
                Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
                //将实体属性装入字典集合
                listropertyInso.ForEach(p => dic.Add(p.Name, p));
                foreach (var myModelUser in listBeiModifyUser)
                {
                    foreach (var param in column)
                    {
                        if (dic.ContainsKey(param))
                        {
                            //因为字典中存的值是属性对象，所以根据键取出该对象
                            PropertyInfo propertyInfo = dic[param];
                            //从修改值类中取出param对应的值
                            object modifyValue = propertyInfo.GetValue(modelUser, null);
                            //把值设置给要修改的类中的属性
                            propertyInfo.SetValue(myModelUser, modifyValue, null);
                        }
                    }
                }
                return db.SaveChanges();
            }
            return 0;
        }
        #endregion

        #endregion

    }
}
