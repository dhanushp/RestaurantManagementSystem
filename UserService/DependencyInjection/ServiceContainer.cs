using RestaurantManagement.SharedLibrary.DependencyInjection;
using UserService.Data;
using UserService.Interfaces;
using UserService.Repositories;

namespace UserService.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity

            // Add JWT Authentication Scheme
            SharedServiceContainer.AddSharedServices<UserDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            services.AddScoped<IAuthentication, AuthenticationRepository>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IRole, RoleRepository>();

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
