# OpsPulse

OpsPulse is a sample ASP.NET Core operations dashboard that models a common internal support workflow: tracking supported sites, recurring maintenance jobs, open incidents, validation tasks, audit activity, and exportable reporting data.

The app uses local SQLite and seeded sample data so it can be cloned and run without access to any internal system.

## What this project demonstrates

- Building a data-backed ASP.NET Core Razor Pages application
- Modeling operational workflows with Entity Framework Core
- Exposing simple JSON API endpoints and CSV exports
- Writing unit and controller-level tests for core behavior
- Using GitHub Actions to build and test the solution

## Features

- Operations dashboard
- Site/server-set tracking
- Maintenance job tracking
- Incident register
- Incident details and timeline notes
- Incident resolution workflow
- Validation checklist
- Audit log
- Synthetic health check results
- CSV exports
- JSON API endpoints
- SQL reporting examples
- xUnit tests
- GitHub Actions build/test workflow

## Technology

- C#
- ASP.NET Core Razor Pages
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Bootstrap
- xUnit
- GitHub Actions

## Running the project from GitHub

### Prerequisites

Install these first:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)

SQL Server is not required. The app uses a local SQLite database and creates it automatically when the app starts.

### Clone with Git

Open PowerShell and run:

```powershell
git clone https://github.com/Trione117/opspulse-operations-dashboard.git
cd opspulse-operations-dashboard
dotnet test
cd src\OpsPulse.Web
dotnet run
```

After `dotnet run` starts, open the localhost URL shown in the terminal.

## API examples

After starting the app, these endpoints can be opened in the browser:

- `/api/health/summary`
- `/api/incidents/open`
- `/api/jobs/failed`
- `/api/audit/recent`
- `/api/export/failed-jobs.csv`
- `/api/export/incidents.csv`

## Screenshots

### Dashboard
![Dashboard](docs/screenshots/01-dashboard.png)

### Sites
![Sites](docs/screenshots/02-sites.png)

### Jobs
![Jobs](docs/screenshots/03-jobs.png)

### Incidents
![Incidents](docs/screenshots/04-incidents.png)

### Incident Details
![Incident Details](docs/screenshots/05-incident-details.png)

### Validation
![Validation](docs/screenshots/06-validation.png)

### Audit Log
![Audit Log](docs/screenshots/07-audit-log.png)

### Exports and API
![Exports and API](docs/screenshots/08-exports-api.png)

## Repository layout

- `src/OpsPulse.Web` - web application
- `tests/OpsPulse.Tests` - unit and controller-level tests
- `docs` - design notes, API notes, backlog, and manual test checklist
- `sql` - reporting query examples
- `.github/workflows` - GitHub Actions build/test workflow

## Design decisions

- Razor Pages keeps each workflow screen close to its page handlers.
- SQLite keeps the project self-contained for local review.
- EF Core keeps the data access pattern portable if the app later moves to SQL Server.
- Seeded sample data makes the dashboard usable immediately after startup.

## Sample data notice

OpsPulse is intentionally self-contained. It uses seeded sample data and a local SQLite database. It does not connect to production systems, internal networks, live services, or real operational logs.

## Future improvements

The current version focuses on core workflow and data-backed UI behavior. Next improvements would be:

- Authentication and role-based access for different support roles
- Input models for create/update workflows
- Filtering by site, status, and date range
- EF Core migrations and optional SQL Server provider support
- Import workflow for external job history
