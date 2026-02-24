# API List

> Complete inventory of all API endpoints in the system.
> AI must check this file before creating new endpoints to avoid duplication.
> Update this file every time a new endpoint is added, modified, or deprecated.

---

## Feature: Auth

| Method | Route | Description | Input DTO | Output DTO | Auth Required |
|---|---|---|---|---|---|
| POST | /api/v1/auth/register | Register new user | RegisterRequest | RegisterResponse | No |
| POST | /api/v1/auth/login | Login & get tokens | LoginRequest | TokenResponse | No |
| POST | /api/v1/auth/refresh | Refresh access token | RefreshTokenRequest | TokenResponse | No |
| POST | /api/v1/auth/logout | Revoke refresh token | RefreshTokenRequest | — | Yes |
| PUT | /api/v1/auth/password | Change user password | ChangePasswordRequest | — | Yes |

### DTOs

**RegisterRequest**
```
Email: string
Password: string
FirstName: string
LastName: string
```

**LoginRequest**
```
Email: string
Password: string
```

**TokenResponse**
```
AccessToken: string   (JWT, 15min expiry)
RefreshToken: string  (UUID, 7d expiry)
ExpiresIn: int        (seconds)
```

**RegisterResponse**
```
UserId: Guid
Email: string
```

**ChangePasswordRequest**
```
CurrentPassword: string
NewPassword: string
ConfirmPassword: string
```

---

## Feature: User Management

| Method | Route | Description | Input DTO | Output DTO | Auth Required |
|---|---|---|---|---|---|
| GET | /api/v1/users/me | Get current user profile | — | UserProfileResponse | Yes |
| PUT | /api/v1/users/me | Update current user profile | UpdateProfileRequest | UserProfileResponse | Yes |
| GET | /api/v1/users/{id} | Get user by ID (Admin) | — | UserProfileResponse | Yes (CanManageUsers) |
| GET | /api/v1/users | List users with pagination | PaginationQuery | PagedResult\<UserProfileResponse\> | Yes (CanManageUsers) |
| DELETE | /api/v1/users/{id} | Deactivate user (soft delete) | — | — | Yes (CanManageUsers) |

### DTOs

**UserProfileResponse**
```
Id: Guid
Email: string
FirstName: string
LastName: string
Roles: string[]
CreatedAt: DateTimeOffset
```

**UpdateProfileRequest**
```
FirstName: string
LastName: string
```

**PaginationQuery** (shared)
```
Page: int       (default: 1)
PageSize: int   (default: 20, max: 100)
Search: string? (optional)
```

**PagedResult\<T\>** (shared)
```
Items: T[]
TotalCount: int
Page: int
PageSize: int
TotalPages: int
```

---

## Feature: Catalog

| Method | Route | Description | Input DTO | Output DTO | Auth Required |
|---|---|---|---|---|---|
| GET | /api/v1/products | List products with stock | — | ProductResponse[] | No |

### DTOs

**ProductResponse**
```
Id: Guid
Sku: string
Name: string
Price: decimal
StockQuantity: int
```

---

## Feature: Sales (Cart)

| Method | Route | Description | Input DTO | Output DTO | Auth Required |
|---|---|---|---|---|---|
| GET | /api/v1/cart/{cartId} | Get current cart | — | CartResponse | No |
| POST | /api/v1/cart/{cartId}/items | Add item to cart | AddItemRequest | { CartId: Guid } | No |
| PUT | /api/v1/cart/{cartId}/items/{productId} | Update item quantity | UpdateItemRequest | { CartId: Guid } | No |
| DELETE | /api/v1/cart/{cartId}/items/{productId} | Remove item | — | { CartId: Guid } | No |
| DELETE | /api/v1/cart/{cartId} | Clear cart | — | — | No |
| POST | /api/v1/cart/{cartId}/checkout | Checkout cart | — | — | No |

### DTOs

**CartResponse**
```
Id: Guid
Items: CartItemResponse[]
TotalBalance: decimal
```

**CartItemResponse**
```
ProductId: Guid
ProductName: string
ProductPrice: decimal
Quantity: int
TotalPrice: decimal
```

**AddToCartRequest**
```
ProductId: Guid
Quantity: int
```

**UpdateCartItemRequest**
```
Quantity: int
```

---

## Frontend API Hooks Map

> Maps BE endpoints to FE TanStack Query hooks.
> Located in: `frontend/src/features/[domain]/[feature]/hooks.ts`

| Hook | Method | Endpoint | Cache Key |
|---|---|---|---|
| `useRegister()` | POST | /auth/register | — (mutation) |
| `useLogin()` | POST | /auth/login | — (mutation) |
| `useChangePassword()` | PUT | /auth/password | — (mutation, invalidates all auth) |
| `useCurrentUser()` | GET | /users/me | `['user', 'me']` |
| `useUpdateProfile()` | PUT | /users/me | — (mutation, invalidates `['user','me']`) |
| `useUserList(params)` | GET | /users | `['users', params]` |
| `useUserById(id)` | GET | /users/{id} | `['users', id]` |
| `useProducts()` | GET | /products | `['products']` |
| `useCart()` | GET | /cart/{cartId} | `['cart', cartId]` |
| `useAddToCart()` | POST | /cart/{cartId}/items | — (mutation, invalidates `['cart', cartId]`) |
| `useUpdateCartItem()` | PUT | /cart/{cartId}/items/{productId} | — (mutation, invalidates `['cart', cartId]`) |
| `useRemoveCartItem()` | DELETE | /cart/{cartId}/items/{productId} | — (mutation, invalidates `['cart', cartId]`) |
| `useClearCart()` | DELETE | /cart/{cartId} | — (mutation, invalidates `['cart', cartId]`) |
| `useCheckout()` | POST | /cart/{cartId}/checkout | — (mutation, invalidates `['cart']`, `['products']`) |

---

> **Note for AI:** All new endpoints must be added to both the BE section AND the FE Hooks Map section.
> Deprecated endpoints must be marked with ~~strikethrough~~ and a deprecation date.
