---
name: exception-handling-skill
description: Kỹ năng này định nghĩa cơ chế xử lý exception tập trung thông qua Global Exception Middleware, quy tắc sử dụng custom exceptions, mapping HTTP status codes và chuẩn hóa API error responses trong hệ thống TechSales Management Backend.
---

# Exception Handling Convention

# 1. Overview

The system uses centralized exception handling through Global Exception Middleware.

Controllers must NOT handle business exceptions manually.

Business Services are responsible for throwing exceptions.

Global Exception Middleware is responsible for:
- Catching exceptions
- Mapping HTTP status codes
- Returning standardized error responses
- Preventing internal system information leakage

---

# 2. Exception Flow

The system follows this exception flow:

```text
Controller
    ↓
Business Service
    ↓
Exception Thrown
    ↓
Global Exception Middleware
    ↓
Standardized HTTP Response
```

---

# 3. Responsibilities

## Business Services

Business Services are responsible for:
- Throwing custom exceptions
- Validating business rules
- Detecting business failures

---

## Controllers

Controllers must:
- NOT use try-catch for business exceptions
- Simply call services and return responses

---

## Global Exception Middleware

Middleware is responsible for:
- Catching all exceptions
- Mapping status codes
- Creating standardized error responses
- Logging exceptions if necessary

---

# 3.1. Standard Exception Types

Business Services must use the following standard exceptions:

- **NotFoundException**: Use when a resource (User, Product, Order) does not exist.
  - *Result: 404 Not Found*
- **ConflictException**: Use for business rule violations or data conflicts (Email already exists, Invalid state transition).
  - *Result: 409 Conflict*
- **BadRequestException**: Use for invalid business requests or logic failures.
  - *Result: 400 Bad Request*
- **UnauthorizedException**: Use for authentication failures.
  - *Result: 401 Unauthorized*
- **ForbiddenException**: Use for authorization/permission failures.
  - *Result: 403 Forbidden*
- **AndMore...**

---

# 4. Global Exception Middleware Location

Middleware should be placed inside:

```text
Presentation_WebAPI/Middlewares
```

Example:

```text
Presentation_WebAPI/
│
└── Middlewares/
    └── GlobalExceptionMiddleware.cs
```

---

# 5. Custom Exception Convention

The system should use custom exceptions for predictable business failures.

---

# 6. Custom Exception Examples

```text
BadRequestException
UnauthorizedException
ForbiddenException
NotFoundException
ConflictException
ValidationException
```

---

# 7. Exception Folder Structure

Custom exceptions should be placed inside:

```text
Application/Exceptions
```

Example:

```text
Application/
│
└── Exceptions/
    ├── BadRequestException.cs
    ├── NotFoundException.cs
    ├── ConflictException.cs
    ├── UnauthorizedException.cs
    └── ValidationException.cs
```

---

# 8. Exception Usage Example

Correct:

```csharp
if(existingUser != null)
{
    throw new ConflictException("Email already exists");
}
```

Correct:

```csharp
if(product.StockQuantity < request.Quantity)
{
    throw new BadRequestException("Product is out of stock");
}
```

---

# 9. Forbidden Exception Usage

Forbidden:

```csharp
throw new Exception("Email already exists");
```

Business failures should use custom exceptions instead of generic Exception.

---

# 10. HTTP Status Code Mapping

The middleware must map exceptions to HTTP status codes consistently.

---

## 400 Bad Request

Mapped from:

```text
BadRequestException
ValidationException
```

---

## 401 Unauthorized

Mapped from:

```text
UnauthorizedException
```

---

## 403 Forbidden

Mapped from:

```text
ForbiddenException
```

---

## 404 Not Found

Mapped from:

```text
NotFoundException
```

---

## 409 Conflict

Mapped from:

```text
ConflictException
```

---

## 500 Internal Server Error

Mapped from:
- Unhandled exceptions
- Unknown exceptions

---

# 11. Standardized Error Response

All exceptions must return standardized API responses.

---

## Standard Error Response

```json
{
  "success": false,
  "message": "Email already exists",
  "data": null
}
```

---

## Validation Error Response

```json
{
  "success": false,
  "message": "Validation failed",
  "data": {
    "email": [
      "Email is required"
    ]
  }
}
```

---

# 12. Security Rules

The system must NEVER expose:
- Stack traces
- Database connection details
- SQL queries
- Internal file paths
- Internal server information

Forbidden:

```json
{
  "stackTrace": "...",
  "source": "...",
  "innerException": "..."
}
```

---

# 13. Controller Exception Rules

Controllers must NOT wrap every endpoint with try-catch blocks.

Correct:

