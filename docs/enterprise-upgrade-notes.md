# Project Notes

This project was built as a small internal operations tool.

The first version focused on these areas:

- database-backed pages
- incident tracking
- audit logging
- validation checks
- API endpoints
- CSV exports
- unit tests
- documentation

## Current workflow

1. Sites represent supported environments.
2. Jobs represent recurring maintenance or validation tasks.
3. Incidents represent operational issues that need follow-up.
4. Timeline entries record the investigation history.
5. Audit entries record important application actions.
6. Exports and API endpoints make the data easier to review outside the UI.

## Future work

- Add authentication
- Add role-based access
- Add SQL Server provider support
- Add more complete CRUD workflows
- Add import support for external job history
- Add real health check integrations
