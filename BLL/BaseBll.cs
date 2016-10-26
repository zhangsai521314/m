using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL;
using MODEl;

namespace BLL
{
    public abstract class BaseBll<T> where T : class, new()
    {

        ////不能确定实力化那个类 
        protected BaseDAL<T> dal = null;

        /// <summary>
        /// 用于子类重写
        /// </summary>
        public abstract void SetDal();

        public BaseBll()
        {
            SetDal();
        }



        #region 查询相关

        public List<Student> GetListStudentByCache()
        {
            return dal.GetListStudentByCache();
        }

        #region 根条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <returns></returns> 
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        public List<T> GetListByWhere(Expression<Func<T, bool>> whereLamda)
        {
            return dal.GetListByWhere(whereLamda);
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
        public List<T> GetListByWhereOrderBbyAsc<Order>(Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderColumn)
        {
            return dal.GetListByWhereOrderBbyAsc(whereLamda, orderColumn);
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
        public List<T> GetUserByWhereOrderByDesc<Order>(Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderDescColumn)
        {
            return dal.GetUserByWhereOrderByDesc(whereLamda, orderDescColumn);
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
        public List<T> GetListPageByWhereOrderByAsc<Order>(int index, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderAscColumn)
        {
            //skip应该在orderby之后，使之生成rowNamBer排序列
            return dal.GetListPageByWhereOrderByAsc(index, pageSize, whereLamda, orderAscColumn);
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
        public List<T> GetUserPageByWhereOrderByDesc<Order>(int index, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderDescColumn)
        {
            //skip应该在orderby之后，使之生成rowNamBer排序列
            return dal.GetUserPageByWhereOrderByDesc(index, pageSize, whereLamda, orderDescColumn);
        }
        #endregion

        #endregion

        #region 更改相关
        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(T model, out int id)
        {
            return dal.Add(model, out id);
        }
        #endregion

        #region 根据ID删除
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeleteByID(T model)
        {
            return dal.DeleteByID(model);
        }
        #endregion

        #region 根据条件删除
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="deleteWhere"></param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        public int DeleteByWhere(Expression<Func<T, bool>> deleteWhere)
        {
            return dal.DeleteByWhere(deleteWhere);
        }
        #endregion

        #region 更改信息，信息必须是通过ef查询出来的
        /// <summary>
        /// 更改信息，信息必须是通过ef查询出来的
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ModifyByIsEfSelect(T model)
        {
            return dal.ModifyByIsEfSelect(model);
        }
        #endregion

        #region 更改信息，信息不是经过EF查询出来的
        /// <summary>
        /// 更改信息，信息不是经过EF查询出来的
        /// </summary>
        /// <param name="model"></param>
        /// <param name="column">要更改的列</param>
        /// <returns></returns>
        public int ModifyByNotEfSelect(T model, params string[] column)
        {
            return dal.ModifyByNotEfSelect(model, column);
        }
        #endregion

        #region 根据条件更改
        /// <summary>
        /// 根据条件更改
        /// </summary>
        /// <param name="listBeiModifyModel">等待修改的对象</param>
        /// <param name="model">修改成的值</param>
        /// <param name="column">要修改的列</param>
        /// <returns></returns>
        public int BatchModifyByWhere(T model, Expression<Func<T, bool>> whereLamda, params string[] column)
        {

            return dal.BatchModifyByWhere(model, whereLamda, column);
        }
        #endregion

        #endregion



    }
}
