---
name: project-structured-skill
description: Kỹ năng này định nghĩa cấu trúc thư mục chi tiết theo kiến trúc Clean Architecture cho dự án TechSales Management Backend, quy định cách phân chia các lớp và quy tắc phụ thuộc trong dự án.
---

# TechSales Management Backend - Project Structure

## 1. Overview

TechSales Management Backend is designed using:
- Modular Monolithic Architecture
- Clean Architecture
- Layered Architecture

The system is divided into four main layers:
- Domain
- Application
- Infrastructure
- Presentation (WebAPI)

Business logic is centralized inside the Application layer.

The architecture follows:
- Dependency Injection (DI)
- Dependency Inversion Principle (DIP)
- Separation of Concerns (SoC)

This structure improves:
- Maintainability
- Scalability
- Testability
- Team collaboration
- AI-assisted development consistency

---

# 2. Project Structure

```text
TechSalesManagement/
│
├───Application
│   ├───Common
│   ├───Exceptions
│   ├───ExternalServices
│   ├───Features
│   ├───HelperServices
│   ├───Services
│   │   ├───Implementations
│   │   └───Interfaces
│   └───Validations
│
├───Domain
│   ├───Common
│   ├───Entities
│   ├───Enums
│   ├───Exceptions
│   ├───Interfaces
│   └───ValueObjects
│
├───Infrastructure
│   ├───ExternalServices
│   ├───HelperServices
│   ├───Persistence
│   └───Repositories
│
├───Presentation_WebAPI
│   ├───Controllers
│   ├───DTOs
│   │   ├───RequestDTOs
│   │   └───ResponseDTOs
│   └───Middlewares
│
└───Properties
```

---

# 3. Clean Architecture Dependency Direction

The system follows Clean Architecture dependency rules.

```text
Presentation_WebAPI
            ↓
       Application
            ↓
          Domain
            ↑
      Infrastructure
```

Dependency direction always points inward.

Rules:
- Outer layers can depend on inner layers
- Inner layers must never depend on outer layers

---

# 4. Domain Layer

## Purpose

The Domain layer contains the core business domain of the system.

It defines:
- Business entities
- Business rules
- Domain models
- Enums
- Value objects
- Domain contracts

The Domain layer must remain independent from:
- Database technologies
- Frameworks
- HTTP logic
- Infrastructure logic

---

## Structure

```text
Domain/
│
├───Common
├───Entities
├───Enums
├───Exceptions
├───Interfaces
└───ValueObjects
```

---

## Responsibilities

### Entities/
Contains core business entities.

Examples:
- User
- Product
- Order
- Voucher

---

### Enums/
Contains business enums.

Examples:
- UserRole
- OrderStatus
- PaymentStatus

---

### ValueObjects/
Contains immutable business objects.

Examples:
- Address
- Money

---

### Exceptions/
Contains domain-specific exceptions.

---

### Interfaces/
Contains domain contracts and abstractions.

---

### Common/
Contains shared domain components.

---

## Important Rules

The Domain layer:
- Must not depend on Infrastructure
- Must not depend on Presentation_WebAPI
- Must not contain DTOs
- Must not access database directly
- Must not contain framework-specific code

---

# 5. Application Layer

## Purpose

The Application layer is the main business logic layer of the system.

Business logic is centralized inside this layer.

The Application layer handles:
- Business workflows
- Use cases
- Business services
- Validation logic
- Service interfaces
- Repository interfaces

Controllers communicate with services through interfaces using Dependency Injection (DI).

---

## Structure

```text
Application/
│
├───Common
├───Exceptions
├───ExternalServices
├───Features
├───HelperServices
├───Services
│   ├───Implementations
│   └───Interfaces
└───Validations
```

---

# 6. Features Folder

## Purpose

Contains feature-based business modules.

Each feature represents an independent business boundary.

Example features:
- Identity
- Product
- Order
- Inventory
- Cart
- Payment
- Voucher

---

# 7. Services Folder

## Purpose

Contains business services and service interfaces.

Business Services contain the primary business logic of the system.

---

## Structure

```text
Services/
│
├───Implementations
└───Interfaces
```

---

## Interfaces/

Contains service contracts.

Examples:
- IAuthService
- IProductService
- IOrderService

Controllers communicate with services through these interfaces.

---

## Implementations/

Contains service implementations.

Examples:
- AuthService
- ProductService
- OrderService

Business Services are responsible for:
- Executing business workflows
- Coordinating repositories
- Coordinating helper services
- Validating business rules
- Managing transactions

---

# 8. HelperServices Folder

## Purpose

Contains helper service interfaces.

Helper Services provide reusable technical functionalities.

Helper Services do not contain business workflows.

Examples:
- JWT generation
- Password hashing
- Email sending
- OTP generation
- Cache handling

