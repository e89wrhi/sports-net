# Next.js Site Implementation Guide for Sport-Net

This document outlines the implementation strategy, API integrations, and architecture for the Next.js frontend applications (Admin and Client).

## 1. Implementation Types

We recommend a hybrid approach using Next.js 14+ (App Router) to maximize performance and SEO.

### A. Server-Side Rendering (SSR)
*   **Use Case**: Match details, Admin dashboards, and pages with highly dynamic data.
*   **Benefit**: Ensures data is fresh on every request and provides better SEO for match pages.
*   **Implementation**: Use `async` Server Components to fetch data directly from the backend API.

### B. Static Site Generation (SSG) with Incremental Static Regeneration (ISR)
*   **Use Case**: Match listings, historical data, and static content (About, Contact).
*   **Benefit**: Extremely fast load times.
*   **Implementation**: Set a `revalidate` interval (e.g., 60 seconds) to update the cache in the background.

### C. Client-Side Rendering (CSR)
*   **Use Case**: Voting components, real-time score updates, and user profile management.
*   **Implementation**: Use `use client` components with hooks like `useSWR` or `React Query` for polling or SignalR for real-time updates.

---

## 2. API Configuration & Environment Variables

Create a `.env.local` file in your Next.js root:

```env
# Backend API Base URL
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000/api/v1.0

# Identity Server Configuration
NEXT_PUBLIC_IDENTITY_URL=http://localhost:5001
NEXT_PUBLIC_CLIENT_ID=sport-nextjs-client
NEXT_PUBLIC_SCOPE=match event vote identity
```

---

## 3. Client Admin Implementation (Management Portal)

The Admin portal is responsible for managing the lifecycle of matches and match events.

### Features:
*   **Create/Update Match**: Management of teams, league, status, and match time.
*   **Event Management**: Adding real-time events (Goals, Fouls, Cards) to a match.

### Key API Endpoints:
| Feature | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **Create Match** | `POST` | `/matches` | Initialize a new match |
| **Update Match** | `PUT` | `/matches/{id}` | Modify match details or status |
| **Delete Match** | `DELETE` | `/matches/{id}` | Remove a match |
| **Add Event** | `POST` | `/events` | Add a match event (e.g., Goal) |
| **Delete Event** | `DELETE` | `/events/{id}` | Remove an event |

### Admin Flow:
1.  **Auth**: User logs in via Identity Server with `admin` role.
2.  **Dashboard**: Fetches all matches via `GET /match/get-matches`.
3.  **Actions**: Uses standard forms to send `POST/PUT` requests with Bearer tokens.

---

## 4. Client View & Voting Implementation (Public Site)

The public site focuses on consumption and interaction (Voting).

### Features:
*   **View Matches**: Live and upcoming match listings.
*   **Match Details**: Viewing specific match info and timeline of events.
*   **Voting**: Users can vote for their favorite team or MVP.

### Key API Endpoints:
| Feature | Method | Endpoint | Description |
| :--- | :--- | :--- | :--- |
| **Get Matches** | `GET` | `/match/get-matches` | List all available matches |
| **Get Match** | `GET` | `/match/{id}` | Get detailed info for a match |
| **Get Events** | `GET` | `/events` | Get all events for a match |
| **Submit Vote** | `POST` | `/votes` | Cast a vote for a team/match |

### Voting Interaction Flow:
1.  **Fetch**: Check if the user has already voted for the match using `GET /votes?matchId={id}&voterId={userId}`.
2.  **Toggle**: Display voting buttons (Home / Draw / Away).
3.  **Submit**: On click, call `POST /votes`.
    *   *Payload*: `{ matchId, voterId, type }`
4.  **Feedback**: Update UI state to "Voted" and show updated percentages.

---

## 5. Technology Stack Recommendations

*   **Framework**: [Next.js](https://nextjs.org/) (App Router)
*   **Styling**: [Tailwind CSS](https://tailwindcss.com/)
*   **State Management**: [Zustand](https://github.com/pmndrs/zustand) or [React Query](https://tanstack.com/query/latest)
*   **Auth**: [NextAuth.js](https://next-auth.js.org/) (connecting to Duende IdentityServer)
*   **Icons**: [Lucide React](https://lucide.dev/)

---

## 6. TypeScript Data Models

To ensure type safety, use the following interfaces and enums corresponding to the backend DTOs.

### Enums
```typescript
export enum MatchLeague {
  PremierLeague = 1,
  LaLiga = 2,
  Bundesliga = 3,
}

export enum MatchStatus {
  Upcoming = 1,
  Live = 2,
  Postpond = 3,
  Over = 4,
}

export enum VoteType {
  Home = 1,
  Away = 2,
  Draw = 3,
}

export enum EventType {
  Goal = 1,
  Substitution = 2,
  Offside = 3,
  Penality = 4,
  Foul = 5,
  FreeKick = 6,
  YellowCard = 7,
  RedCard = 8,
}
```

### Match Models
```typescript
export interface MatchDto {
  id: string;
  homeTeam: string;
  awayTeam: string;
  homeTeamScore: number;
  awayTeamScore: number;
  league: MatchLeague;
  status: MatchStatus;
  matchTime: string; // ISO string
  eventsCount: number;
  homeVotesCount: number;
  awayVotesCount: number;
  drawVotesCount: number;
}

export interface GetMatchsResponse {
  matchDtos: MatchDto[];
}

### Event Models
```typescript
export interface EventDto {
  id: string;
  matchId: string;
  title: string;
  time: string; // ISO string
  type: EventType;
}

export interface GetEventsResponse {
  eventDtos: EventDto[];
}
```
```

### Request DTOs (Admin)
```typescript
export interface CreateMatchRequest {
  homeTeam: string;
  awayTeam: string;
  league: MatchLeague;
  status: MatchStatus;
  matchTime: string; // ISO string
}

export interface UpdateMatchRequest {
  matchId: string;
  homeTeam: string;
  awayTeam: string;
  league: MatchLeague;
  status: MatchStatus;
  matchTime: string; // ISO string
}

export interface AddEventRequest {
  matchId: string;
  title: string;
  time: string; // ISO string
  type: EventType;
}
```

### Request DTOs (Client/User)
```typescript
export interface AddVoteRequest {
  matchId: string;
  voterId: string;
  type: VoteType;
}
```
