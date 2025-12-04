# Ticketeer - Ticket Backlog

> **Note**: New tickets are always added at the top. Status: 🔴 To Do | 🟡 In Progress | 🟢 Done

---

## 🎫 Ticket #010: Configure Unit Testing
**Status**: 🔴 To Do
**Iteration**: 1
**Type**: Setup

### Description
Add testing packages to `Ticketeer.Tests` project and validate that the test pipeline works.

### Acceptance Criteria
- [ ] Packages `xUnit`, `NSubstitute`, `Shouldly`, `Bogus` added
- [ ] Command `dotnet test` executes without errors
- [ ] CI pipeline runs tests automatically

### Technical Notes
- Check latest package versions on NuGet
- Review ADR 005 to understand NSubstitute choice

---

## 🎫 Ticket #009: Create Seat Value Object
**Status**: 🔴 To Do
**Iteration**: 1
**Type**: Feature + Test

### Description
Implement the `Seat` Value Object with equality tests.

### Acceptance Criteria
- [ ] Test `SeatValueObjectTests.cs` created
- [ ] Test validates equality of two seats with same Row and Number
- [ ] Test validates inequality of different seats
- [ ] Value Object `Seat` implemented in Domain
- [ ] All tests pass

### Technical Notes
- Value Objects must override `Equals()` and `GetHashCode()`
- Research Value Object pattern in DDD

---

## 🎫 Ticket #008: Implement Session and Ticket with Availability Logic
**Status**: 🔴 To Do
**Iteration**: 1
**Type**: Feature + Test

### Description
Create `Session` and `Ticket` entities with business rule to check seat availability.

### Acceptance Criteria
- [ ] Test `SessionTests.cs` validates `CanPurchaseSeat()` with occupied seat returns false
- [ ] Test validates `CanPurchaseSeat()` with available seat returns true
- [ ] Entity `Session` created in Domain
- [ ] Entity `Ticket` created in Domain
- [ ] Method `CanPurchaseSeat(Seat)` implemented
- [ ] All tests pass

### Technical Notes
- Business logic must be in Domain, not Application
- Consult code-guidelines.md for naming conventions

---

## 🎫 Ticket #007: Create Hall Entity with Tests
**Status**: 🔴 To Do
**Iteration**: 1
**Type**: Feature + Test

### Description
Implement the `Hall` entity with total seats calculation.

### Acceptance Criteria
- [ ] Test `HallTests.cs` created
- [ ] Test validates `TotalSeats` calculation (RowCount * SeatsPerRow)
- [ ] Entity `Hall` created in Domain with correct properties
- [ ] All tests pass

### Technical Notes
- Follow naming conventions from code-guidelines.md
- Properties: Id, CinemaId, Name, TotalSeats, RowCount, SeatsPerRow

---

## 🎫 Ticket #006: Create Movie Entity with Tests
**Status**: 🔴 To Do
**Iteration**: 1
**Type**: Feature + Test

### Description
Implement the first domain entity (`Movie`) following TDD.

### Acceptance Criteria
- [ ] Test `MovieTests.cs` created in Tests project
- [ ] Test validates Movie creation with valid data
- [ ] Entity `Movie` created in Domain project
- [ ] Properties: Id, Title, Description, Duration, ReleaseDate, Genre
- [ ] All tests pass

### Technical Notes
- Follow AAA pattern (Arrange-Act-Assert) in tests
- Use Shouldly for assertions (consult code-guidelines.md)

---

## 🎫 Ticket #005: Validate CI Pipeline
**Status**: 🔴 To Do
**Iteration**: 0
**Type**: Test

### Description
Push to GitHub and validate that the CI pipeline executes successfully.

### Acceptance Criteria
- [ ] Push performed to `main` branch
- [ ] GitHub Actions workflow executes
- [ ] Build passes (green)
- [ ] Tests execute (even without tests yet)

### Technical Notes
- Check GitHub Actions logs in case of failure
- Workflow should be in `.github/workflows/ci.yml`

---

## 🎫 Ticket #004: Configure Git and GitHub
**Status**: 🔴 To Do
**Iteration**: 0
**Type**: Setup

### Description
Initialize local Git repository and connect to GitHub.

### Acceptance Criteria
- [ ] Git repository initialized (`git init`)
- [ ] `.gitignore` for .NET created
- [ ] Initial commit performed
- [ ] Remote repository on GitHub configured
- [ ] Initial push performed

### Technical Notes
- Use official .NET/Visual Studio `.gitignore` template
- Follow commit message convention from code-guidelines.md

---

## 🎫 Ticket #003: Create GitHub Actions Workflow
**Status**: 🔴 To Do
**Iteration**: 0
**Type**: Setup

### Description
Create basic CI/CD workflow for automated build and tests.

### Acceptance Criteria
- [ ] File `.github/workflows/ci.yml` created
- [ ] Workflow executes `dotnet build`
- [ ] Workflow executes `dotnet test`
- [ ] Workflow runs on push to `main` and on pull requests

### Technical Notes
- Use action `actions/setup-dotnet@v4` with .NET 10
- Search for .NET workflow examples on GitHub Actions Marketplace

---

## 🎫 Ticket #002: Configure .NET Aspire
**Status**: 🔴 To Do
**Iteration**: 0
**Type**: Setup
**Related ADR**: ADR 004 (Polyglot Persistence)

### Description
Add Aspire AppHost project and configure PostgreSQL container.

### Acceptance Criteria
- [ ] Project `Ticketeer.AppHost` created
- [ ] PostgreSQL container configured via Aspire
- [ ] Aspire dashboard accessible at `http://localhost:15888`
- [ ] PostgreSQL accessible by API

### Technical Notes
- Aspire requires .NET 10 SDK
- Consult official .NET Aspire documentation
- Dashboard shows telemetry from all services

---

## 🎫 Ticket #001: Create Solution and Projects
**Status**: 🟢 Done
**Iteration**: 0
**Type**: Setup
**Related ADR**: ADR 008 (Clean Architecture)

### Description
Create project structure following Clean Architecture with Domain, Application, Infrastructure, API, and Tests layers.

### Acceptance Criteria
- [X] Solution `Ticketeer.sln` created
- [X] Project `Ticketeer.Domain` (Class Library) created
- [X] Project `Ticketeer.Application` (Class Library) created
- [X] Project `Ticketeer.Infrastructure` (Class Library) created
- [X] Project `Ticketeer.API` (Web API) created
- [X] Project `Ticketeer.Tests` (xUnit) created
- [X] Project references configured correctly
- [X] Command `dotnet build` executes without errors

### Technical Notes
- Application should reference only Domain
- Infrastructure should reference Application and Domain
- API should reference all projects
- Tests should reference all projects
- Consult ADR 008 to understand dependency rules

---

