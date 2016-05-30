using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DapperExtensions;
using Common;

namespace DBUtility
{
    /// <summary>
    /// Repository基类
    /// </summary>
    public class RepositoryBase<T> : IDataRepository<T> where T : class
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public RepositoryBase() { }

        /// <summary>
        /// 通过ID获取记录
        /// </summary>
        /// <param name="primaryId">动态类型ID</param>
        /// <returns>单条满足条件的记录</returns>
        public T GetById(object primaryId)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.Get<T>(primaryId);

                    return result as T;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.GetList<T>();
                    if (result != null)
                    {
                        return result.ToList<T>();
                    }
                    else
                    {
                        return new List<T>();
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return new List<T>();
            }
        }

        /// <summary>
        /// 满足条件的数据条数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>满足条件的记录数</returns>
        public int Count(object predicate)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.Count<T>(predicate);

                    return result;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return 0;
            }
        }

        /// <summary>
        /// 获取满足条件的记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public List<T> GetList(object predicate = null, IList<ISort> sort = null, bool buffered = false)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.GetList<T>(predicate, sort, buffered: buffered);
                    if (result != null)
                    {
                        return result.ToList<T>();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return null;
            }
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">全部记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public List<T> GetPage(int pageIndex, int pageSize, out int allRowsCount, object predicate = null, IList<ISort> sort = null, bool buffered = true)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.GetPage<T>(predicate, sort, pageIndex, pageSize, buffered: buffered);
                    allRowsCount = DbCon.Count<T>(predicate);
                    if (result != null)
                    {
                        return result.ToList<T>();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                allRowsCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public dynamic Insert(T entity)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    var result = DbCon.Insert<T>(entity);

                    return result;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return null;
            }
        }

        /// <summary>
        /// 使用事务批量添加数据
        /// </summary>
        /// <param name="entityList">要添加的数据</param>
        /// <returns></returns>
        public void InsertBatch(IEnumerable<T> entityList)
        {
            if (entityList == null || entityList.Count() <= 0)
            {
                return;
            }
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                var trans = DbCon.BeginTransaction();
                try
                {
                    DbCon.Insert<T>(entityList, trans);
                    trans.Commit();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">要更新的记录</param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                {
                    return DbCon.Update<T>(entity);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return false;
            }
        }

        /// <summary>
        /// 使用事务批量更新数据
        /// </summary>
        /// <param name="entityList">数据列表</param>
        /// <returns></returns>
        public bool UpdateBatch(IEnumerable<T> entityList)
        {
            if (entityList == null || entityList.Count() <= 0)
            {
                return false;
            }
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                int successCount = 0;
                var trans = DbCon.BeginTransaction();
                try
                {
                    foreach (T one in entityList)
                    {
                        if (DbCon.Update<T>(one, trans))
                        {
                            successCount++;
                        }
                    }
                    if (successCount == entityList.Count())
                    {
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="primaryId"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool Delete(object primaryId)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                try
                { 
                return DbCon.Delete<T>(primaryId);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return false;
            }
        }

        /// <summary>
        /// 使用事务按条件批量删除数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public bool DeleteList(object predicate)
        {
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                var trans = DbCon.BeginTransaction();
                bool isOK = false;
                try
                {
                    isOK = DbCon.Delete<T>(predicate, trans);
                    trans.Commit();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return isOK;
            }
        }

        /// <summary>
        /// 使用事务按条件批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Obsolete("这个方法由于效率相对较低尽量不要使用，请使用方法：DeleteList替代")]
        public bool DeleteBatch(IEnumerable<object> ids)
        {
            if (ids == null || ids.Count() <= 0)
            {
                return false;
            }
            using (var DbCon = SqlConnectionFactory.CreateSqlCon())
            {
                int successCount = 0;
                var trans = DbCon.BeginTransaction();
                try
                { 
                foreach (object one in ids)
                {
                    if (DbCon.Delete<T>(one, trans))
                    {
                        successCount++;
                    }
                }
                if (successCount == ids.Count())
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.Rollback();
                    return false;
                }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SqlServerCe.SqlCeException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    DBHelper.WriteLog(typeof(RepositoryBase<T>), ex);
                }
                finally
                {
                    DbCon.Close();
                    DbCon.Dispose();
                }
                return false;
            }
        }
    }
}