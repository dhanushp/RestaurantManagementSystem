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
        private readonly IUser _userInterface;

        public AuthenticationController(IUser userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userInterface.Register(userDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(UserLoginDto userLoginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userInterface.Login(userLoginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(Guid id)
        {
            var user = await _userInterface.GetUser(id);
            return user is not null ? Ok(user) : NotFound();
        }

    }
}
