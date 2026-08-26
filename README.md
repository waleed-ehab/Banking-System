# Banking System — Clean Architecture (C# / .NET)

A console-based banking system built to practice real-world **Clean Architecture** — strict layer separation, dependency inversion, and business-rule-driven design, rather than a CRUD app with a UI bolted on.

> Built as a learning project to internalize Clean Architecture, authorization design, and domain-driven thinking — not just to make code that runs.

## Features

- **Authentication & role-based access** — Admin / Client roles, with a separate, mutable **permission system** (Read / Write / Execute flags) layered on top of role identity
- **Client management** — register, update, delete, search (Admin-only)
- **Account management** — create, delete, view accounts
- **Transactions** — deposit and withdraw, scoped strictly to the account owner
- **Transfers** — move funds between accounts, visible to **both** parties involved (source and destination), while remaining invisible to unrelated clients
- **Currency exchange** — live conversion between currencies, with rate management restricted to Admins
- **Permission management** — Admins can grant, revoke, and reset a client's permissions independently of their role
- **JSON-based persistence** — swappable behind repository interfaces, so storage can be replaced (e.g. with a real database) without touching business logic

## Architecture

Four layers, strict inward dependency direction (outer layers depend on inner ones, never the reverse):

```
Presentation   → console screens & menus, no business logic
Application    → use cases, orchestration, authorization rules
Domain         → entities, value objects, enums, business policies
Infrastructure → JSON repositories, implementing interfaces defined in Application
```

**Key design decisions worth noting:**

- **Authorization lives in the use case, never the UI.** Presentation-level checks exist only to avoid wasting the user's time (skip a menu option they can't use); the actual security boundary is a guard clause inside every use case, so it can't be bypassed by a different caller later.
- **Role vs. Permission are treated as separate concerns.** `UserRole` (Admin/Client) is fixed identity; `Permissions` (Read/Write/Execute, a `[Flags]` enum) is mutable, grantable capability. Some features gate on role, others on permission — deliberately, based on what each feature actually needs to protect.
- **Data filtering happens at the query, not in memory.** Repositories return only what a client is authorized to see, resolved by the use case — never "fetch everything, then filter" in application code.

## Tech Stack

- C# / .NET 9
- Console UI (no external UI framework)
- JSON file-based persistence (no external database dependency — runs anywhere)

## Getting Started

```bash
git clone https://github.com/waleed-ehab/Banking-System.git
cd Banking-System
dotnet build
dotnet run --project BankingSystem.Presentation
```

### Default Admin Login

```
Username: admin
Password: 1234
```

## Project Structure

```
BankingSystem.Domain/         Entities, value objects, enums, business policies
BankingSystem.Application/    Use cases, DTOs, repository interfaces
BankingSystem.Infrastructure/ JSON repository implementations
BankingSystem.Presentation/   Console screens, menus, input handling
```

## What This Project Demonstrates

- Applying SOLID and Clean Architecture principles in a non-trivial domain (not a tutorial app)
- Designing an authorization model from first principles (role vs. permission, defense-in-depth across layers)
- Making and documenting real architectural trade-offs — what belongs in which layer, and why
- Iteratively refactoring for readability and maintainability without changing behavior

## License

MIT
