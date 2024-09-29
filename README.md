# Airbnb Clone - Backend

## Project Overview
This project is a backend implementation for an Airbnb clone, developed using .NET Core. The system provides essential features like property listings, booking management, user authentication, and Stripe payment integration.

## Features
- **User Authentication**: Register, login, and manage user sessions securely using JWT.
- **Property Listings**: Create, update, and manage property listings with filtering options.
- **Booking Management**: Manage property bookings with task scheduling for updates.
- **Search and Filters**: Implemented advanced filtering for property search (location, price, availability).
- **Payment Integration**: Seamless payment processing using Stripe API.
- **Role-Based Access**: Admin, host, and guest roles with specific privileges.

## Technology Stack
- **Language**: C#
- **Framework**: .NET Core
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Payment Gateway**: Stripe API
- **Database**: SQL Server
- **Architecture**: Clean Architecture
- **Design Patterns**: Repository, Unit of Work, Dependency Injection
- **Caching**: Implemented to optimize performance

## Setup & Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/SharaawyMohamed/Airbnb-Project.git
    ```
2. Navigate to the project directory:
    ```bash
    cd Airbnb-Project
    ```
3. Install the necessary packages:
    ```bash
    dotnet restore
    ```
4. Update the connection string in `appsettings.json` to match your database configuration.
5. Run database migrations:
    ```bash
    dotnet ef database update
    ```
6. Run the project:
    ```bash
    dotnet run
    ```

# Airbnb Clone API Documentation

## Account Endpoints

- **POST** `/api/Account/Login`  
  Allows users to log in to the system.

- **GET** `/api/Account/SignOut`  
  Signs out the user from the system.

- **POST** `/api/Account/Register`  
  Registers a new user in the system.

- **POST** `/api/Account/EmailConfirmation`  
  Confirms user email.

- **GET** `/api/Account/ForgetPassword`  
  Initiates the password reset process by sending an email to the user.

- **PUT** `/api/Account/ResetPassword`  
  Resets the user's password after confirmation.

---

## Booking Endpoints

- **POST** `/api/Booking/CreateBooking`  
  Creates a new booking.

- **GET** `/api/Booking/GetBookings/{userId}`  
  Retrieves all bookings for a specific user.

- **DELETE** `/api/Booking/DeleteBooking/{bookingId}`  
  Deletes a booking with the specified ID.

---

## Property Endpoints

- **GET** `/api/Property/GetProperties`  
  Retrieves all available properties.

- **GET** `/api/Property/GetProperty/{propertyId}`  
  Retrieves details of a property by its ID.

- **POST** `/api/Property/CreateProperty`  
  Creates a new property listing.

- **DELETE** `/api/Property/DeleteProperty/{propertyId}`  
  Deletes a property by its ID.

- **PUT** `/api/Property/UpdateProperty`  
  Updates details of an existing property.

---

## Review Endpoints

- **GET** `/api/Review/GetAllReviews`  
  Retrieves all reviews for properties.

- **GET** `/api/Review/GetReviewDetails`  
  Retrieves details of a specific review.

- **POST** `/api/Review/CreateReview`  
  Creates a new review for a property.

- **PUT** `/api/Review/UpdateReview/{id}`  
  Updates a specific review by its ID.

- **DELETE** `/api/Review/{id}`  
  Deletes a specific review by its ID.

---

## Users Endpoints

- **GET** `/api/Users/GetAllUsers`  
  Retrieves a list of all users.

- **GET** `/api/Users/GetUserById/{Id}`  
  Retrieves details of a user by their ID.

- **DELETE** `/api/Users/RemoveUser/{Id}`  
  Deletes a user by their ID.

- **POST** `/api/Users/CreateUser`  
  Creates a new user in the system.

- **PUT** `/api/Users/UpdateUser`  
  Updates an existing user's information.

---

## Additional Schemas and DTOs

- **CreateBookingCommand**  
  Data transfer object for creating a booking.

- **ForgetPasswordDto**  
  Data transfer object for forgetting a password.

- **LoginDTO**  
  Data transfer object for user login.

- **ResetPasswordDTO**  
  Data transfer object for resetting a password.

- **ReviewDto**  
  Data transfer object for reviews.

---

## Contribution
Feel free to contribute to this project by submitting a pull request or reporting issues.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
