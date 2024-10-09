using Microsoft.EntityFrameworkCore;
using RestaurantOperationsService.Models;

namespace RestaurantOperationsService.Data
{
    public class RestaurantOperationsDbContext : DbContext
    {
        public RestaurantOperationsDbContext(DbContextOptions<RestaurantOperationsDbContext> options) : base(options) { }

        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Prevents duplicate emails
            modelBuilder.Entity<Table>()
                .HasIndex(u => u.Number)
                .IsUnique();


            // Seed initial data for Tables
            modelBuilder.Entity<Table>().HasData(
                new Table
                {
                    Id = Guid.NewGuid(),
                    Number = "T1",
                    Capacity = 4,
                    Status = TableStatus.Available,
                    CreatedAt = DateTime.UtcNow
                },
                new Table
                {
                    Id = Guid.NewGuid(),
                    Number = "T2",
                    Capacity = 6,
                    Status = TableStatus.Available,
                    CreatedAt = DateTime.UtcNow
                },
                new Table
                {
                    Id = Guid.NewGuid(),
                    Number = "T3",
                    Capacity = 2,
                    Status = TableStatus.Available,
                    CreatedAt = DateTime.UtcNow
                }
            );


        }
    }
}
