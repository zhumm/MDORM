using System.Collections.Generic;
using System.Data;
using MDORM.DapperExt;

namespace MDORM.DBUtility
{
    /// <summary>
    /// 数据仓库接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataRepository<T> where T:class
    {
        /// <summary>
        /// 通过ID获取单条记录
        /// </summary>
        /// <param name="primaryId">动态类型的ID</param>
        /// <returns>单个实体</returns>
        T GetById(object primaryId);

        /// <summary>
        /// 获取全部记录
        /// </summary>
        /// <returns>全部记录</returns>
        List<T> GetAll();

        /// <summary>
        /// 获取满足条件的记录条数
        /// </summary>
        /// <param name="predicate">查找条件</param>
        /// <returns>满足条件的数据条数</returns>
        int Count(object predicate);

        /// <summary>
        /// 获取满足条件的数据列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序列表</param>
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

        /// <summary>
        /// 插入一条数据并返回该记录ID
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>该记录ID</returns>
        dynamic Insert(T entity);

        /// <summary>
        /// 使用事务批量插入
        /// </summary>
        /// <param name="entityList">实体列表</param>
        /// <returns>是否成功</returns>
        bool InsertBatch(IEnumerable<T> entityList);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">一个实体（主键必须有，其他的按需要更新）</param>
        /// <returns>是否成功</returns>
        bool Update(T entity);

        /// <summary>
        /// 使用事务批量更新
        /// </summary>
        /// <param name="entityList">要更新的实体列表</param>
        /// <returns>是否成功</returns>
        bool UpdateBatch(IEnumerable<T> entityList);

        /// <summary>
        /// 删除满足条件的数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否成功</returns>
        bool Delete(object predicate);

        /// <summary>
        /// 使用事务删除满足条件的数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>是否成功</returns>
        bool DeleteList(object predicate);

        /// <summary>
        /// 使用事务批量删除
        /// </summary>
        /// <param name="ids">ID列表</param>
        /// <returns>是否成功</returns>
        bool DeleteBatch(IEnumerable<object> ids);
    }
}