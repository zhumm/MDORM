
///=========================================================================
/// 版    权： Coypright 2012 - 2014 @ 朱明明个人
/// 文件名称： PageArgs.cs
/// 模版作者： 朱明明 最后修改于 2014-3-20 16:10:10
/// 作者邮箱： zhumingming1040@163.com,937553351@qq.com
/// 描    述： 分页参数类
/// 创 建 人： 朱明明 (CodeSmith V5.2.2 自动生成的代码)
/// 创建时间： 2014-4-9 10:16:11
///=========================================================================

using System;

namespace MDORM.DapperExt.Utility
{
    /// <summary>
    /// 分页参数类
    /// </summary>
    /// 创建人：朱明明
    /// 创建时间：2014-4-9 10:16:11
    [Serializable]
    public class PageArgs
    {
        #region 分页属性
        private int _pageIndex = 1;

        /// <summary>
        /// 获取或设置当前页索引,第一页索引为 1
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                if (value == 0)
                    _pageIndex = 1;
                else
                    _pageIndex = value;
            }
        }

        private int _pageSize = 50;

        /// <summary>
        /// 获取或设置页大小
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value <= 0)
                    _pageSize = 5;
                else
                    _pageSize = value;
            }
        }

        private int _totalRecord = 0;

        /// <summary>
        /// 获取总记录数
        /// </summary>
        public int TotalRecord
        {
            get
            {
                return _totalRecord;
            }
            set
            {
                _totalRecord = value;
            }
        }

        /// <summary>
        /// 获取记录总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (_totalRecord <= 0)
                    return 1;
                int tempPageCount = 0;
                int tempOther = 0;
                tempOther = _totalRecord % _pageSize;
                if (tempOther == 0)
                    tempPageCount = _totalRecord / _pageSize;
                else
                    tempPageCount = (_totalRecord / _pageSize) + 1;


                return tempPageCount;
            }
        }
        #endregion

        #region 扩展属性
        #endregion

    }
}
