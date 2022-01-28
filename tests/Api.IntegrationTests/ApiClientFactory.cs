using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using Template.Api.IntegrationTests.Extensions;
using Template.Api.IntegrationTests.Helpers;
using Template.Api.Persistence;
using Xunit.Abstractions;

namespace Template.Api.IntegrationTests
{
    public class ApiClientFactory : WebApplicationFactory<Startup>
    {
        public ITestOutputHelper Output { get; set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();

            builder.UseSerilog((context, services, configuration) => configuration
                .WriteTo.TestOutput(Output));

            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IHostedService));
                services.AddInMemoryDbContext<UserContext>("users").Seed(TestSeeder.Seed);
            });
        }
    }
}
