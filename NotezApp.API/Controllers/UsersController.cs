using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotezApp.Business.BusinessObjects.Requests;
using NotezApp.Business.Services;
using System.Security.Claims;

namespace NotezApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("list-users")]
        [Authorize(Roles = "admin")]
        public IActionResult ListUsers([FromServices] UserService userService)
        {
            var users = userService.ListUsersAsync();
            return Ok(users);
        }

        [HttpPut("update-user")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest,
                                                    [FromServices] UserService userService)
        {
            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await userService.UpdateUser(userId, updateUserRequest);

            if (response.User == null)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest, 
                                                    [FromServices] UserService userService)
        {
            var response = await userService.CreateUser(createUserRequest.Name,
                                                        createUserRequest.Email,
                                                        createUserRequest.Password);

            if (response.Exists && response.User == null)
            {
                return Conflict(response);
            }

            if (!response.Exists && response.User == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount([FromServices] UserService userService)
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var response = await userService.DeleteAccount(userId);

            if (response)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
