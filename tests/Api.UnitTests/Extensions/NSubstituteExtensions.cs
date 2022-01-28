using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Template.Api.UnitTests.Extensions
{
    public static partial class NSubstituteExtensions
    {
        public static DbSet<T> CreateMockDbSet<T>(this IEnumerable<T> data) where T : class
        {
            return data.AsQueryable().BuildMockDbSet();
        }
    }
}
