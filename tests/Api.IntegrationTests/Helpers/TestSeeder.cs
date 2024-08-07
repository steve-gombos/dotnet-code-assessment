using Template.Api.Models;
using Template.Api.Persistence;

namespace Template.Api.IntegrationTests.Helpers
{
    public static class TestSeeder
    {
        public static void Seed(UserContext context)
        {
            context.Users.Add(new User
            {
                Id = 1,
                FirstName = "user",
                LastName = "one",
                IsComplete = false,
                IsActive = true
            });

            context.Users.Add(new User
            {
                Id = 2,
                FirstName = "user",
                LastName = "two",
                IsComplete = true,
                IsActive = true
            });

            context.Users.Add(new User
            {
                Id = 3,
                FirstName = "user",
                LastName = "three",
                IsComplete = false,
                IsActive = true
            });

            context.SaveChanges();
        }
    }
}
