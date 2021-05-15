using System.Threading.Tasks;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal interface ITest
    {
         Task RusAsync(TestContext dbContext);
    }
}