using Microsoft.EntityFrameworkCore;
using AdminService.Models;

namespace AdminService.Data
{
    public class AdminContext : DbContext
    {
        /*
         DbSet Addition: Adding DbSet<OrderItem> ensures that EF Core tracks OrderItem entities and creates the corresponding table.
         Relationship Configurations: Defining relationships in OnModelCreating ensures that the database schema reflects the intended foreign key constraints.
        */
        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } // Add this line
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Order - User relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders) // If you added Orders to User
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure OrderItem - Order relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Delete OrderItems when Order is deleted

            // Optional: Configure unique constraints or indexes if necessary
        }
    }
}
