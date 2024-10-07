using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestaurantManagement.SharedLibrary.Responses;

namespace RestaurantManagement.SharedLibrary.Middleware
{
    public class ListenToOnlyAPIGateway(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Extract specific header from the request
            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL means the request is not coming from the API Gateway - 503 service unavailable
            if(signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                var jsonResponse = JsonSerializer.Serialize(
                    Response<string>.ErrorResponse("Sorry, service is unavailable", Data.ErrorCode.ServiceUnavailable),
                    new JsonSerializerOptions { WriteIndented = true } 
                );

                await context.Response.WriteAsync(jsonResponse);
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
