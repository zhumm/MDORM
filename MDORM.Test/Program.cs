using MDORM.BusinessRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MDORM.DapperExt;
using MDORM.Entity;

namespace MDORM.Test
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
                //    Console.Write("获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetAll());
                //    Console.Write("按条件获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetList());
                //    Console.Write("分页获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetPage());
                //    Console.Write("按条件分页获取数据：");
                //    Console.WriteLine("第{0}次{1}", i, GetPageBy());
                //}
                //Console.Write(GetById());
                Console.Write(Insert());
                //Console.Write(InsertBatch());
                //Console.Write(Delete());
                //Console.Write(DeleteList());
                //Console.Write(GetPage());
                //Console.Write(GetPageBy());
                Console.WriteLine("是否退出？y退出，其他继续");
                readLine = Console.ReadLine();
            }
            while (readLine != null && !readLine.Equals("y", StringComparison.CurrentCultureIgnoreCase));
        }

        private static string GetById()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Person result = PersonRepository.Value.GetById(300);
            sw.Stop();
            return string.Format("按主键获取记录:：{0}，耗时：{1}毫秒", result.ToString(), sw.ElapsedMilliseconds);
        }

        private static string GetAll()
        {
            var sw = new Stopwatch();
            sw.Start();
            IList<Person> result = PersonRepository.Value.GetAll();
            sw.Stop();
            return string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds);
        }

        private static string GetList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IPredicate pga = Predicates.Field<Person>(f => f.CrteatDate, Operator.Ge, DateTime.Now.AddMonths(-5));
            IList<Person> result = PersonRepository.Value.GetList(pga);
            sw.Stop();
            return string.Format("共获取{0}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds);
        }

        private static string Insert()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Person model;
            model = new Person();
            model.Active = true;
            model.CrteatDate = DateTime.Now;
            model.FirstName = "SingleInsertFirst";
            model.LastName = "SingleInsertFirst";
            model.Sex = 1;
            var result = PersonRepository.Value.Insert(model);
            sw.Stop();
            return string.Format("插入单条记录{0}，耗时：{1}毫秒",result, sw.ElapsedMilliseconds);
        }

        private static string InsertBatch()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Person> pList = new List<Person>();
            Person model;
            for (int i = 1; i <= 50; i++)
            {
                model = new Person();
                model.Active = true;
                model.CrteatDate = DateTime.Now;
                model.FirstName = string.Format("First_{0}", i);
                model.LastName = string.Format("Last_{0}", i);
                model.Sex = 1;
                pList.Add(model);
            }
            PersonRepository.Value.InsertBatch(pList);
            sw.Stop();
            return string.Format("共插入50条记录，耗时：{0}毫秒", sw.ElapsedMilliseconds);
        }

        private static string Delete()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            PersonRepository.Value.Delete(Predicates.Field<Person>(f => f.Id, Operator.Eq, 260));
            sw.Stop();
            return string.Format("按主键删除记录，耗时：{0}毫秒", sw.ElapsedMilliseconds);
        }

        private static string DeleteList()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IPredicate pga = Predicates.Field<Person>(f => f.Id, Operator.Gt, 300);
            PersonRepository.Value.DeleteList(pga);
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
                                        Predicates.Sort<Person>(p => p.CrteatDate)
                                    };
            IList<Person> result = PersonRepository.Value.GetPage(2, 10, out totalCount, sort: sort);
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
                                        Predicates.Sort<Person>(p => p.CrteatDate)
                                    };
            int totalCount = 0;
            var pga = new PredicateGroup() { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pga.Predicates.Add(Predicates.Field<Person>(f => f.CrteatDate, Operator.Ge, DateTime.Now.AddDays(-1)));
            IList<Person> result = PersonRepository.Value.GetPage(2, 10, out totalCount, pga, sort);
            sw.Stop();
            return string.Format("共获取{0}条记录,共{2}条记录，耗时：{1}毫秒", result.Count, sw.ElapsedMilliseconds, totalCount);
        }

        public static string Update()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Person p = new Person();
            p.Id = 300;
            p.FirstName = "updated";
            PersonRepository.Value.Update(p);
            sw.Stop();
            return string.Format("按主键更新一条记录。耗时：{0}毫秒", sw.ElapsedMilliseconds);
        }

        //public static string UpdateBatch()
        //{
        //    //Stopwatch sw = new Stopwatch();
        //    //sw.Start();
        //    //Person p = new Person();
        //    //p.Id = 300;
        //    //p.FirstName = "updated";
        //    //PersonRepository.Value.UpdateBatch()
        //    //sw.Stop();
        //    //return string.Format("按主键更新一条记录。耗时：{0}毫秒", sw.ElapsedMilliseconds);
        //}
    }
}