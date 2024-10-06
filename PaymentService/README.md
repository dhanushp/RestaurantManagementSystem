# PaymentService

## Overview
The **PaymentService** handles all payment-related functionalities, including processing payments, generating bills, and marking orders as paid. It provides a RESTful API for handling payments, supporting both online and cash payment methods. It plays a critical role in ensuring the transaction process is seamless for customers and administrators.

---

## Code Structure

### 1. Project Files
- **PaymentService.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Holds configuration settings like connection strings for the database.
- **appsettings.Development.json**: Contains settings specific to the development environment.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing payments.
  - **PaymentController.cs**: This controller handles all payment-related requests.
    - `GetPayments()`: Retrieves a list of all payment transactions.
    - `GetPaymentById(int id)`: Retrieves a specific payment transaction based on its ID.
    - `CreatePayment(Payment payment)`: Processes a new payment for an order.
    - `GenerateBill(int orderId)`: Generates a bill for a completed order.
  - The methods in this controller use the `PaymentContext` to interact with payment and billing data, ensuring that payments are accurately recorded.

### 3. Data
- **PaymentContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the payment database.
  - Contains **DbSet<Payment> Payments** and **DbSet<Bill> Bills** representing payments and their associated bills.
  - Configures relationships between entities, such as the one-to-one relationship between `Payment` and `Bill`.

### 4. Migrations
- The **Migrations** folder contains generated classes that represent changes to the database schema.
  - Migrations provide a history of schema modifications, making it easy to apply or roll back changes as needed.

### 5. Models
- The **Models** folder defines the data structures used in the PaymentService.
  - **Payment.cs**: Represents a payment made by a customer, with properties like:
    - `PaymentId`: Unique identifier for each payment.
    - `OrderId`: The ID of the order associated with this payment.
    - `Amount`: The amount paid.
    - `PaymentMethod`: Specifies the payment method (e.g., cash, credit card).
    - `PaymentDate`: The date when the payment was made.
  - **Bill.cs**: Represents a bill generated for an order, with properties like:
    - `BillId`: Unique identifier for each bill.
    - `OrderId`: The ID of the order for which the bill is generated.
    - `TotalAmount`: Total amount including taxes.
    - `IsPaid`: Boolean field indicating whether the bill has been paid.
- These models map to database tables and provide an easy way to interact with the payment data.

### 6. Program.cs
- **Program.cs** configures the service, including:
  - Setting up **dependency injection** for `PaymentContext`.
  - Configuring the **HTTP request pipeline**.
  - Registering **Swagger** for API documentation.
  - Setting up **CORS policies** to allow cross-origin requests.

### 7. Properties
- Contains **launchSettings.json**, which is used by Visual Studio for debugging settings.
- Defines settings such as the application URL and environment variables used during development.

### 8. Additional Files
- **PaymentService.http**: This file contains HTTP request examples that can be used to test the API endpoints using tools like **Visual Studio Code REST Client**.

---

## Example Usage
1. **Processing a Payment**:
   - Endpoint: `POST /api/payments`
   - The customer sends a `Payment` object in the request body to process a payment. Required fields include `OrderId`, `Amount`, and `PaymentMethod`.

2. **Generating a Bill**:
   - Endpoint: `GET /api/payments/bill/{orderId}`
   - This action generates a bill for a specific order, including the total amount and the breakdown of charges.

3. **Retrieving All Payments**:
   - Endpoint: `GET /api/payments`
   - This action returns a list of all payment transactions, which can be filtered by `OrderId` to get payment details for a specific order.

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the PaymentService.
- **Entity Framework Core**: To manage database operations and data persistence.
- **SQL Server**: The underlying database for storing payment and bill information.
- **Swagger**: Provides an interactive API documentation interface to test endpoints during development.

## Future Improvements
- **Refund Management**: Add functionality to handle payment refunds in case of order cancellations.
- **Payment Gateway Integration**: Integrate with an external payment gateway for processing credit/debit card payments.
- **Error Handling**: Improve error handling to gracefully handle failed payment attempts and notify customers accordingly.

---