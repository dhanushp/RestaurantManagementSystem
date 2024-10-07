using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRole _roleInterface;

        public RolesController(IRole roleInterface)
        {
            _roleInterface = roleInterface;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<RoleResponseDTO>>>> GetAllRoles()
        {
            var result = await _roleInterface.GetAllRoles();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Response<RoleResponseDTO>>> CreateRole([FromBody] RoleCreateUpdateDTO roleCreateDTO)
        {
            var result = await _roleInterface.CreateRole(roleCreateDTO);
            return Ok(result);
        }

        [HttpPut("{roleId}")]
        public async Task<ActionResult<Response<string>>> UpdateRole(Guid roleId, [FromBody] RoleCreateUpdateDTO roleUpdateDTO)
        {
            var result = await _roleInterface.UpdateRole(roleId, roleUpdateDTO);
            return Ok(result);
        }

        [HttpDelete("{roleId}")]
        public async Task<ActionResult<Response<string>>> SoftDeleteRole(Guid roleId)
        {
            var result = await _roleInterface.SoftDeleteRole(roleId);
            return Ok(result);
        }
    }
}
