# 4. On-demand AI translation with caching

Date: 2026-10-17 · Status: accepted

## Context
With content stored in one language (ADR 0003), users still need to read
international opportunities in their own language. Manual translation of every
record is not realistic.

## Decision
A "Translate" action calls an `ITranslationService` backed by an LLM. The result
is stored in an `OpportunityTranslation` table keyed by (opportunity, language)
together with a hash of the source text. A cached translation is reused while the
hash matches; editing the original invalidates it. The prompt treats the text as
data to translate, preserves names, dates, numbers and links, and requires a
JSON-only reply that is validated before it is saved.

## Consequences
+ Each opportunity is translated at most once per language.
+ The service sits behind an interface, so it can be replaced and mocked in tests.
- Translation quality depends on the model, so the UI marks it as automatic and
  offers the original text.
- Adds an external dependency, so it needs rate limiting, a retry on malformed
  output and a clear error when the provider fails.
