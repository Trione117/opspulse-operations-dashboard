# Roadmap

OpsPulse is currently a self-contained sample operations dashboard. The first version focuses on core workflow, data-backed pages, reporting exports, and testable business rules.

## Current workflow

1. Sites represent supported application environments.
2. Jobs represent recurring maintenance or validation tasks.
3. Incidents represent operational issues that need follow-up.
4. Timeline entries record investigation history.
5. Audit entries record important application actions.
6. Exports and API endpoints make the data easier to review outside the UI.

## Practical next improvements

- Add authentication and role-based access for support and administrator roles.
- Add input/view models for create and update workflows.
- Add filters for site, status, severity, and date range.
- Add EF Core migrations and optional SQL Server provider support.
- Add import support for external job history.
- Add deployment notes for IIS or container-based hosting.
