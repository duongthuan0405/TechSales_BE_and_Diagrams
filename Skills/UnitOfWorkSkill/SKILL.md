---
name: unit-of-work-skill
description: Kỹ năng này quy định cách sử dụng Unit Of Work pattern để đảm bảo tính nhất quán của dữ liệu (transactional consistency) thông qua việc quản lý các phương thức BeginAsync, FinishAsync và RollbackAsync.
---

# Unit Of Work Convention

## 1. Overview

The system uses the Unit Of Work (UoW) pattern to ensure transactional consistency across multiple database operations.

All business workflows involving:
- Multiple repository operations
- Multiple database updates
- Critical transactional logic

must be executed inside a single Unit Of Work transaction.

The Unit Of Work controls:
- Transaction start
- Transaction commit
- Transaction rollback

This ensures:
- Data consistency
- Atomic operations
- Safe rollback when errors occur

---

# 2. Core Principle

Database changes are NOT permanently applied immediately.

Changes are only permanently committed when:

```csharp
await _unitOfWork.FinishAsync();
```

is executed successfully.

If `FinishAsync()` is never called:
- changes must not persist,
- transaction must not commit.

---

# 3. IUnitOfWork Interface

## Purpose

Provides transaction management for business workflows.

---

## Interface Example

```csharp
public interface IUnitOfWork
{
    Task BeginAsync();

    Task FinishAsync();

    Task RollbackAsync();
}
```

---

# 4. Responsibilities

## BeginAsync()

Starts a database transaction.

Must be called before transactional operations begin.

Example:

```csharp
await _unitOfWork.BeginAsync();
```

---

## FinishAsync()

Commits all database changes.

This is the MOST IMPORTANT operation.

Without calling:

```csharp
await _unitOfWork.FinishAsync();
```

changes must not be permanently saved.

---

## RollbackAsync()

Cancels all pending database changes.

Used when:
- validation fails,
- business logic fails,
- exceptions occur.

---

# 5. Architecture Rule

## IMPORTANT RULE

Every business workflow that modifies data MUST:

1. Begin transaction
2. Execute business logic
3. Call FinishAsync() at the end

Example:

```text
Begin Transaction
        ↓
Business Logic
        ↓
Repository Operations
        ↓
FinishAsync()
```

---

# 6. Failure Flow

If an exception occurs:

```text
Begin Transaction
        ↓
Business Logic
        ↓
Exception Occurs
        ↓
RollbackAsync()
```

---

# 7. Example Usage

## Correct Example

```csharp
public async Task RegisterAsync(RegisterRequest request)
{
    try
    {
        await _unitOfWork.BeginAsync();

        var existingUser =
            await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }

        var hashedPassword =
            _passwordHasher.Hash(request.Password);

        var user = new User
        {
            Email = request.Email,
            Password = hashedPassword
        };

        await _userRepository.AddAsync(user);

        await _emailService.SendWelcomeEmailAsync(user.Email);

        await _unitOfWork.FinishAsync();
    }
    catch
    {
        await _unitOfWork.RollbackAsync();
        throw;
    }
}
```

---

# 8. Why FinishAsync() Is Mandatory

Repositories only prepare changes inside DbContext.

Changes are not truly saved until:

```csharp
await _unitOfWork.FinishAsync();
```

is executed.

Without FinishAsync():
- INSERT does not persist
- UPDATE does not persist
- DELETE does not persist

FinishAsync() is the final commit point.

---

# 9. Agent Development Rules

## Rule 1
Every multi-step business workflow must use IUnitOfWork.

---

## Rule 2
Every transactional workflow must call:

```csharp
await _unitOfWork.FinishAsync();
```

at the end.

---

## Rule 3
If an exception occurs:
- RollbackAsync() must be called.

---

## Rule 4
Repositories must NEVER call SaveChangesAsync() directly.

Repositories only:
- Add
- Update
- Remove
- Query

Transaction commit is controlled only by Unit Of Work.

---

## Rule 5
Business Services are responsible for transaction management.

Controllers must never manage transactions.

---

# 10. Repository Behavior

## Repositories MUST NOT

```csharp
await _dbContext.SaveChangesAsync();
```

directly.

---

## Repositories SHOULD ONLY

```csharp
_dbContext.Users.Add(user);
```

or

```csharp
_dbContext.Users.Update(user);
```

The actual persistence happens only after:

```csharp
await _unitOfWork.FinishAsync();
```

---

# 11. Dependency Flow

```text
Controller
    ↓
Business Service
    ├── Repositories
    ├── Helper Services
    └── IUnitOfWork
```

---

# 12. Typical Workflow

```text
BeginAsync()
    ↓
Validate Business Rules
    ↓
Repository Operations
    ↓
Call Helper Services
    ↓
FinishAsync()
```

---

# 13. Rollback Workflow

```text
BeginAsync()
    ↓
Repository Operations
    ↓
Exception
    ↓
RollbackAsync()
```

---

# 14. Development Philosophy

The Unit Of Work pattern is used to:
- Ensure transaction consistency
- Prevent partial updates
- Centralize transaction management
- Simplify rollback handling
- Improve reliability of business workflows
- Ensure all database changes are committed safely