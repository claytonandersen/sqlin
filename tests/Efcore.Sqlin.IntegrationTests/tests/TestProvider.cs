using System.Collections.Generic;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal class TestProvider : ITestProvider
    {
        private readonly IEnumerable<ITest> _tests;

        public TestProvider(IEnumerable<ITest> tests)
        {
            _tests = tests;
        }
        public IEnumerable<ITest> GetTests()
        {
            return _tests;
        }
    }
}