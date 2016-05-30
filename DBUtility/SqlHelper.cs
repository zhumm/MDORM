///=========================================================================
/// 版    权： Coypright 2012 - 2014 @ 朱明明
/// 文件名称： SqlHelper.cs
/// 模版作者： 朱明明 最后修改于 2014-3-20 16:10:10
/// 作者邮箱： zhumingming1040@163.com,937553351@qq.com
/// 描    述： 数据库操作方法集合。添加配置程序集
/// 创 建 人： 朱明明 (CodeSmith V5.2.2 自动生成的代码)
/// 创建时间： 2012-3-24 14:03:11
///=========================================================================
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DBUtility
{
    public class SqlHelper
    {
        public static string ConnectionString = PubConstant.ConnectionString;

        public static event EventHandler BeforeExecuting = null;

        private static void OnBeforeExecuting(SqlCommand command, EventArgs e)
        {
            if (BeforeExecuting == null)
                return;

            BeforeExecuting(command, e);
        }

        /// <summary>
        /// Get Sql Connection
        /// </summary>
        /// <param name="connectionString">connection string</param>
        /// <returns>Database Connection</returns>
        public static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="strSql">SQL语句</param>
        /// <param name="cmdType">CommandType枚举</param>
        /// <param name="cmdParms">SQL参数</param>
        /// <returns></returns>
        public static bool Exists(string connectionString,string strSql,CommandType cmdType, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(connectionString,strSql, cmdType,cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <param name="cmdType">CommandType类型</param>
        /// <param name="cmdParms">SQL参数</param>
        /// <returns>
        /// 查询结果（object）
        /// </returns>
        public static object GetSingle(string connectionString, string SQLString, CommandType cmdType, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, cmdType,SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
#if DEBUG
                        throw e;
#endif
                        return null;
                    }
                }
            }
        }

 

        /// <summary>
        /// 执行插入,修改,删除操作语句
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            int val = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParameters);
                    conn.Open();
                    OnBeforeExecuting(cmd, EventArgs.Empty);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    #if DEBUG
                    throw e;
                    #endif
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                        conn.Dispose();
                    }
                }
            }

            return val;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="keep">open connection</param>
        /// <returns>int</returns>
        public static int ExecuteNonQuery(SqlCommand cmd, bool keep)
        {
            if (cmd == null)
                return 0;

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            OnBeforeExecuting(cmd, EventArgs.Empty);
            if (keep)
                return cmd.ExecuteNonQuery();

            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#endif
                return 0;

            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
           
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        public static void ExecuteNonQueryTrans(SqlConnection conn, SqlCommand cmd, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            int val = 0;
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
            OnBeforeExecuting(cmd, EventArgs.Empty);
            val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();


        }

        /// <summary>
        /// 返回SqlDataReader
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                conn.Open();
                OnBeforeExecuting(cmd, EventArgs.Empty);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#endif
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return dr;
        }
    
        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = 80;  //超时时间搞长一点，复杂查询需要更多时间。
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    OnBeforeExecuting(cmd, EventArgs.Empty);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#endif
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = 80;  //超时时间搞长一点，复杂查询需要更多时间。
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    OnBeforeExecuting(cmd, EventArgs.Empty);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#endif
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet_NoTimeLimit(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = 600;  //超时时间搞长一点，复杂查询需要更多时间。10分钟
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    OnBeforeExecuting(cmd, EventArgs.Empty);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#endif
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return ds;
        }




        /// <summary>
        /// 返回第一行第一列数据:洪拾金
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            object o = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    conn.Open();
                    OnBeforeExecuting(cmd, EventArgs.Empty);
                    o = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
#if DEBUG
                    throw e;
