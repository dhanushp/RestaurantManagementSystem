using MenuService.DTOs;
using RestaurantManagement.SharedLibrary.Responses;
using System;
using System.Threading.Tasks;

namespace OrderService.Interfaces
{
    public interface IMenuItems // Corrected to interface
    {
        Task<Response<MenuItemDetailResponseDTO>> GetMenuItemDetailsByIdAsync(Guid menuItemId);
    }
}
