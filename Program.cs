using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestGinIndex.Models;

namespace TestGinIndex
{
    class Program
    {

        static IEnumerable<BarInfo> GetMockData()
        {
            var bars = new List<BarInfo>();
            foreach (var index in Enumerable.Range(0, 1000000))
            {
                bars.Add(new BarInfo
                {
                    Guid = $"{index}{Guid.NewGuid()}-{Guid.NewGuid()}",
                    Foo = new List<FooInfo>()
                    {
                        new FooInfo()
                        {
                            Guid = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
                        }
                    }
                });
            }
            return bars;
        }

        static void GenerateMockData()
        {
            foreach (var mock in GetMockData())
            {
                using (var db = new TestDbContext())
                {
                    db.Bars.Add(mock);
                    db.SaveChanges();
                }
            }
        }

        static void Main(string[] args)
        {

            using (var db = new TestDbContext())
            {
                // db.Database.EnsureDeleted();
                // db.Database.EnsureCreated();
                // GenerateMockData();
                var searchStr = new List<string> { "สวัสดี", "ข้าวอยู่ในนา", "ฮาเลลูย่า", "111" };
                var likeQuery = searchStr.Select(x => $"%{x}%").ToList();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var count = db.Bars.Where(x => likeQuery.Any(y => EF.Functions.Like(x.Guid, y)) || x.Foo.Any(y => likeQuery.Any(z => EF.Functions.Like(y.Guid, z)))).Count();
                stopWatch.Stop();

                Console.WriteLine($"Found {count} (total time: {stopWatch.Elapsed.TotalMilliseconds})");

            }

        }
    }
}
