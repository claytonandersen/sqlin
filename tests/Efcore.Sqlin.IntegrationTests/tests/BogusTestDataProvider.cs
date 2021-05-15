using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal class BogusTestDataProvider : ITestDataProvider
    {
        private const int NumberOfRows = 1000;

        public async Task SeedData(TestContext dbContext)
        {
            var bogusEfModels = new List<TestEfModel>();
            var faker = new Faker<TestEfModel>()
                .RuleFor(p => p.Name, v => v.Name.FullName())
                .RuleFor(p => p.Email, v => v.Internet.Email())
                ;

            bogusEfModels = faker.Generate(NumberOfRows);

            await dbContext.TestEfModel.AddRangeAsync(bogusEfModels);
            await dbContext.SaveChangesAsync();
        }
    }
}