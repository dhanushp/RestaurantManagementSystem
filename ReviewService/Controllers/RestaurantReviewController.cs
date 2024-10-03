using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewService.Data;
using ReviewService.Models;

namespace ReviewService.Controllers
{
    // This API allows users to submit and retrieve reviews for the overall restaurant experience.
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantReviewController : ControllerBase
    {
        private readonly ReviewContext _context;

        public RestaurantReviewController(ReviewContext context)
        {
            _context = context;
        }

        // POST: api/restaurantreview
        // Allows customers to submit reviews for their restaurant experience
        [HttpPost]
        public async Task<IActionResult> SubmitRestaurantReview([FromBody] RestaurantReview review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.RestaurantReviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok("Restaurant review submitted successfully");
        }

        // GET: api/restaurantreview/getByUser/{userId}
        // Allows customers or admins to retrieve reviews submitted by a specific user
        [HttpGet("getByUser/{userId}")]
        public async Task<IActionResult> GetRestaurantReviewsByUser(int userId)
        {
            var reviews = await _context.RestaurantReviews
                                        .Where(r => r.UserId == userId)
                                        .ToListAsync();

            if (reviews == null || !reviews.Any())
                return NotFound("No reviews found for this user");

            return Ok(reviews);
        }
    }
}
