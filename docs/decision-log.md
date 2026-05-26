# Design Notes

## Razor Pages

Razor Pages were used because the application is workflow-oriented. Each major screen maps cleanly to a page: dashboard, incidents, jobs, validation, and audit.

## SQLite

SQLite keeps the project easy to clone and run. The application still uses Entity Framework Core, so the data access pattern can be moved to SQL Server later.

## Service classes

Business rules are not kept only in the UI. Logic such as job health and incident priority is placed in service classes and covered by unit tests.

## Audit logging

The audit log records important user actions, such as creating incidents, adding timeline notes, and resolving incidents. This is a common requirement for internal support tools.

## API and CSV export

The project includes JSON endpoints and CSV exports so operational data can be consumed outside of the UI.

## Current limitations

- No authentication yet
- No role-based authorization yet
- No SQL Server deployment profile yet
- Health summaries are simulated
- Edit/delete workflows are intentionally limited in this version
