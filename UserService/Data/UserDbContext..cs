using Microsoft.EntityFrameworkCore;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configures one-to-many relationship: one Role can have many Users, and a Role cannot be deleted if referenced by any User
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users) 
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevents duplicate emails
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); 

            // Defines unique constraint on the Role.Name field
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Seed data for 'Customer' and 'Admin' roles

            var adminRoleId = Guid.NewGuid();

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.NewGuid(), // Generate a unique ID for Customer role
                    Name = "Customer",
                    Description = "Default customer role",
                    CreatedAt = DateTime.UtcNow
                },
                new Role
                {
                    Id = adminRoleId, // Generate a unique ID for Admin role
                    Name = "Admin",
                    Description = "Administrator role",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Fetch the admin password from environment variables
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PWD");

            if (string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("Environment variable 'ADMIN_PWD' is not set.");
            }
            
            // Seed an initial admin user with the fetched password
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    FullName = "Admin User",
                    Email = "admin@eg.dk",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),  // Hash the fetched password
                    RoleId = adminRoleId,  // Assign the Admin role
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
