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
- [Installation and Setup](#installation-and-setup)
- [Usage](#usage)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## **Technologies Used**
- **Backend Framework:** .NET Core 8
- **Real-Time Communication:** SignalR
- **Data Storage:** SQL Server, Entity Framework Core
- **Authentication & Authorization:** ASP.NET Identity, JWT Tokens
- **Caching:** In-Memory Cache
- **API Documentation:** Swagger/OpenAPI
- **Testing:** xUnit with Mocking
- **Messaging Pattern:** MediatR for CQRS
- **Others:** AutoMapper, FluentValidation, Dependency Injection

---

## **Features**

### **1. User Management**
- **Registration & Login**: Secure user authentication with email confirmation.
- **JWT Authentication**: Generate and validate JSON Web Tokens for API access.
- **User Profiles**: View and update profile information.

### **2. Property Management**
- **CRUD Operations**: Create, update, delete, and retrieve property listings.
- **Search & Filter**: Search properties by location, price, or features.
- **Hosting Features**: Users can manage their listed properties.

### **3. Booking System**
- **Reserve Properties**: Book available properties with date validation.
- **View Bookings**: Users can view and manage their reservations.
- **Cancellation Policies**: Implemented through business rules.

### **4. Real-Time Notifications**
- **Public Notifications**: Broadcast system-wide updates.
- **User-Specific Notifications**: Notify users about their actions or updates (e.g., booking confirmations) using **SignalR**.

### **5. Caching**
- **In-Memory Cache**: Reduces load on the database for frequently accessed data.

### **6. Testing**
- **Unit Tests**: Comprehensive unit tests for business logic and services.
- **Mocking Framework**: Ensures isolated tests for each component.

---

## **Project Architecture**

This project follows **Clean Architecture** principles, ensuring separation of concerns:
- **Domain Layer**: Business logic and core entities.
- **Application Layer**: Application-specific business rules (CQRS, MediatR).
- **Infrastructure Layer**: Data persistence, third-party integrations (e.g., SignalR, SMTP).
- **Presentation Layer**: Exposes RESTful APIs via controllers.

### **Folder Structure**
```plaintext
├── Airbnb.Domain                 # Entities, Interfaces, DTOs
├── Airbnb.Application            # Business logic (CQRS, Services, Event Handling)
├── Airbnb.Infrastructure         # Database, External Services
├── Airbnb.API                    # Controllers and Middleware