---

## Example Interfaces

```text
HelperServices/
│
├───IJwtService.cs
├───IPasswordHasher.cs
├───IEmailService.cs
├───IOtpService.cs
└───ICacheService.cs
```

---

# 9. ExternalServices Folder

## Purpose

Contains abstractions/interfaces for external third-party services.

Examples:
- Payment gateway services
- Shipping services
- SMS services

---

# 10. Validations Folder

## Purpose

Contains validation logic.

Examples:
- RegisterValidator
- CreateOrderValidator

---

# 11. Infrastructure Layer

## Purpose

The Infrastructure layer contains technical implementations.

This layer implements:
- Repository interfaces
- Helper service interfaces
- External service integrations
- Database access logic

Infrastructure handles all technical concerns outside the business domain.

---

## Structure

```text
Infrastructure/
│
├───ExternalServices
├───HelperServices
├───Persistence
└───Repositories
```

---

# 12. Persistence Folder

## Purpose

Contains database configuration and database context.

Examples:
- ApplicationDbContext
- Entity configurations
- Database migrations

---

# 13. Repositories Folder

## Purpose

Contains repository implementations.

Repositories handle:
- Querying data
- Inserting data
- Updating data
- Deleting data

Repositories must not contain business workflows.

Examples:
- UserRepository
- ProductRepository
- OrderRepository

---

# 14. HelperServices Folder

## Purpose

Contains helper service implementations.

These services implement interfaces defined in Application layer.

Examples:
- JwtService
- PasswordHasher
- EmailService
- OtpService

---

## Responsibilities

### JwtService
Responsible for:
- Access token generation
- Refresh token generation
- Token validation

---

### PasswordHasher
Responsible for:
- Password hashing
- Password verification

---

### EmailService
Responsible for:
- Sending emails
- Sending OTP emails
- Sending notification emails

---

### OtpService
Responsible for:
- OTP generation
- OTP validation
- OTP expiration handling

---

# 15. ExternalServices Folder

## Purpose

Contains implementations for external third-party integrations.

Examples:
- VNPay integration
- Shipping provider integration
- SMS provider integration

---

# 16. Presentation_WebAPI Layer

## Purpose

The Presentation_WebAPI layer is the entry point of the application.

It is responsible for:
- HTTP communication
- Controllers
- Middleware
- Request/response handling
- API configuration

---

## Structure

```text
Presentation_WebAPI/
│
├───Controllers
├───DTOs
│   ├───RequestDTOs
│   └───ResponseDTOs
└───Middlewares
```

---

# 17. Controllers Folder

## Purpose

Contains API controllers.

Controllers are responsible only for:
- Receiving HTTP requests
- Validating request format
- Calling services
- Returning API responses

Controllers must remain thin.

Controllers must not:
- Contain business logic
- Access repositories directly
- Access DbContext directly

---

# 18. DTOs Folder

## Purpose

Contains request and response DTOs.

---

## RequestDTOs/

Contains request models.

Examples:
- LoginRequestDto
- RegisterRequestDto

---

## ResponseDTOs/

Contains response models.

Examples:
- LoginResponseDto
- ProductResponseDto

---

# 19. Middlewares Folder

## Purpose

Contains global middleware components.

Examples:
- ExceptionHandlingMiddleware
- JwtMiddleware
- RequestLoggingMiddleware

---

# 20. Dependency Injection Architecture

The project uses Dependency Injection (DI).

Controllers depend on abstractions/interfaces instead of concrete implementations.

Example:

```text
AuthController
    ↓
IAuthService
    ↓
AuthService
```

Business Services also depend on abstractions/interfaces.

Example:

```text
AuthService
    ├── IUserRepository
    ├── IJwtService
    ├── IPasswordHasher
    ├── IEmailService
    └── IOtpService
```

---

# 21. Request Processing Flow

## Full Request Flow

```text
Client
    ↓
Controller
    ↓
Service Interface
    ↓
Business Service
    ↓
Repositories / Helper Services
    ↓
Database / External Services
```

---

# 22. Architecture Rules

## Rule 1
Business logic must exist inside Business Services.

---

## Rule 2
Controllers must remain thin.

---

## Rule 3
Repositories handle database operations only.

---

## Rule 4
Helper Services contain reusable technical logic only.

---

## Rule 5
Controllers must never access repositories directly.

---

## Rule 6
Controllers must never access DbContext directly.

---

## Rule 7
Interfaces must be defined inside Application layer.

---

## Rule 8
Infrastructure must implement interfaces defined in Application layer.

---

# 23. Development Philosophy

The architecture is designed to:
- Centralize business logic
- Reduce coupling
- Improve maintainability
- Improve scalability
- Simplify testing
- Enforce clean architecture boundaries
- Support long-term project growth
- Support AI-assisted development workflows