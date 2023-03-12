using System;
using System.Linq;
using System.Threading.Tasks;
using Efcore.Sqlin.IntegrationTests.tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Efcore.Sqlin.IntegrationTests
{
    class Program
    {
        const string SqlServer = "sqlserver";
        const string PostgresSql = "pgsql";

        static async Task Main(string[] args)
        {
            var connectionString = (args?.Any()).GetValueOrDefault() ? args[0] : "Server=localhost;Database=sqlin;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=true;";
            Console.WriteLine("EFCore Sqlin Integration Tests\nStarting up...");
            Console.WriteLine($"Sql Connection string registered as: {connectionString}");

            var engine = getDbEngine(args);
            
            var host = CreateHostBuilder(new string[] { connectionString, engine }).Build();

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.AddDbContext<TestContext>(options => {
                        DbContextOptionsBuilder builder = null;
                        if (args[1] == SqlServer)
                        {
                            builder = options.UseSqlServer(args[0]);
                        }
                        else if (args[1] == PostgresSql)
                        {
                            builder = options.UseNpgsql(args[0]);
                        }

                        builder.EnableDetailedErrors()
                               .EnableSensitiveDataLogging()
                               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    })
                    .AddHostedService<TestRunner>()
                    .AddTests()
                );

        static string getDbEngine(string[] args)
        {
            string defaultDbEngine = SqlServer;
            string[] alternateDbEngineOptions = new[] { defaultDbEngine, "pgsql" };

            var engine = (args?.Any()).GetValueOrDefault()
                ? args[1].ToLowerInvariant()
                : defaultDbEngine;

            if (alternateDbEngineOptions.Contains(engine))
            {
                Console.WriteLine($"Selected Database engine: {engine}");
                return engine;
            }

            Console.WriteLine($"Selected default Database engine: {engine}");
            return engine;
        }
    }
}
