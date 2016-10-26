using DAL;
using MODEl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BllUser
    {
        private DalUser dalUser = new DalUser();
        #region 查询相关

        #region 根据ID查询
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public User GetUserByUserID(int userID)
        {
            return dalUser.GetUserByUserID(userID);
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
            return dalUser.GetUserByWhere(whereLamda);
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
            return dalUser.GetUserByWhereOrderBbyAsc(whereLamda, orderColumn);
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
            return dalUser.GetUserByWhereOrderByDesc(whereLamda, orderDescColumn);
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
            return dalUser.GetUserPageByWhereOrderByAsc(index, pageSize, whereLamda, orderAscColumn);
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

            return dalUser.GetUserPageByWhereOrderByDesc(index, pageSize, whereLamda, orderDescColumn); ;
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
            return dalUser.AddUser(modelUser);
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
            return dalUser.DeleteUserByUserID(userID);
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
            return dalUser.DeleteUserByWhere(deleteWhere);
        }
        #endregion

        #region  更改销售信息，销售的信息不是经过EF查询出来的
        /// <summary>
        ///  更改销售信息，销售的信息不是经过EF查询出来的
        /// </summary>
        /// <param name="modelUser"></param>
        /// <returns></returns>
        public int ModifyByUserNotEfSelect(User modelUser, params string[] column)
        {
            return dalUser.ModifyByUserNotEfSelect(modelUser, column);
        }
        #endregion

        #region 更改销售信息，销售的信息必须是通过EF查询出来的
        /// <summary>
        ///  更改销售信息，销售的信息必须是通过ef查询出来的
        /// </summary>
        /// <param name="modelUser"></param>
        /// <returns></returns>
        public int ModifyUserByUserIsEfSelect(User modelUser)
        {
            return dalUser.ModifyUserByUserIsEfSelect(modelUser);
        }
        #endregion


        #region 根据ID批量更改
        /// <summary>
        /// 根据ID批量更改
        /// </summary>
        /// <param name="listUserID">等待修改的对象"1,2,3,4"</param>
        /// <param name="modelUser">修改成的值</param>
        /// <param name="column">要修改的列</param>
        /// <returns></returns>
        public int BatchModifyByUserID(int[] listUserID, User modelUser, params string[] column)
        {
            return dalUser.BatchModifyByUserID(listUserID, modelUser, column);
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
            return dalUser.BatchModifyByWhere(modelUser, whereLamda, column);
        }
        #endregion

        #endregion

    }
}
