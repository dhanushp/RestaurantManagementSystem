# Restaurant Management System

This project is a Restaurant Management System built using **Blazor**, **.NET**, and **SQL Server**. The application allows users to manage orders, menu items, and payments while providing an admin dashboard for restaurant owners.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Contributing](#contributing)
- [License](#license)

## Features

- User registration and login
- Select table for ordering
- Menu browsing with item descriptions
- Place orders and view order summary
- Request bills and process payments
- Admin dashboard for managing menu items and monitoring orders

## Technologies

- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Architecture**: Microservices

## Architecture

The application is structured as a microservice architecture with the following services:

- **UserService**: Handles user registration, authentication, and role management.
- **OrderService**: Manages orders and order items.
- **MenuService**: Handles menu item operations (CRUD).
- **PaymentService**: Manages payment processing.
- **AdminService**: Admin functionalities such as managing menu items and viewing orders.
- **WebApp**: Blazor frontend for user interaction.

## Setup

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (latest version)
- [Visual Studio](https://visualstudio.microsoft.com/) or any preferred IDE
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Clone the Repository

```bash
git clone https://github.com/your-username/RestaurantManagementSystem.git
cd RestaurantManagementSystem
```

### Create the Database

1. Open **SQL Server Management Studio**.
2. Create a new database named `RestaurantDB`.

### Configure Connection String

1. Open the `appsettings.json` file in each service project.
2. Update the connection string under `"ConnectionStrings"` section:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server_name;Database=RestaurantDB;User Id=your_user_id;Password=your_password;"
    }
}
```

### Install Entity Framework Core

In each service project, install the necessary EF Core packages via NuGet Package Manager Console:

```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

### Run Migrations

In the **UserService** project, open the Package Manager Console and run:

```bash
Add-Migration InitialCreate
Update-Database
```

## Running the Application

1. Open the solution in Visual Studio.
2. Set the **UserService**, **OrderService**, **MenuService**, **PaymentService**, **AdminService**, and **WebApp** as startup projects.
3. Start the application (press F5 or click the Start button).

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any suggestions or improvements.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
