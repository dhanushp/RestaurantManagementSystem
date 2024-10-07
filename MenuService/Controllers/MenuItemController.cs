using MenuService.DTOs;
using MenuService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : ControllerBase
{
    private readonly MenuItemService _menuItemService;

    public MenuItemController(MenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    // GET: api/menuitems/available
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableMenuItems()
    {
        var response = await _menuItemService.GetAvailableMenuItems();
        return Ok(response); // Return success response
    }

    // GET:     
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetMenuItemsByCategory(string category)
    {
        var response = await _menuItemService.GetMenuItemsByCategory(category);
        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response);
    }

    // GET: api/menuitems/name/{name}
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetMenuItemByName(string name)
    {
        var response = await _menuItemService.GetMenuItemByName(name);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response);
    }

    // POST: api/menuitems
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(Guid id, MenuItemCreateUpdateDTO menuItemUpdateDTO)
    {
        var response = await _menuItemService.UpdateMenuItem(id, menuItemUpdateDTO);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    // DELETE: api/menuitems/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(Guid id)
    {
        var response = await _menuItemService.DeleteMenuItem(id);
        if (!response.Success)
            return NotFound(response.Message);

        return Ok(response.Message);
    }
}
