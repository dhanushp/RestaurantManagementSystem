using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IRole
    {
        Task<Response<List<RoleResponseDTO>>> GetAllRoles();
        Task<Response<RoleResponseDTO>> CreateRole(RoleCreateUpdateDTO roleCreateDTO);
        Task<Response<string>> UpdateRole(Guid roleId, RoleCreateUpdateDTO roleUpdateDTO);
        Task<Response<string>> SoftDeleteRole(Guid roleId);
    }
}
