# 🏆 SportNet: AI-Powered Sports Ecosystem

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512bd4.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Architecture: Modular Monolith](https://img.shields.io/badge/Architecture-Modular%20Monolith-blue.svg)](#architecture)
[![AI-Enhanced](https://img.shields.io/badge/AI-Enhanced-brightgreen.svg)](#intelligence-module)

**SportNet** is a state-of-the-art sports platform built with a high-performance Modular Monolith architecture. It integrates real-time match data, user engagement via voting, and advanced AI-driven insights to provide a next-generation sports experience.

---

## 🚀 Key Features

- **🏟️ Real-time Match Center**: Live updates, scores, and event tracking.
- **🗳️ Fan Engagement**: Real-time voting system for match outcomes and MVP selections.
- **🤖 Intelligence Module**: AI-powered predictive analytics, sentiment analysis, and personalized recommendations.
- **⚡ High Performance**: Built on EventStoreDB for event sourcing and MongoDB for read models.
- **🛠️ Modular Design**: Highly decoupled architecture allowing teams to work independently on modules like Match, Votes, and Identity.

---

## 🏗️ Architecture & Philosophy

SportNet follows the **Modular Monolith** pattern, balancing the simplicity of a single deployment unit with the scalability and decoupling of microservices.

### Tech Stack
- **Core**: .NET 8, C#, Domain-Driven Design (DDD)
- **Persistance**: EF Core (PostgreSQL), MongoDB (Read Models)
- **Event Sourcing**: EventStoreDB
- **Messaging**: MassTransit with RabbitMQ/Azure Service Bus
- **Intelligence**: LLM Integration (OpenAI/Gemini), Vector Search (Milvus/Pinecone)
- **Observability**: OpenTelemetry, Jaeger, Prometheus

---

## 🧠 Intelligence Module (The AI Edge)

The `Intelligence` module is the brain of SportNet. It leverages modern AI techniques to transform raw sports data into actionable insights.

### 1. Predictive Match Analytics
Using historical data and real-time form, our AI models predict:
- Win/Draw/Loss probabilities.
- Dynamic score predictions.
- Over/Under goal expectations.

### 2. Social Sentiment Analysis
By ingestion of social feeds and platform comments, SportNet gauges the "vibe" of a match or team in real-time.

### 3. Personalized Hyper-Recommendations
An embedding-based recommendation engine that suggests matches and content based on deep user behavioral patterns, not just simple filters.

---

## 📂 Project Structure

```text
Src/
├── Api/              # API Gateway & Controllers
├── Common/           # Shared Infrastructure & Core DDD patterns
└── Modules/          
    ├── Match/        # Match management & Live scores
    ├── Votes/        # Fan voting system
    ├── Events/       # Match event stream
    ├── Identity/     # User & Security
    └── Intelligence/ # AI-Powered Analytics & Insights (NEW)
```

---

## 🛠️ Getting Started

1. **Clone the repository**
2. **Setup Infrastructure**: Use the provided `docker-compose.yml` to spin up PostgreSQL, MongoDB, and EventStoreDB.
3. **Run the API**: `dotnet run --project Src/Api/Api.csproj`

---

## 👨‍💻 Author
Built with ❤️ for the future of sports.
Available for hire. [LinkedIn](https://linkedin.com) | [Portfolio](https://portfolio.com)
