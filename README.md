<p align="center">
  <img src="assets/sports_logo.png" alt="snowball-Logo" width="120">
</p>

# SnowBall

<p align="center">
  <img src="assets/sports_banner.png" alt="SnowBall Platform Banner" width="100%">
</p>

<p align="center">

<a href="https://github.com/e89wrhi/sport-client">
  <img src="https://img.shields.io/badge/Frontend-GitHub-blue?style=for-the-badge&logo=github" alt="Frontend Repo">
</a>

<br/>

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Local Build](https://img.shields.io/badge/local_build-passing-blue)
![Deployment](https://img.shields.io/badge/deployment-pending-yellow)

![Dev](https://img.shields.io/badge/Dev-Not_Deployed-grey)
![Staging](https://img.shields.io/badge/Staging-Not_Deployed-grey)
![Production](https://img.shields.io/badge/Production-Awaiting-yellow)

</p>

**SnowBall** is a state-of-the-art sports platform built with a high-performance Modular Monolith architecture. It integrates real-time match data, user engagement via voting, and advanced AI-driven insights to provide a next-generation sports experience.

---

## 🚀 Key Features

*   **🏟️ Real-time Match Center**: Live updates, scores, and event tracking.
*   **🗳️ Fan Engagement**: Real-time voting system for match outcomes and MVP selections.
*   **🤖 Intelligence Module**: AI-powered predictive analytics, sentiment analysis, and personalized recommendations.
*   **⚡ High Performance**: Built on EventStoreDB for event sourcing and MongoDB for read models.
*   **🛠️ Modular Design**: Highly decoupled architecture allowing teams to work independently on modules like Match, Votes, Identity, and Intelligence.

---

## 🏗️ Architecture & Philosophy

SportNet follows the **Modular Monolith** pattern, balancing the simplicity of a single deployment unit with the scalability and decoupling of microservices.

### Tech Stack
*   **Core**: .NET 10, C#, Domain-Driven Design (DDD)
*   **API & Framework**: ASP.NET Core
*   **Persistence**: EF Core (PostgreSQL) for write models, MongoDB for read models (CQRS)
*   **Event Sourcing**: EventStoreDB
*   **Messaging**: MassTransit with RabbitMQ
*   **Intelligence**: AI / LLM Integration
*   **Observability & Orchestration**: .NET Aspire, OpenTelemetry

---

## 🧠 Intelligence Module (The AI Edge)

The `Intelligence` module is the brain of SportNet. It leverages modern AI techniques to transform raw sports data into actionable insights:

1.  **Predictive Match Analytics**: Win/Draw/Loss probabilities and dynamic score predictions based on historical and real-time data.
2.  **Social Sentiment Analysis**: Gauging the "vibe" of a match or team by ingesting live interactions and comments.
3.  **Personalized Recommendations**: Suggesting matches and content tailored to deep user behavioral patterns.

---

## 📂 Project Structure

The solution leverages a Clean Architecture and Domain-Driven Design inspired structure across highly cohesive modules:

```text
d:\dev\_projects\Sport\
├── Docs/             # Project documentation
├── Scripts/          # Developer scripts (DB migrations, etc.)
│   ├── Add-Migrations.ps1
│   ├── Update-Databases.ps1
│   └── ...
└── Src/
    ├── Api/          # Outer-facing API Gateway & Controllers
    ├── Aspire/       # .NET Aspire orchestration (AppHost & ServiceDefaults)
    ├── Common/       # Shared Infrastructure, Core DDD abstractions, and UI
    ├── Modules/      # Independent domain boundaries
    │   ├── Events/       # Match event streams
    │   ├── Identity/     # User authentication & security
    │   ├── Intelligence/ # AI-Powered Analytics
    │   ├── Match/        # Match management & scheduling
    │   └── Votes/        # Fan voting interactions
    └── Tests/        # Unit & Integration tests
```

---

## 🛠️ Getting Started

Running SportNet locally is seamless thanks to **.NET Aspire**, which automatically handles infrastructure dependencies like databases, message brokers, and event stores using container orchestration.

### Prerequisites
*   [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
*   Docker Desktop (required by .NET Aspire to spin up backing services)
*   Visual Studio 2022 (v17.9+) or JetBrains Rider (latest)

### Running the Ecosystem

1.  **Start the Aspire AppHost**
    Run the application via the `.NET Aspire AppHost`. This will launch the Dashboard, API, and all required containers (PostgreSQL, MongoDB, EventStore, RabbitMQ) automatically.
    
    ```bash
    dotnet run --project Src/Aspire/AppHost/AppHost.csproj
    ```
    
    _Or set `AppHost` as your startup project in your IDE and hit Run (F5)._

2.  **Apply Database Migrations**
    To set up your relational databases, run the provided utility scripts from the repository root:
    
    **PowerShell (Windows)**
    ```powershell
    ./Scripts/Update-Databases.ps1
    ```
    
    **Bash (Linux/macOS)**
    ```bash
    chmod +x Scripts/update-db.sh
    ./Scripts/update-db.sh
    ```

3.  **Explore the Dashboard**
    Upon launch, access the **.NET Aspire Dashboard** (at the URL specified in your terminal, typically `http://localhost:18888` or similar) to view application logs, metrics, distributed traces, and running containers.
