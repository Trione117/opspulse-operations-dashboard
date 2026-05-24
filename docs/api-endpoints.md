# API Notes

OpsPulse exposes a small set of read-only endpoints for reporting and integration scenarios.

## JSON endpoints

- GET /api/health/summary
- GET /api/incidents/open
- GET /api/jobs/failed
- GET /api/audit/recent

## CSV exports

- GET /api/export/failed-jobs.csv
- GET /api/export/incidents.csv

## Current use

The Razor Pages UI is the primary interface. The API endpoints provide another way to review the same operational data without scraping the UI.

## Possible production use

In a production version, these endpoints could support reporting, automation, dashboards, or scheduled exports. Authentication and authorization would be added before exposing the endpoints outside a local/demo environment.
