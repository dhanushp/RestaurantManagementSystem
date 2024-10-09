using MenuService.DTOs;
using MenuService.Interfaces;
using RestaurantManagement.SharedLibrary.Responses;

namespace MenuService.Services
{
    public class MenuItemService
    {
        private readonly IMenuItem _menuItemRepository;

        public MenuItemService(IMenuItem menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        // Get all menu items
        public async Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItems()
        {
            return await _menuItemRepository.GetAllMenuItems();
        }

        // Get all available menu items
        public async Task<Response<List<MenuItemResponseDTO>>> GetAvailableMenuItems()
        {
            return await _menuItemRepository.GetAvailableMenuItems();
        }

        // Get menu items by category 
        public async Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategory(string category)
        {
            // Validate if the category is null or empty
            if (string.IsNullOrWhiteSpace(category))
            {
                return Response<List<MenuItemResponseDTO>>.ErrorResponse("Category cannot be empty.");
            }

            // Fetch items by category
            return await _menuItemRepository.GetMenuItemsByCategory(category);
        }
        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemById(Guid menuItemId)
        {
            if (menuItemId == Guid.Empty)
            {
                return Response<MenuItemDetailResponseDTO>.ErrorResponse("Invalid menu item ID.");
            }

            // Call the repository method
            return await _menuItemRepository.GetMenuItemById(menuItemId);
        }

        // Get menu item by name 
        public async Task<Response<MenuItemResponseDTO>> GetMenuItemByName(string name)
        {
            // Validate if the name is null or empty
            if (string.IsNullOrWhiteSpace(name))
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Menu item name cannot be empty.");
            }

            // Fetch the menu item by name
            return await _menuItemRepository.GetMenuItemByName(name);
        }

        // Add a new menu item n
        public async Task<Response<MenuItemResponseDTO>> AddMenuItem(MenuItemCreateUpdateDTO menuItemCreateDTO)
        {
            // Check if DTO is null
            if (menuItemCreateDTO == null)
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Menu item data is required.");
            }

            // Call the repository method
            return await _menuItemRepository.AddMenuItem(menuItemCreateDTO);
        }

        // Update an existing menu item
        public async Task<Response<MenuItemResponseDTO>> UpdateMenuItem(Guid menuItemId, MenuItemCreateUpdateDTO menuItemUpdateDTO)
        {
            // Check if the ID is valid
            if (menuItemId == Guid.Empty)
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Invalid menu item ID.");
            }

            // Check if DTO is null
            if (menuItemUpdateDTO == null)
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Menu item data is required.");
            }

            // Call the repository method
            return await _menuItemRepository.UpdateMenuItem(menuItemId, menuItemUpdateDTO);
        }

        // Delete a menu item with additional validation
        public async Task<Response<string>> DeleteMenuItem(Guid menuItemId)
        {
            // Check if the ID is valid
            if (menuItemId == Guid.Empty)
            {
                return Response<string>.ErrorResponse("Invalid menu item ID.");
            }

            // Call the repository method
            return await _menuItemRepository.DeleteMenuItem(menuItemId);
        }

        public async Task<Response<CategoryDTO>> AddCategory(CategoryCreateUpdateDTO categoryCreateDTO)
        {
            return await _menuItemRepository.AddCategory(categoryCreateDTO);
        }

        // Update an existing category
        public async Task<Response<CategoryDTO>> UpdateCategory(Guid categoryId, CategoryCreateUpdateDTO categoryUpdateDTO)
        {
            return await _menuItemRepository.UpdateCategory(categoryId, categoryUpdateDTO);
        }

        // Delete a category
        public async Task<Response<string>> DeleteCategory(Guid categoryId)
        {
            return await _menuItemRepository.DeleteCategory(categoryId);
        }
    }
}