#endif
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            return o;
        }

        /// <summary>
        /// 返回第一行第一列数据:洪拾金
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalarWithTransaction(SqlConnection conn, SqlCommand cmd, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            object o = null;
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OnBeforeExecuting(cmd, EventArgs.Empty);
                o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#endif
            }

            return o;
        }

        /// <summary>
        /// 返回DataTable的分页
        /// </summary>
        /// <param name="pageCount">共有多少页</param>
        /// <param name="pageSize">每页显示的记录条数</param>
        /// <param name="pagePrve">当前显示第几页</param>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(ref int pageCount, int pageSize, int pagePrve, string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            int startRec;
            int endRec;
            int recNo;

            DataTable dtTemp;
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OnBeforeExecuting(cmd, EventArgs.Empty);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                cmd.Parameters.Clear();

                #region 分页

                int maxRec = ds.Tables[0].DefaultView.Table.Rows.Count;//总条数
                pageCount = (maxRec / pageSize);//共有多少页
                if ((maxRec % pageSize) > 0)//取余数
                {
                    pageCount++;
                }

                recNo = pageSize * (pagePrve - 1);


                dtTemp = ds.Tables[0].DefaultView.Table.Clone();
                if (pagePrve == pageCount) endRec = maxRec;
                else endRec = pageSize * pagePrve;
                startRec = recNo;
                for (int i = startRec; i < endRec; i++)
                {
                    dtTemp.ImportRow(ds.Tables[0].DefaultView.Table.Rows[i]);
                    recNo++;
                }
                conn.Close();
                #endregion
            }
            catch (Exception e)
            {
#if DEBUG
                throw e;
#endif
                dtTemp = null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return dtTemp;
        }

        /// <summary>
        /// 预处理Command
        /// </summary>
        /// <param name="cmd">要处理的Command对象</param>
        /// <param name="conn">连接字符串</param>
        /// <param name="trans">数据库事务对象</param>
        /// <param name="cmdType">CommandType枚举</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="cmdParms">SQL参数列表</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            DateTime dt;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms == null)
                return;

            foreach (SqlParameter parm in cmdParms)
            {
                if (parm == null)
                    continue;

                if (parm.Value == null)
                    parm.SqlValue = DBNull.Value;
                if (parm.Value is DateTime)
                {
                    dt = (DateTime)parm.Value;
                    if (dt <= DateTime.MinValue || dt >= DateTime.MaxValue)
                        parm.SqlValue = DBNull.Value;
                }
                cmd.Parameters.Add(parm);
            }
        }

        

        //分頁sql語句
        /*
         * DECLARE @pagenum AS INT, @pagesize AS INT
            SET @pagenum = 3
            SET @pagesize = 5
                      SELECT *
                    FROM (SELECT ROW_NUMBER() OVER (ORDER BY Guid DESC) AS rownum, *
                    FROM SaleOrderInfo) AS D
            WHERE rownum BETWEEN (@pagenum - 1) * @pagesize + 1 AND @pagenum * @pagesize
            -------------------------------------------
            SELECT TOP 10 *
            FROM (SELECT ROW_NUMBER() OVER (ORDER BY Guid DESC) AS rownum, *
            FROM SaleOrderInfo) AS D
            WHERE rownum > 40
         */
        public static string PageFile(int pageSize, string keys, string AscBy) //分页头部分
        {
            if (pageSize != 0)
            {
                return " top " + pageSize.ToString() + " * from (SELECT ROW_NUMBER() OVER (ORDER BY " + keys + " " + AscBy + ") AS RowNum,";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string PageWhere(int pagePrve, int pageSize) //分页尾部分
        {
            if (pageSize == 0 && pagePrve == 0)
            {
                return string.Empty;
            }
            else
            {
                string IniRow = Convert.ToString((pagePrve - 1) * pageSize);
                return ") as NewTable WHERE RowNum > " + IniRow;
            }
        }
        public static int PageCount(string FileName, int pageSize, string sqlPageCount, string connectionString, CommandType cmdType, params SqlParameter[] commandParameters) //返回总页数
        {
            try
            {
                if (pageSize != 0)
                {
                    SqlDataReader dr = ExecuteReader(connectionString, CommandType.Text, sqlPageCount, commandParameters);
                    dr.Read();
                    int recordCount = (int)dr[FileName];
                    dr.Close();
                    return (int)(recordCount / pageSize) + (recordCount % pageSize > 0 ? 1 : 0);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static int PageCount(string FileName, ref int recordCount, int pageSize, string sqlPageCount, string connectionString, CommandType cmdType, params SqlParameter[] commandParameters) //返回总页数
        {
            try
            {
                if (pageSize != 0)
                {
                    SqlDataReader dr = ExecuteReader(connectionString, CommandType.Text, sqlPageCount, commandParameters);
                    dr.Read();
                    recordCount = (int)dr[FileName];
                    dr.Close();
                    return (int)(recordCount / pageSize) + (recordCount % pageSize > 0 ? 1 : 0);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
