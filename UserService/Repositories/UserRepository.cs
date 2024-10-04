using RestaurantManagement.SharedLibrary.Responses;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Data;
using UserService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace UserService.Repositories
{
    public class UserRepository(UserDbContext context, IConfiguration config) : IUser
    {
        private async Task<User?> GetUserByEmail(string email)
        {
            return await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<GetUserDTO?> GetUser(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new GetUserDTO(user.FullName, user.Email, user.Role.Name) : null;
        }


        public async Task<Response> Login(UserLoginDto loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser is null)
                return new Response(false, "Invalid Credentials");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.PasswordHash);
            if (!verifyPassword)
            {
                return new Response(false, "Invalid Credentials");
            }

            string token = GenerateJwtToken(getUser);
            return new Response(true, token);
            
        }

        private string GenerateJwtToken(User user)
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
                expires: DateTime.Now.AddMinutes(30), // Token expiration
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Return the token as a string
        }

        public async Task<Response> Register(UserDTO userDTO)
        {
            var getUser = await GetUserByEmail(userDTO.Email);
            if (getUser is not null)
            {
                return new Response(false, "User Already Exists");
            }

            var result = context.Users.Add(new User()
            {
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash),
                RoleId = userDTO.RoleId
            });

            var saveResult = await context.SaveChangesAsync();
            if (saveResult == 0)
            {
                return new Response(false, "Failed to Register User");
            }
            return new Response(true, "User Registered Successfully");
        }
    }
}
