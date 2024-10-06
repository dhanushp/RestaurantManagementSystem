# OrderService

## Overview
The **OrderService** handles all the operations related to customer orders, including placing orders, updating order statuses, and managing the overall order lifecycle. It exposes a RESTful API that allows customers to place orders and allows staff to update their statuses.

---

## Code Structure

### 1. Project Files
- **OrderService.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Holds configuration settings like connection strings for the database.
- **appsettings.Development.json**: Contains settings specific to the development environment.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing customer orders.
  - **OrderController.cs**: This controller handles all order-related requests.
    - `GetOrders()`: Retrieves all orders made by customers.
    - `GetOrderById(int id)`: Retrieves a specific order based on its ID.
    - `CreateOrder(Order order)`: Places a new order for a customer.
    - `UpdateOrderStatus(int id, string status)`: Updates the status of an existing order (e.g., from "pending" to "in preparation").
  - Methods in this controller interact with the `OrderContext` to manage order data and use dependency injection to call services or repositories.

### 3. Data
- **OrderContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the order database.
  - Contains **DbSet<Order> Orders** and **DbSet<OrderItem> OrderItems** which represent orders and their associated items.
  - Defines relationships between entities, such as the one-to-many relationship between `Order` and `OrderItem`.

### 4. Migrations
- The **Migrations** folder contains generated classes representing changes to the database schema.
  - Migrations provide a history of schema modifications, making it easy to track and roll back changes if needed.

### 5. Models
- The **Models** folder defines the data structures used in the OrderService.
  - **Order.cs**: Represents an order placed by a customer, with properties like:
    - `OrderId`: Unique identifier for each order.
    - `UserId`: The ID of the user who placed the order.
    - `OrderDate`: The date and time when the order was placed.
    - `CreatedAt`: A timestamp indicating when the order was created.
    - `OrderItems`: A collection of items associated with the order.
  - **OrderItem.cs**: Represents individual items within an order, with properties like:
    - `OrderItemId`: Unique identifier for each order item.
    - `MenuItemId`: The ID of the menu item ordered.
    - `Quantity`: The number of units ordered.
    - `MenuItemPrice`: Price of the menu item at the time of order.
- These models map to database tables and provide an easy way to interact with the order data.

### 6. Program.cs
- **Program.cs** configures the service, including:
  - Setting up **dependency injection** for `OrderContext`.
  - Configuring the **HTTP request pipeline**.
  - Registering **Swagger** for API documentation.
  - Configuring **CORS policies** to allow cross-origin requests.

### 7. Properties
- Contains **launchSettings.json**, which is used by Visual Studio for debugging settings.
- Defines settings such as the application URL and environment variables used during development.

---

## Example Usage
1. **Placing a New Order**:
   - Endpoint: `POST /api/orders`
   - The customer sends an `Order` object in the request body to place a new order. Required fields include `UserId`, `OrderItems`, and `OrderDate`.

2. **Updating Order Status**:
   - Endpoint: `PUT /api/orders/{id}/status`
   - Staff members can update the status of an order by specifying the `OrderId` and the new status (e.g., "in preparation", "ready for pickup").

3. **Retrieving All Orders**:
   - Endpoint: `GET /api/orders`
   - This action returns a list of all orders placed, which can be filtered by `UserId` to get orders for a specific customer.

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the OrderService.
- **Entity Framework Core**: To manage database operations and data persistence.
- **SQL Server**: The underlying database for storing orders and order items.
- **Swagger**: Provides an interactive API documentation interface to test endpoints during development.

## Future Improvements
- **Error Handling**: Improve error handling for cases where an order cannot be placed or updated due to data inconsistencies.
- **Data Validation**: Add validation attributes to ensure only valid data is entered (e.g., `Quantity` should be greater than 0).
- **Order Tracking**: Add the ability for customers to track the status of their orders in real-time.

---