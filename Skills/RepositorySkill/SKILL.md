---
name: repository-convention-skill
description: Kỹ năng này định nghĩa trách nhiệm, quy tắc thiết kế và cách hoạt động của Repository trong hệ thống TechSales Management Backend, bao gồm cách tương tác với DbContext, Unit Of Work và Business Services.
---

# Repository Convention

# 1. Overview

Repositories are responsible for database access operations.

Repositories abstract persistence logic from business logic.

Repositories are part of the Infrastructure layer and implement repository interfaces defined in the Application layer.

Repositories must remain focused on:
- Data querying
- Data insertion
- Data updating
- Data deletion

Repositories must NOT contain business workflows.

---

# 2. Repository Responsibilities

Repositories are responsible ONLY for:
- Querying entities
- Persisting entities
- Updating entities
- Deleting entities
- Database filtering
- Database pagination
- Database sorting

Repositories must not contain:
- Business workflows
- Transaction logic
- Validation logic
- HTTP logic
- External service orchestration

---

# 3. Repository Architecture

Repository interfaces are defined inside the Application layer.

Repository implementations are defined inside the Infrastructure layer.

---

## Dependency Flow

```text
Controller
    ↓
Business Service
    ↓
Repository Interface
    ↓
Repository Implementation
    ↓
DbContext
```

---

# 4. Repository Interface Location

Repository interfaces must be placed inside:

```text
Application/Interfaces
```

or feature-specific folders.

Example:

```text
Application/
│
├── Interfaces/
│   ├── IUserRepository.cs
│   ├── IProductRepository.cs
│   └── IOrderRepository.cs
```

---

# 5. Repository Implementation Location

Repository implementations must be placed inside:

```text
Infrastructure/Repositories
```

Example:

```text
Infrastructure/
│
└── Repositories/
    ├── UserRepository.cs
    ├── ProductRepository.cs
    └── OrderRepository.cs
```

---

# 6. Async-First Architecture

The entire repository layer follows an Async-First architecture.

All repository methods must be asynchronous.

This applies to:
- Create operations
- Read operations
- Update operations
- Delete operations
- Existence checks
- Filtering operations
- Pagination operations

Even if the underlying ORM operation appears synchronous,
repositories must still expose asynchronous methods for architectural consistency.

---

# 7. Repository Interface Example

```csharp
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task<List<User>> GetAllAsync();

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}
```

---

# 8. Repository Implementation Example

```csharp
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);

        return Task.CompletedTask;
    }
}
```

---

# 9. Async Rules

## Rule 1

All repository methods must return:
- Task
- Task<T>

Correct:

```csharp
Task<User?> GetByIdAsync(Guid id);

Task AddAsync(User user);

Task DeleteAsync(User user);
```

Forbidden:

```csharp
User GetById(Guid id);

void Delete(User user);
```

---

## Rule 2

All Entity Framework Core queries must use async methods.

Correct:

```csharp
await _dbContext.Users.FirstOrDefaultAsync();

await _dbContext.Products.ToListAsync();

await _dbContext.Users.AnyAsync();
```

Forbidden:

```csharp
_dbContext.Users.FirstOrDefault();

_dbContext.Products.ToList();
```

---

## Rule 3

Async methods must always end with:

```text
Async
```

Examples:

```text
GetByIdAsync
GetByEmailAsync
ExistsAsync
AddAsync
UpdateAsync
DeleteAsync
```

---

## Rule 4

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

# 10. DbContext Rules

DbContext must only be accessed inside:
- Repositories
- Unit Of Work implementation

DbContext must NOT be accessed directly from:
- Controllers
- Business Services
- Helper Services

---

# 11. SaveChangesAsync Rules

Repositories must NEVER call:

```csharp
await _dbContext.SaveChangesAsync();
```

directly.

Repositories only prepare changes.

Actual database persistence happens only after:

```csharp
await _unitOfWork.FinishAsync();
```

---

# 12. Transaction Rules

Repositories must NOT:
- Begin transactions
- Commit transactions
- Rollback transactions

Transaction management belongs ONLY to:
- Business Services
- Unit Of Work

---

# 13. Business Logic Rules

Repositories must NEVER:
- Validate business rules
- Check workflow rules
- Execute business processes

Forbidden examples:

```csharp
if(product.StockQuantity < quantity)
```

```csharp
if(user.IsBlocked)
```

```csharp
await _emailService.SendAsync()
```

These belong to Business Services.

---

# 14. Repository Query Rules

Repositories may contain:
- Filtering
- Includes
- Sorting
- Pagination
- Optimized database queries

Example:

```csharp
return await _dbContext.Products
    .Where(x => x.IsActive)
    .OrderBy(x => x.Name)
    .Skip(page * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

---

# 15. DTO Rules

Repositories must NEVER:
- Return Response DTOs
- Accept Request DTOs

Repositories should only work with:
- Entities
- Domain models

---

# 16. Entity Rules

Repositories should:
- Return entities
- Persist entities

Repositories must not:
- Expose database models outside architecture boundaries improperly

---

# 17. Helper Service Rules

Repositories must NOT:
- Send emails
- Generate JWT tokens
- Hash passwords
- Generate OTPs
- Access Redis directly unless explicitly designed

These belong to Helper Services.

---

# 18. Validation Rules

Repositories must NOT perform:
- Business validation
- Workflow validation
- Permission validation

Repositories may perform:
- Query existence checks

Example:

```csharp
Task<bool> ExistsByEmailAsync(string email);
```

But decision-making logic belongs to Business Services.

---

# 19. Repository Naming Convention

Interfaces:

```text
IUserRepository
IProductRepository
IOrderRepository
```

Implementations:

```text
UserRepository
ProductRepository
OrderRepository
```

---

# 20. Repository Workflow Example

```text
Controller
    ↓
Business Service
    ↓
IUserRepository
    ↓
UserRepository
    ↓
ApplicationDbContext
    ↓
Database
```

---

# 21. Unit Of Work Integration

Repositories cooperate with Unit Of Work.

Repositories prepare entity changes.

Unit Of Work controls:
- SaveChangesAsync
- Transaction Commit
- Transaction Rollback

---

# 22. Forbidden Architecture Violations

Repositories must NEVER:
- Access Controllers
- Access HTTP Context
- Return IActionResult
- Access Request DTOs
- Access Response DTOs
- Call external APIs
- Send emails
- Generate tokens

---

# 23. Development Philosophy

Repositories exist to isolate persistence logic from business logic.

The repository layer follows a fully asynchronous architecture to:
- Improve scalability
- Prevent thread blocking
- Improve concurrent request handling
- Improve maintainability
- Simplify future architecture evolution
- Support future distributed systems
- Enforce clean architecture boundaries

Repositories should remain:
- Simple
- Predictable
- Persistence-focused
- Fully asynchronous