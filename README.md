# **Airbnb Clone Backend Project**

## **Overview**
This project is a **backend implementation of an Airbnb Clone**, designed to replicate core functionalities of the Airbnb platform. Built with **.NET Core 8**, it focuses on clean architecture, scalability, and real-time communication. It provides seamless features for user authentication, property listings, booking management, and notification services.  

Whether you’re a developer exploring modern .NET practices or a stakeholder looking for a robust solution, this project demonstrates best practices, including **CQRS**, **Dependency Injection**, and real-time notifications with **SignalR**.

---

## **Table of Contents**
- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Project Architecture](#project-architecture)
- [Design Patterns](#design-patterns)
- [Database Schema](#database-schema)
- [Endpoints](#endpoints)
- [Payment Integration](#payment-integration)
- [Redis Integration](#redis-integration)
- [Installation and Setup](#installation-and-setup)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## **Technologies Used**
- **Backend Framework:** .NET Core 8
- **Real-Time Communication:** SignalR
- **Data Storage:** SQL Server with Entity Framework Core
- **Caching:** Redis for improved performance and scalability
- **Authentication & Authorization:** ASP.NET Identity, JWT Tokens
- **API Documentation:** Swagger
- **Testing Framework:** xUnit with Moq
- **Messaging Pattern:** MediatR for CQRS implementation
- **Additional Tools:** AutoMapper, Mapster, FluentValidation, SendEmail, Dependency Injection

---

## **Features**

### **1. User Management**
- Secure registration and login with email verification.
- Password reset functionality for user convenience.
- JWT token-based authentication for secure and stateless communication.

### **2. Property Management**
- Advanced filtering and search options for property listings.
- CRUD operations for managing properties by hosts.
- Image upload and management for properties.

### **3. Booking System**
- Dynamic pricing based on demand and availability.
- Booking creation, management, and cancellation features.

### **4. Real-Time Notifications**
- **Public Notifications:** Broadcast system-wide updates.
- **User-Specific Notifications:** Personalized notifications for actions like booking confirmations.

### **5. Caching**
- **Redis** is used for optimizing frequently accessed resources such as:
  - User sessions
  - Property listings
---

## **Project Architecture**

This project adopts the **Clean Architecture** approach to ensure modularity, scalability, and ease of testing.

### **Core Layers**
1. **Domain Layer**
   - Contains the core business entities, interfaces, and rules.
   - Example: `Property`, `User`, `Booking`.

2. **Application Layer**
   - Handles business logic, services, and use cases.
   - Includes MediatR handlers, validation, and mapping logic.

3. **Infrastructure Layer**
   - Handles communication with external systems.
   - Manages database context (EF Core), Redis caching, and SignalR hubs.

4. **Presentation Layer (API)**
   - Provides endpoints for the frontend to interact with the backend.
   - Includes controllers and middleware for request/response handling.

### **Advantages**
- **Scalability:** Layers can be extended independently.
- **Separation of Concerns:** Each layer has a distinct responsibility.
- **Testability:** Business logic is isolated for easier unit testing.
- **Flexibility:** Technologies like Redis and SignalR are decoupled from core business logic.

---

## **Design Patterns**
This project implements several design patterns:
- **CQRS (Command Query Responsibility Segregation):**
  - Separates the read and write operations to enhance performance and maintainability.
- **Repository Pattern:**
  - Centralizes database queries to keep the codebase clean and testable.
- **Singleton Pattern:**
  - Ensures a single instance of Redis client and connection manager.
- **Observer Pattern:**
  - Used in SignalR to push real-time notifications to connected clients.
- **Dependency Injection:**
  - Achieved via ASP.NET Core's built-in DI container for better modularity.
- **UnitOfWork:**
  - Ensures atomicity by coordinating multiple repository operations within a single transaction, allowing changes to be committed or rolled back as a unit.

---

## **Database Schema**

### **Key Tables**
1. **Users**: Stores user credentials and profile details.
2. **Properties**: Contains property details like description, location, and images.
3. **Bookings**: Tracks property reservations and booking statuses.
---

## **Endpoints**

Here is a comprehensive list of **31 endpoints**, categorized by their roles in the system:

### **Authentication Endpoints**
1. **POST `/api/Account/Login`**: Authenticate user and return a JWT token.
2. **GET `/api/Account/SignOut`**: Sign out the user.
3. **POST `/api/Account/Register`**: Register a new user and send a confirmation email.
4. **POST `/api/Account/EmailConfirmation`**: Activate user account through email verification.
5. **GET `/api/Account/ForgetPassword`**: Send password reset instructions.
6. **PUT `/api/Account/ResetPassword`**: Reset user password.

---

### **Booking Endpoints**
7. **GET `/api/Bookings/GetUserBookings`**: Retrieve bookings for a user.
8. **GET `/api/Bookings/GetBookingById`**: Retrieve booking details by ID.
9. **GET `/api/Bookings/GetAllBookings`**: Retrieve all bookings.

---

### **Payment Booking Endpoints**
10. **GET `/api/PaymentBooking/GetBookingById/{bookingId}`**: Get payment details for a specific booking.
11. **POST `/api/PaymentBooking/CreateBooking`**: Create a payment booking.
12. **PUT `/api/PaymentBooking/UpdateBooking`**: Update a payment booking.
13. **DELETE `/api/PaymentBooking/DeleteBooking/{bookingId}`**: Delete a booking.
14. **POST `/api/PaymentBooking/PayBooking`**: Pay for a booking.
15. **POST `/api/PaymentBooking/RegisterBooking`**: Register a new booking.

---

### **Property Endpoints**
16. **[GET `/api/Property/GetProperties`](#detailed-description-of-get-apipropertygetproperties)**: Retrieve a list of properties.
17. **GET `/api/Property/GetProperty/{propertyId}`**: Retrieve a specific property by ID.
18. **POST `/api/Property/CreateProperty`**: Add a new property.
19. **DELETE `/api/Property/DeleteProperty/{propertyId}`**: Delete a property by ID.
20. **PUT `/api/Property/UpdateProperty`**: Update an existing property.

---

### **Review Endpoints**
21. **GET `/api/Review/GetPropertyReviews`**: Retrieve reviews for a property.
22. **GET `/api/Review/GetUserReviews`**: Retrieve reviews made by a user.
23. **GET `/api/Review/GetReviewById/{id}`**: Retrieve a review by its ID.
24. **DELETE `/api/Review/DeleteReview/{id}`**: Delete a review by ID.
25. **POST `/api/Review/CreateReview`**: Create a new review.
26. **PUT `/api/Review/UpdateReview`**: Update an existing review.

---

### **User Management Endpoints**
27. **GET `/api/Users/GetAllUsers`**: Retrieve a list of all users.
28. **GET `/api/Users/GetUserById/{Id}`**: Retrieve a user by ID.
29. **DELETE `/api/Users/RemoveUser/{Id}`**: Delete a user by ID.
30. **POST `/api/Users/CreateUser`**: Create a new user.
31. **PUT `/api/Users/UpdateUser`**: Update user details.

---

## **Property Filtering and Sorting**

## **Detailed Description of GET /api/Property/GetProperties**

### **GET `/api/Property/GetProperties`**

This endpoint retrieves a list of properties with various filtering and sorting options.

#### **Query Parameters**
- **sort** (`string`): Sort properties by rate or price (e.g., `RateAsc`, `NightPriceDesc`, `Name`).
- **categoryName** (`string`): Filter by property category (e.g., apartment, house).
- **locationId** (`integer`): Filter by location ID.
- **startDate** (`string` - date-time): Filter properties available starting from this date.
- **endDate** (`string` - date-time): Filter properties available until this date.
- **pageIndex** (`integer`): The page number for pagination.
- **pageSize** (`integer`): The number of properties per page.
