using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuService.Interfaces;
using FakeItEasy;
using MenuService.Models;
using RestaurantManagement.SharedLibrary.Responses;
using MenuService.DTOs;
using MenuService.Services;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest.MenuApi.Controllers
{
    public class MenuItemsControllerTest
    {
        readonly MenuItemService menuItemService;
        readonly MenuItemsController menuItemsController;

        public MenuItemsControllerTest()
        {
            //Set up Dependencies
            var menuItemInterface = A.Fake<IMenuItem>();


            //Set Up System under test
            menuItemService = new MenuItemService(menuItemInterface);

        }

        //Get all menu items 

        [Fact]
        public async Task GetMenutItems_WhenProductExists_ReturnOkResponseWithMenuItems()
        {
            var menuitems = new List<MenuItem>()
            {
                new MenuItem
                {
                    Id = Guid.NewGuid(), // Assuming Id is a Guid from BaseEntity
                    Name = "Margherita Pizza",
                    Description = "Classic cheese and tomato pizza",
                    Price = 9.99m,
                    IsNonVeg = false,
                    IsAvailable = true,
                    ImageUrl = "https://example.com/images/margherita_pizza.jpg",
                    CategoryId = Guid.NewGuid() // Assuming a valid CategoryId
                },
                new MenuItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Tikka",
                    Description = "Spicy grilled chicken pieces marinated in yogurt",
                    Price = 12.99m,
                    IsNonVeg = true,
                    IsAvailable = true,
                    ImageUrl = "https://example.com/images/chicken_tikka.jpg",
                    CategoryId = Guid.NewGuid()
                }
            };

            //Set up Fake response for GetAllMenuItems()
            var menuItemResponseDTOs = menuitems.Select(menuItem => new MenuItemResponseDTO
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                //IsNonVeg = menuItem.IsNonVeg,
                IsAvailable = menuItem.IsAvailable,
                //ImageUrl = menuItem.ImageUrl
            }).ToList();
            var response = Response<List<MenuItemResponseDTO>>.SuccessResponse("Menu items fetched successfully", menuItemResponseDTOs);

            var menuItemService = A.Fake<MenuItemService>();

            A.CallTo(() => menuItemService.GetAllMenuItems()).Returns(Task.FromResult(response));

            //Act

            var results = await menuItemsController.GetAllMenuItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(results); // Check if the result is Ok (HTTP 200)
            var actualResponse = Assert.IsType<Response<List<MenuItemResponseDTO>>>(okResult.Value); // Check if the returned object is the expected Response type
            Assert.Equal("Menu items fetched successfully", actualResponse.Message); // Verify message
            Assert.Equal(2, actualResponse.Data.Count); // Verify that there are 2 menu items
            Assert.Equal(menuItemResponseDTOs.First().Name, actualResponse.Data.First().Name); // Verify the first item name
        }

    }
}
