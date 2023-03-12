using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Logging;
using System;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    internal class BogusTestDataProvider : ITestDataProvider
    {
        private const int NumberOfRows = 1000;
        private IList<TestIntegerKeyModel> _bogusData = null;

        private bool _seedComplete = false;

        private readonly ILogger<BogusTestDataProvider> _logger;

        public BogusTestDataProvider(ILogger<BogusTestDataProvider> logger)
        {
            _logger = logger;
        }

        public async Task SeedData(TestContext dbContext)
        {
            if (_seedComplete)
            {
                _logger.LogInformation("Skipping seed, data already created");
                return;
            }

            var faker = new Faker<TestIntegerKeyModel>()
                .RuleFor(p => p.Name, v => v.Name.FullName())
                .RuleFor(p => p.Email, v => v.Internet.Email())
                ;

            _bogusData = faker.Generate(NumberOfRows);

            //clear out all the rows
            dbContext.RemoveRange(dbContext.TestEfIntModel);
            dbContext.RemoveRange(dbContext.TestEfBigIntModel);
            dbContext.RemoveRange(dbContext.TestGuidModel);

            //insert the new data
            await dbContext.AddRangeAsync(_bogusData);
            await dbContext.AddRangeAsync(_bogusData.Select(bd => new TestBigIntegerKeyModel
            {
                Id = bd.Id,
                Name = bd.Name,
                Email = bd.Email
            }));

            await dbContext.AddRangeAsync(_bogusData.Select(bd => new TestGuidKeyModel
            {
                Id = ToGuid(bd.Id),
                Name = bd.Name,
                Email = bd.Email
            }));

            //execute both above commands
            await dbContext.SaveChangesAsync();
            _seedComplete = true;
        }

        public IList<TestIntegerKeyModel> Get()
        {
            return _bogusData;
        }

        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}