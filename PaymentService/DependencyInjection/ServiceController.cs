using PaymentService.Data;
using PaymentService.Interfaces;
using PaymentService.Repositories;
using RestaurantManagement.SharedLibrary.DependencyInjection;

namespace PaymentService.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {

            SharedServiceContainer.AddSharedServices<PaymentDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection
            // Register the repository and service for DI
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register Middleware such as:
            // Global Exception
            // API Gateway
            //SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
