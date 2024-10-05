using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.SharedLibrary.Logs;

namespace RestaurantManagement.SharedLibrary.Middleware
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // default values
            string message = "Sorry, an unknown errror occured. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                // going to next middleware
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests ||
                    context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                    context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = context.Response.StatusCode switch
                    {
                        StatusCodes.Status429TooManyRequests => "Warning",
                        StatusCodes.Status401Unauthorized => "Alert",
                        StatusCodes.Status403Forbidden => "Out of Access",
                        _ => title
                    };

                    message = context.Response.StatusCode switch
                    {
                        StatusCodes.Status429TooManyRequests => "Too many requests made.",
                        StatusCodes.Status401Unauthorized => "You are not authorized to access.",
                        StatusCodes.Status403Forbidden => "You are not allowed/required to access.",
                        _ => message
                    };

                    statusCode = context.Response.StatusCode;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                // Log Original Exceptions / File, Console, Debuger
                LogException.LogExceptions(ex);

                // check if Exception is Timeout
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of Time";
                    message = "Request Timeout... Try Again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                // If none of the Exception then use default values
                await ModifyHeader(context, title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail= message,
                Status = statusCode,
                Title = title,

            }), CancellationToken.None);
        }
    }
}
