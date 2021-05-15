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

        public TestRunner(ILogger<TestRunner> logger,
            TestContext dbContext,
            ITestProvider testProvider,
            ITestDataProvider testDataProvider)
        {
            _logger = logger;
            _dbContext = dbContext;
            _testProvider = testProvider;
            _testDataProvider = testDataProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Looking for sqlserver...");
            _logger.LogInformation("Note: The sql server container can take some time to start up. We will retry for 60 seconds");
            
            //await EnsureSqlServerConnectionIsValid();

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
                await test.RusAsync(_dbContext);
                testCount++;
            }
            sw.Stop();

            _logger.LogInformation("Tests Completed");
            _logger.LogInformation($"Executed {testCount} tests in {sw.Elapsed:s}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {typeof(TestRunner).Assembly.FullName}");
            return Task.CompletedTask;
        }

        private async Task EnsureSchemaIsCreated(CancellationToken cancellationToken)
        {
            var sqlText = string.Empty;
            var filename = "create-db-schema.sql";
            var folderName = "SQL";
            using (var fileStream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, folderName, filename)))
            using (var streamReader = new StreamReader(fileStream))
            {
                sqlText = await streamReader.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(sqlText))
            {
                throw new FileNotFoundException($"Could not find sql schema file {filename}");
            }

            //await _dbContext.Database.ExecuteSqlRawAsync(sqlText, cancellationToken);
            await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

            _logger.LogInformation("db-schema updated");
        }

        private async Task EnsureSqlServerConnectionIsValid()
        {
            var connected = await TryConnect();
            if (!connected)
            {
                _logger.LogCritical("Could not connect to Sql Server...\nExiting Now...");
                throw new InvalidOperationException("Cannot continue execution with disconnected Sql Server Instance");
            }
            else
            {
                _logger.LogInformation("Connected Successfully");
            }
        }

        private async Task<bool> TryConnect(int retries = 30, int retryIntervalInSeconds = 2)
        {
            var retryCount = 0;
            while (!await IsConnected()
                   && retries < retryCount)
            {
                retryCount ++;
                await Task.Delay(TimeSpan.FromSeconds(retryIntervalInSeconds));
            }

            return await IsConnected();
        }

        private async Task<bool> IsConnected()
        {
            return await _dbContext.Database.CanConnectAsync();
        }
    }
}