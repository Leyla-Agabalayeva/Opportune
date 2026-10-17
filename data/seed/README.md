# Seed data

Real opportunities used to fill the catalog. Collect them in
`opportunities.template.csv` (copy it to `opportunities.csv` and fill it in).
Only add opportunities you have checked on the official page, and always keep the
link, because every record shows its verification level and last check date.

| Column | Meaning |
|---|---|
| `title` | Name of the opportunity, in the source language |
| `description` | Short description, in the source language |
| `original_language` | `az`, `en` or `ru` |
| `organization` | Organizing body |
| `category` | e.g. environment, education, culture, social, events |
| `scope` | `Local` or `International` |
| `country` / `city` | Where it takes place (`city` may be empty) |
| `is_online` | `true` or `false` |
| `start_date` / `end_date` | `YYYY-MM-DD`, may be empty |
| `application_deadline` | `YYYY-MM-DD` |
| `min_age` / `max_age` | Numbers, may be empty |
| `duration_days` | Number of days, may be empty |
| `required_languages` | e.g. `en:B1;az:native` |
| `accommodation_covered` | `true` or `false` |
| `meals_covered` | `true` or `false` |
| `travel_covered` | `true` or `false` |
| `participation_fee` | Amount, `0` if free |
| `currency` | e.g. `EUR`, `AZN` |
| `apply_url` | Official application page |
| `source_url` | Page the data was taken from |
| `verification` | `Verified` (checked by the team) or `External` (from a source, confirm with the organizer) |
| `last_checked` | `YYYY-MM-DD` |
