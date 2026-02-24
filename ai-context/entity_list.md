# Entity List

> Source of truth for all Domain Entities.
> AI must check this file before creating new entities or DTOs to avoid duplication.
> Update this file whenever a new entity or significant property is added.

---

## Product

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | Guid | PK | Unique identifier |
| Sku | string | Max(50), Required, Unique | Stock Keeping Unit |
| Name | string | Max(255), Required | Product Name |
| Price | decimal | Required | Unit price |
| StockQuantity | int | Required | Available stock |
| CreatedAt | DateTimeOffset | Required, Auto | UTC timestamp |
| UpdatedAt | DateTimeOffset | Required, Auto | UTC timestamp |

---

## Cart

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | Guid | PK | Unique identifier |
| Items | List\<CartItem\> | One-to-Many | Items in the cart |
| CreatedAt | DateTimeOffset | Required, Auto | UTC timestamp |
| UpdatedAt | DateTimeOffset | Required, Auto | UTC timestamp |

---

## CartItem

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | Guid | PK | Unique identifier |
| CartId | Guid | FK → Cart.Id | Parent cart |
| ProductId | Guid | FK → Product.Id | Associated product |
| Quantity | int | Required | Number of items |
| CreatedAt | DateTimeOffset | Required, Auto | UTC timestamp |
| UpdatedAt | DateTimeOffset | Required, Auto | UTC timestamp |

---

> **Note for AI:** The starter project contains only these seed entities.
> Add new domain entities to this file each time they are created.
> Follow the same table format. Never abbreviate Constraints or Type columns.
