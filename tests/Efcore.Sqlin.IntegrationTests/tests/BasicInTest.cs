using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal class BasicInTest : ITest
    {
        private IList<TestIntegerKeyModel> _testData => _testDataProvider.Get();
        private readonly ITestDataProvider _testDataProvider;
        private readonly ILogger<BasicInTest> _logger;

        public BasicInTest(ITestDataProvider testDataProvider, ILogger<BasicInTest> logger)
        {
            _testDataProvider = testDataProvider;
            _logger = logger;
        }

        public async Task RunAsync(TestContext dbContext)
        {
            var timeSpent = await StopwatchUtilities.ExecuteTimedTaskAsync<TestContext>(RunFirst50Async, dbContext);
            _logger.LogInformation($"Completed First50 Test in: {timeSpent}");

            var timeSpentws = await StopwatchUtilities.ExecuteTimedTaskAsync<TestContext>(RunWholeSetAsync, dbContext);
            _logger.LogInformation($"Completed WholeSet Test in: {timeSpentws}");
        }

        private async Task RunFirst50Async(DbContext dbContext)
        {
            var first50 = _testData.Take(50).Select(d => (long)d.Id).ToArray();
            var query = ((TestContext)dbContext).TestEfBigIntModel.Where(td => first50.Contains(td.Id));
            var items = await query.ToArrayAsync();
#if DEBUG
            if (items.Length != 50)
            {
                throw new Exception("First 50 test failed, did not return 50 items");
            }
#endif
        }

        private async Task RunWholeSetAsync(DbContext dbContext)
        {
            var dataSet = _testData.Select(d => (long)d.Id).ToArray();
            var query = ((TestContext)dbContext).TestEfBigIntModel.Where(td => dataSet.Contains(td.Id));
            var items = await query.ToArrayAsync();
#if DEBUG
            if (items.Length != dataSet.Length)
            {
                throw new Exception("First 50 test failed, did not return 50 items");
            }
#endif
        }
    }
}