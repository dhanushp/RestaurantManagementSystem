using OrderService.Data;
using OrderService.Interfaces;
using OrderService.Repositories;
using RestaurantManagement.SharedLibrary.DependencyInjection;

namespace OrderService.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            // Register the repository and service for DI
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register Middleware such as:
            // Global Exception
            // API Gateway
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
