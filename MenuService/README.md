# MenuService

## Overview
The **MenuService** is responsible for managing the restaurant's menu items, including adding, updating, deleting, and retrieving menu items. It provides a RESTful API that allows interaction with the menu, which can be accessed by both customers and administrators.

---

## Code Structure

### 1. Project Files
- **MenuService.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Holds configuration settings like connection strings for the database.
- **appsettings.Development.json**: Contains settings specific to the development environment, often overriding values in `appsettings.json`.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing menu items.
  - **MenuController.cs**: This controller handles all menu-related requests.
    - `GetMenuItems()`: Retrieves all menu items available in the restaurant.
    - `GetMenuItemById(int id)`: Retrieves a specific menu item based on its ID.
    - `AddMenuItem(MenuItem menuItem)`: Adds a new menu item to the database.
    - `UpdateMenuItem(int id, MenuItem menuItem)`: Updates an existing menu item.
    - `DeleteMenuItem(int id)`: Deletes a menu item by its ID.
  - Each of these methods calls the `MenuContext` to interact with the database and performs validation to ensure data integrity.

### 3. Data
- **MenuContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the database.
  - Contains **DbSet<MenuItem> MenuItems** which represents the menu items in the database.
  - Configures relationships, constraints, and database table mappings.

### 4. Migrations
- The **Migrations** folder contains generated classes that represent changes to the database schema.
  - Migration files are used to apply or rollback changes to the database, providing a history of schema modifications.

### 5. Models
- The **Models** folder defines the data structures used in the MenuService.
  - **MenuItem.cs**: Represents the structure of a menu item, with properties like:
    - `Id`: Unique identifier for each menu item.
    - `Name`: Name of the menu item.
    - `Description`: A short description of the menu item.
    - `Price`: Price of the menu item.
    - `Category`: Category to which the menu item belongs (e.g., Starter, Main Course, Dessert).
    - `IsNonVeg`: Boolean field indicating if the item is non-vegetarian.
    - `ImageUrl`: URL of the image representing the menu item.
  - These models are used by Entity Framework to create and manage database tables.

### 6. Program.cs
- **Program.cs** configures the service, including:
  - Setting up **dependency injection** for `MenuContext`.
  - Configuring the **HTTP request pipeline**.
  - Registering **Swagger** for API documentation.
  - Setting up **CORS policies** to allow cross-origin requests.

### 7. Properties
- Contains **launchSettings.json** which is used by Visual Studio for debugging settings.
- Defines the application URL and environment variables used during development.

### 8. Additional Files
- **MenuService.http**: This file contains HTTP request examples that can be used to test the API endpoints using tools like **Visual Studio Code REST Client**.

---

## Example Usage
1. **Retrieving All Menu Items**:
   - Endpoint: `GET /api/menu`
   - This action returns a list of all menu items available in the restaurant.

2. **Adding a New Menu Item**:
   - Endpoint: `POST /api/menu`
   - The admin sends a `MenuItem` object in the request body to add it to the database. Required fields include `Name`, `Price`, `Category`, and `IsNonVeg`.

3. **Updating an Existing Menu Item**:
   - Endpoint: `PUT /api/menu/{id}`
   - Allows the admin to update details of a specific menu item, such as changing the price or updating the description.

4. **Deleting a Menu Item**:
   - Endpoint: `DELETE /api/menu/{id}`
   - This action removes the menu item from the database based on its ID.

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the MenuService.
- **Entity Framework Core**: To manage database operations and data persistence.
- **SQL Server**: The underlying database for storing menu items.
- **Swagger**: Provides an interactive API documentation interface to test endpoints during development.

## Future Improvements
- **Data Validation**: Add validation attributes to `MenuItem` properties to ensure that only valid data is entered (e.g., `Price` should be positive).
- **Caching**: Implement caching to improve performance when retrieving frequently accessed data like menu items.
- **Search and Filtering**: Add support for searching menu items by name or filtering by category to improve user experience.

---