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
        static Task Main(string[] args)
        {
            var connectionString = (args?.Any()).GetValueOrDefault() ? args[0] : "Server=nuc;Database=testsqlin;User Id=sa;Password=Pass@word";
            Console.WriteLine("EFCore.Sqlin Integration Tests\nStarting up...");
            Console.WriteLine($"Sql Connection string registered as: {connectionString}");
            
            return CreateHostBuilder(new string[] { connectionString }).Build().RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                    services.AddDbContext<TestContext>(options => {
                        options.UseSqlServer(args[0])
                            .EnableDetailedErrors()
                            .EnableSensitiveDataLogging();
                    })
                    .AddHostedService<TestRunner>()
                    .AddTests()
                );
    }
}
