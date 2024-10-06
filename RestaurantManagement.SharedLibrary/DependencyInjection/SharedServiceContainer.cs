using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.SharedLibrary.Middleware;
using Serilog;

namespace RestaurantManagement.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config, string fileName) where TContext: DbContext
        {
            // Add Generic Database Context
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                config
                .GetConnectionString("DefaultConnection"), sqlserverOption =>
                sqlserverOption.EnableRetryOnFailure()));

            // configure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"Logs//{fileName}-.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add JWT Authentication Scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            // Use Global Exception
            app.UseMiddleware<GlobalException>();

            // Register middle to block all outsiders API calls
            //app.UseMiddleware<ListenToOnlyAPIGateway>();

            return app;
        }
    }
}
