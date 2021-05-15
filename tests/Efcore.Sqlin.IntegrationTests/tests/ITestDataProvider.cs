using System.Threading.Tasks;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal interface ITestDataProvider
    {
         Task SeedData(TestContext dbContext);
    }
}