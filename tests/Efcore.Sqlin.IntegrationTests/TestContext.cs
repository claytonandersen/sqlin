using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Efcore.Sqlin.IntegrationTests
{
    internal class TestContext : DbContext
    {
        public TestContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        protected TestContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestIntegerKeyModel>(e =>
            {
                e.HasIndex(i => i.Id, "PK_TestIntegerKeyModel");
            });

            modelBuilder.Entity<TestBigIntegerKeyModel>(e =>
            {
                e.HasIndex(i => i.Id, "PK_TestBigIntegerKeyModel");
                e.Property(p => p.Id).HasColumnType("BIGINT");
            });

            modelBuilder.Entity<TestGuidKeyModel>(e =>
            {
                e.HasIndex(i => i.Id, "PK_TestGuidKeyModel");
            });
        }

        public virtual DbSet<TestIntegerKeyModel> TestEfIntModel { get; set; }

        public virtual DbSet<TestBigIntegerKeyModel> TestEfBigIntModel { get; set; }

        public virtual DbSet<TestGuidKeyModel> TestGuidModel { get; set; }
    }
}