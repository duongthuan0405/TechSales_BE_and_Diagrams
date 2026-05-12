---
name: controller-convention-skill
description: Kỹ năng này định nghĩa trách nhiệm, quy tắc thiết kế và cách hoạt động của Controllers trong hệ thống TechSales Management Backend, bao gồm API conventions, validation boundaries, async conventions, response handling và global exception handling.
---

# Controller Convention

# 1. Overview

Controllers are the entry point of the application.

Controllers are responsible for:
- Receiving HTTP requests
- Validating request format
- Calling Business Services
- Returning standardized API responses

Controllers must remain thin.

Controllers must NOT contain business logic.

---

# 2. Thin Controller Principle

Controllers should only:
1. Receive requests
2. Validate request format
3. Call Business Services
4. Map Domain Entities/Results to Response DTOs
5. Return standardized responses

Controllers must NEVER:
- Implement business workflows
- Query databases directly
- Access DbContext directly
- Access repositories directly
- Manage transactions
- Send emails
- Generate JWT tokens
- Hash passwords
- Validate business rules

---

# 3. Dependency Rules

Controllers may depend ONLY on:
- Service interfaces
- DTOs
- ASP.NET Core abstractions

Controllers must NOT depend on:
- Repository implementations
- DbContext
- Infrastructure services directly

---

# 4. Dependency Flow

```text
Client
    ↓
Controller
    ↓
Business Service Interface
    ↓
Business Service
```

---

# 5. Constructor Injection Convention

Controllers must use Dependency Injection (DI).

Dependencies must be injected through constructor injection.

Example:

```csharp
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
}
```

---

# 6. Async-First Architecture

All controller endpoints must be asynchronous.

Controllers follow an Async-First architecture.

All endpoints must return:

```csharp
Task<IActionResult>
```

---

# 7. Async Endpoint Convention

Correct:

```csharp
[HttpPost("login")]
public async Task<IActionResult> LoginAsync(
    [FromBody] LoginRequestDto request)
{
    var response = await _authService.LoginAsync(request);

    return Ok(response);
}
```

Forbidden:

```csharp
public IActionResult Login(LoginRequestDto request)
```

---

# 8. Async Rules

## Rule 1

All controller actions must be async.

---

## Rule 2

All service calls must use await.

Correct:

```csharp
await _authService.LoginAsync(request);
```

Forbidden:

```csharp
_authService.LoginAsync(request).Result;
```

---

## Rule 3

Blocking async calls are forbidden.

Forbidden:

```csharp
.Result
.Wait()
.GetAwaiter().GetResult()
```

These may cause:
- Deadlocks
- Thread blocking
- Performance degradation

---

# 9. Request Validation Rules

Controllers are responsible ONLY for request format validation.

Controllers may validate:
- Required fields
- Request body format
- Primitive data validation
- ModelState validation

Controllers must NOT validate:
- Business rules
- Workflow rules
- Database state

---

# 10. Validation Boundary

## Controller Validation

Examples:

```text
✔ Required fields
✔ Email format
✔ Password length
✔ Request DTO structure
```

---

## Business Service Validation

Examples:

```text
✔ Email already exists
✔ Product out of stock
✔ Voucher expired
✔ User blocked
✔ Order already completed
```

Business validation must occur inside Business Services.

---

# 11. DTO Mapping Rules

Controllers must:
- Receive Request DTOs
- Call Services to get Domain Entities
- Map Domain Entities to Response DTOs
- Wrap Response DTOs in ApiResponse

Controllers must NEVER:
- Expose Entity objects directly to the Client
- Return database entities directly in the Response Body

---

# 12. Request DTO Convention

Correct:

```csharp
public async Task<IActionResult> RegisterAsync(
    RegisterRequestDto request)
```

Forbidden:

```csharp
public async Task<IActionResult> RegisterAsync(
    User entity)
```

---

# 13. Response Convention

All APIs should return standardized responses.

