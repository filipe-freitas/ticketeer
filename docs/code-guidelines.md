# Ticketeer Code Guidelines

> **Living Document**: Este arquivo será atualizado conforme o projeto evolui.

---

## ⚠️ CODE REVIEW PHILOSOPHY (CRITICAL)

> [!CAUTION]
> **NEVER provide solutions or code examples during code review.**
> 
> **Only point out issues, violations, or areas for improvement.**
> 
> The goal is learning through discovery. The developer must research and implement the solution themselves.

**Example of CORRECT review**:
- ❌ "This method violates Single Responsibility Principle"
- ❌ "Missing XML documentation on public method"
- ❌ "Using `var` here makes the type unclear"

**Example of INCORRECT review** (DO NOT DO THIS):
- ❌ "This method violates SRP. You should split it into two methods like this: `CalculateTotal()` and `ApplyDiscount()`"
- ❌ "Add XML docs like: `/// <summary>Calculates total</summary>`"

---

## 1. Naming Conventions

### C# Conventions
- **Classes**: `PascalCase` (ex: `MovieService`, `TicketeerDbContext`)
- **Interfaces**: Prefixo `I` + `PascalCase` (ex: `IMovieRepository`, `IUnitOfWork`)
- **Methods**: `PascalCase` (ex: `PurchaseTicketAsync`, `CanPurchaseSeat`)
- **Properties**: `PascalCase` (ex: `Title`, `ReleaseDate`)
- **Private Fields**: `_camelCase` (ex: `_dbContext`, `_logger`)
- **Local Variables**: `camelCase` (ex: `movieId`, `seatNumber`)
- **Constants**: `UPPER_SNAKE_CASE` (ex: `MAX_SEATS_PER_ROW`, `DEFAULT_TIMEOUT`)

### File Naming
- **Classes**: Match class name (ex: `MovieService.cs`)
- **Interfaces**: Match interface name (ex: `IMovieRepository.cs`)
- **Tests**: `{ClassName}Tests.cs` (ex: `MovieServiceTests.cs`)

---

## 2. Architecture Rules (Clean Architecture - ADR 008)

### Dependency Rules
- ✅ **Domain** → No dependencies (pure business logic)
- ✅ **Application** → References Domain only
- ✅ **Infrastructure** → References Application + Domain
- ✅ **API** → References all layers
- ❌ **Never** reference outer layers from inner layers

### Layer Responsibilities
- **Domain**: Entities, Value Objects, Domain Events, Business Rules
- **Application**: DTOs, Interfaces, Services, Validators
- **Infrastructure**: EF Core, Dapper, Redis, MassTransit implementations
- **API**: Controllers, Minimal APIs, Middleware, Filters

### Key Principles
- Never expose Domain Entities directly via API (use DTOs)
- All cross-cutting concerns (logging, validation) in Application/Infrastructure
- Use Dependency Injection for all services

---

## 3. Testing Standards

### Test Naming
Format: `MethodName_Scenario_ExpectedResult`

**Examples**:
```csharp
CanPurchaseSeat_WhenSeatIsOccupied_ReturnsFalse()
PurchaseTicketAsync_WithValidRequest_ReturnsTicketResponse()
CreateMovie_WithEmptyTitle_ThrowsValidationException()
```

### Test Structure (AAA Pattern)
```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var movie = new Movie { Title = "Avatar" };
    
    // Act
    var result = movie.IsValid();
    
    // Assert
    result.ShouldBeTrue();
}
```

### Assertion Library
- ✅ Use **Shouldly** for assertions
- ❌ Avoid `Assert.Equal`, `Assert.True`, etc.

**Examples**:
```csharp
result.ShouldBe(expected);
result.ShouldNotBeNull();
list.ShouldContain(item);
exception.ShouldBeOfType<ValidationException>();
```

### Test Data
- Use **Bogus** for realistic fake data
- Use **AutoFixture** for quick object generation
- Avoid magic numbers (use constants or variables with descriptive names)

---

## 4. Code Style

### General
- **Max line length**: 120 characters
- **Indentation**: 4 spaces (no tabs)
- **Braces**: Always use braces, even for single-line statements

