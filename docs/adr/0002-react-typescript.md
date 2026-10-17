# 2. React + TypeScript for the frontend

Date: 2026-10-17 · Status: accepted

## Context
The previous project (Learnix) used a vanilla JavaScript frontend. The goal here
is a portfolio project that reflects what the job market asks for, with a
type-safe contract to the API.

## Decision
Build the frontend with React and TypeScript on Vite. Server state is handled by
TanStack Query, forms by React Hook Form with Zod, routing by React Router. The
TypeScript API client is generated from the backend's OpenAPI document.

## Consequences
+ A widely requested skill set; component-based, testable UI.
+ Generated API types cannot drift from the backend.
- Longer learning curve and setup than plain JavaScript.
- Node.js tooling becomes part of the build and the CI pipeline.
