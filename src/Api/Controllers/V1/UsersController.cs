using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Api.Interfaces;
using Template.Api.Models;

namespace Template.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = nameof(GetUsers))]
        public ActionResult<IList<User>> GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        public async Task<ActionResult<User>> GetUserById([FromQuery(Name = "id")] int userId)
        {
            var user = await _userService.GetUserById(userId);
            return Ok(user);
        }

        [HttpPost("{id}/refresh", Name = nameof(RefreshUserById))]
        public async Task<ActionResult<User>> RefreshUserById([FromRoute(Name = "id")] int userId)
        {
            var user = await _userService.RefreshUserById(userId);
            return Ok(user);
        }
    }
}
