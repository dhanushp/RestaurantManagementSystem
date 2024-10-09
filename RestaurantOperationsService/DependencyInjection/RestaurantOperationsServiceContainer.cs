using RestaurantManagement.SharedLibrary.DependencyInjection;
using RestaurantOperationsService.Data;
using RestaurantOperationsService.Interfaces;
using RestaurantOperationsService.Repositories;

namespace RestaurantOperationsService.DependencyInjection
{
    public static class RestaurantOperationsServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity

            // Add JWT Authentication Scheme
            SharedServiceContainer.AddSharedServices<RestaurantOperationsDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            services.AddScoped<ITable, TableRepository>();

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
