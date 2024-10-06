namespace RestaurantManagement.SharedLibrary.Data
{
    public enum ErrorCode
    {
        InvalidCredentials = 1001,
        UserAlreadyExists = 1002,
        RegistrationFailed = 1003,
        UserNotFound,
        RoleNotFound,
        ServiceUnavailable
    }

}
