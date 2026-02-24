# ARCHITECTURE — AI Context Master File

> **Stack:** Next.js 14+ (App Router) + TypeScript 5 (FE) · .NET 8 + C# 12 (BE)
> **Pattern:** Vertical Slice Architecture (VSA) — both ends
> **Rule:** AI must read this file FIRST before generating any code.

---

## Stack Overview

| Layer | Technology | Version |
|---|---|---|
| Frontend | Next.js (App Router) | 14+ |
| FE Language | TypeScript | 5+ |
| FE State | Zustand | latest |
| FE Data Fetching | TanStack Query (React Query) | v5 |
| FE HTTP Client | Axios (typed) | latest |
| FE Validation | Zod | latest |
| FE Styling | Tailwind CSS | v3 |
| Backend | ASP.NET Core Web API | .NET 8 |
| BE Language | C# | 12 |
| BE ORM | Entity Framework Core | 8 |
| BE Mediator | MediatR | latest |
| BE Validation | FluentValidation | latest |
| BE Auth | JWT Bearer | — |
| BE Routing | Carter | latest |
| Database | PostgreSQL | 16 |

---

## Repository Structure

```
/
├── frontend/                    ← Next.js App
│   ├── src/
│   │   ├── app/                 ← App Router pages & layouts
│   │   ├── features/            ← VSA: feature slices
│   │   │   └── [domain]/
│   │   │       └── [feature]/
│   │   │           ├── api.ts           ← API call functions
│   │   │           ├── schema.ts        ← Zod schemas & types
│   │   │           ├── hooks.ts         ← React Query hooks
│   │   │           ├── store.ts         ← Zustand slice (if needed)
│   │   │           └── components/      ← UI components for this feature
│   │   ├── shared/
│   │   │   ├── api/             ← Axios instance, interceptors
│   │   │   ├── components/      ← Shared UI (Button, Modal, etc.)
│   │   │   ├── hooks/           ← Shared hooks
│   │   │   └── types/           ← Global types
│   │   └── middleware.ts        ← Auth middleware (Next.js)
│   ├── public/
│   └── .ai-context/             ← THIS FOLDER (FE context)
│
└── backend/                     ← .NET 8 Web API
    ├── src/
    │   └── MyProject.Api/
    │       ├── Features/         ← VSA: feature slices
    │       │   └── [Domain]/
    │       │       └── [Feature]/
    │       │           ├── [Feature]Endpoint.cs
    │       │           ├── [Feature]Request.cs
    │       │           ├── [Feature]Response.cs
    │       │           ├── [Feature]Handler.cs
    │       │           └── [Feature]Validator.cs
    │       ├── Shared/
    │       │   ├── Behaviors/    ← Pipeline behaviors
    │       │   ├── Exceptions/   ← Custom exceptions
    │       │   └── Extensions/   ← IServiceCollection extensions
    │       ├── Infrastructure/
    │       │   ├── Persistence/  ← DbContext, Migrations
    │       │   └── Auth/         ← JWT config
    │       ├── Domain/           ← Shared entities (cross-feature)
    │       └── Program.cs
    ├── tests/
    │   └── MyProject.Tests/
    │       └── Features/         ← Integration tests mirror Features/
    └── .ai-context/              ← THIS FOLDER (BE context)
```

---

## Golden Rules (AI Must Follow)

### Both FE & BE
- Never create a generic `Services/` or `Repositories/` folder at root level
- If logic is reused 3+ times → move to `Shared/`
- Every feature is self-contained in its own folder

### Frontend
- Use `schema.ts` (Zod) for ALL request/response type definitions
- Never call API directly in components — use hooks from `hooks.ts`
- Async server components for data fetching where possible (App Router)
- Client components (`"use client"`) only when interactivity needed

### Backend
- Use Primary Constructors for all DI classes
- Use `record` for all Request/Response DTOs
- Never throw raw `Exception` — use typed exceptions from `Shared/Exceptions/`
- Return `Result<T>` pattern, never throw for business logic failures
- All endpoints must use Carter `ICarterModule`

---

## API Base URL Convention

```
Development:  http://localhost:5242/api
Production:   https://api.[domain].com/api

Versioning:   /api/v{n}/{resource}
Example:      /api/v1/users
```

---

## Auth Flow

```
FE: NextAuth.js (or custom) → stores JWT in httpOnly cookie
BE: JWT Bearer middleware validates token on every protected request
Policy-Based Auth only — never [Authorize(Roles = "...")]
```
