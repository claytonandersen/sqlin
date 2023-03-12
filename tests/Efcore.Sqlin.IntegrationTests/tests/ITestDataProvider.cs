using System.Collections.Generic;
using System.Threading.Tasks;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal interface ITestDataProvider
    {
        Task SeedData(TestContext dbContext);

        /// <summary>
        /// Get the generated seed data
        /// </summary>
        /// <returns>List of generated seed data</returns>
        IList<TestIntegerKeyModel> Get();
    }
}