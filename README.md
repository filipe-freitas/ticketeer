# Ticketeer 🎬

> A cinema network management API built with .NET 10 to master "Must Know" concepts from the .NET Developer Roadmap.

---

## 🎯 Project Vision

Ticketeer is a **learning-focused** production-grade API that manages movies, cinemas, halls, sessions, and ticket sales. The project serves as a practical laboratory to apply enterprise-level .NET patterns and technologies.

**Key Focus**: Apply "Must Know" technologies from the [.NET Roadmap](https://roadmap.sh/r/net-developer-roadmap-1hryy) in a real-world scenario.

---

## 📚 Essential Documentation

### Architecture & Planning
- **[API Product Plan](docs/api-product-plan.md)** - Complete product vision, tech stack, and development roadmap (Portuguese)
- **[Architecture Decision Records (ADRs)](docs/adr/)** - 8 ADRs documenting key architectural choices:
  - [ADR 001](docs/adr/001-use-jwt-authentication.md) - JWT Authentication
  - [ADR 002](docs/adr/002-hybrid-api-approach.md) - Hybrid API (Minimal + Controllers)
  - [ADR 003](docs/adr/003-hybrid-data-access.md) - Hybrid Data Access (EF Core + Dapper)
  - [ADR 004](docs/adr/004-polyglot-persistence.md) - Polyglot Persistence (Postgres + Redis + Elastic)
  - [ADR 005](docs/adr/005-nsubstitute-for-mocking.md) - NSubstitute for Mocking
  - [ADR 006](docs/adr/006-integration-testing-strategy.md) - Integration Testing Strategy
  - [ADR 007](docs/adr/007-observability-strategy.md) - Observability Strategy (Logs vs Metrics)
  - [ADR 008](docs/adr/008-clean-architecture.md) - Clean Architecture

### Development Guidelines
- **[Code Guidelines](docs/code-guidelines.md)** - Coding standards, naming conventions, and **critical code review rules**
- **[Tickets](docs/tickets.md)** - Current backlog with status updates (🔴 To Do | 🟡 In Progress | 🟢 Done)

---

## 🏗️ Architecture

**Pattern**: Clean Architecture (4 layers)

```
┌─────────────────────────────────────┐
│  Ticketeer.API                      │  ← Controllers, Minimal APIs, Middleware
├─────────────────────────────────────┤
│  Ticketeer.Infrastructure           │  ← EF Core, Dapper, Redis, MassTransit
├─────────────────────────────────────┤
│  Ticketeer.Application              │  ← Services, DTOs, Validators
├─────────────────────────────────────┤
│  Ticketeer.Domain                   │  ← Entities, Value Objects, Business Rules
└─────────────────────────────────────┘
```

**Dependency Rule**: Inner layers never depend on outer layers.

---

## 🛠️ Tech Stack (Must Know)

### Core
- **.NET 10** - Latest LTS framework
- **C# 14** - Modern language features
- **ASP.NET Core** - Web API framework
- **.NET Aspire** - Orchestration and telemetry

### Data & Persistence
- **PostgreSQL** - Primary relational database
- **EF Core** - ORM for writes (Code First, Migrations)
- **Dapper** - Micro-ORM for high-performance reads
- **Redis** - Caching and real-time state management
- **ElasticSearch** - Full-text search for movies

### Messaging & Background Jobs
- **MassTransit + RabbitMQ** - Asynchronous messaging
- **BackgroundService + PeriodicTimer** - Scheduled tasks

### Observability
- **Serilog** - Structured logging → Elasticsearch → Kibana
- **OpenTelemetry** - Metrics and traces → Prometheus → Grafana

### Testing
- **xUnit** - Test framework
- **NSubstitute** - Mocking
- **Shouldly** - Assertions
- **Bogus** - Test data generation
- **TestContainers** - Integration tests with real infrastructure
- **Playwright** - E2E browser tests
- **K6** - Load testing

### Additional Libraries
- **FluentValidation** - Input validation
- **Polly** - Resilience (retry, circuit breaker)

---

## 📊 Current Status

**Phase**: Iteration 0 - Foundation  
**Active Ticket**: [Check tickets.md](docs/tickets.md) for latest status

### Completed
- ✅ Architecture planning (8 ADRs)
- ✅ API Product Plan
- ✅ Code Guidelines
- ✅ Initial backlog (10 tickets)

### In Progress
- See [tickets.md](docs/tickets.md) for current work

---

## 🚦 Getting Started

### Prerequisites
- **.NET 10 SDK**
- **Docker Desktop** (for Aspire & Containers)
- **IDE**: Rider, Visual Studio 2022, or VS Code

### Running the Project

1. **Clone the repository**
   ```bash
   git clone https://github.com/filipefreitas/ticketeer.git
   cd ticketeer
   ```

2. **Build the Solution**
   ```bash
   dotnet build
   ```

3. **Run Tests**
   ```bash
   dotnet test
   ```

4. **Run with Aspire (Future)**
   Once configured (Ticket #002), you will run:
   ```bash
   dotnet run --project src/Ticketeer.AppHost
   ```
   This will spin up the API, Database, and Dashboard.

---

## 🚀 Quick Start (For New Conversations)

When starting a new chat for **code review or questions**, mention:

```
@README.md @tickets.md @code-guidelines.md

[Your question or request here]
```

This gives full context of:
- Project architecture and decisions
- Current work status
- Code review rules

---

## 📁 Project Structure

```
ticketeer/
├── Ticketeer.Domain/          # Entities, Value Objects, Business Rules
├── Ticketeer.Application/     # Services, DTOs, Interfaces, Validators
├── Ticketeer.Infrastructure/  # EF Core, Dapper, Redis, MassTransit
├── Ticketeer.API/            # Controllers, Minimal APIs, Middleware
├── Ticketeer.Tests/          # Unit, Integration, E2E tests
├── Ticketeer.AppHost/        # .NET Aspire orchestration
├── docs/
│   ├── adr/                  # Architecture Decision Records
│   ├── api-product-plan.md   # Product vision and roadmap
│   ├── code-guidelines.md    # Development standards
│   └── tickets.md            # Work backlog
└── README.md                 # This file
```

---

## 🎓 Learning Philosophy

> **Critical**: This project prioritizes **learning through discovery**.
>
> During code reviews, issues are **pointed out**, but **solutions are never provided**.  
> The developer must research and implement fixes independently.

See [Code Guidelines - Code Review Philosophy](docs/code-guidelines.md#️-code-review-philosophy-critical) for details.

---

## 🔗 Key Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Clean Architecture (Uncle Bob)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)

---

## 📝 Notes

- **Language**: All code and documentation in **English**, except `api-product-plan.md` (Portuguese)
- **Git Workflow**: Conventional commits (see [code-guidelines.md](docs/code-guidelines.md#9-git-workflow))
- **CI/CD**: GitHub Actions (automated build + tests)

---

**Last Updated**: 2025-11-30  
**Project Owner**: [@filipefreitas](https://github.com/filipefreitas)