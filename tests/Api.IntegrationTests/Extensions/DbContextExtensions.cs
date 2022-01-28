using Microsoft.EntityFrameworkCore;
using System;

namespace Template.Api.IntegrationTests.Extensions
{
    public static class DbContextExtensions
    {
        public static T Seed<T>(this T context, Action<T> action) where T : DbContext
        {
            action(context);

            return context;
        }
    }
}
