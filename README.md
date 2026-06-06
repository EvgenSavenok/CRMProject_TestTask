# CRM TestTask — Contacts Management

A single-page ASP.NET web application for managing contacts with full CRUD functionality.

## Architecture

Layered (Clean) architecture with 4 projects:

| Project | Role |
|---|---|
| `Contacts.Domain` | Entities, error types, domain constants |
| `Contacts.Application` | DTOs, interfaces, use cases, validation, mapping |
| `Contacts.Infrastructure` | EF Core DbContext, repositories, middleware, extensions |
| `Contacts.Presentation` | ASP.NET controllers, static frontend (HTML + JS) |

**Stack:** ASP.NET Core 10 · Entity Framework Core · PostgreSQL (Npgsql) · FluentValidation · Bootstrap 5

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Running locally

### 1. Start the database

```bash
docker compose up -d
```

This starts a PostgreSQL 18 container on port **5435**.

### 2. Restore packages

```bash
dotnet restore
```

### 3. Add a migration (first time only)

```bash
dotnet ef migrations add InitialCreate \
  --project Contacts.Infrastructure \
  --startup-project Contacts.Presentation
```

> Migrations are applied automatically on startup — no need to run `dotnet ef database update`.

### 4. Run the application

```bash
dotnet run --project Contacts.Presentation
```

Open [http://localhost:5000](http://localhost:5000) in your browser.

Swagger UI is available at [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Connection string

Configured in `Contacts.Presentation/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "contactsConnection": "Host=localhost;Port=5435;Database=ContactsDb;Username=postgres;Password=1111"
}
```

## API Endpoints

| Method | URL | Description |
|---|---|---|
| `GET` | `/api/contacts/getAllContacts` | Get paginated list (`?PageNumber=1&PageSize=10`) |
| `GET` | `/api/contacts/getContactById/{id}` | Get single contact |
| `POST` | `/api/contacts/addContact` | Create contact |
| `PUT` | `/api/contacts/updateContact/{id}` | Update contact |
| `DELETE` | `/api/contacts/deleteContact/{id}` | Delete contact |

## Contact fields

| Field | Required | Rules                                 |
|---|---|---------------------------------------|
| `Name` | Yes | Max 1000 characters                   |
| `MobilePhone` | Yes | Max 20 characters, valid phone format |
| `JobTitle` | No | Max 100 characters                    |
| `BirthDate` | Yes | Must be in the past, after 1900-01-01 |
