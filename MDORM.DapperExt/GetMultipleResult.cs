using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDORM.Dapper;

namespace MDORM.DapperExt
{
    /// <summary>
    /// 复合结果读取器接口
    /// </summary>
    public interface IMultipleResultReader
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        IEnumerable<T> Read<T>();
    }

    /// <summary>
    /// 表格结果读取器
    /// </summary>
    public class GridReaderResultReader : IMultipleResultReader
    {
        private readonly SqlMapper.GridReader _reader;

        public GridReaderResultReader(SqlMapper.GridReader reader)
        {
            _reader = reader;
        }

        public IEnumerable<T> Read<T>()
        {
            return _reader.Read<T>();
        }
    }

    /// <summary>
    /// 循环读取结果读取器
    /// </summary>
    public class SequenceReaderResultReader : IMultipleResultReader
    {
        private readonly Queue<SqlMapper.GridReader> _items;

        public SequenceReaderResultReader(IEnumerable<SqlMapper.GridReader> items)
        {
            _items = new Queue<SqlMapper.GridReader>(items);
        }

        public IEnumerable<T> Read<T>()
        {
            SqlMapper.GridReader reader = _items.Dequeue();
            return reader.Read<T>();
        }
    }
}