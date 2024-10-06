# UserService

## Overview
The **UserService** is responsible for managing all user-related operations within the Restaurant Management System. This includes user registration, login, password management, and user role management. It provides a RESTful API to handle user authentication and profile management.

---

## Code Structure

### 1. Project Files
- **UserService.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Holds configuration settings like connection strings for the database.
- **appsettings.Development.json**: Contains settings specific to the development environment.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing users.
  - **UserController.cs**: This controller handles all user-related requests.
    - `RegisterUser(UserRegisterDto userRegisterDto)`: Registers a new user with details like full name, email, and password.
    - `Login(UserLoginDto userLoginDto)`: Authenticates a user and returns a JWT token.
    - `ResetPassword(ResetPasswordDto resetPasswordDto)`: Handles password reset functionality using email verification.
  - Methods in this controller interact with the `UserContext` to manage user data and ensure proper authentication and role assignment.

### 3. Data
- **UserContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the user database.
  - Contains **DbSet<User> Users** and **DbSet<Role> Roles** representing user accounts and associated roles.
  - Configures relationships between entities, such as the one-to-many relationship between `Role` and `User`.

### 4. DependencyInjection
- This folder contains helper classes that register dependencies such as repositories, services, and contexts.
- **ServiceRegistration.cs**: Registers services like `UserRepository`, `JwtService`, etc., for dependency injection.

### 5. DTOs
- The **DTOs** folder contains data transfer objects used for transferring data between client and server.
  - **UserRegisterDto.cs**: Contains fields like `FullName`, `Email`, and `Password` used during user registration.
  - **UserLoginDto.cs**: Contains `Email` and `Password` used during login.
  - **ResetPasswordDto.cs**: Contains fields used for resetting a user's password.

### 6. Interfaces
- The **Interfaces** folder defines repository interfaces to ensure a separation of concerns.
  - **IUserRepository.cs**: Defines methods for accessing user data, such as `GetUserByEmail(string email)` and `AddUser(User user)`.

### 7. Migrations
- The **Migrations** folder contains generated classes representing changes to the database schema.
  - Migrations provide a history of schema modifications, making it easy to track and roll back changes if needed.

### 8. Models
- The **Models** folder defines the data structures used in the UserService.
  - **User.cs**: Represents a user in the system, with properties like:
    - `UserId`: Unique identifier for each user.
    - `FullName`: The user's full name.
    - `Email`: The user's email address, used for authentication.
    - `PasswordHash`: A hashed representation of the user's password.
    - `Role`: The role assigned to the user (e.g., Customer, Admin).
  - **Role.cs**: Represents different roles that a user can have, such as `Admin`, `Staff`, and `Customer`.
- These models map to database tables and provide an easy way to interact with the user data.

### 9. Repositories
- The **Repositories** folder contains classes that encapsulate database access logic.
  - **UserRepository.cs**: Implements `IUserRepository` to provide methods for user management, such as adding a new user, finding users by email, etc.

### 10. Program.cs
- **Program.cs** configures the service, including:
  - Setting up **dependency injection** for `UserContext` and repositories.
  - Configuring the **HTTP request pipeline**.
  - Registering **Swagger** for API documentation.
  - Setting up **JWT Authentication** to protect routes.

### 11. Properties
- Contains **launchSettings.json**, which is used by Visual Studio for debugging settings.
- Defines settings such as the application URL and environment variables used during development.

---

## Example Usage
1. **User Registration**:
   - Endpoint: `POST /api/users/register`
   - A `UserRegisterDto` object is sent in the request body, which includes `FullName`, `Email`, and `Password` to register a new user.

2. **User Login**:
   - Endpoint: `POST /api/users/login`
   - A `UserLoginDto` object is sent in the request body, containing the `Email` and `Password`. If authenticated, a JWT token is returned.

3. **Password Reset**:
   - Endpoint: `POST /api/users/resetpassword`
   - A `ResetPasswordDto` object is sent in the request body to reset a user's password after verifying their email.

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the UserService.
- **Entity Framework Core**: To manage database operations and data persistence.
- **SQL Server**: The underlying database for storing user and role information.
- **JWT (JSON Web Tokens)**: To authenticate and authorize users.
- **Swagger**: Provides an interactive API documentation interface to test endpoints during development.

## Future Improvements
- **User Profile Management**: Add functionality to allow users to update their profile information.
- **Account Deactivation**: Implement functionality to allow users to deactivate or delete their accounts.
- **Enhanced Security**: Improve password security by implementing multi-factor authentication (MFA).

---