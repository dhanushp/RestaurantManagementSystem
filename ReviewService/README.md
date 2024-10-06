# ReviewService

## Overview
The **ReviewService** handles customer feedback and reviews for menu items and the overall restaurant experience. This information can be used to enhance the quality of service, generate insights, and provide personalized recommendations through AI. It provides a RESTful API to allow customers to submit reviews and view feedback.

---

## Code Structure

### 1. Project Files
- **ReviewService.csproj**: Defines the project, specifying dependencies, frameworks, and other build configurations.
- **appsettings.json**: Holds configuration settings like connection strings for the database.
- **appsettings.Development.json**: Contains settings specific to the development environment.

### 2. Controllers
- The **Controllers** folder contains the API endpoints for managing customer reviews.
  - **ReviewController.cs**: This controller handles all review-related requests.
    - `GetReviews()`: Retrieves a list of all reviews submitted by customers.
    - `GetReviewById(int id)`: Retrieves a specific review based on its ID.
    - `SubmitReview(Review review)`: Submits a new review for an item or the overall restaurant experience.
    - `GetRestaurantReviewsByUser(int userId)`: Retrieves all reviews submitted by a specific user.
  - Methods in this controller use the `ReviewContext` to interact with review data, ensuring that feedback is accurately stored and retrieved.

### 3. Data
- **ReviewContext.cs**: This file is located within the **Data** folder and extends `DbContext`. It is used by Entity Framework to interact with the review database.
  - Contains **DbSet<Review> Reviews** and **DbSet<Rating> Ratings** which represent customer reviews and associated ratings.
  - Configures relationships between entities, such as the one-to-many relationship between `Review` and `Rating`.

### 4. Migrations
- The **Migrations** folder contains generated classes representing changes to the database schema.
  - Migrations provide a history of schema modifications, making it easy to track and roll back changes if needed.

### 5. Models
- The **Models** folder defines the data structures used in the ReviewService.
  - **Review.cs**: Represents a review submitted by a customer, with properties like:
    - `ReviewId`: Unique identifier for each review.
    - `UserId`: The ID of the user who submitted the review.
    - `MenuItemId`: The ID of the menu item being reviewed (optional for restaurant-level feedback).
    - `Comment`: The feedback text provided by the customer.
    - `Rating`: Numerical rating provided by the customer (e.g., on a scale from 1 to 5).
    - `ReviewDate`: The date when the review was submitted.
  - **Rating.cs**: Represents the rating aspect of the review, which can be used for various aspects like quality, taste, etc.
- These models map to database tables and provide an easy way to interact with the review data.

### 6. Program.cs
- **Program.cs** configures the service, including:
  - Setting up **dependency injection** for `ReviewContext`.
  - Configuring the **HTTP request pipeline**.
  - Registering **Swagger** for API documentation.
  - Setting up **CORS policies** to allow cross-origin requests.

### 7. Properties
- Contains **launchSettings.json**, which is used by Visual Studio for debugging settings.
- Defines settings such as the application URL and environment variables used during development.

### 8. Additional Files
- **ReviewService.http**: This file contains HTTP request examples that can be used to test the API endpoints using tools like **Visual Studio Code REST Client**.

---

## Example Usage
1. **Submitting a New Review**:
   - Endpoint: `POST /api/reviews`
   - The customer sends a `Review` object in the request body to submit feedback. Required fields include `UserId`, `Comment`, and `Rating`.

2. **Retrieving All Reviews**:
   - Endpoint: `GET /api/reviews`
   - This action returns a list of all reviews submitted by customers, providing valuable insights for the restaurant staff.

3. **Retrieving Reviews by User**:
   - Endpoint: `GET /api/reviews/user/{userId}`
   - Retrieves all reviews submitted by a specific user, allowing them to view their past feedback.

---

## Technologies Used
- **ASP.NET Core Web API**: To build the REST API for the ReviewService.
- **Entity Framework Core**: To manage database operations and data persistence.
- **SQL Server**: The underlying database for storing reviews and ratings.
- **Swagger**: Provides an interactive API documentation interface to test endpoints during development.

## Future Improvements
- **AI Integration**: Use customer feedback to generate personalized menu suggestions based on frequently reviewed items.
- **Sentiment Analysis**: Implement sentiment analysis on review comments to provide deeper insights into customer satisfaction.
- **Data Validation**: Add validation attributes to ensure only valid data is entered (e.g., `Rating` should be within a specified range).

---