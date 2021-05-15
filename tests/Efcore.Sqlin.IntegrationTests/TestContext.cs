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
            
        }

        public virtual DbSet<TestEfModel> TestEfModel { get; set; }
    }
}