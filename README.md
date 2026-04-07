# Ecommerce Website Backend

An **ASP.NET Core Web API** backend for an e-commerce application built with **.NET 10** and **C# 14**. Currently in its early phase, focusing on the **Product Category** feature with full CRUD operations, validation, global exception handling, audit logging, and CORS configuration.


## Technology Stack

| Concern        | Technology                  |
|----------------|-----------------------------|
| Runtime        | .NET 10                     |
| Language       | C# 14                       |
| Web Framework  | ASP.NET Core Web API        |
| ORM            | Entity Framework Core       |
| Database       | SQLite                      |
| API Docs       | OpenAPI (built-in .NET 9+)  |

---

## Project Structure

```text
Ecommerce-Website-Backend/
│
├── Program.cs                               # App entry point & DI registration
│
├── Configuration/
│   └── CorsOptions.cs                       # Strongly-typed CORS config model
│
├── Common/
│   └── Constants/
│       └── ValidationConstants.cs           # Shared validation length constants
│
├── Data/
│   ├── AppDbContext.cs                       # EF Core DbContext with audit logging
│   └── Entities/
│       ├── BaseEntity.cs                    # Shared base (Id property)
│       ├── ProductCategoryEntity.cs         # ProductCategory table entity
│       └── AuditLogsEntity.cs               # AuditLogs table entity
│
├── Models/
│   ├── Request/
│   │   └── ProductCategoryRequest.cs        # Validated inbound DTO
│   └── Response/
│       ├── ApiErrorResponse.cs              # Standardised error shape
│       ├── CheckResponse.cs                 # Health-check response shape
│       └── ProductCategoryResponse.cs       # Outbound category DTO
│
├── Services/
│   └── ProductCategoryService.cs            # Business logic for categories
│
├── Controllers/
│   └── ProductCategoryController.cs         # HTTP endpoints for categories
│
├── Middleware/
│   └── GlobalExceptionHandler.cs            # Centralised exception-to-response mapping
│
└── Extensions/
    ├── ServiceExtensions.cs                 # Validation error handling setup
    └── CorsExtensions.cs                    # CORS policy DI registration
```


---

## Layers in Detail

### Configuration

**`CorsOptions`** — Strongly-typed model bound to the `Cors` section of `appsettings.json`.

| Property          | Description                        |
|-------------------|------------------------------------|
| `AllowedOrigins`  | Array of permitted client origins  |
| `PolicyName`      | Named CORS policy identifier       |

---

### Data Layer

#### `BaseEntity`
Base class for all entities. Provides a single auto-incremented `Id` primary key.

#### `ProductCategoryEntity`

| Column                        | Type     | Constraint                       |
|-------------------------------|----------|----------------------------------|
| `Id`                          | `int`    | PK, auto-increment               |
| `ProductCategoryName`         | `string` | Unique index · max 15 chars      |
| `ProductCategoryDescription`  | `string` | max 50 chars                     |

#### `AuditLogsEntity`

Auto-populated on every `SaveChangesAsync` call.

| Column       | Description                                           |
|--------------|-------------------------------------------------------|
| `EntityName` | Name of the affected entity class                     |
| `EntityId`   | Primary key value of the affected row                 |
| `Action`     | `"Created"`, `"Updated"`, or `"Deleted"`              |
| `OldValues`  | JSON snapshot of original values (updates only)       |
| `NewValues`  | JSON snapshot of new values (create & update)         |
| `ChangedBy`  | `null` — reserved for user identity after auth        |
| `ChangedAt`  | UTC timestamp                                         |

#### `AppDbContext`

- Registers `ProductCategories` and `AuditLogs` `DbSet`s
- Applies unique index and max-length constraints via `OnModelCreating`
- **Overrides `SaveChangesAsync`** to automatically write audit entries on every data mutation

---

### Models

#### Request DTOs

**`ProductCategoryRequest`** — used for both `POST` and `PUT`.

| Property                      | Validation                    |
|-------------------------------|-------------------------------|
| `ProductCategoryName`         | Required · Max 15 chars       |
| `ProductCategoryDescription`  | Optional · Max 50 chars       |

#### Response DTOs

**`ProductCategoryResponse`** — C# `record` returned from all category endpoints, mapped from entity via `FromEntity()`.

**`ApiErrorResponse`** — Unified error shape used by both the global exception handler and model validation.

