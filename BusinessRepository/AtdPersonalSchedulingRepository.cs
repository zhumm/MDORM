
///=========================================================================
/// 版    权： Coypright 2012 - 2016 @ 朱明明个人
/// 文件名称： GroupPower_AtdPersonalSchedulingBLL.cs
/// 模版作者： 朱明明 最后修改于 2014-3-20 16:10:10
/// 作者邮箱： zhumingming1040@163.com,937553351@qq.com
/// 描    述： 单独生成的代码，请在代码生成后手动添加【描述】信息
/// 创 建 人： 朱明明 (CodeSmith V5.2.2 自动生成的代码 模板V3.0.5)
/// 创建时间： 2016/5/23 14:52:26
///=========================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using DBUtility;
using DapperExtensions;
using Entity;

namespace BusinessRepository
{
    /// <summary>
    /// 表[GroupPower_AtdPersonalScheduling]的BU层代码
    /// </summary>
    /// 创建人：朱明明
    /// 创建时间：2016/5/23 14:52:26
    public partial class AtdPersonalSchedulingRepository:RepositoryBase<AtdPersonalScheduling>
        
    {
        #region 静态AtdPersonalSchedulingRepository对象
        private static AtdPersonalSchedulingRepository _value;

        /// <summary>
        /// 静态AtdPersonalSchedulingRepository对象，单例模式
        /// </summary>
        public static AtdPersonalSchedulingRepository Value
        {
            get
            {
                if (AtdPersonalSchedulingRepository._value == null)
                    AtdPersonalSchedulingRepository._value = new AtdPersonalSchedulingRepository();
                return AtdPersonalSchedulingRepository._value;
            }
        }
        #endregion

        #region 构造函数
        public AtdPersonalSchedulingRepository()
        {
            AtdPersonalSchedulingRepository._value = this;
        }
        #endregion

        #region 基本方法
        /// <summary>
        /// 分页获取,默认按照时间降序排序
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="allRowsCount">全部记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        public List<AtdPersonalScheduling> GetPage(int pageIndex, int pageSize, out int allRowsCount, object predicate = null, IList<ISort> sort = null)
        {
            if (sort == null || sort.Count <= 0)
            {
                sort.Add(Predicates.Sort<AtdPersonalScheduling>(p => p.CreateDate, true));
            }
            return base.GetPage(pageIndex, pageSize, out allRowsCount, predicate, sort);
        }
        #endregion

        #region 扩展的方法
        #endregion
    }
}
