# Entity List

> Source of truth for all Domain Entities.
> AI must check this file before creating new entities or DTOs to avoid duplication.
> Update this file whenever a new entity or significant property is added.

---

## User

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | Guid | PK | Unique identifier |
| Email | string | Max(255), Required, Unique | Login identity |
| PasswordHash | string | Required | Bcrypt hashed |
| FirstName | string | Max(100), Required | — |
| LastName | string | Max(100), Required | — |
| IsActive | bool | Default(true) | Soft-delete flag |
| Roles | List\<Role\> | Many-to-Many | Permission roles |
| CreatedAt | DateTimeOffset | Required, Auto | UTC timestamp |
| UpdatedAt | DateTimeOffset | Required, Auto | UTC timestamp |

**Indexes:** Email (Unique)
**Domain Events:** UserRegisteredEvent, UserDeactivatedEvent

---

## Role

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | int | PK, Identity | — |
| Name | string | Max(50), Unique | e.g. Admin, User |
| Permissions | List\<string\> | JSON column | Permission claims |

---

## RefreshToken

| Property | Type | Constraints | Description |
|---|---|---|---|
| Id | Guid | PK | — |
| UserId | Guid | FK → User.Id | Owner |
| Token | string | Required, Unique | Hashed token |
| ExpiresAt | DateTimeOffset | Required | Expiry time |
| IsRevoked | bool | Default(false) | Revocation flag |
| CreatedAt | DateTimeOffset | Required, Auto | — |

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
| UserId | Guid | FK → User.Id | Owner |
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
