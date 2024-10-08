using MenuService.Models;
using MenuService.DTOs;
using RestaurantManagement.SharedLibrary.Responses;
using MenuService.Interfaces;
using MenuService.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Repositories
{
    public class MenuItemRepository : IMenuItem
    {
        private readonly MenuContext _context;

        public MenuItemRepository(MenuContext context)
        {
            _context = context;
        }
        // Get all menu items
        public async Task<Response<List<MenuItemResponseDTO>>> GetAllMenuItems()
        {
            try
            {
                var menuItems = await _context.MenuItems
                    .Select(item => new MenuItemResponseDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        Category = item.Category != null ? item.Category.Name : "Unknown",
                        IsAvailable = item.IsAvailable
                    })
                    .ToListAsync();

                return Response<List<MenuItemResponseDTO>>.SuccessResponse("All menu items fetched successfully", menuItems);
            }
            catch (Exception ex)
            {
                return Response<List<MenuItemResponseDTO>>.ErrorResponse($"An error occurred while fetching the menu items: {ex.Message}");
            }
        }
        // Get all available menu items
        public async Task<Response<List<MenuItemResponseDTO>>> GetAvailableMenuItems()
        {
            try
            {
                var menuItems = await _context.MenuItems
                    .Where(item => item.IsAvailable)
                    .Select(item => new MenuItemResponseDTO
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        Category = item.Category != null ? item.Category.Name : "Unknown",
                        IsAvailable = item.IsAvailable
                    })
                    .ToListAsync();

                return Response<List<MenuItemResponseDTO>>.SuccessResponse("Menu items fetched successfully", menuItems);
            }
            catch (Exception ex)
            {
                return Response<List<MenuItemResponseDTO>>.ErrorResponse($"An error occurred while fetching the menu items: {ex.Message}");
            }
        }

        // Get menu items by category
        public async Task<Response<List<MenuItemResponseDTO>>> GetMenuItemsByCategory(string category)
        {
            // Validate if the category is null or empty
            if (string.IsNullOrWhiteSpace(category))
            {
                return Response<List<MenuItemResponseDTO>>.ErrorResponse("Category cannot be empty.");
            }

            // Fetch menu items by category with case-insensitive comparison
            var menuItems = await _context.MenuItems
                .Where(item => item.Category.Name.ToLower() == category.ToLower() && item.IsAvailable)
                .Select(item => new MenuItemResponseDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Category = item.Category != null ? item.Category.Name : "Unknown",
                    IsAvailable = item.IsAvailable
                })
                .ToListAsync();

            if (menuItems == null || !menuItems.Any())
            {
                return Response<List<MenuItemResponseDTO>>.ErrorResponse("No menu items found for the specified category.");
            }

            return Response<List<MenuItemResponseDTO>>.SuccessResponse("Menu items fetched successfully", menuItems);
        }

        // Get menu item by name
        // Get menu item by name 
        public async Task<Response<MenuItemResponseDTO>> GetMenuItemByName(string name)
        {
            // Validate if the name is null or empty
            if (string.IsNullOrWhiteSpace(name))
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Menu item name cannot be empty.");
            }

            // Fetch the menu item by name with case-insensitive comparison
            var menuItem = await _context.MenuItems
                .Where(item => item.Name.ToLower() == name.ToLower() && item.IsAvailable)
                .Select(item => new MenuItemResponseDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Category = item.Category != null ? item.Category.Name : "Unknown",
                    IsAvailable = item.IsAvailable
                })
                .FirstOrDefaultAsync();

            if (menuItem == null)
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Menu item not found.");
            }

            return Response<MenuItemResponseDTO>.SuccessResponse("Menu item fetched successfully", menuItem);
        }

        public async Task<Response<MenuItemDetailResponseDTO>> GetMenuItemById(Guid menuItemId)
        {
            try
            {
                var menuItem = await _context.MenuItems
                    .Include(item => item.Category) // Include category if necessary
                    .FirstOrDefaultAsync(item => item.Id == menuItemId && item.IsAvailable);

                if (menuItem == null)
                {
                    return Response<MenuItemDetailResponseDTO>.ErrorResponse("Menu item not found.");
                }

                var menuItemDetailResponse = new MenuItemDetailResponseDTO
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    Category = menuItem.Category != null ? menuItem.Category.Name : "Unknown",
                    IsAvailable = menuItem.IsAvailable
                };

                return Response<MenuItemDetailResponseDTO>.SuccessResponse("Menu item fetched successfully", menuItemDetailResponse);
            }
            catch (Exception ex)
            {
                return Response<MenuItemDetailResponseDTO>.ErrorResponse($"An error occurred while fetching the menu item: {ex.Message}");
            }
        }
        // Add a new menu item
        public async Task<Response<MenuItemResponseDTO>> AddMenuItem(MenuItemCreateUpdateDTO menuItemCreateDTO)
        {
            if (string.IsNullOrWhiteSpace(menuItemCreateDTO.Category))
            {
                return Response<MenuItemResponseDTO>.ErrorResponse("Category cannot be empty.");
            }
            try
            {
                // Find the category by name using ToLower() for case-insensitive comparison
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == menuItemCreateDTO.Category.ToLower());

                if (category == null)
                {
                    return Response<MenuItemResponseDTO>.ErrorResponse("Category not found.");
                }

                // Create new menu item
                var menuItem = new MenuItem
                {
                    Name = menuItemCreateDTO.Name,
                    Description = menuItemCreateDTO.Description,
                    Price = menuItemCreateDTO.Price,
                    CategoryId = category.Id,
                    IsAvailable = menuItemCreateDTO.IsAvailable
                };

                await _context.MenuItems.AddAsync(menuItem);
                await _context.SaveChangesAsync();

                var menuItemResponse = new MenuItemResponseDTO
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    Category = category.Name,
                    IsAvailable = menuItem.IsAvailable
                };

                return Response<MenuItemResponseDTO>.SuccessResponse("Menu item added successfully", menuItemResponse);
            }
            catch (Exception ex)
            {
               var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception.";
        return Response<MenuItemResponseDTO>.ErrorResponse($"An error occurred while adding the menu item: {innerExceptionMessage}");
            }
        }


        // Update an existing menu item
        public async Task<Response<MenuItemResponseDTO>> UpdateMenuItem(Guid menuItemId, MenuItemCreateUpdateDTO menuItemUpdateDTO)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(menuItemId);
                if (menuItem == null)
                {
                    return Response<MenuItemResponseDTO>.ErrorResponse("Menu item not found.");
                }

                // Find the category by name
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == menuItemUpdateDTO.Category.ToLower());

                if (category == null)
                {
                    return Response<MenuItemResponseDTO>.ErrorResponse("Category not found.");
                }

                // Update properties
                menuItem.Name = menuItemUpdateDTO.Name;
                menuItem.Description = menuItemUpdateDTO.Description;
                menuItem.Price = menuItemUpdateDTO.Price;
                menuItem.CategoryId = category.Id; // Set CategoryId from the fetched category
                menuItem.IsAvailable = menuItemUpdateDTO.IsAvailable;

                await _context.SaveChangesAsync();

                var updatedMenuItemResponse = new MenuItemResponseDTO
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Description = menuItem.Description,
                    Price = menuItem.Price,
                    Category = category.Name,
                    IsAvailable = menuItem.IsAvailable
                };

                return Response<MenuItemResponseDTO>.SuccessResponse("Menu item updated successfully", updatedMenuItemResponse);
            }
            catch (Exception ex)
            {
                return Response<MenuItemResponseDTO>.ErrorResponse($"An error occurred while updating the menu item: {ex.Message}");
            }
        }




        // Delete a menu item (soft delete)
        public async Task<Response<string>> DeleteMenuItem(Guid menuItemId)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(menuItemId);
                if (menuItem == null)
                {
                    return Response<string>.ErrorResponse("Menu item not found.");
                }

                // Soft delete by marking as unavailable
                menuItem.IsAvailable = false;
                menuItem.DeletedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Response<string>.SuccessResponse("Menu item deleted successfully", "Menu item is marked unavailable.");
            }
            catch (Exception ex)
            {
                return Response<string>.ErrorResponse($"An error occurred while deleting the menu item: {ex.Message}");
            }
        }
        public async Task<Response<CategoryDTO>> AddCategory(CategoryCreateUpdateDTO categoryCreateDTO)
        {
            if (string.IsNullOrWhiteSpace(categoryCreateDTO.Name))
            {
                return Response<CategoryDTO>.ErrorResponse("Category name cannot be empty.");
            }

            var category = new Category
            {
                Name = categoryCreateDTO.Name,
                Description = categoryCreateDTO.Description
            };

            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return Response<CategoryDTO>.SuccessResponse("Category added successfully", categoryDTO);
            }
            catch (Exception ex)
            {
                return Response<CategoryDTO>.ErrorResponse($"An error occurred while adding the category: {ex.Message}");
            }
        }

        public async Task<Response<CategoryDTO>> UpdateCategory(Guid categoryId, CategoryCreateUpdateDTO categoryUpdateDTO)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return Response<CategoryDTO>.ErrorResponse("Category not found.");
            }

            category.Name = categoryUpdateDTO.Name;
            category.Description = categoryUpdateDTO.Description;

            try
            {
                await _context.SaveChangesAsync();

                var categoryDTO = new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return Response<CategoryDTO>.SuccessResponse("Category updated successfully", categoryDTO);
            }
            catch (Exception ex)
            {
                return Response<CategoryDTO>.ErrorResponse($"An error occurred while updating the category: {ex.Message}");
            }
        }

        public async Task<Response<string>> DeleteCategory(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return Response<string>.ErrorResponse("Category not found.");
            }

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return Response<string>.SuccessResponse("Category deleted successfully", "Category removed.");
            }
            catch (Exception ex)
            {
                return Response<string>.ErrorResponse($"An error occurred while deleting the category: {ex.Message}");
            }
        }
    }
}
