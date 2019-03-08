using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace TestGinIndex.Models
{
    public class TestDbContext : DbContext
    {
        public DbSet<BarInfo> Bars { get; set; }
        public DbSet<FooInfo> Foos { get; set; }

        private static readonly LoggerFactory loggerFactory = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseNpgsql("Server=localhost;Database=TestFullTextSearch2;User Id=postgres;Password=abcABC123;Port=5433");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_trgm");
            modelBuilder.Entity<BarInfo>().HasIndex(x => x.Guid).ForNpgsqlHasMethod("gin").ForNpgsqlHasOperators("gin_trgm_ops");
            modelBuilder.Entity<FooInfo>().HasIndex(x => x.Guid).ForNpgsqlHasMethod("gin").ForNpgsqlHasOperators("gin_trgm_ops");
        }
    }

    public class BarInfo
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public IEnumerable<FooInfo> Foo { get; set; }
    }

    public class FooInfo
    {
        public int Id { get; set; }
        public int BarId { get; set; }
        public string Guid { get; set; }
        public BarInfo Bar { get; set; }
    }
}