
# Restaurant Management System

This is a full-fledged **Restaurant Management System** built using **Blazor**, **.NET**, and **SQL Server**. The application is designed to streamline restaurant operations, offering features like order management, menu management, payment processing, and customer feedback, with a microservices architecture to ensure flexibility and scalability.

## Table of Contents

- [About the Project](#about-the-project)
- [Key Features](#key-features)
- [Technologies Used](#technologies-used)
- [Microservices Architecture](#microservices-architecture)
- [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## About the Project

The Restaurant Management System aims to simplify restaurant operations for both customers and owners. It allows customers to view the menu, place orders, track order status, and make payments. For restaurant owners, it offers an admin dashboard to manage tables, monitor orders, and track payments.

This project is designed with a **microservices architecture** to ensure different functionalities are modular and easy to manage, providing a scalable platform for future improvements.

## Key Features

- **Customer Lifecycle**: Users can register, log in, select tables, place orders, view order statuses, and make payments.
- **Menu Management**: Admins can manage menu items, including adding descriptions, categorizing items (e.g., `IsNonVeg`), and uploading images.
- **Order Tracking**: Customers can track the status of their orders in real time (pending, in preparation, ready for pickup, served).
- **Payments**: Customers can pay for their orders via cash or online methods.
- **Feedback System**: Customers can leave reviews and rate items after the meal. This feedback can later be used for AI-driven menu recommendations.
- **Admin Dashboard**: Allows restaurant owners to manage orders, update order statuses, monitor table availability, and track payments.
- **Microservices Architecture**: The system is modular with separate services for user, menu, order, payment, admin, and review management.

## Technologies Used

- **Blazor WebAssembly** for the frontend.
- **.NET** for building backend services.
- **SQL Server** for database management.
- **JWT Authentication** for secure user login and registration.
- **Swagger** for API testing and documentation.
- **Microservices** architecture for modular service management.

## Microservices Architecture

The system is split into the following microservices:

- **UserService**: Manages user registration, login, and password reset.
- **MenuService**: Handles menu items, including item descriptions, images, and categorization.
- **OrderService**: Manages customer orders, including order placement, status tracking, and order summaries.
- **PaymentService**: Handles payments, marking orders as paid, and releasing tables once payment is confirmed.
- **AdminService**: Allows the admin to update order statuses, view current tables, and monitor payments.
- **ReviewService**: Allows users to provide feedback on menu items and overall restaurant experience.

## Installation

To run the project locally, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/restaurant-management-system.git
   ```

2. Navigate to the project directory:
   ```bash
   cd restaurant-management-system
   ```

3. Install dependencies:
   ```bash
   dotnet restore
   ```

4. Update the database connection strings in the `appsettings.json` files of each microservice.

5. Apply migrations and update the database:
   ```bash
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```

## Usage

- **Customer**: Register and log in to the system, select a table, browse the menu, place orders, and make payments.
- **Admin**: Use the admin dashboard to monitor tables, update order statuses, and manage payments.
- **Feedback**: After completing an order, customers can leave reviews and rate the ordered items.

## API Documentation

The API for this project is documented using **Swagger**. You can access the API documentation at:

```
http://localhost:<port>/swagger
```

Use this documentation to explore and test the various endpoints for managing users, orders, menus, payments, and reviews.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests for any improvements or new features you'd like to see.

To contribute:

1. Fork the project.
2. Create a feature branch: `git checkout -b feature/YourFeature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/YourFeature`
5. Open a pull request.

## License

Distributed under the MIT License. See `LICENSE` for more information.
