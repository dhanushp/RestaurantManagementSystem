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
            string message = "Sorry, internal server errror occured. Kindly try again";
            int statusCode = (int)HttpStatusCode .InternalServerError;
            string title = "Error";

            try
            {
                // going to next middleware
                await _next(context);

                // check if Exception is Too Many Request - 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request made.";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);

                }

                // if Response is UnAuthorized - 401 Status Code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access.";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }

                // If Response is Forbidden - 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowed/required to access.";
                    statusCode = (int)StatusCodes.Status403Forbidden;
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