| Property  | Description                                  |
|-----------|----------------------------------------------|
| `Status`  | HTTP status code                             |
| `Title`   | Short error title                            |
| `Detail`  | Exception message (optional)                 |
| `Errors`  | Field-level validation errors (optional)     |

**`CheckResponse`** — Used for health-check endpoints.

| Property    | Description              |
|-------------|--------------------------|
| `Status`    | e.g. `"Healthy"`         |
| `Timestamp` | UTC time of the check    |

---

### Services

**`ProductCategoryService`** — Scoped service containing all category business logic.

| Method              | Description                      | Business Rules                                        |
|---------------------|----------------------------------|-------------------------------------------------------|
| `GetAllAsync()`     | Returns all categories           | —                                                     |
| `GetByIdAsync(id)`  | Returns a single category        | Throws `KeyNotFoundException` if not found            |
| `CreateAsync()`     | Creates a new category           | Throws `InvalidOperationException` on duplicate name  |
| `UpdateAsync()`     | Updates an existing category     | Duplicate name check only if name actually changed    |
| `DeleteAsync(id)`   | Permanently deletes a category   | Throws `KeyNotFoundException` if not found            |

---

### Controllers

**`ProductCategoryController`** — Routes under `api/productcategory`, uses primary constructor injection.

---

### Middleware & Extensions

| Class                                          | Role                                                                  |
|------------------------------------------------|-----------------------------------------------------------------------|
| `GlobalExceptionHandler`                       | Maps unhandled exceptions to a structured `ApiErrorResponse`          |
| `ServiceExtensions.AddValidationErrorHandling` | Returns `ApiErrorResponse` instead of default ProblemDetails on 400s  |
| `CorsExtensions.AddCorsPolicies`               | Reads CORS config and registers the named policy                      |

---

## API Endpoints

**Base URL:** `https://<host>/api/productcategory`

| Method   | Path    | Request Body             | Success Response                      |
|----------|---------|--------------------------|---------------------------------------|
| `GET`    | `/`     | —                        | `200 OK` — `ProductCategoryResponse[]`|
| `GET`    | `/{id}` | —                        | `200 OK` — `ProductCategoryResponse`  |
| `POST`   | `/`     | `ProductCategoryRequest` | `201 Created` — `ProductCategoryResponse` |
| `PUT`    | `/{id}` | `ProductCategoryRequest` | `200 OK` — `ProductCategoryResponse`  |
| `DELETE` | `/{id}` | —                        | `204 No Content`                      |

---

## Error Handling

All errors return a unified `ApiErrorResponse`:

`{ "status": 404, "title": "Not Found", "detail": "Category with Id 99 not found." }`


Validation errors include a field-level `errors` map:

`{ "status": 400, "title": "Validation Failed", "errors": { "ProductCategoryName": ["Category name is required"] } }`


| Exception                   | HTTP Status         |
|-----------------------------|---------------------|
| `KeyNotFoundException`      | `404 Not Found`     |
| `InvalidOperationException` | `409 Conflict`      |
| Any other                   | `500 Internal Server Error` |

---

## Validation

Constraints are centralised in `ValidationConstants` to keep the API and database in sync.

| Constant                          | Value |
|-----------------------------------|-------|
| `ProductCategoryNameMaxLength`    | `15`  |
| `ProductCategoryDescriptionMaxLength` | `50`  |

---

## Audit Logging

Every create, update, and delete operation is **automatically** recorded. The `AppDbContext.SaveChangesAsync` override inspects `ChangeTracker` entries and writes audit log rows after the main save completes. The `ChangedBy` field is reserved for user identity once authentication is implemented.

---

## CORS Policy

Configure allowed origins in `appsettings.json` under the `Cors` key:

`{ "Cors": { "PolicyName": "AllowFrontend", "AllowedOrigins": ["https://your-frontend.com"] } }`


The policy allows **any method**, **any header**, **credentials**, and supports **wildcard subdomains**.

---

## Getting Started

1. **Clone the repository**

`git clone https://github.com/vncebrmjo/Ecommerce-Website-Backend.git cd Ecommerce-Website-Backend`


2. **Configure secrets** —create `secrets.json` at the project root (already git-ignored):

`
2. **Configure secrets** —create `secrets.json` at the project root (already git-ignored):`


3. **Apply migrations**

`dotnet ef database update`

4. **Run the API**

`dotnet run`

5. **Explore endpoints** — navigate to `https://localhost:<port>/openapi` in your browser (Development only).




