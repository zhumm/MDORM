using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 获取复合结果谓词
    /// </summary>
    public class GetMultiplePredicate
    {
        private readonly List<GetMultiplePredicateItem> _items;

        public GetMultiplePredicate()
        {
            _items = new List<GetMultiplePredicateItem>();
        }

        public IEnumerable<GetMultiplePredicateItem> Items
        {
            get { return _items.AsReadOnly(); }
        }

        public void Add<T>(IPredicate predicate, IList<ISort> sort = null) where T : class
        {
            _items.Add(new GetMultiplePredicateItem
                           {
                               Value = predicate,
                               Type = typeof(T),
                               Sort = sort
                           });
        }

        public void Add<T>(object id) where T : class
        {
            _items.Add(new GetMultiplePredicateItem
                           {
                               Value = id,
                               Type = typeof (T)
                           });
        }

        /// <summary>
        /// 获取复合谓词项
        /// </summary>
        public class GetMultiplePredicateItem
        {
            /// <summary>
            /// 获取或设置值
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value { get; set; }

            /// <summary>
            /// 获取或设置类型
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public Type Type { get; set; }

            /// <summary>
            /// 获取或设置排序列表
            /// </summary>
            /// <value>
            /// The sort.
            /// </value>
            public IList<ISort> Sort { get; set; }
        }
    }
}