```csharp
[HttpPost]
public async Task<IActionResult> RegisterAsync(
    RegisterRequestDto request)
{
    var result = await _authService.RegisterAsync(request);

    return Ok(result);
}
```

---

## Forbidden Pattern

```csharp
try
{
}
catch(Exception ex)
{
    return BadRequest(ex.Message);
}
```

This causes:
- Duplicate logic
- Fat controllers
- Inconsistent error responses

---

# 14. Middleware Responsibility Example

The middleware should:
1. Catch exception
2. Determine status code
3. Build standardized response
4. Return JSON response

---

# 15. Middleware Example Structure

```csharp
try
{
    await _next(context);
}
catch (Exception ex)
{
    // Map exception
    // Build response
    // Return standardized JSON
}
```

---

# 16. Validation Exception Convention

Validation failures should use ValidationException.

ValidationException may contain:
- Field names
- Error messages
- Validation details

Example:

```csharp
throw new ValidationException(errors);
```

---

# 17. Logging Rules

Unhandled exceptions should be logged.

Expected business exceptions may optionally avoid error-level logging.

Examples:
- Validation failures
- Duplicate email
- Unauthorized access

These are expected application behaviors.

---

# 18. Async Exception Rules

Exception handling must support async/await correctly.

Forbidden:

```csharp
.Result
.Wait()
.GetAwaiter().GetResult()
```

These may:
- Break async exception flow
- Cause deadlocks

---

# 19. Exception Naming Convention

Custom exceptions must end with:

```text
Exception
```

Examples:

```text
BadRequestException
NotFoundException
ConflictException
ValidationException
```

---

# 20. Architecture Rules

## Rule 1

Business Services throw exceptions.

---

## Rule 2

Controllers do not handle business exceptions manually.

---

## Rule 3

Middleware centralizes exception handling.

---

## Rule 4

All API errors must follow standardized response format.

---

## Rule 5

Internal server details must never be exposed to clients.

---

# 21. Example Full Exception Flow

```text
Client Request
    ↓
Controller
    ↓
Business Service
    ↓
ConflictException Thrown
    ↓
Global Exception Middleware
    ↓
409 Conflict Response
```

---

# 22. Forbidden Architecture Violations

The system must NEVER:
- Catch business exceptions inside controllers
- Return raw exception objects
- Expose stack traces
- Throw generic Exception for business failures
- Mix HTTP logic inside services

---

# 23. Development Philosophy

Centralized exception handling improves:
- Maintainability
- Consistency
- Security
- Readability
- Scalability

This architecture ensures:
- Thin controllers
- Predictable API behavior
- Standardized error responses
- Clean separation of concerns
- Safer production systems

---

# 24. Centralized Constants Organization for System Messages

To ensure maintainability, consistent reuse across all layers, and future localization support, all user-facing strings, business rules codes (MSG), domain errors, and system responses MUST be gathered into a single centralized folder named `Common` at the root of the project.

All files in this directory MUST use the global namespace `TechSalesManagement.Common` and be accessible from any layer.

## 24.1. Main Message Constants: MessageConstants (MSG)

Houses all business flow, success confirmations, and standard user-facing messages mapped via standard codes (e.g., `MSG1`).

- **Location**: `Common/MessageConstants.cs`
- **Namespace**: `TechSalesManagement.Common`
- **Structure**: Flat constants list.
- **Example**: `MessageConstants.MSG1`, `MessageConstants.MSG24`

## 24.2. Domain Validation Errors: DomainErrors

Specifically used for core business validation and invariant violations inside Domain Entities (usually thrown via `ArgumentException`).

- **Location**: `Common/DomainErrors.cs`
- **Namespace**: `TechSalesManagement.Common`
- **Structure**: Flat nested classes based on Domain Entity name.
- **Example**: `DomainErrors.Product.NameRequired`

## 24.3. Technical System Responses: ApiMessages

Used strictly for system-level failures or WebAPI lifecycle responses.

- **Location**: `Common/ApiMessages.cs`
- **Namespace**: `TechSalesManagement.Common`
- **Structure**: Flat constants.
- **Example**: `ApiMessages.InternalServerError`

---

# 25. Centralized Usage Example

```csharp
using TechSalesManagement.Common;

// 1. In Domain Entity (e.g. Product.cs)
if (price < 0) throw new ArgumentException(DomainErrors.Product.PriceNegative);

// 2. In Application Service (e.g. ProductService.cs)
if (string.IsNullOrWhiteSpace(keyword)) throw new BadRequestException(MessageConstants.MSG1);

// 3. In Global Exception Middleware (e.g. GlobalExceptionMiddleware.cs)
var message = ApiMessages.InternalServerError;
```