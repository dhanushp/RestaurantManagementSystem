using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RestaurantManagement.SharedLibrary.Middleware
{
    public class ListenToOnlyAPIGateway(RequestDelegate _next)
    {
        //private readonly RequestDelegate _next;

        //public ListenToOnlyAPIGateway(RequestDelegate next)
        //{
        //    _next = next;
        //}

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract specific header from the request
            var signedHeader = context.Request.Headers["Api-Gateway"];

            //NULL means the request is not coming from the API Gateway - 503 service unavailable
            if(signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
