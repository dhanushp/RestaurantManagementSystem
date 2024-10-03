using Microsoft.EntityFrameworkCore;
using MenuService.Models;

namespace MenuService.Data
{
    // DbContext for MenuService, managing MenuItem and Category entities
    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {
        }

        // DbSet for MenuItems
        public DbSet<MenuItem> MenuItems { get; set; }

        // DbSet for Categories
        public DbSet<Category> Categories { get; set; }

        // Configuring relationships and behavior
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between MenuItem and Category
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany() // A category can have multiple menu items
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of categories if they have menu items
        }
    }
}
