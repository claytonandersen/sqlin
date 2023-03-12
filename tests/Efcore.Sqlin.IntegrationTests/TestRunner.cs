using System.Diagnostics;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Efcore.Sqlin.IntegrationTests.tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Efcore.Sqlin.IntegrationTests
{
    internal class TestRunner : IHostedService
    {
        private readonly ILogger<TestRunner> _logger;
        private readonly TestContext _dbContext;
        private readonly ITestProvider _testProvider;
        private readonly ITestDataProvider _testDataProvider;
        private readonly IHost _host;

        public TestRunner(ILogger<TestRunner> logger,
            TestContext dbContext,
            ITestProvider testProvider,
            ITestDataProvider testDataProvider,
            IHost host)
        {
            _logger = logger;
            _dbContext = dbContext;
            _testProvider = testProvider;
            _testDataProvider = testDataProvider;
            _host = host;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Setting up db-schema");
            await EnsureSchemaIsCreated(cancellationToken);

            _logger.LogInformation("Seeding Data");

            await _testDataProvider.SeedData(_dbContext);

            _logger.LogInformation("Executing integration tests");

            var sw = new Stopwatch();
            sw.Start();
            var testCount = 0;
            foreach (var test in _testProvider.GetTests())
            {
                await test.RunAsync(_dbContext);
                testCount++;
            }
            sw.Stop();

            _logger.LogInformation("Tests Completed");
            _logger.LogInformation($"Executed {testCount} tests in {sw.Elapsed.TotalMicroseconds}");

            await _host.StopAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {typeof(TestRunner).Assembly.FullName}");
            return Task.CompletedTask;
        }

        private async Task EnsureSchemaIsCreated(CancellationToken cancellationToken)
        {
            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

            _logger.LogInformation("db-schema updated");
        }
    }
}