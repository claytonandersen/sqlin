using System.Threading.Tasks;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal class BasicInTest : ITest
    {
        public Task RusAsync(TestContext dbContext)
        {
            return Task.CompletedTask;
        }
    }
}