### Type Usage
- ✅ Use `var` when type is obvious from right-hand side
  ```csharp
  var movie = new Movie(); // Good
  var count = GetCount();  // Bad (not obvious)
  ```
- ✅ Use explicit types for primitives and unclear cases
  ```csharp
  int count = GetCount();
  string title = GetTitle();
  ```

### Async/Await
- All I/O operations must be async
- Suffix async methods with `Async` (ex: `PurchaseTicketAsync`)
- Always use `ConfigureAwait(false)` in library code (not needed in ASP.NET Core)

### Null Handling
- Use nullable reference types (`string?`, `Movie?`)
- Prefer `ArgumentNullException.ThrowIfNull(parameter)` over manual checks
- Use null-coalescing operator when appropriate (`??`, `??=`)

---

## 5. Documentation

### XML Documentation
- All public classes, methods, and properties must have XML docs
- Use `<summary>`, `<param>`, `<returns>`, `<exception>` tags

**Example**:
```csharp
/// <summary>
/// Purchases a ticket for the specified session and seat.
/// </summary>
/// <param name="request">The purchase request containing session and seat information.</param>
/// <returns>The purchased ticket details.</returns>
/// <exception cref="ValidationException">Thrown when the seat is already occupied.</exception>
public async Task<TicketResponse> PurchaseTicketAsync(PurchaseTicketRequest request)
```

### Comments
- Use comments to explain **why**, not **what**
- Avoid obvious comments
- Use `// TODO:` for temporary code that needs improvement

---

## 6. Error Handling

### Exceptions
- Use specific exception types (avoid generic `Exception`)
- Create custom exceptions when needed (ex: `SeatAlreadyOccupiedException`)
- Never swallow exceptions silently

### Validation
- Use **FluentValidation** for input validation
- Validate at the Application layer (not Domain or API)
- Return meaningful error messages to the client

---

## 7. Performance

### Database
- Use **Dapper** for read-heavy queries (ADR 003)
- Use **EF Core** for writes and complex domain logic
- Always use `.AsNoTracking()` for read-only queries in EF Core
- Avoid N+1 queries (use `.Include()` or Dapper joins)

### Caching
- Cache expensive read operations in **Redis**
- Use Output Caching for API endpoints when appropriate
- Invalidate cache when data changes

---

## 8. Security

### Authentication
- All endpoints except `/auth/*` must require authentication
- Use `[Authorize]` attribute on controllers/endpoints
- Never log sensitive data (passwords, tokens)

### Input Validation
- Validate all user inputs
- Sanitize data before storing in database
- Use parameterized queries (EF Core/Dapper handle this automatically)

---

## 9. Git Workflow

### Commit Messages
Format: `<type>: <description>`

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `test`: Adding tests
- `refactor`: Code refactoring
- `docs`: Documentation changes
- `chore`: Build/tooling changes

**Examples**:
```
feat: add ticket purchase endpoint
fix: prevent double-booking with concurrency token
test: add integration tests for MovieRepository
docs: update ADR 008 with dependency rules
```

### Branch Naming
- `feature/ticket-purchase`
- `fix/double-booking-issue`
- `refactor/movie-service`

---

## 10. Code Review Checklist

Before submitting code for review, ensure:
- [ ] All tests pass (`dotnet test`)
- [ ] Code follows naming conventions
- [ ] No warnings or errors in IDE
- [ ] XML documentation added for public APIs
- [ ] No hardcoded values (use configuration or constants)
- [ ] Async methods use `Async` suffix
- [ ] Dependency injection used (no `new` for services)
- [ ] Clean Architecture layers respected

---

## 11. Pull Request Guidelines

### Template
We use a standard Pull Request template. Please ensure you fill out all relevant sections:
- **Summary**: What does this PR do?
- **Changes**: Bullet points of technical changes.
- **Related Issues**: Link to the ticket (e.g., `Closes #123`).
- **Testing**: How did you verify your changes?

### Expectations
- **One PR per Feature/Fix**: Avoid massive PRs that touch everything.
- **Self-Review**: Review your own code before assigning reviewers.
- **CI Green**: Ensure the build and tests pass before requesting review.
- **Screenshots**: For UI/API changes, provide proof of work (screenshots or response snippets).

