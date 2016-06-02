using System;
using System.Linq;
using System.Collections.Generic;
using MDORM.DapperExt;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 数据视图仓库实现基类
    /// </summary>
    public class ViewRepositoryBase<T> : IDataViewRepository<T> where T : class
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ViewRepositoryBase() { }

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
        /// <param name="pageIndex">页索引页索引（从0开始）</param>
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
    }
}