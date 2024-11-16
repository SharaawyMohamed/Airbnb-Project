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
- **API Documentation:** Swagger/OpenAPI
- **Testing Framework:** xUnit with Moq
- **Messaging Pattern:** MediatR for CQRS implementation
- **Additional Tools:** AutoMapper, FluentValidation, Dependency Injection

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
  - Notification data

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

---

## **Database Schema**

### **Key Tables**
1. **Users**: Stores user credentials and profile details.
2. **Properties**: Contains property details like description, location, and images.
3. **Bookings**: Tracks property reservations and booking statuses.
4. **Notifications**: Stores notifications for system-wide and user-specific messages.

---

## **Endpoints**

Here is a comprehensive list of **31 endpoints**, categorized by their roles in the system:

### **Authentication Endpoints**
1. **POST `/api/auth/register`**: Register a new user and send a confirmation email.  
2. **POST `/api/auth/login`**: Authenticate user and return a JWT token.  
3. **POST `/api/auth/confirm-email`**: Activate user account through email verification.  
4. **POST `/api/auth/forgot-password`**: Send password reset instructions.  
5. **POST `/api/auth/reset-password`**: Reset user password.  

---

### **User Management Endpoints**
6. **GET `/api/users/profile`**: Retrieve logged-in user's profile.  
7. **PUT `/api/users/profile`**: Update user profile details.  
8. **DELETE `/api/users`**: Delete user account.  

---

### **Property Management Endpoints**
9. **GET `/api/properties`**: List all properties with filters (e.g., price, location).  
10. **GET `/api/properties/{id}`**: Get details of a specific property.  
11. **POST `/api/properties`**: Add a new property.  
12. **PUT `/api/properties/{id}`**: Update a property.  
13. **DELETE `/api/properties/{id}`**: Remove a property from the system.  

---

### **Booking Management Endpoints**
14. **GET `/api/bookings`**: List all bookings for a user or admin.  
15. **GET `/api/bookings/{id}`**: Retrieve booking details.  
16. **POST `/api/bookings`**: Create a new booking for a property.  
17. **PUT `/api/bookings/{id}`**: Modify booking details.  
18. **DELETE `/api/bookings/{id}`**: Cancel a booking.  

---

### **Notification Endpoints**
19. **GET `/api/notifications`**: Retrieve user-specific notifications.  
20. **POST `/api/notifications/public`**: Broadcast public notifications.  
21. **DELETE `/api/notifications/{id}`**: Remove specific notifications.  

---

### **Admin Endpoints**
22. **GET `/api/admin/users`**: Retrieve all user profiles for administrative purposes.  
23. **DELETE `/api/admin/users/{id}`**: Remove a user account.  
24. **GET `/api/admin/properties`**: Retrieve all properties for review.  
25. **DELETE `/api/admin/properties/{id}`**: Remove a property from the system.  

---

### **Real-Time Communication**
26. **Hub `/notificationHub`**: Handle SignalR real-time communication.  

---

### **Utility Endpoints**
27. **GET `/api/cache/properties`**: Fetch cached property data.  
28. **POST `/api/cache/clear`**: Clear specific Redis cache entries.  
29. **GET `/api/stats`**: Retrieve system performance metrics.  
30. **GET `/api/logs`**: Fetch application logs.  
31. **POST `/api/reports`**: Submit bug reports or issues.  

---

## **Redis Integration**

Redis is utilized for:
- **Session Caching:** Reduces database load by caching frequently used user session data.
- **Property Listings Cache:** Enhances response time for property search and filters.
- **Notifications Cache:** Stores user-specific notification data for quick access.

### **Configuration**
Ensure Redis is running and update the connection string in `appsettings.json`:
```json
"Redis": {
  "ConnectionString": "localhost:6379"
}
