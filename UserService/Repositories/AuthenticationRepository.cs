using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RestaurantManagement.SharedLibrary.Data;

namespace UserService.Repositories
{
    public class AuthenticationRepository(UserDbContext context, IConfiguration config) : IAuthentication
    {
        private async Task<User?> GetUserByEmail(string email)
        {
            return await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        private async Task<UserResponseDTO?> GetUser(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new UserResponseDTO(user.Id, user.FullName, user.Email, user.Role.Name, user.RoleId) : null;
        }


        public async Task<Response<JwtResponseDto>> Login(UserLoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser is null)
                return Response<JwtResponseDto>.ErrorResponse("Invalid Credentials", ErrorCode.InvalidCredentials);

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.PasswordHash);
            if (!verifyPassword)
            {
                return Response<JwtResponseDto>.ErrorResponse("Invalid Credentials", ErrorCode.InvalidCredentials);
            }

            JwtResponseDto jwtResponse = GenerateJwtToken(getUser);

            jwtResponse.UserInfo = new UserResponseDTO(getUser.Id, getUser.FullName, getUser.Email, getUser.Role.Name, getUser.RoleId);


            return Response<JwtResponseDto>.SuccessResponse("Logged In Succesfully",jwtResponse);
            
        }


        private JwtResponseDto GenerateJwtToken(User user)
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret not found in environment variables.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Add user email as a claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                new Claim(ClaimTypes.Role, user.Role.Name) // Add user's role to the token
            };

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Token expiration
                signingCredentials: credentials
            );

            return new JwtResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = DateTime.UtcNow.AddDays(1) // Set the expiration time
            };
        }


        public async Task<Response<string>> Register(UserRegisterDTO userRegisterDTO)
        {
            var getUser = await GetUserByEmail(userRegisterDTO.Email);
            if (getUser is not null)
            {
                return Response<string>.ErrorResponse("User Already Exists", ErrorCode.UserAlreadyExists);
            }

            var result = context.Users.Add(new User()
            {
                FullName = userRegisterDTO.FullName,
                Email = userRegisterDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password),
                RoleId = context.Roles.FirstOrDefault(r => r.Name == "Customer")!.Id
            });

            var saveResult = await context.SaveChangesAsync();
            if (saveResult == 0)
            {
                return Response<string>.ErrorResponse("Failed to Register User", ErrorCode.RegistrationFailed);
            }
            return Response<string>.SuccessResponse($"User Registered Successfully", userRegisterDTO.Email);
        }



        /*
        public async Task<Response<string>> ForgotPassword(string email)
        {
            var user = await GetUserByEmail(email);
            if (user is null)
                return Response<string>.ErrorResponse("User not found", ErrorCode.UserNotFound);

            // Generate a reset token (could use a library like Identity)
            var resetToken = GenerateResetToken();
            // TODO: Send email with resetToken to user.Email (implement email service)

            return Response<string>.SuccessResponse("Reset link sent to email", resetToken);
        }

        private string GenerateResetToken()
        {
            // Generate a secure token for password reset (this is just a placeholder)
            return Guid.NewGuid().ToString();
        }

        public async Task<Response<string>> ResetPassword(string resetToken, string newPassword)
        {
            // Validate the resetToken (this needs to be stored and checked)
            // Assume token is valid for this example
            var user = await GetUserByEmail("test@example.com"); // Implement proper lookup here
            if (user is null)
                return Response<string>.ErrorResponse("Invalid reset token", ErrorCode.InvalidResetToken);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await context.SaveChangesAsync();

            return Response<string>.SuccessResponse("Password reset successfully");
        }
        */
    }
}
