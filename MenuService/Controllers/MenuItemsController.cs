using MenuService.DTOs;
using MenuService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly MenuItemService _menuItemService;

    public MenuItemsController(MenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    // GET: api/menuitems
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllMenuItems()
    {
        var response = await _menuItemService.GetAllMenuItems(); // Assuming you have this method in your service
        return Ok(response); // Return success response
    }

    // GET: api/menuitems/available
    [AllowAnonymous]    
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableMenuItems()
    {
        var response = await _menuItemService.GetAvailableMenuItems();
        return Ok(response); // Return success response
    }

    [AllowAnonymous]
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var response = await _menuItemService.GetCategories();

        if (response.Success)
        {
            // Return 200 OK status with the data if the request is successful
            return Ok(response);
        }
        else
        {
            // Handle the error case, return a specific status code based on the error
            return StatusCode((int)response.ErrorCode, response); // Example: 503 for service unavailable
        }
    }


    // GET:     
    [AllowAnonymous]
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetMenuItemsByCategory(string category)
    {
        var response = await _menuItemService.GetMenuItemsByCategory(category);
        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response);
    }

    // GET: api/menuitems/{id}
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMenuItemById(Guid id)
    {
        var response = await _menuItemService.GetMenuItemById(id);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response);
    }

    // GET: api/menuitems/name/{name}
    [AllowAnonymous]
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetMenuItemByName(string name)
    {
        var response = await _menuItemService.GetMenuItemByName(name);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response);
    }



    // POST: api/menuitems
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> AddMenuItem(MenuItemCreateUpdateDTO menuItemCreateDTO)
    {
        var response = await _menuItemService.AddMenuItem(menuItemCreateDTO);
        if (!response.Success)
            return BadRequest(response.Message);

        if (response.Data == null)
        {
            return BadRequest("Unable to create the menu item. The data returned was null.");
        }

        return CreatedAtAction(nameof(GetMenuItemByName), new { name = response.Data.Name }, response.Data);
    }

    // PUT: api/menuitems/{id}
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(Guid id, MenuItemCreateUpdateDTO menuItemUpdateDTO)
    {
        var response = await _menuItemService.UpdateMenuItem(id, menuItemUpdateDTO);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // DELETE: api/menuitems/{id}
    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(Guid id)
    {
        var response = await _menuItemService.DeleteMenuItem(id);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Message);
    }

    // POST: api/menuitems/category
    [AllowAnonymous]
    [HttpPost("category")]
    public async Task<IActionResult> AddCategory(CategoryCreateUpdateDTO categoryCreateDTO)
    {
        var response = await _menuItemService.AddCategory(categoryCreateDTO);
        if (!response.Success)
            return BadRequest(response.Message);

        return CreatedAtAction(nameof(GetMenuItemsByCategory), new { category = response.Data.Name }, response.Data);
    }

    // PUT: api/menuitems/category/{id}
    [AllowAnonymous]
    [HttpPut("category/{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, CategoryCreateUpdateDTO categoryUpdateDTO)
    {
        var response = await _menuItemService.UpdateCategory(id, categoryUpdateDTO);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // DELETE: api/menuitems/category/{id}
    [AllowAnonymous]
    [HttpDelete("category/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var response = await _menuItemService.DeleteCategory(id);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Message);
    }
}