Success example:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": {}
}
```

Failure example:

```json
{
  "success": false,
  "message": "Validation failed",
  "data": {
    "email": [
      "Email already exists"
    ]
  }
}
```

---

# 14. HTTP Status Code Convention

Controllers must use correct HTTP status codes.

---

## 200 OK

Used for:
- Successful GET
- Successful PUT
- Successful PATCH
- Successful DELETE with response body

---

## 201 Created

Used for:
- Successful resource creation

Example:

```csharp
return Created();
```

---

## 204 No Content

Used for:
- Successful operations without response body

---

## 400 Bad Request

Used for:
- Invalid request format
- Validation failure

---

## 401 Unauthorized

Used for:
- Missing authentication
- Invalid JWT token

---

## 403 Forbidden

Used for:
- Permission denied

---

## 404 Not Found

Used for:
- Resource not found

---

## 409 Conflict

Used for:
- Duplicate resource
- Business conflicts

---

## 500 Internal Server Error

Used for:
- Unhandled server errors

Handled through middleware.

---

# 15. Exception Handling Convention

Controllers must NOT use try-catch for business exceptions.

Business Services are responsible for throwing exceptions.

Global Exception Middleware is responsible for:
- Catching exceptions
- Mapping HTTP status codes
- Returning standardized error responses

---

# 16. Exception Flow

Correct flow:

```text
Controller
    ↓
Business Service
    ↓
Exception Thrown
    ↓
Global Exception Middleware
    ↓
HTTP Response
```

---

# 17. Correct Controller Pattern

Controllers should remain simple and clean.

Correct:

```csharp
[HttpPost("login")]
public async Task<IActionResult> LoginAsync(
    LoginRequestDto request)
{
    var result = await _authService.LoginAsync(request);

    return Ok(result);
}
```

---

# 18. Forbidden Controller Pattern

Controllers must NOT wrap every endpoint in try-catch blocks.

Forbidden:

```csharp
[HttpPost]
public async Task<IActionResult> LoginAsync(
    LoginRequestDto request)
{
    try
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }
    catch(Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
```

This causes:
- Duplicated code
- Fat controllers
- Poor maintainability
- Mixed responsibilities

---

# 19. Transaction Rules

Controllers must NEVER:
- Begin transactions
- Commit transactions
- Rollback transactions

Transaction management belongs ONLY to:
- Business Services
- Unit Of Work

---

# 20. Repository Access Rules

Controllers must NEVER:
- Access repositories directly
- Access DbContext directly

Forbidden:

```csharp
await _userRepository.GetByIdAsync(id);
```

Forbidden:

```csharp
await _dbContext.Users.ToListAsync();
```

Controllers must always communicate through Business Services.

---

# 21. Authorization Convention

Protected endpoints must use authorization attributes.

Examples:

```csharp
[Authorize]
```

```csharp
[Authorize(Roles = "Admin")]
```

---

# 22. Routing Convention

Controllers must use RESTful routing conventions.

Examples:

```text
api/auth
api/products
api/orders
api/users
api/vouchers
```

---

# 23. HTTP Method Convention

## GET

Used for:
- Retrieving resources

---

## POST

Used for:
- Creating resources
- Executing actions

---

## PUT

Used for:
- Full resource updates

---

## PATCH

Used for:
- Partial resource updates

---

## DELETE

Used for:
- Resource deletion

---

# 24. Controller Naming Convention

Controllers must end with:

```text
Controller
```

Examples:

```text
AuthController
ProductController
OrderController
```

---

# 25. Route Naming Convention

Action names should remain clean and RESTful.

Correct:

```text
POST   /api/auth/login
POST   /api/auth/register
GET    /api/products
GET    /api/products/{id}
POST   /api/orders
DELETE /api/cart/items/{id}
```

Forbidden:

```text
/api/doLogin
/api/createProductNow
/api/deleteCartItemById
```

---

# 26. API Versioning Convention

If API versioning is used:

```text
/api/v1/products
/api/v1/orders
```

---

# 27. Controller Folder Structure

```text
Presentation_WebAPI/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── ProductController.cs
│   ├── OrderController.cs
│   └── CartController.cs
```

---

# 28. Example Controller Workflow

```text
Client
    ↓
Controller
    ↓
Request Validation
    ↓
Business Service
    ↓
Business Validation
    ↓
Repositories
    ↓
Database
```

---

# 29. Forbidden Architecture Violations

Controllers must NEVER:
- Access DbContext
- Access repositories
- Implement business logic
- Send emails
- Generate tokens
- Handle transaction logic
- Perform business validation
- Return Entity objects
- Call external services directly

---

# 30. Development Philosophy

Controllers exist to handle HTTP communication only.

The architecture keeps controllers thin to:
- Reduce coupling
- Improve maintainability
- Improve readability
- Simplify testing
- Enforce clean architecture boundaries
- Centralize business logic inside services

Global Exception Middleware centralizes error handling to:
- Remove duplicated try-catch blocks
- Standardize API error responses
- Improve maintainability
- Keep controllers clean

Controllers should remain:
- Lightweight
- Predictable
- HTTP-focused
- Fully asynchronous