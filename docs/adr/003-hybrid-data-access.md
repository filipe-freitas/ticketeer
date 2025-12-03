# ADR 003: Hybrid Data Access (EF Core + Dapper)

## Status
Accepted

## Context
The application has two distinct data access patterns:
1.  **Command / Write Model**: Complex business transactions involving domain entities, validation, and consistency checks (e.g., Purchasing a Ticket, Scheduling a Session).
2.  **Query / Read Model**: High-performance data retrieval for lists, dashboards, and reports (e.g., "List all movies playing today", "Sales report by Cinema").

Using a single tool for both often leads to suboptimal results:
*   **EF Core** is excellent for state management but can generate inefficient SQL for complex analytical queries or bulk reads.
*   **Raw SQL (Dapper)** is fast but requires manual mapping and lacks the productivity features of a full ORM for complex object graphs.

## Decision
We decided to use a **Hybrid Data Access Strategy**:

1.  **EF Core** for **Commands (Writes)** and Domain Logic.
    *   It will manage the Write Model.
    *   We will leverage its Change Tracker, Unit of Work pattern, and Migrations.
    *   *Goal*: Consistency and productivity in business logic.

2.  **Dapper** for **Queries (Reads)** and Reporting.
    *   It will manage the Read Model (CQRS-lite).
    *   We will write raw, optimized SQL for complex projections and dashboards.
    *   *Goal*: Performance and control over the execution plan.

## Consequences

### Positive
*   **Performance**: Critical read paths (which usually outnumber writes 10:1) will be as fast as raw SQL allows.
*   **Separation of Concerns**: Encourages a mental separation between "modifying state" and "viewing state" (CQRS principle).
*   **Roadmap Alignment**: Both EF Core and Dapper are "Must Know" technologies in the roadmap.

### Negative
*   **Maintenance**: Developers must know SQL well to use Dapper effectively.
*   **Duplication**: Some simple projections might be duplicated in EF Core entities and Dapper SQL queries, though this is often intentional in CQRS.
