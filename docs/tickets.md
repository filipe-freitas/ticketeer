# Ticketeer - Ticket Backlog

> **Note**: New tickets are always added at the top. Status: 🔴 To Do | 🟡 In Progress | 🟢 Done

---

## Iteração 9: Real-Time (Future)
> *To be refined*

**Objective**: SignalR for live seat map

**Planned Tickets**:
- Implement SignalR + SeatHub
- E2E tests with Playwright (2 synchronized browsers)

---

## Iteração 8: Background Jobs
> *To be refined*

**Objective**: Scheduled tasks

**Planned Tickets**:
- Implement expired reservation cleanup (BackgroundService)

---

## Iteração 7: Observability
> *To be refined*

**Objective**: Structured logs + metrics

**Planned Tickets**:
- Configure Serilog → Elasticsearch → Kibana
- Configure OpenTelemetry → Prometheus → Grafana
- Create metrics dashboard

---

## Iteração 6: Search & Messaging
> *To be refined*

**Objective**: ElasticSearch + MassTransit/RabbitMQ

**Planned Tickets**:
- Configure ElasticSearch + RabbitMQ via Aspire
- Implement movie text search
- Create MovieChangedEvent + Consumer
- Configure Polly (retry + circuit breaker)

---

## Iteração 5: Cache & Performance
> *To be refined*

**Objective**: Redis for heavy read endpoints

**Planned Tickets**:
- Configure Redis via Aspire
- Implement Output Caching on /movies
- Cache seat map (Redis Hash)
- Cache invalidation on ticket purchase

---

## Iteração 4: Core Business - Ticket Sales
> *To be refined*

**Objective**: Ticket purchase with concurrency protection

**Planned Tickets**:
- Create Sessions CRUD
- Implement ticket purchase (TicketService)
- Add Concurrency Token for double-booking protection
- Create Minimal API for /tickets (hot path)
- Load tests with K6

---

## Iteração 3: API + Authentication
> *To be refined*

**Objective**: Functional endpoints with JWT

**Planned Tickets**:
- Configure ASP.NET Core Identity
- Implement Register/Login with JWT
- Configure Swagger with Bearer authentication
- Create Movies CRUD (Controller) + FluentValidation
- Implement optimized reads with Dapper

---

## Iteração 2: Infrastructure + Persistence
> *To be refined*

**Objective**: EF Core with PostgreSQL via Aspire

**Planned Tickets**:
- Configure .NET Aspire + PostgreSQL container
- Create DbContext with EF Core
- Configure Fluent API (FKs, indexes, constraints)
- Create migrations and validate schema
- Configure TestContainers for integration tests
- Create Movie Repository + integration tests

---

## Iteração 1: Domain Layer (TDD)
> *To be refined after Iteration 0 is complete*

**Objective**: Domain entities tested without infrastructure dependencies

**Planned Tickets**:
- Configure test packages (xUnit, NSubstitute, Shouldly, Bogus)
- Create .editorconfig for code formatting
- Create Movie entity + tests
- Create Cinema entity + tests
- Create Hall entity + tests (seat calculation)
- Create Seat Value Object + equality tests
- Create Session and Ticket entities + availability logic

---

## Iteração 0: Fundação & DevOps

### 🎫 Ticket #003: Validate CI Pipeline + Branch Protection
**Status**: 🔴 To Do
**Type**: Test / Setup

#### Description
Push code to GitHub and validate that the CI pipeline executes successfully. Configure branch protection rules.

#### Acceptance Criteria
- [ ] Push performed to `main` branch
- [ ] GitHub Actions workflow executes
- [ ] Build passes (green)
- [ ] Branch `main` protected (require PR + status check passing)

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
- [X] File `.github/workflows/ci.yml` created
- [X] Workflow executes `dotnet build`
- [X] Workflow executes `dotnet test`
- [X] Workflow runs on push to `main` and on pull requests

#### Technical Notes
- Use action `actions/setup-dotnet@v4` with .NET 10
- Search for .NET workflow examples on GitHub Actions Marketplace

---

### 🎫 Ticket #001: Create Solution and Projects
**Status**: 🟢 Done
**Type**: Setup
**Related ADR**: ADR 008 (Clean Architecture)

#### Description
Create project structure following Clean Architecture with Domain, Application, Infrastructure, API, and Tests layers.

#### Acceptance Criteria
- [X] Solution `Ticketeer.sln` created
- [X] Project `Ticketeer.Domain` (Class Library) created
- [X] Project `Ticketeer.Application` (Class Library) created
- [X] Project `Ticketeer.Infrastructure` (Class Library) created
- [X] Project `Ticketeer.API` (Web API) created
- [X] Project `Ticketeer.Tests` (xUnit) created
- [X] Project references configured correctly
- [X] Command `dotnet build` executes without errors

#### Technical Notes
- Application should reference only Domain
- Infrastructure should reference Application and Domain
- API should reference all projects
- Tests should reference all projects
- Consult ADR 008 to understand dependency rules

---
