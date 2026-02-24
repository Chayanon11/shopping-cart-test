# Coding Conventions

> Enforced standards for both Frontend (Next.js/TypeScript) and Backend (.NET 8/C#).
> AI must follow ALL rules in this file without exception.

---

## Backend — .NET 8 / C# 12

### Naming

| Element | Convention | Example |
|---|---|---|
| Class / Record | PascalCase | `RegisterUserHandler` |
| Interface | `I` prefix + PascalCase | `IUserRepository` |
| Method | PascalCase | `GetByIdAsync` |
| Async method | Suffix `Async` (except Carter endpoints) | `SaveChangesAsync` |
| Variable / Parameter | camelCase | `userId`, `emailAddress` |
| Private field | `_` + camelCase | `_dbContext` |
| Endpoint class | `[Action][Entity]Endpoint` | `RegisterUserEndpoint` |
| Request DTO | `[Action][Entity]Request` | `RegisterUserRequest` |
| Response DTO | `[Action][Entity]Response` | `RegisterUserResponse` |
| Validator | `[Request]Validator` | `RegisterUserRequestValidator` |
| Handler | `[Action][Entity]Handler` | `RegisterUserHandler` |
| Test method | `[Method]_[Scenario]_[Expected]` | `Register_WithDuplicateEmail_ReturnsConflict` |

### Syntax Rules

```csharp
// ✅ Primary Constructor (REQUIRED for all DI classes)
public class RegisterUserHandler(
    AppDbContext context,
    IPasswordHasher hasher,
    ILogger<RegisterUserHandler> logger
) : IRequestHandler<RegisterUserCommand, Result<Guid>> { }

// ❌ Old constructor pattern — FORBIDDEN
public class RegisterUserHandler : IRequestHandler<...>
{
    private readonly AppDbContext _context;
    public RegisterUserHandler(AppDbContext context) { _context = context; }
}

// ✅ Collection expressions (C# 12)
List<string> roles = ["Admin", "User"];
int[] ids = [1, 2, 3];

// ❌ Old collection init — FORBIDDEN
var roles = new List<string> { "Admin", "User" };

// ✅ Record for DTOs (immutable)
public record RegisterUserRequest(string Email, string Password, string FirstName, string LastName);

// ✅ Result<T> pattern for business logic
return Result.Failure<Guid>(UserErrors.EmailNotUnique);
return user.Id; // implicit success
```

### Error Handling

| Exception Class | HTTP Status | Location |
|---|---|---|
| `ValidationException` | 400 | Shared/Exceptions/ |
| `NotFoundException` | 404 | Shared/Exceptions/ |
| `UnauthorizedException` | 401 | Shared/Exceptions/ |
| `ConflictException` | 409 | Shared/Exceptions/ |
| `ForbiddenException` | 403 | Shared/Exceptions/ |

- All exceptions handled by `IExceptionHandler` (global)
- Response format: RFC 7807 `ProblemDetails`
- Never expose StackTrace in Production
- Never `throw new Exception("message")` — always use typed exceptions

### Testing (Backend)

- **Runner:** xUnit
- **Style:** Integration tests (WebApplicationFactory + Testcontainers)
- **Scope:** Every test covers full slice: HTTP request → DB → HTTP response
- **Naming:** `[Method]_[Scenario]_[Expected]`
- No mocking of DB — use real Postgres via Testcontainers

---

## Frontend — Next.js 14 / TypeScript 5

### Naming

| Element | Convention | Example |
|---|---|---|
| Component | PascalCase | `UserProfileCard` |
| Hook | camelCase, `use` prefix | `useCurrentUser` |
| Utility function | camelCase | `formatDate` |
| Zod schema | camelCase, `Schema` suffix | `registerSchema` |
| Inferred type from Zod | PascalCase | `RegisterRequest` |
| Store (Zustand) | camelCase, `use` prefix + `Store` | `useAuthStore` |
| API function | camelCase, verb first | `fetchUserById`, `createUser` |
| File (component) | PascalCase | `UserProfileCard.tsx` |
| File (hook/util/api) | camelCase | `hooks.ts`, `api.ts`, `schema.ts` |

### Component Rules

```typescript
// ✅ Server Component (default — no directive needed)
// Use for: data display, no interactivity
export default async function UserPage({ params }: { params: { id: string } }) {
  const user = await getUserById(params.id); // direct server-side fetch
  return <UserProfileCard user={user} />;
}

// ✅ Client Component (explicit directive required)
// Use ONLY when: useState, useEffect, event handlers, browser APIs needed
"use client";
export function LoginForm() { ... }

// ❌ Never fetch data inside a Client Component directly — use hooks.ts
```

### API Layer Rules

```typescript
// ✅ schema.ts — define Zod schema, infer type
import { z } from "zod";
export const registerSchema = z.object({
  email: z.string().email(),
  password: z.string().min(8),
  firstName: z.string(),
  lastName: z.string(),
});
export type RegisterRequest = z.infer<typeof registerSchema>;

// ✅ api.ts — typed API function using Axios instance
import { apiClient } from "@/shared/api/client";
import type { RegisterRequest, RegisterResponse } from "./schema";
export const registerUser = (data: RegisterRequest) =>
  apiClient.post<RegisterResponse>("/v1/auth/register", data);

// ✅ hooks.ts — React Query mutation/query
import { useMutation } from "@tanstack/react-query";
import { registerUser } from "./api";
export const useRegister = () =>
  useMutation({ mutationFn: registerUser });
```

### State Management

- **Server state:** TanStack Query (React Query v5) — all API data
- **Client/UI state:** Zustand — auth tokens, UI flags, modals
- **Form state:** React Hook Form + Zod resolver
- Never use `useState` for data that comes from an API

### Error Handling (Frontend)

```typescript
// Axios interceptor in shared/api/client.ts handles:
// 401 → clear auth store, redirect to /login
// 403 → toast "Access Denied"
// 500 → toast "Server Error"
// Validation errors (400) → parsed from ProblemDetails and mapped to form fields
```

---

## Shared: Security Policies (Backend Authorization)

| Policy Name | Claim Required | Used On |
|---|---|---|
| `CanManageUsers` | `permission: users.manage` | Admin user endpoints |
| `IsAuthenticated` | Any valid JWT | Default for protected routes |

**Rule:** Never use `[Authorize(Roles = "Admin")]` — always use named Policies.
