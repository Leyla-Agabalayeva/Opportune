# 3. Opportunity content stored in one language

Date: 2026-10-17 · Status: accepted

## Context
Opportunities come from different sources and arrive mostly in Azerbaijani or
English. The previous project stored translations for every entity in separate
tables, which complicated every query and every form.

## Decision
Each opportunity stores its title and description in the language of its source
and records that language (`OriginalLanguage`). The interface itself is
localized (AZ / EN / RU) with translation files on the frontend. Other content
languages are served through on-demand translation (see ADR 0004).

## Consequences
+ A much simpler data model and simpler forms for organizers.
+ No translation tables to keep in sync.
- Users may see content in a language other than their interface language until
  they request a translation.
- Search is language-aware only for the stored language.
