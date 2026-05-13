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
Each method in Services, must use class xxxParams instead of RequestDTO
Example:

```csharp
public interface IAuthService
{
    Task<User> LoginAsync(LoginParams parameters);
}
```

Implementation:

```csharp
public class AuthService : IAuthService
{
    public async Task<User> LoginAsync(LoginParams parameters)
    {
        // Implementation
    }
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
- Specific parameter classes (xxxParams)

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
    ├── Parameter Classes (xxxParams)
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

The system centralizes ALL validation inside Business Services.

There is no separate layer boundary for validation. The Business Service is responsible for both:
- Request Parameter Format Validation
- Business Rule Validation

Validation of the incoming `xxxParams` must happen explicitly within the service method logic or through validators invoked inside the service.

---

# 9. Validation-Free DTOs

Controllers and DTOs must NEVER contain validation logic or attributes.

DTO classes in the Presentation layer must be "naked" - containing only properties with no annotations (e.g., no `[Required]`, no `[EmailAddress]`, no `[Compare]`, etc.).

Controllers must simply receive the DTO, map it straight to the corresponding `xxxParams` object, and forward it to the service. 

Controllers must NOT perform format validation.

---

# 10. Full Service-Side Validation

Business Services are the absolute gatekeepers of the application. They must validate EVERYTHING.

Validations within Business Services include:
- Required field checks (e.g., Null or Empty)
- Formatting checks (e.g., Email format, Phone number regex)
- Structural checks (e.g., Passwords matching)
- Existence checks
- Duplicate data checks
- Complex business rules

All format and value errors found must result in a `BadRequestException` (which can carry a structured Dictionary of validation errors).

## Example

```csharp
if (string.IsNullOrWhiteSpace(parameters.Email))
{
    throw new BadRequestException(MessageConstants.MSG1);
}

if (parameters.Password != parameters.ConfirmPassword)
{
    throw new BadRequestException(MessageConstants.MSG4);
}

var existingUser = await _userRepository.GetByEmailAsync(parameters.Email);
if (existingUser != null)
{
    throw new ConflictException(MessageConstants.MSG5);
}
```

---

# 11. Validation Responsibility Rules

Controllers MUST NOT contain validation logic or rely on ASP.NET built-in Model Validation via DataAnnotations.

Validators must live in the Application layer and validate the internal parameters classes (`xxxParams`), NOT Request DTOs.

---

# 12. FluentValidation & Custom Validation Convention

If FluentValidation is used, validators MUST target `xxxParams` objects instead of Request DTOs.

Validation classes should live in:
```text
Application/Validations
```

They must be injected into or instantiated inside Business Services, and executed directly at the start of the service method.

---

# 12.1. Format Validation Utility Convention (ValidationUtils)

Common format validation technical rules (like Email regex, Phone number formats, Password minimum complexity) must be encapsulated inside a reusable static class:
```text
Application/Common/Utils/ValidationUtils.cs
```

### Rules for ValidationUtils:
1. **NO Exception Throwing**: Utility methods inside `ValidationUtils` must NEVER throw Exceptions. They must only return a boolean (`true`/`false`) indicating structural validity.
2. **Decoupled From Business Rules**: Utility only focuses on technical patterns (Regex/Length). It does NOT decide if a field is "Required" for a particular use case.
3. **Exception Handled Externally**: The calling Business Service method is responsible for taking action if the utility returns `false` and throwing the appropriate `BadRequestException` using the specific message key from `MessageConstants`.

### Proper Pattern Example in Service:
```csharp
// 1. Check Required (Service decides if field is required)
if (string.IsNullOrWhiteSpace(parameters.Email))
{
    throw new BadRequestException(MessageConstants.MSG1); // Missing field
}

// 2. Check Format (Utility validates structure, Service throws exception)
if (!ValidationUtils.IsValidEmail(parameters.Email))
{
    throw new BadRequestException(MessageConstants.MSG2); // Format issue
}
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
- Receive a specific `xxxParams` parameters class
- Return Domain Entities or Business Models

Business Services must NOT:
- Return Response DTOs (Mapping to Response DTOs is a Controller responsibility)
- Wrap results in ApiResponse (Presentation concern)

---

# 18. Internal Layer Model Convention

Business Services must use specialized Parameter objects for their inputs and Entity models for core business logic/repository communication.

Services must NOT accept Request DTOs from Controllers. The Controller is responsible for mapping the Request DTO into the corresponding `xxxParams` class.

Services must NOT accept primitive types as single arguments, nor should they accept naked Domain Entities as direct arguments for modification operations.

Services must return Entities (or Domain Models) back to the Controller.

The Mapping from Entity to Response DTO must occur ONLY in the Controller.

---

# 18.1. Specific Parameter Object Pattern (xxxParams)

Every service method MUST receive exactly ONE parameter object, named specifically after the method's function: `[MethodName]Params`.

### Reason for this pattern:
1. **Encapsulation**: Adding or removing fields in the input only changes the specific `Params` class.
2. **Modularity**: Changing parameters of one method (e.g., `LoginAsync`) does NOT affect any other method's structure.
3. **Decoupling**: Keeps the Application Layer independent from the Presentation Layer (DTOs) and database entities.

### Example

Correct:
```csharp
Task<User> RegisterAsync(RegisterParams parameters);
Task<User> LoginAsync(LoginParams parameters);
Task VerifyEmailAsync(VerifyEmailParams parameters);
```

Forbidden (Single primitive parameters):
```csharp
Task<User> LoginAsync(string email, string password);
```

Forbidden (Exposing Domain Entities as input):
```csharp
Task<User> RegisterAsync(User newUser);
```

Forbidden (Exposing DTOs from Presentation):
```csharp
Task<User> RegisterAsync(RegisterRequestDto request);
```

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
│   ├── Implementations/
│   │   ├── AuthService.cs
│   │   ├── ProductService.cs
│   │   └── OrderService.cs
│   │
│   └── Params/
│       ├── AuthParams.cs
│       ├── ProductParams.cs
│       └── OrderParams.cs
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