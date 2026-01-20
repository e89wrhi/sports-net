# Sport.Common

Welcome to the heart of the system! The **Sport.Common** project is a shared kernel designed to provide a robust, consistent foundation for all modules in our modular monolith.

Think of it as the "standard library" for our application—it handles the heavy lifting of infrastructure, patterns, and cross-cutting concerns so that you can focus on writing business logic.

## 🏗️ What's Inside?

This project is organized into several key areas, each tackling a specific responsibility:

### 🧩 Core Concepts (`/Core`)
The backbone of our Domain-Driven Design (DDD) and CQRS approach.
- **Aggregates & Entities**: Base classes that handle ID management and domain event tracking.
- **Event Dispatching**: A smart system that takes domain events and maps them to integration events or internal commands.
- **CQRS**: Interfaces for Commands, Queries, and their Handlers to keep our read and write sides separated and clean.

### 💾 Persistence (`/EFCore`, `/Mongo`, `/EventStoreDB`)
Flexible data storage options.
- **EFCore**: Base configurations and repositories for SQL databases.
- **Mongo**: Ready-to-use setups for NoSQL storage.
- **EventStoreDB**: Support for event-sourced aggregates.

### 📡 Messaging & Communication (`/MassTransit`, `/PersistMessageProcessor`)
How our modules talk to each other without getting tangled.
- **MassTransit**: Integration with message brokers (like RabbitMQ) for reliable asynchronous communication.
- **Outbox Pattern**: Ensuring that database updates and message publishing happen atomically.

### 🛡️ Resilience & Validation (`/Polly`, `/Validation`)
Making sure the system stays up and data stays clean.
- **Polly**: Policies for retries, circuit breakers, and timeouts.
- **Validation**: Shared FluentValidation rules and logic to catch errors early.

### 🌐 Web & API (`/Web`, `/OpenApi`, `/ProblemDetails`)
Consistent API behavior across all modules.
- **ProblemDetails**: Standardized error responses (RFC 7807).
- **Middlewares**: Shared logic for correlation IDs, exception handling, and logging.
- **OpenApi**: Automated Swagger documentation setup.

### 🔍 Observability (`/Logging`, `/OpenTelemetryCollector`, `/HealthCheck`)
Keeping an eye on the system's pulse.
- **Logging**: Pre-configured Serilog for structured logs.
- **Traces & Metrics**: Integration with OpenTelemetry to see how requests flow through the system.

## 🚀 How to Use

Most of the time, you'll be inheriting from classes in this project:
- Inherit from `Aggregate<TId>` for your domain models.
- Inherit from `EfCoreRepository` for data access.
- Use `IEventDispatcher` to broadcast what happened in your domain.

## 🛠️ Tech Stack
- **ORM**: Entity Framework Core & MongoDB
- **Messaging**: MassTransit
- **Validation**: FluentValidation
- **Resilience**: Polly
- **Observability**: Serilog & OpenTelemetry

---

*Remember: This project is shared across all modules. If you're thinking of adding something here, make sure it's truly "common" and not specific to just one business domain!*
