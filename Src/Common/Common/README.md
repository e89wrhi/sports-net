# 🚀 Sport.Common

Welcome to the **Sport.Common** shared kernel! This project serves as the foundational backbone for our modular monolith, providing a robust, consistent, and highly reusable set of infrastructure, patterns, and cross-cutting concerns.

By abstracting away the complexity of messaging, persistence, and observability, **Sport.Common** allows developers to focus on what truly matters: **Business Logic.**

---

## 🏗️ Architectural Pillars

Our architecture is built on four main pillars, all of which are orchestrated within this project:

### 1. 🧩 Core Domain (DDD & CQRS)
The heart of our system, located in `/Core`.
- **Base Entities**: Standardized base classes for `Entity<TId>` and `Aggregate<TId>` with built-in domain event support.
- **CQRS Abstractions**: Clean interfaces for `ICommand`, `IQuery`, and their respective handlers using MediatR.
- **Domain Events**: A sophisticated `EventDispatcher` that handles the mapping of internal domain events to external integration events.

### 2. 💾 Hybrid Persistence
We support multiple storage paradigms to fit different module needs:
- **Relational**: `/EFCore` provides base repositories and automatic auditing for PostgreSQL.
- **Document**: `/Mongo` (NoSQL) offers high-performance storage for flexible data structures.
- **Event Sourced**: `/EventStoreDB` enables full audit trails and temporal state reconstruction.

### 3. 📡 Reliable Messaging
Communication between modules is handled asynchronously and reliably:
- **MassTransit**: Abstractions over RabbitMQ for easy message publishing and consuming.
- **Transactional Outbox/Inbox**: Located in `/PersistMessageProcessor`, ensuring "exactly-once" delivery and processing. It captures events in the same database transaction as your data.

### 4. 🔍 Deep Observability
See everything that happens in your system:
- **Tracing & Metrics**: `/OpenTelemetryCollector` integrates with Jaeger, Zipkin, and Grafana.
- **Logging**: Pre-configured structured logging with Serilog.
- **Health Checks**: `/HealthCheck` provides endpoints for monitoring system uptime and dependency health.

---

## 📂 Folder Structure

| Path | Description |
| :--- | :--- |
| `/Caching` | MediatR behaviors for automatic response caching. |
| `/Contracts` | Base interfaces for integration events and common types. |
| `/Core` | DDD base classes, CQRS interfaces, and Event Dispatching. |
| `/EFCore` | Entity Framework Core base context, repositories, and auditing. |
| `/EventStoreDB` | Support for Event Sourcing and Projections. |
| `/Exception` | Global exception handling and specialized exception types. |
| `/Jwt` | JSON Web Token authentication and authorization helpers. |
| `/MassTransit` | Messaging extensions and consumer filters (Idempotency). |
| `/Mongo` | MongoDB repository patterns and transaction management. |
| `/OpenTelemetryCollector` | Fully automated tracing, metrics, and exporter setup. |
| `/PersistMessageProcessor` | Implementation of the Outbox and Inbox patterns. |
| `/Polly` | Resilience policies (Retries, Circuit Breakers, Timeouts). |
| `/Validation` | FluentValidation integration and automatic validation behaviors. |
| `/Web` | Extensions for Minimal APIs, Versioning, and Controllers. |

---

## 🚀 Quick Start Examples

### Implementing an Aggregate
```csharp
public class Match : Aggregate<Guid>
{
    public string Name { get; private set; }
    
    public static Match Create(string name)
    {
        var match = new Match { Id = Guid.NewGuid(), Name = name };
        match.AddDomainEvent(new MatchCreated(match.Id));
        return match;
    }
}
```

### Creating a Command Handler
```csharp
public class CreateMatchHandler : ICommandHandler<CreateMatchCommand>
{
    private readonly IMatchRepository _repository;

    public async Task<Unit> Handle(CreateMatchCommand request, CancellationToken ct)
    {
        var match = Match.Create(request.Name);
        await _repository.AddAsync(match, ct);
        return Unit.Value;
    }
}
```

---

## 🛠️ Technology Stack

- **Framework**: .NET 8 / 9
- **Messaging**: MassTransit, RabbitMQ
- **Persistence**: EF Core (PostgreSQL), MongoDB, EventStoreDB
- **Observability**: OpenTelemetry, Serilog, Grafana
- **Utilities**: MediatR, FluentValidation, Polly, Mapster

---

## ⚖️ Guidelines

> [!IMPORTANT]
> **Sport.Common** is shared across all modules.
> 1. **Keep it Generic**: Never add business logic here.
> 2. **Avoid Heavy Dependencies**: Only add NuGet packages that are truly cross-cutting.
> 3. **Semantic Versioning**: Be careful with breaking changes, as they affect the entire monolith.

---
*Built with ❤️ by the Sport Engineering Team.*
