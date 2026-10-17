# 5. Clean Architecture on ASP.NET Core

Date: 2026-10-17 · Status: accepted

## Context
The backend must be easy to test, must keep external services (AI, email,
storage, database) replaceable, and the author already has a working structure
from a previous project.

## Decision
A modular monolith on ASP.NET Core with the layers Domain, Contracts,
Application, Persistence, Infrastructure and WebApi, plus a Tests project.
Dependencies point inward: Application depends only on interfaces, and
Infrastructure implements them. Identity with JWT access tokens and a refresh
token in an httpOnly cookie. Validation with FluentValidation. Errors are mapped
to HTTP responses in one middleware. The target is .NET 10, which is installed
on the development machine.

## Consequences
+ Business logic is tested without a database or network by mocking interfaces.
+ AI, email and storage providers can be swapped without touching the core.
- More projects and indirection than a single-project API.
- Microservices are deliberately out of scope; the structure leaves room to split
  later if needed.
