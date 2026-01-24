# 🏟️ Match Module

The **Match Module** is the core of the SportNet platform, responsible for managing match lifecycles, real-time score updates, and sports metadata.

## 🌟 Key Features

- **Lifecycle Management**: Tracking matches from scheduled to live and finished.
- **Real-time Scores**: Optimized for low-latency score propagation.
- **Event Streaming**: Integration with the `Events` module for granular match events (goals, cards, VAR).
- **AI-Ready Data**: Exposes rich historical data for the `Intelligence` module to generate predictions.

## 🏗️ Domain Models

### Match
The central aggregate representing a sporting event.
- **Home/Away Teams**
- **Live Status**
- **Dynamic Scoring**
- **Aggregated Statistics** (Votes count, Event count)

## 🔄 Integration
- **Votes Module**: Listens for `VoteCast` events to update match popularity.
- **Events Module**: Broadcasts `MatchEvent` to update live scores.
- **Intelligence Module**: Provides data for `MatchPrediction`.

## 🛠️ Tech Highlights
- Uses **Event Sourcing** for match history tracking.
- **MongoDB Read Models** for ultra-fast queries in the discovery feed.
