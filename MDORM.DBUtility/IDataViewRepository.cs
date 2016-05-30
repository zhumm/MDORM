using System.Collections.Generic;
using System.Data;
using MDORM.DapperExt;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 数据视图仓库接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataViewRepository<T> where T:class
    {
        /// <summary>
        /// 获取全部记录
        /// </summary>
        /// <returns>全部记录</returns>
        List<T> GetAll();

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <param name="predicate">查找条件</param>
        /// <returns>满足条件的数据条数</returns>
        int Count(object predicate);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns>满足条件的数据列表</returns>
        List<T> GetList(object predicate = null, IList<ISort> sort = null, bool buffered = false);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">全部记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns>当前页数据</returns>
        List<T> GetPage(int pageIndex, int pageSize, out int allRowsCount, object predicate = null, IList<ISort> sort = null, bool buffered = true);
    }
}