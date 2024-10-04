using Microsoft.EntityFrameworkCore;
using MenuService.Models;
using Newtonsoft.Json;

namespace MenuService.Data
{
    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships without seeding data
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public void SeedData()
        {
            try
            {
                SeedCategories();
                SeedMenuItems();
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }

        public void SeedCategories()
        {
            if (!Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Id = Guid.NewGuid(), Name = "Appetizers", Description = "Delicious starters to whet your appetite." },
                    new Category { Id = Guid.NewGuid(), Name = "Soups", Description = "Warm and comforting options to soothe the palate." },
                    new Category { Id = Guid.NewGuid(), Name = "Salads", Description = "Fresh and healthy salads to refresh your meal." },
                    new Category { Id = Guid.NewGuid(), Name = "Main Course", Description = "Hearty dishes to fill you up." },
                    new Category { Id = Guid.NewGuid(), Name = "Pasta", Description = "A variety of pasta dishes with rich sauces." },
                    new Category { Id = Guid.NewGuid(), Name = "Pizza", Description = "Tasty pizzas with a variety of toppings." },
                    new Category { Id = Guid.NewGuid(), Name = "Breads", Description = "Freshly baked bread varieties to accompany your meal." },
                    new Category { Id = Guid.NewGuid(), Name = "Rice and Noodles", Description = "Savory rice and noodle dishes for a fulfilling experience." },
                    new Category { Id = Guid.NewGuid(), Name = "Desserts", Description = "Sweet treats to conclude your meal." },
                    new Category { Id = Guid.NewGuid(), Name = "Beverages", Description = "Refreshing drinks to accompany your meal." },
                };

                Categories.AddRange(categories);
                try
                {
                    SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error saving the data: {ex.Message}");
                }
            }
        }

        public void SeedMenuItems()
        {
            var menuItems = LoadMenuItemsFromJson("C:\\Users\\sneba\\source\\repos\\RestaurantManagementSystem\\MenuService\\Data\\MenuItems.json");
            if (menuItems != null && !MenuItems.Any())
            {
                MenuItems.AddRange(menuItems);
                try
                {
                    SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error saving the data: {ex.Message}");
                }
            }
        }

        public IEnumerable<MenuItem> LoadMenuItemsFromJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var menuItemDtos = JsonConvert.DeserializeObject<List<MenuItemDTO>>(json); // Use a DTO instead

            return menuItemDtos.Select(item => new MenuItem
            {
                Id = Guid.NewGuid(), // Generate new GUID for MenuItem
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                IsNonVeg = item.IsNonVeg,
                IsAvailable = item.IsAvailable,
                ImageUrl = item.ImageUrl,
                CategoryId = GetCategoryId(item.Category) // Use GetCategoryId to fetch the category's ID
            }).ToList();
        }

        public Guid GetCategoryId(string categoryName)
        {
            // Logic to fetch CategoryId from the database or in-memory collection
            var category = Categories.SingleOrDefault(c => c.Name == categoryName);
            return category?.Id ?? Guid.Empty; // or handle the case where the category isn't found
        }
    }
}
