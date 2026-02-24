# API List

> Complete inventory of all API endpoints in the system.
> AI must check this file before creating new endpoints to avoid duplication.
> Update this file every time a new endpoint is added, modified, or deprecated.

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
