# Working agreement

How changes are made in this repository.

## Branches
- `main` is always working. Nothing is committed to it directly after the first
  commit; every change arrives through a pull request.
- One branch per piece of work, named `type/short-description`:
  `feat/opportunity-catalog`, `fix/token-refresh`, `docs/readme`,
  `chore/ci-pipeline`, `test/recommendation-service`.

## Commits
Small and meaningful, in the form `type: what and why`:

| Type | Use for |
|---|---|
| `feat` | a new capability |
| `fix` | a bug fix |
| `test` | adding or changing tests |
| `docs` | documentation only |
| `chore` | tooling, dependencies, configuration |
| `refactor` | restructuring without behavior change |

## Pull requests
- Describe what changed and how it was checked.
- CI (build, lint, tests) must be green before merging.
- Merge with a merge commit so the branch history stays visible.

## Definition of done for a feature
- [ ] Works in the UI, including loading, empty and error states
- [ ] Covered by tests
- [ ] Described in the OpenAPI document
- [ ] No secrets in the diff
- [ ] Merged through a pull request
