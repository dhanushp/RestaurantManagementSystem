﻿using RestaurantManagement.SharedLibrary.DependencyInjection;
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
            services.AddScoped<IUser, UserRepository>();
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