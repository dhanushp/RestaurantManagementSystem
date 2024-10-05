using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _userInterface;

        public AuthenticationController(IAuthentication userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<string>>> Register(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userInterface.Register(userRegisterDTO);

            if (result.Success)
                return Ok(result);

            return result.ErrorCode == RestaurantManagement.SharedLibrary.Data.ErrorCode.UserAlreadyExists ? Conflict(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<JwtResponseDto>>> Login(UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userInterface.Login(userLoginDTO);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        /*
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(Guid id)
        {
            var user = await _userInterface.GetUser(id);
            return user is not null ? Ok(user) : NotFound();
        }
        */

        /*
        [HttpPost("forgot-password")]
        public async Task<ActionResult<Response<string>>> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var result = await _userInterface.ForgotPassword(email);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<Response<string>>> ResetPassword(string resetToken, string newPassword)
        {
            if (string.IsNullOrEmpty(resetToken) || string.IsNullOrEmpty(newPassword))
                return BadRequest("Reset token and new password are required.");

            var result = await _userInterface.ResetPassword(resetToken, newPassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        */
    }
}
