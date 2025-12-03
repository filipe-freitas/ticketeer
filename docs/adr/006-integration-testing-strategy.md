# ADR 006: Integration Testing with WebApplicationFactory + TestContainers

## Status
Accepted

## Context
Integration tests validate that multiple components of the system work together correctly. For the Ticketeer API, this means testing the full HTTP request pipeline (Controllers, Middleware, Services, Database) without deploying to a real environment.

We need a strategy that:
1.  Tests the **real application code** (not mocked controllers).
2.  Uses **real infrastructure** (PostgreSQL, Redis) to catch database-specific bugs.
3.  Provides **test isolation** (parallel tests don't interfere with each other).
4.  Runs **fast** (no manual Docker setup or slow external dependencies).

## Alternatives Considered

### 1. In-Memory Databases (SQLite)
*   **Description**: Use SQLite in-memory mode as a substitute for PostgreSQL.
*   **Pros**: Fast, no external dependencies.
*   **Cons**: SQLite does not behave identically to PostgreSQL (different SQL dialects, missing features like `JSONB`, different concurrency models). Tests may pass but fail in production.

### 2. Shared Test Database
*   **Description**: All tests connect to a single PostgreSQL instance.
*   **Pros**: Uses the real database engine.
*   **Cons**: Tests cannot run in parallel (data conflicts). Requires manual cleanup. Flaky tests if cleanup fails.

### 3. Manual Docker Containers
*   **Description**: Developers manually start PostgreSQL/Redis containers before running tests.
*   **Pros**: Uses real infrastructure.
*   **Cons**: Requires manual setup. Developers forget to start containers. Not portable across CI/CD environments.

## Decision
We decided to use **WebApplicationFactory + TestContainers**.

### How It Works
1.  **TestContainers** automatically spins up Docker containers (PostgreSQL, Redis) at the start of each test class.
2.  **WebApplicationFactory** hosts the entire ASP.NET Core application in-memory.
3.  We override the application's configuration to point to the TestContainers-managed databases.
4.  Tests make real HTTP requests to the in-memory API, which interacts with real containerized infrastructure.
5.  Containers are destroyed after tests complete, ensuring a clean slate.

### Example
```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private PostgreSqlContainer _postgres;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _postgres = new PostgreSqlBuilder().Build();
        _postgres.StartAsync().Wait();
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<TicketeerDbContext>>();
            services.AddDbContext<TicketeerDbContext>(options =>
                options.UseNpgsql(_postgres.GetConnectionString()));
        });
    }
}

public class TicketTests : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CanPurchaseTicket()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsync("/tickets", ...);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}
```

## Consequences

### Positive
*   **Real Infrastructure**: Tests run against actual PostgreSQL/Redis, catching production-like issues.
*   **Isolation**: Each test class gets its own containers. Parallel execution is safe.
*   **Zero Manual Setup**: Developers just run `dotnet test`. TestContainers handles Docker automatically.
*   **CI/CD Ready**: Works seamlessly in GitHub Actions or any CI environment with Docker support.

### Negative
*   **Docker Dependency**: Requires Docker to be installed and running on developer machines and CI agents.
*   **Slower than Unit Tests**: Spinning up containers adds overhead (typically 2-5 seconds per test class). Still much faster than manual setup.
*   **Resource Usage**: Multiple parallel test runs consume more CPU/memory due to containerization.
