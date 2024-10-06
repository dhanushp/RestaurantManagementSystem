using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userInterface;

        public UserController(IUser userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<UserResponseDTO>>>> GetAllUsers()
        {
            var result = await _userInterface.GetAllUsers();
            return Ok(result);
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<Response<List<UserResponseDTO>>>> GetUsersByRole(Guid roleId)
        {
            var result = await _userInterface.GetUsersByRole(roleId);
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Response<UserResponseDTO>>> GetUserByEmail(string email)
        {
            var result = await _userInterface.GetUserByEmail(email);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Response<UserResponseDTO>>> GetUserById(Guid userId)
        {
            var result = await _userInterface.GetUserById(userId);
            return Ok(result);
        }

        [HttpPut("{userId}/role/{roleId}")]
        public async Task<ActionResult<Response<string>>> UpdateUserRole(Guid userId, Guid roleId)
        {
            var result = await _userInterface.UpdateUserRole(userId, roleId);
            return Ok(result);
        }

        [HttpPut("{userId}/fullname")]
        public async Task<ActionResult<Response<string>>> UpdateUserFullName(Guid userId, [FromBody] string fullName)
        {
            var result = await _userInterface.UpdateUserFullName(userId, fullName);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<Response<string>>> SoftDeleteUser(Guid userId)
        {
            var result = await _userInterface.SoftDeleteUser(userId);
            return Ok(result);
        }
    }
}
