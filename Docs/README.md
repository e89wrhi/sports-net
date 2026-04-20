# 📚 SportNet Documentation

Welcome to the documentation directory for the **SportNet** platform. This directory is intended to house all technical specifications, architectural decisions, and feature proposals for the project.

## 🗂️ Documentation Structure

As the project grows, documentation should be organized into the following categories (feel free to create these folders/files as needed):

*   **Architecture (`/Architecture`)**: System Context Diagrams, EventStore Sequence Diagrams, Read Model Projections.
*   **API Specifications (`/API`)**: OpenAPI/Swagger specs, GraphQL schemas.
*   **Modules (`/Modules`)**: Detailed breakdown of each bounded context and Domain Event schemas.
*   **Infrastructure (`/Infrastructure`)**: .NET Aspire Orchestration details, Docker compose configurations.
*   **Proposals & Innovations (`/Proposals`)**: Detailed feature additions such as the ones below.

---

## 💡 The Platform Backlog

To elevate **SportNet** from a standard data-aggregation app to a unique, highly-engaging platform, we have outlined the following technical implementations designed to support an **expansive Multi-Sport ecosystem (Soccer/Football, American Football, Basketball, MMA/Boxing, and Golf)**:

### 1. The Multi-Agent Live Commentary System
*Standard apps give you a single text feed of what happened. Your app will have AI personas discussing the match, fight, or tournament live in real-time.*

**Technical Implementation:**
*   **Backend (Intelligence Module):**
    *   Integrate **Microsoft Semantic Kernel**.
    *   Create Sport-Specific AI personas: `The Tactical Manager` (Soccer), `The Gridiron Defensive Guru` (American Football), `The Striking & Grappling Analyst` (MMA/Boxing), `The Swing Mechanic` (Golf), and `The Analytics Nerd` (Basketball).
    *   Add a **SignalR Hub** (`CommentaryHub.cs`) to stream the AI dialogue to the frontend in real-time.
    *   Build an Event Consumer using **MassTransit**. When a `GoalScored`, `KnockdownScored`, or `BirdiePuttMade` event hits the broker, the agents react to it and push dialogue.
*   **Frontend (Sports-Client):**
    *   A chat-like interface beside the match feed.
    *   A `@mention` feature: *"@GridironGuru, why did the defense call a Cover 0 blitz?"*, *"@SwingMechanic, how did that lie affect the spin?"*, or *"@CombatAnalyst, how is he setting up that takedown?"*

### 2. High-Frequency Micro-Predictions (The "Next 60 Seconds" Market)
*Standard apps let you bet on who wins. Your app lets users compete on what happens in the very next play, sequence, or shot.*

**Technical Implementation:**
*   **Backend (Votes & Events Modules):**
    *   Create a fast-in-memory write model for predictions (e.g., Redis).
    *   Create cross-sport Commands:
        *   **Soccer**: `PredictNextEventCommand(Event: "CornerKick", TimeWindow: "Next 120s")`
        *   **American Football**: `PredictNextEventCommand(Event: "NextPlayType:Pass", TimeWindow: "CurrentDrive")`
        *   **Basketball**: `PredictNextEventCommand(Event: "NextFreeThrow:Made")`
        *   **MMA / Boxing**: `PredictNextEventCommand(Event: "TakedownAttempt", TimeWindow: "Next 60s")`
        *   **Golf**: `PredictNextEventCommand(Event: "PuttDistance:MakeFrom20Ft")`
    *   Create a background worker service (`PredictionResolutionService.cs`) that listens to the `EventStoreDB` to resolve predictions instantly and distribute points.
*   **Frontend (Sports-Client):**
    *   "Action Cards" that pop up on screen. (e.g., *Messi steps up to the free kick. Goal or Miss? -> 10 seconds to vote!*).
    *   A live Leaderboard sliding in from the right showing who is the prediction MVP for this event.

### 3. The "Vibe Map" (Real-Time Sentiment Ticker)
*Show the heartbeat of the arena and the fanbase live on the screen during massive shifts in momentum.*

**Technical Implementation:**
*   **Backend (Votes/Intelligence Modules):**
    *   Add an API endpoint/SignalR method to ingest high-frequency "taps" from users.
    *   Add a stream analytics processor wrapper (Rx.NET) that aggregates these events every 2 seconds.
    *   Calculate a rolling "Hype Index" (0 to 100).
*   **Frontend (Sports-Client):**
    *   An interactive button users can aggressively tap/mash when something insane happens (e.g., a massive Boxing knockout, a 90th-minute Soccer winner, or a crazy Golf hole-in-one on the 16th).
    *   A dynamic line chart at the bottom of the screen (the "Vibe Map"). People can scroll back through the timeline to see exactly when the fans went crazy.

### 4. Player "Heat, Form & Fatigue" Telemetry Simulator
*Simulating real-time momentum and biological/mental data based on match events using the AI module.*

**Technical Implementation:**
*   **Backend (Match & Intelligence Subsystem):**
    *   Create a `TelemetryReadModel`.
    *   Build algorithms that adjust "Momentum", "Fatigue", and "Pressure" scores based on real events:
        *   **Soccer**: Player completes 5 high-intensity sprints in 10 minutes (Stamina drain shown visually).
        *   **American Football**: Running back takes 3 heavy tackles (+Fatigue spike).
        *   **Basketball**: Player hits 3 consecutive shots (Triggers "On Fire" status).
        *   **MMA / Boxing**: Fighter sustains body shots or leg kicks (Simulated movement speed/stamina drop).
        *   **Golf**: "Pressure Index" spikes based on leaderboard closeness entering the 18th hole.
    *   Expose a GraphQL or gRPC endpoint pushing out this simulated state at 5 Hz.
*   **Frontend (Sports-Client):**
    *   Visual indicators like glowing rings around a Basketball avatar on fire, damage/stamina bars for Fighters, or a pulsing "heartbeat" graphic for a Golfer on a tournament-winning putt.
