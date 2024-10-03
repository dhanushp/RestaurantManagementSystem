using MenuService.Data;
using MenuService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Controllers
{
    // API Controller for managing menu items
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuContext _context;

        public MenuController(MenuContext context)
        {
            _context = context;
        }

        // GET: api/menu - Retrieves a paginated list of menu items
        [HttpGet]
        public async Task<IActionResult> GetMenuItems(int page = 1, int pageSize = 10)
        {
            // Fetch menu items with pagination
            var items = await _context.MenuItems
                                      .Skip((page - 1) * pageSize) // Skip items for pagination
                                      .Take(pageSize) // Take a limited number of items
                                      .ToListAsync();
            return Ok(items);
        }

        // POST: api/menu - Adds a new menu item to the database
        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItem menuItem)
        {
            // Validate the incoming data before processing
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Add the new menu item to the database
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return Ok("Menu item added");
        }
    }
}
