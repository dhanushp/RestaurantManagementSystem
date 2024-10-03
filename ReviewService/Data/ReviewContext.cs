using Microsoft.EntityFrameworkCore;
using ReviewService.Models;

namespace ReviewService.Data
{
    // This context class will manage access to the Review and RestaurantReview tables.
    public class ReviewContext : DbContext
    {
        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options) { }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<RestaurantReview> RestaurantReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If there are any specific configurations for relationships, add them here.
        }
    }
}
