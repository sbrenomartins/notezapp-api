using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotezApp.Business.BusinessObjects.Requests;
using NotezApp.Business.Services;

namespace NotezApp.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        [HttpGet("check-user/{email}")]
        [AllowAnonymous]
        public IActionResult CheckUserByEmail(string email, [FromServices] SessionService sessionService)
        {
            var response = sessionService.CheckUserByEmail(email);

            if (!String.IsNullOrEmpty(response.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("authenticate-user")]
        public IActionResult AuthenticateUser([FromBody] AuthenticateUserRequest request, 
                                              [FromServices] SessionService sessionService)
        {
            var response = sessionService.Authenticate(request.Email, request.Password);

            if (response.User == null && response.Token == null)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
