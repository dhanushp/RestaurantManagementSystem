using MenuService.Data;
using MenuService.Interfaces;
using MenuService.Repositories;
using MenuService.Services;
using RestaurantManagement.SharedLibrary.DependencyInjection;

namespace MenuService.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainer.AddSharedServices<MenuContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            // Register the repository and service for DI
            services.AddScoped<IMenuItem, MenuItemRepository>();
            services.AddScoped<MenuItemService>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register Middleware such as:
            // Global Exception
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
