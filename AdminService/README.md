# AdminService

## Overview
The **AdminService** is a part of the Restaurant Management System that handles all the administrative tasks for the restaurant. This includes managing menu items, user roles, and monitoring customer orders. The AdminService follows a RESTful API architecture, which allows it to perform CRUD (Create, Read, Update, Delete) operations for the administrative domain.

---

## Code Structure

### 1. Project Files
- **AdminService.csproj**: This file defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Contains configuration settings such as connection strings for the database.
- **appsettings.Development.json**: Contains development-specific settings, usually overriding values in `appsettings.json`.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing administrative functions.
  - **AdminController.cs**: This controller provides endpoints to manage menu items, update user roles, and monitor order statuses. Typical actions include:
    - `GetMenuItems()`: Retrieves all menu items.
    - `AddMenuItem(MenuItem menuItem)`: Adds a new menu item.
    - `UpdateRole(int userId, string role)`: Updates the role of a specific user.
  - Each action method uses dependency injection to call service classes or repositories, making it easier to manage business logic.

### 3. Data
- **AdminContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the database.
  - It contains **DbSet<T>** properties such as:
    - `DbSet<MenuItem> MenuItems` - Represents the menu items managed by the admin.
    - `DbSet<Role> Roles` - Represents roles assigned to users.
- The context also defines relationships and database table mappings.

### 4. Migrations
- The **Migrations** folder contains generated classes that define database schema changes.
  - Migration files are created using the Entity Framework command line tools and provide a history of schema modifications. This enables tracking changes and rolling back to previous states if needed.

### 5. Models
- The **Models** folder defines the data structures for this service.
  - **MenuItem.cs**: Represents the structure of a menu item, with fields like `Name`, `Price`, `Category`, and `IsAvailable`.
  - **Role.cs**: Represents different user roles, such as `Admin`, `Staff`, and `Customer`.
  - These models are used in conjunction with the `AdminContext` to map to database tables.

### 6. Program.cs
- **Program.cs** configures the service, including dependency injection, middleware, and API routing.
  - Configures services such as **AdminContext** to connect to the database.
  - Registers controllers and sets up Swagger for API documentation.
  - Sets up CORS policies if necessary to allow cross-origin requests.

### 7. Properties
- Contains **launchSettings.json** which is used by Visual Studio for debugging settings.
- Defines the settings such as application URL and environment variables used when running the project locally.

### 8. Additional Notes
- **Dependency Injection**: The service uses dependency injection to provide instances of `AdminContext` and other required services into controllers.
- **Entity Framework Core**: The service uses EF Core as its ORM for interacting with the SQL Server database.
- **Swagger**: Swagger is configured in **Program.cs** to provide a UI for testing the available API endpoints during development.

### Example Usage
1. **Getting All Menu Items**:
   - Endpoint: `GET /api/admin/menuitems`
   - This action returns all the menu items managed by the admin, allowing them to review and modify the list as needed.

2. **Adding a New Menu Item**:
   - Endpoint: `POST /api/admin/menuitems`
   - The admin sends a `MenuItem` object in the request body to add it to the database.

3. **Updating a User's Role**:
   - Endpoint: `PUT /api/admin/user/{userId}/role`
   - This allows the admin to update the role of a particular user (e.g., promoting a staff member to an admin).

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the AdminService.
- **Entity Framework Core**: To manage database operations and mapping.
- **SQL Server**: The underlying database for storing menu items, roles, etc.
- **Swagger**: Provides an interactive API documentation interface to test endpoints.

## Future Improvements
- **Authentication & Authorization**: Add JWT-based authentication to ensure only authorized admins can perform certain actions.
- **Data Validation**: Add validation attributes to models to enforce data integrity before reaching the database.
- **Unit Testing**: Implement unit tests for the controller methods to ensure the reliability of core functionalities.

---