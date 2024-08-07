using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Api.Models;

namespace Template.Api.Interfaces
{
    public interface IUserService
    {
        IList<User> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> RefreshUserById(int id);
    }
}
