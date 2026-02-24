# API Relations

> Dependency map between features and slices.
> AI must read this file before modifying any feature to understand downstream impact.
> Update this file when adding new inter-feature dependencies or domain events.

---

## Features.Auth.Register

- **Depends On:** none
- **Triggers:**
  - `Domain.Events.UserRegisteredEvent` → Send welcome email (future)
- **Invalidates FE Cache:** none (mutation only)

---

## Features.Auth.Login

- **Depends On:**
  - `Features.Users` (verify IsActive, load roles)
- **Triggers:**
  - Creates `RefreshToken` record in DB
- **Invalidates FE Cache:** none

---

## Features.Auth.Refresh

- **Depends On:**
  - `RefreshToken` table (validate & rotate token)
  - `Features.Users.GetProfile` (re-issue claims)
- **Side Effect:** old RefreshToken is revoked, new one issued

---

## Features.Auth.Logout

- **Depends On:**
  - `RefreshToken` table (mark IsRevoked = true)
- **Triggers:** none
- **Invalidates FE Cache:** all user-related keys

---

## Features.Auth.ChangePassword

- **Depends On:**
  - `Features.Users` (verify matching current password)
  - `RefreshToken` table (revocation of ALL existing user tokens)
- **Triggers:** none
- **FE Side Effect:** Navigates to `/login`, invalidates all auth state


---

## Features.Users.GetProfile (`/users/me`)

- **Depends On:** Auth middleware (must be authenticated)
- **Triggers:** none
- **FE Side Effect:** Cached at `['user', 'me']` — invalidated by `useUpdateProfile` and `useLogout`

---

## Features.Users.UpdateProfile

- **Depends On:**
  - `Features.Users.GetProfile` (ownership check)
- **Triggers:** none
- **Invalidates FE Cache:** `['user', 'me']`

---

## Features.Users.ListUsers / GetById (Admin)

- **Depends On:**
  - Policy: `CanManageUsers`
- **Triggers:** none
- **FE Side Effect:** Cached at `['users', params]` / `['users', id]`

---

## Features.Users.DeactivateUser (Admin)

- **Depends On:**
  - Policy: `CanManageUsers`
  - `Features.Auth` — active sessions are NOT automatically revoked (refresh token expiry handles this)
- **Triggers:**
  - `Domain.Events.UserDeactivatedEvent` → (future: notify user via email)
- **Invalidates FE Cache:** `['users']` (list), `['users', id]` (specific)

---

## Features.Sales.Cart.AddItem / UpdateItem / RemoveItem / ClearCart

- **Depends On:**
  - Auth middleware (must be authenticated)
  - `Features.Catalog.Products` (validate product existence and stock)
- **Triggers:** none
- **Invalidates FE Cache:** `['cart']`

---

## Features.Sales.Cart.Checkout

- **Depends On:**
  - Auth middleware (must be authenticated)
  - `Features.Catalog.Products` (deduct stock)
- **Triggers:** none
- **FE Side Effect:** Clears cart. Invalidates `['cart']`, `['products']`

---

## Shared Infrastructure Dependencies

| Component | Used By | Notes |
|---|---|---|
| `AppDbContext` | All BE features | Scoped lifetime |
| `IPasswordHasher` | Auth.Register, Auth.Login | Singleton |
| `IJwtTokenService` | Auth.Login, Auth.Refresh | Singleton |
| `Axios instance` | All FE api.ts files | Singleton, includes auth interceptor |
| `useAuthStore` (Zustand) | All FE features that need user state | Global store |
