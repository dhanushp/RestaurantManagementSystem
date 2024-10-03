using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminService.Data;
using AdminService.Models;
using System.Threading.Tasks;
using System.Linq;

namespace AdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminContext _context;

        public AdminController(AdminContext context)
        {
            _context = context;
        }

        // GET: api/admin/users
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/admin/orders
        /// <summary>
        /// Retrieves a list of all orders with related users and order items.
        /// </summary>
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User) // Include User data
                .Include(o => o.OrderItems) // Include OrderItems
                .ToListAsync();
            return Ok(orders);
        }

        // PUT: api/admin/table/{id}/updateStatus
        /// <summary>
        /// Updates the occupancy status of a table.
        /// </summary>
        [HttpPut("table/{id}/updateStatus")]
        public async Task<IActionResult> UpdateTableStatus(int id, [FromBody] bool isOccupied)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound("Table not found");
            }

            table.IsOccupied = isOccupied;
            await _context.SaveChangesAsync();
            return Ok("Table status updated");
        }

        // Additional endpoints and comments as needed
    }
}
