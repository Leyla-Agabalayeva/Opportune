# Architecture Decision Records

Short notes that explain **why** a decision was made, so the reasoning does not
live only in someone's head. One file per decision, numbered in order.

| # | Decision | Status |
|---|---|---|
| [0001](0001-postgresql.md) | PostgreSQL as the database | Accepted |
| [0002](0002-react-typescript.md) | React + TypeScript for the frontend | Accepted |
| [0003](0003-single-language-content.md) | Opportunity content stored in one language | Accepted |
| [0004](0004-ai-translation-on-demand.md) | On-demand AI translation with caching | Accepted |
| [0005](0005-backend-architecture.md) | Clean Architecture on ASP.NET Core | Accepted |

## Template

```markdown
# N. Title

Date: YYYY-MM-DD · Status: proposed | accepted | superseded

## Context
What problem or constraint forces a decision?

## Decision
What we chose.

## Consequences
+ What gets better
- What gets harder or riskier
```

A decision is never edited away: if it changes, add a new record and mark the
old one as superseded.
