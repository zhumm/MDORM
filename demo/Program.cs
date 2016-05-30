using BusinessRepository;
using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DapperExtensions;
using Common;
using Entity;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var readLine = string.Empty;
            do
            {
                //for (int i = 1; i <= 10; i++)
                //{
                    Console.Write("获取数据：");
                    Console.WriteLine("第{0}次{1}", 1, GetAll());
                //    Console.Write("按条件获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetList());
                //    Console.Write("分页获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetPage());
                //    Console.Write("按条件分页获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetPageBy());
                //}
                LogHelper.WriteLog("测试日志写入");
                //Console.Write(InsertBatch());
                //Console.WriteLine("结果：{0}", usingSqlInsert());
                //Console.Write("不用SQL使用Dapper        ");
                //Console.WriteLine("结果：{0}", nosqlWithDapperInsert());
                ////Console.WriteLine("使用旧框架删除 {0}",oldDel());
                //Console.WriteLine("使用新框架删除 {0}", newDel());
                //for (int i = 1; i <= 10; i++)
                //{
                //    Console.Write("原生旧的原生sql  ");
                //    //Console.WriteLine("第{0}次。结果：{1}", i, usingSqlSearch());
                //    Console.Write("不用sql使用dapper");
                //    //Console.WriteLine("第{0}次。结果：{1}", i, nosqlWithDapperSearch());
                //    Console.WriteLine(Environment.NewLine);
                //}
                Console.WriteLine("是否退出？y退出，其他继续");
                readLine = Console.ReadLine();
            }
            while (readLine != null && !readLine.Equals("y", StringComparison.CurrentCultureIgnoreCase));
        }

        private static string GetAll()
        {
            var sw = new Stopwatch();
            sw.Start();
            IList<AtdPersonalScheduling> result = AtdPersonalSchedulingRepository.Value.GetAll();
            sw.Stop();
            return string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds);
        }

        private static string GetList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IPredicate pga=Predicates.Field<AtdPersonalScheduling>(f => f.SchedulDate, Operator.Ge, DateTime.Now.AddMonths(-5));
            IList<AtdPersonalScheduling> result = AtdPersonalSchedulingRepository.Value.GetList(pga) ;
            sw.Stop();
            return string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds);
        }

        private static string InsertBatch()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AtdPersonalScheduling model;
            for (int i = 1; i <= 50; i++)
            {
                model = new AtdPersonalScheduling();
                model.PersonalScheduID = Guid.NewGuid();
                model.PersonID = Guid.NewGuid().ToString();
                model.SchedulDate = DateTime.Now;
                model.Creator = "MDORM";
                AtdPersonalSchedulingRepository.Value.Insert(model);
            }
            sw.Stop();
            return string.Format("共插入50条记录，耗时：{0}毫秒", sw.ElapsedMilliseconds);
        }

        private static string DeleteList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IPredicate pga=Predicates.Field<AtdPersonalScheduling>(f => f.SchedulDate, Operator.Ge, DateTime.Now.AddDays(-1));
            AtdPersonalSchedulingRepository.Value.DeleteList(pga);
            sw.Stop();
            return string.Format("共删除100条记录，耗时：{0}毫秒", sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        public static string GetPage()
        {
            var sw = new Stopwatch();
            sw.Start();
            int totalCount = 0;
            IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<AtdPersonalScheduling>(p => p.CreateDate)
                                    };
            IList<AtdPersonalScheduling> result = AtdPersonalSchedulingRepository.Value.GetPage(2, 10, out totalCount, sort: sort);
            sw.Stop();
            return string.Format("共获取{0}条记录,共{2}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds, totalCount);
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        public static string GetPageBy()
        {
            var sw = new Stopwatch();
            sw.Start();
            IList<ISort> sort = new List<ISort>
                                    {
                                        Predicates.Sort<AtdPersonalScheduling>(p => p.CreateDate)
                                    };
            int totalCount = 0;
            var pga = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pga.Predicates.Add(Predicates.Field<AtdPersonalScheduling>(f => f.SchedulDate, Operator.Ge, DateTime.Now.AddDays(-1)));
            IList<AtdPersonalScheduling> result = AtdPersonalSchedulingRepository.Value.GetPage(2, 10, out totalCount, pga, sort);
            sw.Stop();
            return string.Format("共获取{0}条记录,共{2}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds, totalCount);
        }
    }
}