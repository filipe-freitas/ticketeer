# Ticketeer - Ticket Backlog

> **Note**: New tickets are always added at the top. Status: 🔴 To Do | 🟡 In Progress | 🟢 Done

---

## Iteração 10: Real-Time (Future)

> _To be refined_

**Objective**: SignalR for live seat map

**Planned Tickets**:

- Implement SignalR + SeatHub
- E2E tests with Playwright (2 synchronized browsers)

---

## Iteração 9: Background Jobs

> _To be refined_

**Objective**: Scheduled tasks

**Planned Tickets**:

- Implement expired reservation cleanup (BackgroundService)

---

## Iteração 8: Observability (Advanced)

> _To be refined_

**Objective**: Logs in Kibana + metrics in Grafana

**Planned Tickets**:

- Configure Serilog → Elasticsearch → Kibana sink
- Configure OpenTelemetry → Prometheus → Grafana
- Create metrics dashboard

---

## Iteração 7: Search & Messaging

> _To be refined_

**Objective**: ElasticSearch + MassTransit/RabbitMQ

**Planned Tickets**:

- Configure ElasticSearch + RabbitMQ via Aspire
- Implement movie text search
- Create MovieChangedEvent + Consumer
- Configure Polly (retry + circuit breaker)

---

## Iteração 6: Cache & Performance

> _To be refined_

**Objective**: Redis for heavy read endpoints

**Planned Tickets**:

- Configure Redis via Aspire
- Implement Output Caching on /movies
- Cache seat map (Redis Hash)
- Cache invalidation on ticket purchase

---

## Iteração 5: Core Business - Ticket Sales

> _To be refined_

**Objective**: Ticket purchase with concurrency protection

**Planned Tickets**:

- Create Sessions CRUD
- Implement ticket purchase (TicketService)
- Add Concurrency Token for double-booking protection
- Create Minimal API for /tickets (hot path)
- Load tests with K6

---

## Iteração 4: CRUD de Movies + Validation

> _To be refined_

**Objective**: Movie management with validation and optimized reads

**Planned Tickets**:

- Create MovieService + tests
- Add FluentValidation
- Create MoviesController (CRUD)
- Implement Dapper for optimized reads

---

## Iteração 3: API + Authentication

> _To be refined_

**Objective**: Authentication endpoints with JWT

**Planned Tickets**:

- Configure ASP.NET Core Identity
- Implement Register/Login with JWT
- Configure Swagger with Bearer authentication

---

## Iteração 2: Infrastructure + Persistence

> _To be refined_

**Objective**: EF Core with PostgreSQL via Aspire + basic logging

**Planned Tickets**:

- Configure .NET Aspire + PostgreSQL container
- Add Serilog (basic structured logging)
- Create DbContext with EF Core
- Configure Fluent API (FKs, indexes, constraints)
- Create migrations and validate schema
- Configure TestContainers for integration tests
- Create Movie Repository + integration tests

---

## Iteração 1: Domain Layer (TDD)

### 🎫 Ticket #010: Create Session and Ticket Entities with Availability Logic

**Status**: 🔴 To Do
**Type**: Feature + Test

#### Description

Implement Session and Ticket entities with business rule to check seat availability.

#### Acceptance Criteria

- [ ] Test `SessionTests.cs` validates `CanPurchaseSeat()` with occupied seat returns false
- [ ] Test validates `CanPurchaseSeat()` with available seat returns true
- [ ] Entity `Session` created in Domain
- [ ] Entity `Ticket` created in Domain
- [ ] Method `CanPurchaseSeat(Seat)` implemented
- [ ] All tests pass

#### Technical Notes

- Business logic must be in Domain, not Application
- Consult code-guidelines.md for naming conventions

---

### 🎫 Ticket #009: Create Seat Value Object

**Status**: 🔴 To Do
**Type**: Feature + Test

#### Description

Implement the Seat Value Object with equality tests.

#### Acceptance Criteria

- [ ] Test `SeatValueObjectTests.cs` created
- [ ] Test validates equality of two seats with same Row and Number
- [ ] Test validates inequality of different seats
- [ ] Value Object `Seat` implemented in Domain
- [ ] All tests pass

#### Technical Notes

- Value Objects must override `Equals()` and `GetHashCode()`
- Research Value Object pattern in DDD

---

### 🎫 Ticket #008: Create Hall Entity with Tests

**Status**: 🔴 To Do
**Type**: Feature + Test

#### Description

Implement the Hall entity with total seats calculation.

#### Acceptance Criteria

- [ ] Test `HallTests.cs` created
- [ ] Test validates `TotalSeats` calculation (RowCount \* SeatsPerRow)
- [ ] Entity `Hall` created in Domain with correct properties
- [ ] All tests pass

