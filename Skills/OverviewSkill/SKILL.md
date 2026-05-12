---
name: overview-skill
description: Kỹ năng này cung cấp cái nhìn tổng quan về hệ thống TechSales Management Backend, bao gồm phong cách kiến trúc (Modular Monolithic), Technology Stack, các module chính và triết lý lập trình cốt lõi của dự án.
---

# TechSales Management Backend

## 1. Overview

TechSales Management Backend is a modular monolithic backend system for an e-commerce platform specializing in technology products.

The system provides RESTful APIs for:
- Authentication and authorization
- Product management
- Inventory management
- Shopping cart operations
- Order processing
- Payment integration
- Voucher management
- Review management
- Administrative operations

The backend is designed with:
- Modular architecture
- Clear separation of concerns
- Secure authentication and authorization
- Maintainable code structure
- Standardized API responses
- Strong business rule enforcement

---

# 2. Architecture Style

The system follows a Modular Monolithic Architecture.

Characteristics:
- Single deployable backend application
- Shared relational database
- Internal module separation
- Layered architecture
- RESTful APIs
- Centralized authentication and exception handling

The architecture is designed to:
- Reduce complexity
- Improve maintainability
- Simplify debugging
- Support future scalability

## 2.1. Clean Architecture Structure

The system is divided into the following layers:

```text
Presentation Layer
        ↓
Application Layer
        ↓
Domain Layer
        ↓
Infrastructure Layer
```

# 3. Technology Stack

## Backend Framework
- ASP.NET Core Web API
- C#

## Database
- PostgreSQL

## ORM
- Entity Framework Core

## Cache
- Redis

## Authentication
- JWT Access Token
- Refresh Token

## API Documentation
- Swagger / OpenAPI

## Testing
- xUnit
- FluentAssertions
- Moq

---

# 4. Main Modules

## Identity Module
Responsibilities:
- Authentication
- Authorization
- RBAC
- Session management
- Password management

---

## Product Module
Responsibilities:
- Product CRUD
- Product specifications
- Product filtering and searching
- Product categorization

---

## Inventory Module
Responsibilities:
- Inventory tracking
- Stock synchronization
- Inventory rollback

---

## Cart Module
Responsibilities:
- Cart management
- Cart item updates
- Selected item persistence

---

## Order Module
Responsibilities:
- Order placement
- Order tracking
- Order cancellation
- Order status management

---

## Payment Module
Responsibilities:
- Payment initialization
- Payment verification
- Payment gateway integration

---

## Voucher Module
Responsibilities:
- Voucher validation
- Promotion rules
- Discount calculation

---

## Review Module
Responsibilities:
- Review submission
- Rating calculation
- Staff review responses

---

## Administration Module
Responsibilities:
- Product management
- Inventory management
- User management
- Configuration management
- Audit logs
- Reports

---

# 5. Security Requirements

The system enforces:
- JWT authentication
- Role-Based Access Control (RBAC)
- Password hashing using bcrypt
- Input validation
- XSS prevention
- SQL Injection prevention
- Secure API communication

Sensitive endpoints require:
- Valid JWT token
- Proper role authorization

---

# 6. Performance Requirements

- All major operations must complete within 5 seconds.
- System must remain stable under concurrent requests.
- External API delays must not crash the application.
- Logging and background operations should not block request processing.

---

# 7. Coding Philosophy

## Thin Controllers
Controllers must not contain business logic.

Controllers should only:
- Receive requests
- Validate request format
- Call services
- Return responses

---

## Service-Based Business Logic
All business logic must be implemented inside services.

---

## Repository Pattern
Database access must be isolated through repositories.

---

## DTO-Based Communication
Entities must never be exposed directly through APIs.

DTOs are required for:
- Requests
- Responses

---

## Centralized Exception Handling
Unhandled exceptions must be processed through middleware.

Sensitive stack traces must never be exposed to clients.

---

## Validation First
All requests must be validated before database operations.

---

# 8. API Standards

## Response Format

Successful response:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": {}
}


{
  "success": false,
  "message": "Validation failed",
  "data": {
    "field1": ["Error message 1", "Error message 2"],
    "field2": ["Error message 1"]
  }
}