# Architecture

OpsPulse is a database-backed ASP.NET Core web application.

## Main parts

## Razor Pages UI

The UI handles the page-based workflows:

- Dashboard
- Sites
- Jobs
- Incidents
- Incident details
- Validation
- Audit log
- Exports/API

## Data layer

Entity Framework Core is used for database access. The demo uses SQLite so the project can run without installing SQL Server.

Main entities:

- Site
- MaintenanceJob
- Incident
- IncidentTimelineEntry
- ValidationTaskItem
- AuditLogEntry

## Services

Business logic is kept in service classes where practical.

- DashboardService builds the dashboard model.
- JobHealthService classifies job health.
- IncidentPriorityService calculates a basic incident priority score.
- AuditService writes audit log entries.

## API controller

OpsPulseApiController exposes JSON and CSV endpoints for reporting and integration.

## Testing

The test project covers the small business rules that are easiest to validate automatically, such as job health classification and incident priority scoring.
