using System.Collections.Generic;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal interface ITestProvider
    {
         IEnumerable<ITest> GetTests();
    }
}