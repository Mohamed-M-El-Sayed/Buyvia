# Buyvia

**Buyvia** is an e-commerce backend built with a focus on clean architecture, maintainability, and scalable backend development. It provides core functionality for authentication, product catalog, shopping cart, orders, payments, caching, and administration.

> 🚧 Live demo coming soon.

## Tech Stack

| Category                      | Technologies                                                     |
| ----------------------------- | ---------------------------------------------------------------- |
| **Backend**                   | ASP.NET Core 8, Entity Framework Core, SQL Server                |
| **Architecture & Validation** | Clean Architecture, MediatR (CQRS), AutoMapper, FluentValidation |
| **Authentication & Security** | ASP.NET Core Identity, JWT, Rate Limiting, AuthEvo (OTP)         |
| **Background Jobs & Caching** | Hangfire, Redis (StackExchange.Redis)                            |
| **Payments & Email**          | Stripe, MailKit                                                  |
| **Logging**                   | Serilog                                                          |
| **API Documentation**         | Swagger / OpenAPI                                                |
| **Version Control**           | Git, GitHub                                                      |

## Features

* User registration and authentication with JWT access and refresh tokens
* Email confirmation and password recovery
* Phone number verification using OTP through AuthEvo
* Role-based authorization with Admin and Customer roles
* API rate limiting for request protection
* Product and hierarchical category management
* Product variants, options, images, discounts, and stock management
* Shopping cart management
* Coupon and discount management
* Order creation and order status management
* Delivery method management
* Stripe payment integration
* Product reviews
* Redis caching
* Background and scheduled jobs with Hangfire
* Email notifications
* Admin management for users, products, orders, and delivery methods
* API documentation with Swagger/OpenAPI

## Architecture

Buyvia follows **Clean Architecture** with **CQRS using MediatR**, separating application logic from infrastructure and API concerns.

Data access uses the **Repository and Unit of Work patterns**, with **Specification objects** for reusable and composable queries.

```text
Buyvia
├── OnlineStore.API
│   └── Controllers, Authentication, Authorization, Rate Limiting, Middleware, Configuration
├── OnlineStore.Application
│   └── CQRS Commands/Queries, DTOs, Validation, Mappings, Abstractions
├── OnlineStore.Domain
│   └── Entities, Enums, Core Business Rules
└── OnlineStore.Infrastructure
    └── EF Core, SQL Server, Redis, Stripe, MailKit, Hangfire, External Services
```

## Architecture Highlights

* Clean Architecture with clear separation of concerns
* CQRS implementation using MediatR
* Repository, Unit of Work, and Specification patterns
* JWT authentication with access and refresh tokens
* ASP.NET Core Identity with role-based authorization
* Redis caching and cache management
* Background and scheduled processing with Hangfire
* Stripe payment integration
* OTP phone verification using AuthEvo
* API rate limiting
* Structured logging with Serilog
* Request validation with FluentValidation
* Object mapping with AutoMapper

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* SQL Server (LocalDB, Express, or full instance)
* Redis (local instance or a cloud provider)
* A Stripe account with test mode keys
* An SMTP-compatible email provider
* An AuthEvo account with test mode credentials

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/Mohamed-M-El-Sayed/Buyvia.git
   cd Buyvia
   ```

2. **Configure application settings**

   Configure `appsettings.json` or `appsettings.Development.json` in `OnlineStore.API` with your own values:

   * `ConnectionStrings:DefaultConnection` — SQL Server connection string

   * `JwtSettings:Key` — JWT signing key

   * `JwtSettings:Issuer` — JWT issuer

   * `JwtSettings:Audience` — JWT audience

   * `Redis:ConnectionString` — Redis connection string

   * `Redis:InstanceName` — Redis instance name

   * `Stripe` — Stripe API configuration

   * `EmailSettings` — SMTP configuration

   * `AuthevoOptions` — AuthEvo OTP configuration

   > **Security:** Never commit real credentials, API keys, JWT signing keys, or connection strings to source control. Use environment variables or .NET User Secrets for local development.

3. **Apply migrations**

   ```bash
   dotnet ef database update --project OnlineStore.Infrastructure --startup-project OnlineStore.API
   ```

4. **Run the application**

   ```bash
   dotnet run --project OnlineStore.API
   ```

   On first run, the database is seeded with roles, categories, brands, and sample products.

5. **Explore the API**

   Once the application is running, Swagger UI is available at:

   ```text
   https://localhost:<port>/swagger
   ```

## Project Structure

```text
OnlineStore.Application/Features/
├── Auth/
├── Users/
├── Products/
├── ProductVariants/
├── VariantImages/
├── ProductOptions/
├── Carts/
├── Orders/
└── Reviews/
```

Each feature folder typically follows a CQRS-oriented structure:

```text
FeatureName/
├── Commands/
│   └── SomeCommand/
│       ├── SomeCommand.cs
│       ├── SomeCommandHandler.cs
│       └── SomeCommandValidator.cs
├── Queries/
├── Dtos/
├── Mappings/
└── Specifications/
```

