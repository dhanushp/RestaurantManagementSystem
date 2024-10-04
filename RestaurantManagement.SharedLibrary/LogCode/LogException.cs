using Serilog;

namespace RestaurantManagement.SharedLibrary.Logs
{
    // static removes the need for having dependency injection
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            // Logs the exception message and stack trace to different outputs
            LogToFile(ex);
            LogToConsole(ex);
            LogToDebugger(ex);
        }
        public static void LogToFile(Exception ex) => Log.Information($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");

        public static void LogToConsole(Exception ex) => Log.Warning($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");

        public static void LogToDebugger(Exception ex) => Log.Debug($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");

        /*
        public static void LogToFile(Exception ex) => Log.Information($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");

        public static void LogToConsole(Exception ex) => Log.Warning($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");

        public static void LogToDebugger(Exception ex) => Log.Debug($"Exception: {ex.Message}, StackTrace: {ex.StackTrace}");
        */
    }
}
