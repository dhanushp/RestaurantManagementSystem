using MenuService.DTOs;
using RestaurantManagement.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenuService.Interfaces
{
    // Interface definition starts
    public interface IMenuItem
    {
        Task<Response<List<MenuItemResponseDTO>>> GetAvailableMenuItems();

        Task<Response<List<CategoryResponseDTO>>> GetCategories();
        Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategory(string category);
        Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItems();
        Task<Response<MenuItemDetailResponseDTO>> GetMenuItemById(Guid menuItemId);
        Task<Response<MenuItemResponseDTO>> GetMenuItemByName(string name);
        Task<Response<MenuItemResponseDTO>> AddMenuItem(MenuItemCreateUpdateDTO menuItemCreateDTO);
        Task<Response<MenuItemResponseDTO>> UpdateMenuItem(Guid menuItemId, MenuItemCreateUpdateDTO menuItemUpdateDTO);
        Task<Response<string>> DeleteMenuItem(Guid menuItemId);
        Task<Response<CategoryDTO>> AddCategory(CategoryCreateUpdateDTO categoryCreateDTO);
        Task<Response<CategoryDTO>> UpdateCategory(Guid categoryId, CategoryCreateUpdateDTO categoryUpdateDTO);
        Task<Response<string>> DeleteCategory(Guid categoryId);
    }
    // Interface definition ends
}
