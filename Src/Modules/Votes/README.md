# 🗳️ Votes Module

The **Votes Module** empowers fans to engage with matches in real-time by casting votes for outcomes, MVPs, and key moments.

## 🌟 Key Features

- **Real-time Voting**: High-throughput voting system designed for live match peaks.
- **Deduplication**: Logic to ensure one vote per user per category.
- **Live Aggregates**: Synchronous and asynchronous aggregation for live charts.

## 🏗️ Domain Models

### Vote
Represents a user's choice in a specific voting context.
- **MatchId**
- **UserId**
- **Choice** (Home/Away/Draw/PlayerId)
- **Timestamp**

## 🤖 AI & Analytics Integration
The voting data is a primary signal for the **Intelligence Module**:
- **Sentiment Weighting**: Votes are used to calculate "Fan Confidence" metrics.
- **Market Prediction**: Aggregated voting patterns often serve as a lead indicator for match outcomes, used to calibrate AI prediction models.
- **Personalization**: User voting history is a key feature in the `UserAnalysis` embedding generation.

## 🛠️ Tech Highlights
- **Redis** for hot-path vote counting.
- **EventStoreDB** for a full audit log of fan engagement.
