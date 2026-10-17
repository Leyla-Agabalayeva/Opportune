# Opportune

> Find where you're needed.

Opportune is a platform that gathers volunteering opportunities in one place:
local projects in Azerbaijan and international programs abroad, including
those that cover accommodation. Search, compare, save and track opportunities,
and get personal recommendations with an explanation of why each one fits.

**Status:** in development. This repository is a portfolio project.

## The problem

Volunteering opportunities are scattered across Instagram, Telegram, WhatsApp,
organization websites and international program pages. Finding a suitable one
means following dozens of sources and watching deadlines by hand.

## Planned features

- Catalog with search and filters (country, category, age, language, dates,
  covered costs)
- Opportunity pages with requirements, deadlines and a verification badge
- Organizer cabinet and admin moderation
- Saved opportunities and an application tracker
- Reviews from past participants
- AI recommendations that explain the match, and on-demand AI translation
- Deadline reminders and notifications

## Planned stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core (Clean Architecture), EF Core, Identity + JWT |
| Database | PostgreSQL |
| Frontend | React, TypeScript, Vite |
| Quality | xUnit, Testcontainers, Playwright, GitHub Actions |
| Delivery | Docker, deployed with a public demo |

Architectural decisions are recorded in [`docs/adr`](docs/adr).

## License

[MIT](LICENSE)
