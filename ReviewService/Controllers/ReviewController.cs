using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewService.Data;
using ReviewService.Models;

namespace ReviewService.Controllers
{
    // This API allows users to submit and retrieve reviews for specific items.
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewContext _context;

        public ReviewController(ReviewContext context)
        {
            _context = context;
        }

        // POST: api/review
        // Allows customers to submit reviews for specific order items
        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok("Review submitted successfully");
        }

        // GET: api/review/getByItem/{itemId}
        // Allows customers or admins to retrieve reviews for specific menu items
        [HttpGet("getByItem/{itemId}")]
        public async Task<IActionResult> GetReviewsByItem(int itemId)
        {
            var reviews = await _context.Reviews
                                        .Where(r => r.OrderItemId == itemId)
                                        .ToListAsync();

            if (reviews == null || !reviews.Any())
                return NotFound("No reviews found for this item");

            return Ok(reviews);
        }
    }
}
