# API Relations

> Dependency map between features and slices.
> AI must read this file before modifying any feature to understand downstream impact.
> Update this file when adding new inter-feature dependencies or domain events.

---

## Features.Sales.Cart.AddItem / UpdateItem / RemoveItem / ClearCart

- **Depends On:**
  - `Features.Catalog.Products` (validate product existence and stock)
- **Triggers:** none
- **Invalidates FE Cache:** `['cart']`

---

## Features.Sales.Cart.Checkout

- **Depends On:**
  - `Features.Catalog.Products` (deduct stock)
- **Triggers:** none
- **FE Side Effect:** Clears cart. Invalidates `['cart']`, `['products']`

---

## Shared Infrastructure Dependencies

| Component | Used By | Notes |
|---|---|---|
| `AppDbContext` | All BE features | Scoped lifetime |
| `Axios instance` | All FE api.ts files | Singleton |
