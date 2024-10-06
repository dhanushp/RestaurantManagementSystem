# RestaurantManagement.SharedLibrary

## Overview
The **RestaurantManagement.SharedLibrary** is a shared library used by different services in the Restaurant Management System. It contains commonly used utilities, constants, models, middleware, and helper functions that are shared across multiple microservices to ensure code reusability and maintain consistency.

---

## Code Structure

### 1. Project Files
- **RestaurantManagement.SharedLibrary.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.

### 2. Common Components
The Shared Library contains various components that can be reused across multiple services:

#### 2.1 Data
- The **Data** folder contains shared data access classes and utility methods that facilitate working with databases across different services.

#### 2.2 Dependency Injection
- The **DependencyInjection** folder contains classes that help in setting up dependency injection.
  - **ServiceRegistration.cs**: Provides methods to register common services and utilities, which can be reused across multiple microservices for consistency.

#### 2.3 Interface
- The **Interface** folder defines interfaces that are implemented across various services.
  - These interfaces provide abstraction, making the services loosely coupled and promoting reusability.
  - For example, `ILoggerManager` defines the logging interface that is implemented across services for consistent logging.

#### 2.4 LogCode
- **LogCode**: This folder contains logging utilities that ensure uniform logging practices across all services.
  - **LoggerManager.cs**: Implements the logging interface and provides methods for logging messages of different severity levels (e.g., Information, Warning, Error).

#### 2.5 Middleware
- The **Middleware** folder contains shared middleware that can be plugged into the request pipeline of various microservices.
  - **ErrorHandlerMiddleware.cs**: A middleware component that handles exceptions globally across services, ensuring a standardized error response.

#### 2.6 Models
- The **Models** folder contains data models that are used across different services to ensure consistency.
  - **UserModel.cs**: Represents a simplified version of the user entity, shared across services where user details are required.
  - **OrderModel.cs**: Contains the structure for order details used by multiple services.
  - **PaymentModel.cs**: Represents the payment data shared among services that interact with payment details.

#### 2.7 Responses
- The **Responses** folder contains standard response models used by APIs to ensure consistent response formats.
  - **ApiResponse.cs**: A generic response model that wraps the data returned by API endpoints, including status, messages, and errors.

---

## Example Usage
1. **Dependency Injection Setup**:
   - The **ServiceRegistration** class provides methods to register common services like logging and error handling, which can be called in the `Program.cs` of each microservice.

2. **Logging**:
   - The **LoggerManager** class is used by multiple services for consistent logging. Instead of each service implementing its own logging logic, they use `ILoggerManager` from the shared library to log messages in a standardized way.

3. **Global Error Handling**:
   - The **ErrorHandlerMiddleware** class can be added to the middleware pipeline in each service to handle exceptions globally and return a consistent error response structure.

4. **Standard API Response**:
   - The **ApiResponse** class is used to standardize the format of API responses across all services, ensuring that clients always receive data in a consistent format.

---

## Technologies Used
- **.NET Standard Library**: The SharedLibrary is built using .NET Standard, which allows it to be referenced by multiple .NET Core projects.
- **C#**: The primary language used to write all the classes, models, and utilities.

## Future Improvements
- **Additional Utilities**: Add more utility functions for handling common functionalities such as caching and request validation.
- **Enhanced Documentation**: Provide detailed documentation for each method and class, so developers integrating the library can quickly understand how to use each component.
- **Improved Security**: Implement encryption utilities to secure sensitive data shared across services, like user and payment information.

---