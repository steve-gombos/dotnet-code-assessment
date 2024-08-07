using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Template.Api.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static T AddInMemoryDbContext<T>(this IServiceCollection services, string databaseName,
            Action<WarningsConfigurationBuilder> warningsConfig = null) where T : DbContext
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<T>));

            services.Remove(descriptor);
            services.RemoveAll<T>();
            services.AddDbContext<T>(options =>
            {
                var db = options.UseInMemoryDatabase(databaseName);

                if (warningsConfig != null)
                    db.ConfigureWarnings(warningsConfig);
            });

            var context = services.GetService<T>();
            return context;
        }

        public static T GetService<T>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            return provider.GetRequiredService<T>();
        }
    }
}
