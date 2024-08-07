using System;
using System.Threading.Tasks;
using Template.Api.Interfaces;
using Template.Api.Models;

namespace Template.Api.Services
{
    public class SomeSystemApiClient : ISomeSystemApiClient
    {
        public async Task<UserStatusCollectionDto> GetAllUserStatuses()
        {
            throw new NotImplementedException();
        }
    }
}
