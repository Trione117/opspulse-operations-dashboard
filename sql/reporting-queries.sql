/*
OpsPulse reporting query examples.

The running demo uses SQLite through EF Core. These examples show the kind
of SQL/reporting questions behind the dashboard.

The intent is not to mirror a production database. The intent is to show how
support questions can be turned into repeatable queries.
*/

-- Jobs currently needing review
SELECT
    s.Name AS SiteName,
    j.JobName,
    j.JobType,
    j.LastRunUtc,
    j.LastStatus,
    j.FailureMessage
FROM MaintenanceJobs AS j
JOIN Sites AS s
    ON s.Id = j.SiteId
WHERE j.LastStatus IN ('Failed', 'Warning')
   OR j.LastRunUtc < datetime('now', '-24 hours')
ORDER BY j.LastRunUtc DESC;

-- Sites with pending validation work
SELECT
    s.Name AS SiteName,
    COUNT(*) AS PendingValidationTasks
FROM ValidationTasks AS v
JOIN Sites AS s
    ON s.Id = v.SiteId
WHERE v.IsComplete = 0
GROUP BY s.Name
ORDER BY PendingValidationTasks DESC;

-- Summary by site
SELECT
    s.Name AS SiteName,
    s.Environment,
    s.Status,
    COUNT(j.Id) AS JobCount,
    SUM(CASE WHEN j.LastStatus = 'Failed' THEN 1 ELSE 0 END) AS FailedJobCount,
    SUM(CASE WHEN v.IsComplete = 0 THEN 1 ELSE 0 END) AS PendingValidationCount
FROM Sites AS s
LEFT JOIN MaintenanceJobs AS j
    ON j.SiteId = s.Id
LEFT JOIN ValidationTasks AS v
    ON v.SiteId = s.Id
GROUP BY
    s.Name,
    s.Environment,
    s.Status
ORDER BY
    FailedJobCount DESC,
    PendingValidationCount DESC,
    s.Name;
