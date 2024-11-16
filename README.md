# **Airbnb Project Clone**

## **Overview**
This is a backend-focused project replicating key functionalities of Airbnb. Built using **.NET Core 8**, it emphasizes clean architecture, real-time notifications, and scalability. The application provides features such as property listings, booking management, and user authentication.

---

## **Table of Contents**
- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Design Patterns](#design-patterns)
- [Services and Tools](#services-and-tools)
- [Database Schema](#database-schema)
- [Endpoints](#endpoints)
- [Installation and Setup](#installation-and-setup)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

---

## **Technologies Used**
- **Backend Framework:** .NET Core 8
- **Real-time Communication:** SignalR
- **Data Storage:** SQL Server, Entity Framework Core
- **Caching:** In-Memory Cache
- **Authentication & Authorization:** ASP.NET Identity, JWT
- **Dependency Injection:** Built-in DI in .NET
- **Messaging:** MediatR for CQRS pattern
- **Testing:** xUnit for unit testing
- **Others:** AutoMapper, FluentValidation, Swagger

---

## **Features**
- **User Management:**
  - Registration, Login, and Email Confirmation
  - Profile management
- **Property Management:**
  - Add, Edit, and Delete listings
  - Property search with filtering
- **Booking System:**
  - Reserve properties with real-time updates
- **Real-time Notifications:**
  - Public notifications for system updates
  - User-specific notifications using SignalR
- **Caching:**
  - Optimized data retrieval for high performance
- **Testing:**
  - Unit tests for core services and controllers

---

## **Design Patterns**
This project follows modern software engineering principles and design patterns:

- **CQRS (Command Query Responsibility Segregation):**
  - Separation of commands (write operations) and queries (read operations) using MediatR.
- **Repository Pattern:**
  - Abstracting database operations with repositories.
- **Dependency Injection:**
  - Used for loose coupling and better testability.
- **Singleton Pattern:**
  - For services like caching and SignalR user connection management.
- **Factory Pattern:**
  - To create instances for different types of notifications.
- **Observer Pattern:**
  - SignalR clients observe changes and receive real-time updates.

---

## **Services and Tools**
- **User Management:** ASP.NET Identity for authentication and JWT for secure API access.
- **Notification Service:** SignalR for real-time user notifications.
- **Email Service:** SMTP or a third-party service like SendGrid for email confirmations.
- **Caching Service:** In-memory caching for reducing database load.
- **Database Access:** Entity Framework Core with migrations for database versioning.

---

## **Database Schema**
### Key Tables:
1. **Users**
   - Stores user details and roles.
2. **Properties**
   - Stores property details (name, location, price, etc.).
3. **Bookings**
   - Tracks property reservations.
4. **Notifications**
   - Logs user-specific and public notifications.

---

## **Endpoints**
### **Authentication:**
- **POST** `/api/auth/register`: Register a new user.
- **POST** `/api/auth/login`: Login and receive a JWT.
- **POST** `/api/auth/confirm-email`: Confirm email address.

### **Properties:**
- **GET** `/api/properties`: Get all properties with filters.
- **POST** `/api/properties`: Add a new property.
- **PUT** `/api/properties/{id}`: Edit property details.
- **DELETE** `/api/properties/{id}`: Delete a property.

### **Bookings:**
- **POST** `/api/bookings`: Book a property.
- **GET** `/api/bookings/user`: Get user-specific bookings.

### **Notifications:**
- **GET** `/api/notifications`: Get user notifications.
- **POST** `/api/notifications/public`: Send a public notification.

### **Real-Time Communication:**
- **Hub** `/notificationHub`: Handle real-time notifications using SignalR.

---

## **Installation and Setup**
### Prerequisites:
- .NET Core SDK 8+
- SQL Server
- Visual Studio or any IDE supporting .NET

### Steps:
1. Clone the repository:
   ```bash
   git clone https://github.com/SharaawyMohamed/Airbnb-Project.git
   cd Airbnb-Project
