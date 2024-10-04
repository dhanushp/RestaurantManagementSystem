using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
                    Id = Guid.NewGuid(), // Generate a unique ID for Admin role
                    Name = "Admin",
                    Description = "Administrator role",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
