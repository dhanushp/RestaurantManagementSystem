using Microsoft.EntityFrameworkCore;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Data;
using UserService.Models;
using RestaurantManagement.SharedLibrary.Data;

namespace UserService.Repositories
{
    public class RoleRepository(UserDbContext context) : IRole
    {
        // Get all roles, excluding soft-deleted ones
        public async Task<Response<List<RoleResponseDTO>>> GetAllRoles()
        {
            var roles = await context.Roles
                .Where(r => r.DeletedAt == null) // Exclude soft-deleted roles
                .ToListAsync();

            var roleDtos = roles.Select(r => new RoleResponseDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();

            return Response<List<RoleResponseDTO>>.SuccessResponse("Roles fetched successfully", roleDtos);
        }

        // Create new role
        public async Task<Response<RoleResponseDTO>> CreateRole(RoleCreateUpdateDTO roleCreateDTO)
        {
            var role = new Role
            {
                Name = roleCreateDTO.Name,
                Description = roleCreateDTO.Description,
                CreatedAt = DateTime.UtcNow // Set creation timestamp
            };

            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();

            return Response<RoleResponseDTO>.SuccessResponse("Role created successfully", new RoleResponseDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            });
        }

        // Update role
        public async Task<Response<string>> UpdateRole(Guid roleId, RoleCreateUpdateDTO roleUpdateDTO)
        {
            var role = await context.Roles.FindAsync(roleId);
            if (role is null)
                return Response<string>.ErrorResponse("Role not found", ErrorCode.RoleNotFound);

            role.Name = roleUpdateDTO.Name;
            role.Description = roleUpdateDTO.Description;
            role.UpdatedAt = DateTime.UtcNow; // Set update timestamp

            await context.SaveChangesAsync();

            return Response<string>.SuccessResponse("Role updated successfully", roleId.ToString());
        }

        // Soft delete role
        public async Task<Response<string>> SoftDeleteRole(Guid roleId)
        {
            var role = await context.Roles.FindAsync(roleId);
            if (role is null)
                return Response<string>.ErrorResponse("Role not found", ErrorCode.RoleNotFound);

            role.DeletedAt = DateTime.UtcNow; // Set soft deletion timestamp
            await context.SaveChangesAsync();

            return Response<string>.SuccessResponse("Role soft deleted successfully", roleId.ToString());
        }
    }
}
