# AI Agent Standard Operating Procedure

> This file defines the exact workflow AI must follow for every development task.
> Non-negotiable. Execute steps in order.

---

## Step 1: Context Loading

Before writing any code, read:

1. `.ai-context/ARCHITECTURE.md` — stack, folder structure, golden rules
2. `.ai-context/entity_list.md` — existing entities (avoid duplication)
3. `.ai-context/api_list.md` — existing endpoints (avoid duplication)
4. `.ai-context/api_relation.md` — dependency map (understand impact)
5. `.ai-context/conventions.md` — naming + syntax rules

---

## Step 2: Planning

Determine task type and answer these questions:

| Question | Action |
|---|---|
| Is this a new Feature/Slice? | Define domain, feature name, folder paths for both FE and BE |
| Is this modifying an existing Slice? | Locate files using VSA path convention |
| Does this touch shared entities? | Check entity_list.md first |
| Does this affect other features? | Check api_relation.md for downstream impact |

**Path Convention (BE):**
`backend/src/MyProject.Api/Features/{Domain}/{FeatureName}/{FeatureName}{File}.cs`

**Path Convention (FE):**
`frontend/src/features/{domain}/{featureName}/{file}.ts`

---

## Step 3: Backend Code Generation

Generate files in this order:

```
1. {Feature}Request.cs         → record type, properties matching DTO in api_list.md
2. {Feature}Response.cs        → record type
3. {Feature}Validator.cs       → FluentValidation AbstractValidator<TRequest>
4. {Feature}Handler.cs         → IRequestHandler, Primary Constructor, Result<T> return
5. {Feature}Endpoint.cs        → Carter ICarterModule, maps route from api_list.md
6. (if new entity) Domain/{Entity}.cs  → Entity class with domain factory method
7. (if needed) {Domain}ModuleExtensions.cs → IServiceCollection extension
```

**Checklist before submitting:**
- [ ] Primary Constructors used (no old-style constructor)
- [ ] `record` type for Request/Response DTOs
- [ ] FluentValidation (not DataAnnotations)
- [ ] All errors use typed exceptions from `Shared/Exceptions/`
- [ ] Carter endpoint registered in correct version group `/api/v1/...`
- [ ] No `Stack Trace` exposure in responses

---

## Step 4: Frontend Code Generation

Generate files in this order:

```
1. schema.ts    → Zod schema + inferred TypeScript types
2. api.ts       → Typed Axios functions (matches endpoint in api_list.md)
3. hooks.ts     → TanStack Query hooks (useQuery / useMutation)
4. store.ts     → Zustand slice (only if client state needed)
5. components/  → UI components that consume hooks (no direct API calls)
```

**Checklist before submitting:**
- [ ] All types derived from Zod schema (no manual interface duplication)
- [ ] Components never call API directly — always via hooks
- [ ] Server Components used where interactivity is NOT needed
- [ ] `"use client"` added ONLY when required
- [ ] Error states handled in hooks (onError / error boundary)
- [ ] Loading states exposed from hooks

---

## Step 5: Test Generation (Backend)

For every new BE slice, generate 1 integration test class:

```
tests/MyProject.Tests/Features/{Domain}/{FeatureName}Tests.cs
```

Each test class must include at minimum:
- Happy path test
- Validation failure test

**Naming:** `{Method}_{Scenario}_{ExpectedResult}`
Example: `Register_WithValidData_Returns201Created`

---

## Step 6: Metadata Update (MANDATORY)

After completing any code generation, update these files:

| File | Update When |
|---|---|
| `entity_list.md` | New entity added or properties changed |
| `api_list.md` | New endpoint added, or DTO changed; also update FE Hooks Map |
| `api_relation.md` | New inter-feature dependency or domain event |
| `conventions.md` | New shared exception, policy, or pattern introduced |

**Rule:** Never skip Step 6. Stale metadata is worse than no metadata.

---

## Quick Reference: File Templates

### BE — Handler Template
```csharp
namespace MyProject.Api.Features.{Domain}.{Feature};

public class {Feature}Handler(
    AppDbContext context,
    ILogger<{Feature}Handler> logger
) : IRequestHandler<{Feature}Command, Result<{ResponseType}>>
{
    public async Task<Result<{ResponseType}>> Handle(
        {Feature}Command command, CancellationToken ct)
    {
        // 1. Business logic
        // 2. Persist
        // 3. Return Result
    }
}
```

### BE — Endpoint Template
```csharp
namespace MyProject.Api.Features.{Domain}.{Feature};

public class {Feature}Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/{resource}", async (
            {Feature}Request request, ISender sender) =>
        {
            var result = await sender.Send(new {Feature}Command(request));
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
        })
        .WithTags("{Domain}")
        .WithName("{Feature}")
        .WithOpenApi();
    }
}
```

### FE — Hook Template
```typescript
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { {featureApiFunction} } from "./api";

export const use{Feature} = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: {featureApiFunction},
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["{cacheKey}"] });
    },
  });
};
```
