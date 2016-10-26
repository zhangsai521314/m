using CommonHelp;
using MODEl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Newtonsoft.Json;
using ZS.Framework.Cache;

namespace DAL
{
    public class BaseDAL<T> where T : class,new()
    {

        ZSEntities db = new ZSEntities();

        #region 查询相关

        #region 根条件查询
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLamda">查询条件lamda</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        public List<T> GetListByWhere(Expression<Func<T, bool>> whereLamda)
        {
            return db.Set<T>().Where(whereLamda).ToList();
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
            return db.Set<T>().Where(whereLamda).OrderBy(orderColumn).ToList();
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
            return db.Set<T>().Where(whereLamda).OrderByDescending(orderDescColumn).ToList();
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
        /// <param name="orderAscColumn">排序列lamda(row_number里的排序列)</param>
        /// <returns></returns>
        //因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<T> GetListPageByWhereOrderByAsc<Order>(int index, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderAscColumn)
        {
            //skip应该在orderby之后，使之生成rowNamBer排序列
            return db.Set<T>().Where(whereLamda).OrderBy(orderAscColumn).Skip((index - 1) * pageSize).Take(pageSize).ToList();
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
        /// <param name="orderDescColumn">排序列lamda(row_number里的排序列)</param>
        /// <returns></returns>
        //Func委托的最后一个参数永远是返回值，其他都是参数。 因为where条件返回的是bool，所以Func委托的第二个参数用的是bool
        //因为不知道用户会根据哪个列进行排序，多以排序列的Func委托的的第二个参数使用泛型，这样编译器会自动推断出来所需类型
        public List<T> GetUserPageByWhereOrderByDesc<Order>(int index, int pageSize, Expression<Func<T, bool>> whereLamda, Expression<Func<T, Order>> orderDescColumn)
        {
            //skip应该在orderby之后，使之生成rowNamBer排序列
            return db.Set<T>().Where(whereLamda).OrderByDescending(orderDescColumn).Skip((index - 1) * pageSize).Take(pageSize).ToList();
        }
        #endregion

        #region 测试数据库缓存依赖

        /// <summary>
        /// 通过缓存获取
        /// </summary>
        /// <returns></returns>
        public List<Student> GetListStudentByCache()
        {

            //使用sql依赖缓存之前需启用ServiceBroker。
            //1检测是否已启用Select  DATABASEpRoPERTYEX('数据库名称','IsBrokerEnabled')。
            //2: ALTER DATABASE DBname SET NEW_BROKER WITH ROLLBACK IMMEDIATE; ALTER DATABASE DBname SET ENABLE_BROKER;
            // 启用之前请退出sql重新打开或者关闭SQLServer的Agent代理功能
            //3需子事件管道的Application_Start事件中SqlDependency.Start(CommonConfigHelp.Config)

            //注意：这段代码在vs2010，sql2008中使用失败。在sql2012中使用成功
            try
            {
                //设置缓存键值
                string cacheKey = "Zs_Student";
                //根据键值获取缓存数据
                List<Student> ListStudent = (List<Student>)HttpRuntime.Cache[cacheKey];
                //缓存中无区域架构数据
                if (ListStudent == null)
                {
                    string strSql = string.Empty;
                    ListStudent = new List<Student>();
                    string s = CommonConfigHelp.Config;
                    //获取有效区域架构
                    using (SqlConnection conn = new SqlConnection(s))
                    {
                        strSql = @"SELECT  [ID]
                                  ,[StudentName]
                                  ,[ClassID]
                                  ,[Age]
                                  ,[Gender]
                                  ,[LoginName]
                                  ,[PassWord]
                                  ,[CreateDate]
                                  ,[ModifyDate]
                                  ,[IsValid]
                                    FROM [dbo].[Student]";
                        conn.Open();
                        SqlCommand command = new SqlCommand(strSql, conn);
                        SqlCacheDependency cd = CacheUtility.GetSqlCacheDependency(command);
                        SqlDataReader reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        ListStudent = CommonDataTableHelp<Student>.ConvertToModelList(dt);

                        //保存至缓存中
                        if (ListStudent.Count > 0)
                            CacheUtility.InsertCache(cacheKey, ListStudent, cd, 0, 70);
                        reader.Close();
                    }
                }
                return ListStudent;
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex);
                return new List<Student>();
            }
        }

        private static List<Student> GetBlockedIPs()
        {
            //设置缓存键值
            string cacheKey = "mStudent";
            //根据键值获取缓存数据
            List<Student> ListStudent = (List<Student>)HttpContext.Current.Cache[cacheKey];
            //ListStudent = null;
            //缓存中无区域架构数据
            if (ListStudent == null)
            {
                string strSql = string.Empty;
                ListStudent = new List<Student>();
                SqlDependency.Start(CommonConfigHelp.Config);
                //获取有效区域架构
                using (SqlConnection conn = new SqlConnection(CommonConfigHelp.Config))
                {
                    strSql = @"SELECT  [ID]
                                  ,[StudentName]
                                  ,[ClassID]
                                  ,[Age]
                                  ,[Gender]
                                  ,[LoginName]
                                  ,[PassWord]
                                  ,[CreateDate]
                                  ,[ModifyDate]
                                  ,[IsValid]
                                    FROM [dbo].[Student] ";
                    conn.Open();
                    SqlCommand command = new SqlCommand(strSql, conn);

                    #region 关键代码，会给指定的表建立一个更改的触发器
                    //启用更改通知
                    SqlCacheDependencyAdmin.EnableNotifications(CommonConfigHelp.Config);
                    //连接到 SQL Server 数据库并为 SqlCacheDependency 更改通知准备数据库表
                    SqlCacheDependencyAdmin.EnableTableForNotifications(CommonConfigHelp.Config, "Student");
                    SqlCacheDependency depend = new SqlCacheDependency("ZsDependency", "Student");
                    #region webconfig中的配置
                    //</system.web>
                    //      <caching>
                    //          <sqlCacheDependency enabled="true" pollTime="1000">
                    //              <databases>
                    //                 add  name="ZsDependency" connectionStringName="config" />
                    //              </databases>
                    //          </sqlCacheDependency>
                    //      </caching>
                    //  </system.web>

                    #endregion
                    #endregion

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    ListStudent = CommonDataTableHelp<Student>.ConvertToModelList(dt);

                    //保存至缓存中
                    if (ListStudent.Count > 0)
                        HttpContext.Current.Cache.Insert(cacheKey, ListStudent, depend);
                    reader.Close();
                }
            }
            return ListStudent;
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
            #region 利用反射如果类对象中包含IsValid 或isvalid怎把IsValid或isvalid的值设置成 true
            //Type t = model.GetType();
            //List<PropertyInfo> listPropertyInfo = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            //listPropertyInfo.ForEach(p => dic.Add(p.Name, p));
            //if (dic.ContainsKey("IsValid"))
            //{
            //    PropertyInfo propertyInfo = dic["IsValid"];
            //    propertyInfo.SetValue(model, true);
            //}
            //else if (dic.ContainsKey("isvalid"))
            //{
            //    PropertyInfo propertyInfo = dic["isvalid"];
            //    propertyInfo.SetValue(model, true);
            //}
            #endregion
            id = 0;
            db.Set<T>().Add(model);
            int result = db.SaveChanges();
            try
            {
                #region 获取model中的属性值
                Type t = model.GetType();
                List<PropertyInfo> listPropertyInfo = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
                listPropertyInfo.ForEach(p => dic.Add(p.Name, p));
                if (dic.ContainsKey("ID"))
                {
                    PropertyInfo propertyInfo = dic["ID"];
                    id = Convert.ToInt32(propertyInfo.GetValue(model, null));
                }
                else if (dic.ContainsKey("id"))
                {
                    PropertyInfo propertyInfo = dic["id"];
                    id = Convert.ToInt32(propertyInfo.GetValue(model, null));
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4net.Error(MethodBase.GetCurrentMethod(), ex, "error");
                return result;
            }
            return result;
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
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
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
            List<T> listUser = db.Set<T>().Where(deleteWhere).ToList();
            listUser.ForEach(u => db.Set<T>().Remove(u));
            return db.SaveChanges();
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
            db.Entry<T>(model).State = System.Data.EntityState.Modified;
            return db.SaveChanges();
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
            try
            {
                DbEntityEntry entry = db.Entry<T>(model);
                entry.State = EntityState.Unchanged;
                foreach (var item in column)
                {
                    entry.Property(item).IsModified = true;
                }
                return EFDBValidateOnSaveEnabled();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 根据条件更改
        /// <summary>
        /// 根据条件更改
        /// </summary>
        /// <param name="model">修改成的值</param>
        /// <param name="whereLamda">查询数据条件(等待修改的对象)</param>
        /// <param name="column">要修改的列</param>
        /// <returns></returns>
        public int BatchModifyByWhere(T model, Expression<Func<T, bool>> whereLamda, params string[] column)
        {
            List<T> listBeiModifyModel = db.Set<T>().Where(whereLamda).ToList();
            if (listBeiModifyModel != null && listBeiModifyModel.Count > 0)
            {
                //获取实体类的类型对象
                Type t = typeof(T);
                //获取实体类的Instance属性和Public属性的所有内容
                List<PropertyInfo> listropertyInso = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                //创建实体属性的字典集合
                Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
                //将实体属性装入字典集合
                listropertyInso.ForEach(p => dic.Add(p.Name, p));
                foreach (var myModelUser in listBeiModifyModel)
                {
                    foreach (var param in column)
                    {
                        if (dic.ContainsKey(param))
                        {
                            //因为字典中存的值是属性对象，所以根据键取出该对象
                            PropertyInfo propertyInfo = dic[param];
                            //从修改值类中取出param对应的值
                            object modifyValue = propertyInfo.GetValue(model, null);
                            //把值设置给要修改的类中的属性
                            propertyInfo.SetValue(myModelUser, modifyValue, null);
                        }
                    }
                }
                return EFDBValidateOnSaveEnabled();
            }
            return 0;
        }
        #endregion

        #endregion

        private int EFDBValidateOnSaveEnabled()
        {
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;//关闭EF的实体验证机制
                int count = db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true; //开启EF的实体验证
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
