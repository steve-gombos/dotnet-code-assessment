using System.Threading.Tasks;
using Template.Api.Models;

namespace Template.Api.Interfaces
{
    public interface ISomeSystemApiClient
    {
        Task<UserStatusCollectionDto> GetAllUserStatuses();
    }
}
