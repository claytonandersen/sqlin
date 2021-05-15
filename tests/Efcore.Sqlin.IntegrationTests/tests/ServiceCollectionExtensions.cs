using Microsoft.Extensions.DependencyInjection;

namespace Efcore.Sqlin.IntegrationTests.tests
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTests(this IServiceCollection services)
        {
            services.AddSingleton<ITestProvider, TestProvider>()
                .AddSingleton<ITestDataProvider, BogusTestDataProvider>()
                .AddSingleton<ITest, BasicInTest>()
            ;
        }    
    }
}