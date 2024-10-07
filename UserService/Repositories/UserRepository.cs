using Microsoft.EntityFrameworkCore;
using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Data;
using RestaurantManagement.SharedLibrary.Data;

namespace UserService.Repositories
{
    public class UserRepository(UserDbContext context) : IUser
    {
        // Get all users, excluding soft-deleted ones
        public async Task<Response<List<UserResponseDTO>>> GetAllUsers()
        {
            var users = await context.Users
                .Include(u => u.Role)
                .Where(u => u.DeletedAt == null) // Exclude soft-deleted users
                .ToListAsync();

            var userDtos = users.Select(u => new UserResponseDTO(u.Id, u.FullName, u.Email, u.Role.Name, u.RoleId)).ToList();
            return Response<List<UserResponseDTO>>.SuccessResponse("Users fetched successfully", userDtos);
        }

        // Get users by role, excluding soft-deleted ones
        public async Task<Response<List<UserResponseDTO>>> GetUsersByRole(Guid roleId)
        {
            var users = await context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId && u.DeletedAt == null) // Exclude soft-deleted users
                .ToListAsync();

            var userDtos = users.Select(u => new UserResponseDTO(u.Id, u.FullName, u.Email, u.Role.Name, u.RoleId)).ToList();
            return Response<List<UserResponseDTO>>.SuccessResponse("Users fetched successfully", userDtos);
        }

        // Get user by email, excluding soft-deleted users
        public async Task<Response<UserResponseDTO>> GetUserByEmail(string email)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .Where(u => u.Email == email && u.DeletedAt == null) // Exclude soft-deleted users
                .FirstOrDefaultAsync();

            if (user is null)
                return Response<UserResponseDTO>.ErrorResponse("User not found", ErrorCode.UserNotFound);

            return Response<UserResponseDTO>.SuccessResponse("User fetched successfully", new UserResponseDTO(user.Id, user.FullName, user.Email, user.Role.Name, user.RoleId));
        }

        // Get user by ID, excluding soft-deleted ones
        public async Task<Response<UserResponseDTO>> GetUserById(Guid userId)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == userId && u.DeletedAt == null) // Exclude soft-deleted users
                .FirstOrDefaultAsync();

            if (user is null)
                return Response<UserResponseDTO>.ErrorResponse("User not found", ErrorCode.UserNotFound);

            return Response<UserResponseDTO>.SuccessResponse("User fetched successfully", new UserResponseDTO(user.Id, user.FullName, user.Email, user.Role.Name, user.RoleId));
        }

        // Update user role
        public async Task<Response<string>> UpdateUserRole(Guid userId, Guid roleId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.DeletedAt != null)
                return Response<string>.ErrorResponse("User not found or has been soft deleted", ErrorCode.UserNotFound);

            user.RoleId = roleId;
            user.UpdatedAt = DateTime.UtcNow; // Set update timestamp

            await context.SaveChangesAsync();
            return Response<string>.SuccessResponse("User role updated successfully", userId.ToString());
        }

        // Update user full name
        public async Task<Response<string>> UpdateUserFullName(Guid userId, string fullName)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.DeletedAt != null)
                return Response<string>.ErrorResponse("User not found or has been soft deleted", ErrorCode.UserNotFound);

            user.FullName = fullName;
            user.UpdatedAt = DateTime.UtcNow; // Set update timestamp

            await context.SaveChangesAsync();
            return Response<string>.SuccessResponse("User full name updated successfully", userId.ToString());
        }

        // Soft delete user
        public async Task<Response<string>> SoftDeleteUser(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.DeletedAt != null)
                return Response<string>.ErrorResponse("User not found or already soft deleted", ErrorCode.UserNotFound);

            user.DeletedAt = DateTime.UtcNow; // Set soft deletion timestamp
            await context.SaveChangesAsync();
            return Response<string>.SuccessResponse("User deleted successfully", userId.ToString());
        }
    }
}
