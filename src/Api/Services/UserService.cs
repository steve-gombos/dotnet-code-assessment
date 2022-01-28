using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Api.Interfaces;
using Template.Api.Models;
using Template.Api.Persistence;

namespace Template.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly ISomeSystemApiClient _someSystemApiClient;

        public UserService(UserContext context, ISomeSystemApiClient someSystemApiClient)
        {
            _context = context;
            _someSystemApiClient = someSystemApiClient;
        }

        public IList<User> GetUsers()
        {
            return _context.Users.Where(u => u.IsActive).ToList();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> RefreshUserById(int id)
        {
            // Get user from data context
            var user = await GetUserById(id);

            // Get all source user data
            var sourceUserData = await _someSystemApiClient.GetAllUserStatuses();

            // Filter source user data to current user id
            var userStatus = sourceUserData.Users.First(u => u.UserId == id);

            user.IsActive = userStatus.IsActive;
            user.IsComplete = userStatus.IsComplete;

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
