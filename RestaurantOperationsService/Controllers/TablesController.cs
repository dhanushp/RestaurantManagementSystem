using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantOperationsService.DTOs;
using RestaurantOperationsService.Interfaces;

namespace RestaurantOperationsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly ITable _tableRepository;

        public TablesController(ITable tableRepository)
        {
            _tableRepository = tableRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTables()
        {
            var response = await _tableRepository.GetAllTables();
            return Ok(response);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAllAvailableTables()
        {
            var response = await _tableRepository.GetAllAvailableTables();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTableById(Guid id)
        {
            var response = await _tableRepository.GetTableById(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTable([FromBody] TableCreateDTO tableCreateDTO)
        {
            var response = await _tableRepository.CreateTable(tableCreateDTO);
            return CreatedAtAction(nameof(GetTableById), new { id = response.Data.Id }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTable(Guid id, [FromBody] TableUpdateDTO tableUpdateDTO)
        {
            var response = await _tableRepository.UpdateTable(id, tableUpdateDTO);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDeleteTable(Guid id)
        {
            var response = await _tableRepository.SoftDeleteTable(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/make-available")]
        public async Task<IActionResult> MakeTableAvailable(Guid id)
        {
            var response = await _tableRepository.MakeTableAvailable(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpPost("occupy")]
        public async Task<IActionResult> OccupyTable([FromBody] OccupyTableRequestDTO request)
        {
            var result = await _tableRepository.OccupyTable(request.TableId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
