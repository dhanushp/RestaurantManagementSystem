using Serilog;

namespace RestaurantManagement.SharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            // Logs the exception message and stack trace to all configured sinks (File, Console, Debugger)
            Log.Error(ex, "An error occurred: {Message}", ex.Message);
        }

        public static void LogExceptions(Exception ex, string userId)
        {
            Log.Error(ex, "An error occurred for user {UserId}: {Message}", userId, ex.Message);
        }

    }
}
