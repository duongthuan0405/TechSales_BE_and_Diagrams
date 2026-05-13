---
name: service-convention-skill
description: Kỹ năng này định nghĩa trách nhiệm và quy tắc hoạt động của Business Services trong hệ thống TechSales Management Backend, bao gồm cách tương tác với Repository, Helper Services, Unit Of Work, Controllers và Validation.
---

# Service Convention

# 1. Overview

Business Services are the primary business logic layer of the system.

All business workflows and business rules must be implemented inside Business Services.

Controllers must never contain business logic.

Repositories must never contain business workflows.

Business Services coordinate:
- Repositories
- Helper Services
- External Services
- Unit Of Work
- Validation logic

---

# 2. Service Responsibilities

Business Services are responsible for:
- Executing business workflows
- Validating business rules
- Coordinating repositories
- Coordinating helper services
- Managing transactions
- Handling application use cases
- Orchestrating external services

Examples:
- User registration
- Login workflow
- Order placement
- Payment processing
- Voucher validation

---

# 3. Service Architecture

Business Services must be accessed through interfaces.

Example:

```csharp
public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
```

Implementation:

```csharp
public class AuthService : IAuthService
{
}
```

Controllers must depend on interfaces only.

---

# 4. Dependency Rules

Business Services may depend on:
- Repository interfaces
- Helper service interfaces
- External service interfaces
- IUnitOfWork
- Validators

Business Services must NOT depend on:
- Controllers
- HTTP Context directly
- WebAPI layer
- Concrete infrastructure implementations

---

# 5. Allowed Dependencies

```text
Business Service
    ├── Repository Interfaces
    ├── Helper Service Interfaces
    ├── External Service Interfaces
    ├── Validators
    └── IUnitOfWork
```

---

# 6. Forbidden Dependencies

Business Services must NOT:
- Access DbContext directly
- Return Entity objects directly
- Handle HTTP requests directly
- Generate HTTP responses
- Access Request/Response objects directly

---

# 7. Thin Controller Principle

Controllers must remain thin.

Controllers should only:
1. Receive request
2. Validate request format
3. Call Business Service
4. Return response

Controllers must NOT:
- Implement business logic
- Query database directly
- Generate JWT tokens directly
- Send emails directly
- Access repositories directly

---

# 8. Validation Architecture

The system separates validation into:
- Request Format Validation
- Business Validation

Validation responsibilities must follow clean architecture boundaries.

---

# 9. Controller Validation Rules

Controllers (or FluentValidation) are responsible ONLY for request format validation.

This includes:
- Required fields
- Data types
- Email format
- String length
- Regex validation
- Primitive input validation

Examples:

```text
✔ Required fields
✔ Email format
✔ Password minimum length
✔ Numeric range validation
✔ Request body format validation
```

---

## Example

```csharp
public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(8);
    }
}
```

---

# 10. Business Validation Rules

Business validation MUST occur inside Business Services.

Business validation includes:
- Existence checks
- Duplicate checks
- Business state validation
- Workflow validation
- Permission validation

Examples:

```text
✔ Email already exists
✔ Product out of stock
✔ Voucher expired
✔ User account blocked
✔ Order already completed
✔ Invalid order state transition
```

---

## Example

```csharp
var existingUser =
    await _userRepository.GetByEmailAsync(request.Email);

if(existingUser != null)
{
    throw new ConflictException("Email already exists");
}
```

---

# 11. Validation Responsibility Rules

## Controllers MUST NOT

```csharp
await _userRepository.ExistsAsync(email);
```

---

## Controllers MUST NOT

```csharp
if(product.StockQuantity < request.Quantity)
```

---

## Controllers MUST NOT

```csharp
await _emailService.SendOtpAsync();
```

These are business workflows and must be handled inside Business Services.

---

# 12. FluentValidation Convention

FluentValidation is used for request DTO validation.

Validation classes should be placed inside:

```text
Application/Validations
```

Examples:

```text
RegisterRequestValidator
CreateOrderValidator
UpdateProfileValidator
```

---

# 13. Repository Responsibility

Repositories are responsible ONLY for:
- Querying data
- Inserting data
- Updating data
- Deleting data

Repositories must NOT:
- Implement business workflows
- Handle validation logic
- Send emails
- Generate JWT tokens
- Manage transactions

---

# 14. Helper Service Responsibility

Helper Services provide reusable technical functionalities.

Examples:
- JWT generation
- Password hashing
- Email sending
- OTP generation
- Cache handling

Helper Services must NOT:
- Contain business workflows
- Access Controllers
- Handle HTTP logic

---

# 15. Transaction Management

Business Services are responsible for transaction management.

Every multi-step workflow must use IUnitOfWork.

Example:

```csharp
try
{
    await _unitOfWork.BeginAsync();

    // Business logic

    await _unitOfWork.FinishAsync();
}
catch
{
    await _unitOfWork.RollbackAsync();
    throw;
}
```

---

# 16. Service Workflow Example

```text
Controller
    ↓
Request Validation
    ↓
IAuthService
    ↓
AuthService
    ├── Business Validation
    ├── IUserRepository
    ├── IPasswordHasher
    ├── IJwtService
    ├── IEmailService
    └── IUnitOfWork
```

---

# 17. Return Value Rules

Business Services should:
- Receive Request DTOs
- Return Domain Entities or Business Models

Business Services must NOT:
- Return Response DTOs (Mapping to Response DTOs is a Controller responsibility)
- Wrap results in ApiResponse (Presentation concern)

---

# 18. Internal Layer Model Convention

Business Services must use Entity models for core business logic and repository communication.

Services receive Request DTOs from Controllers, but they must convert them to Entities or use their properties to interact with Repositories.

Services must return Entities (or Domain Models) back to the Controller.

The Mapping from Entity to Response DTO must occur ONLY in the Controller.

---

# 19. Error Handling Rules

Business Services should:
- Throw custom exceptions
- Never swallow exceptions silently

Examples:
- BadRequestException
- NotFoundException
- UnauthorizedException
- ConflictException

---

# 19. Async Rules

All database and external operations must use async/await.

Examples:

```csharp
await _userRepository.GetByIdAsync(id);

await _emailService.SendAsync();
```

Blocking calls are forbidden.

---

# 20. Naming Convention

Service interfaces:

```text
IAuthService
IProductService
IOrderService
```

Service implementations:

```text
AuthService
ProductService
OrderService
```

Async methods must end with:

```text
Async
```

Examples:

```text
LoginAsync
RegisterAsync
CreateOrderAsync
```

---

# 21. Folder Structure

```text
Application/
│
├── Services/
│   ├── Interfaces/
│   │   ├── IAuthService.cs
│   │   ├── IProductService.cs
│   │   └── IOrderService.cs
│   │
│   └── Implementations/
│       ├── AuthService.cs
│       ├── ProductService.cs
│       └── OrderService.cs
│
└── Validations/
    ├── RegisterRequestValidator.cs
    ├── CreateOrderValidator.cs
    └── UpdateProfileValidator.cs
```

---

# 22. Development Philosophy

Business Services are the heart of the application.

The architecture centralizes business logic inside services to:
- Reduce coupling
- Improve maintainability
- Simplify testing
- Improve readability
- Enforce clean architecture boundaries
- Prevent logic duplication
- Improve long-term scalability

Validation is separated into:
- Format validation
- Business validation

This separation improves:
- Maintainability
- Reusability
- Testability
- Clean architecture consistency