#### Technical Notes

- Follow naming conventions from code-guidelines.md
- Properties: Id, CinemaId, Name, RowCount, SeatsPerRow
- TotalSeats should be calculated property

---

### 🎫 Ticket #007: Create Cinema Entity with Tests

**Status**: 🔴 To Do
**Type**: Feature + Test

#### Description

Implement the Cinema entity following TDD.

#### Acceptance Criteria

- [ ] Test `CinemaTests.cs` created in Tests project
- [ ] Test validates Cinema creation with valid data
- [ ] Entity `Cinema` created in Domain project
- [ ] Properties: Id, Name, Address
- [ ] All tests pass

#### Technical Notes

- Follow AAA pattern (Arrange-Act-Assert) in tests
- Use Shouldly for assertions

---

### 🎫 Ticket #006: Create Movie Entity with Tests

**Status**: 🟢 Done
**Type**: Feature + Test

#### Description

Implement the first domain entity (Movie) following TDD.

#### Acceptance Criteria

- [x] Test `MovieTests.cs` created in Tests project
- [x] Test validates Movie creation with valid data
- [x] Entity `Movie` created in Domain project
- [x] Properties: Id, Title, Description, Duration, ReleaseDate, Genre
- [x] All tests pass

#### Technical Notes

- Follow AAA pattern (Arrange-Act-Assert) in tests
- Use Shouldly for assertions (consult code-guidelines.md)

---

### 🎫 Ticket #005: Create .editorconfig

**Status**: 🟢 Done
**Type**: Setup

#### Description

Create .editorconfig file for code formatting standardization across the project.

#### Acceptance Criteria

- [x] File `.editorconfig` created at solution root
- [x] C# formatting rules defined (indentation, braces, etc.)
- [x] Naming conventions configured
- [x] IDE respects the configuration

#### Technical Notes

- Use official .NET/C# EditorConfig conventions
- Consult code-guidelines.md for project standards

---

### 🎫 Ticket #004: Configure Unit Testing Packages

**Status**: 🟢 Done
**Type**: Setup

#### Description

Add testing packages to Ticketeer.Tests project and validate that the test pipeline works.

#### Acceptance Criteria

- [x] Package `xUnit` added (already exists, validate version)
- [x] Package `NSubstitute` added
- [x] Package `Shouldly` added
- [x] Package `Bogus` added
- [x] Command `dotnet test` executes without errors
- [x] CI pipeline runs tests automatically

#### Technical Notes

- Check latest package versions on NuGet
- Review ADR 005 to understand NSubstitute choice

---

## Iteração 0: Fundação & DevOps

### 🎫 Ticket #003: Validate CI Pipeline + Branch Protection

**Status**: 🟢 Done
**Type**: Test / Setup

#### Description

Push code to GitHub and validate that the CI pipeline executes successfully. Configure branch protection rules.

#### Acceptance Criteria

- [x] Push performed to `main` branch
- [x] GitHub Actions workflow executes
- [x] Build passes (green)
- [x] Branch `main` protected (require PR + status check passing)

#### Technical Notes

- Check GitHub Actions logs in case of failure
- Branch protection configured in GitHub Settings → Branches

---

### 🎫 Ticket #002: Create GitHub Actions Workflow

**Status**: 🟢 Done
**Type**: Setup

#### Description

Create basic CI/CD workflow for automated build and tests.

#### Acceptance Criteria

- [x] File `.github/workflows/ci.yml` created
- [x] Workflow executes `dotnet build`
- [x] Workflow executes `dotnet test`
- [x] Workflow runs on push to `main` and on pull requests

#### Technical Notes

- Use action `actions/setup-dotnet@v5` with .NET 10
- Search for .NET workflow examples on GitHub Actions Marketplace

---

### 🎫 Ticket #001: Create Solution and Projects

**Status**: 🟢 Done
**Type**: Setup
**Related ADR**: ADR 008 (Clean Architecture)

#### Description

Create project structure following Clean Architecture with Domain, Application, Infrastructure, API, and Tests layers.

#### Acceptance Criteria

- [x] Solution `Ticketeer.sln` created
- [x] Project `Ticketeer.Domain` (Class Library) created
- [x] Project `Ticketeer.Application` (Class Library) created
- [x] Project `Ticketeer.Infrastructure` (Class Library) created
- [x] Project `Ticketeer.API` (Web API) created
- [x] Project `Ticketeer.Tests` (xUnit) created
- [x] Project references configured correctly
- [x] Command `dotnet build` executes without errors

#### Technical Notes

- Application should reference only Domain
- Infrastructure should reference Application and Domain
- API should reference all projects
- Tests should reference all projects
- Consult ADR 008 to understand dependency rules

---
