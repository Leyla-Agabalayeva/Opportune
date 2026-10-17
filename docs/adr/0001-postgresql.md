# 1. PostgreSQL as the database

Date: 2026-10-17 · Status: accepted

## Context
The catalog needs full-text search over titles and descriptions, relational
integrity (users, organizations, opportunities, reviews) and a database that can
be hosted for free for a public demo. The author already knows SQL Server.

## Decision
Use PostgreSQL through EF Core (Npgsql provider). Full-text search uses
PostgreSQL's built-in `tsvector` support. Locally it runs in Docker.

## Consequences
+ Free managed hosting options for the public demo.
+ Built-in full-text search, with a path to vector search (`pgvector`) if
  semantic matching is added later.
+ Shows breadth beyond SQL Server.
- A new database to learn; some EF Core features differ from the SQL Server
  provider.
- Local development needs Docker or a local PostgreSQL install.
