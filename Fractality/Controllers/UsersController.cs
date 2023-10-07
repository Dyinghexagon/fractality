using Fractality.Models.Backend;
using Fractality.Models.Frontend;
using Fractality.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _usersServices;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUsersServices userServices, 
            ILogger<UsersController> logger
        )
        {
            _usersServices = userServices;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]UserModel userModel)
        {
            try
            {
                var id = await _usersServices.AddAsync(userModel);
                return Ok(id);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }

        }

    }
}
