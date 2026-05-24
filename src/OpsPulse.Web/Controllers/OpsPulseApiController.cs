using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;

namespace OpsPulse.Web.Controllers;

[ApiController]
[Route("api")]
public class OpsPulseApiController : ControllerBase
{
    private readonly AppDbContext _db;

    public OpsPulseApiController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("health/summary")]
    public async Task<IActionResult> GetHealthSummary()
    {
        var cutoff = DateTime.UtcNow.AddHours(-24);

        return Ok(new
        {
            generatedAtUtc = DateTime.UtcNow,
            totalSites = await _db.Sites.CountAsync(),
            failedJobsLast24Hours = await _db.MaintenanceJobs.CountAsync(j =>
                j.LastStatus == "Failed" && j.LastRunUtc >= cutoff),
            staleJobs = await _db.MaintenanceJobs.CountAsync(j => j.LastRunUtc < cutoff),
            openIncidents = await _db.Incidents.CountAsync(i => i.Status != "Resolved"),
            pendingValidationTasks = await _db.ValidationTasks.CountAsync(t => !t.IsComplete)
        });
    }

    [HttpGet("incidents/open")]
    public async Task<IActionResult> GetOpenIncidents()
    {
        var rows = await _db.Incidents
            .Include(i => i.Site)
            .Where(i => i.Status != "Resolved")
            .OrderByDescending(i => i.DetectedAtUtc)
            .Select(i => new
            {
                i.Id,
                i.Title,
                Site = i.Site == null ? "" : i.Site.Name,
                i.Severity,
                i.Status,
                i.DetectedAtUtc
            })
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("jobs/failed")]
    public async Task<IActionResult> GetFailedJobs()
    {
        var rows = await _db.MaintenanceJobs
            .Include(j => j.Site)
            .Where(j => j.LastStatus == "Failed" || j.LastStatus == "Warning")
            .OrderByDescending(j => j.LastRunUtc)
            .Select(j => new
            {
                j.Id,
                Site = j.Site == null ? "" : j.Site.Name,
                j.JobName,
                j.JobType,
                j.LastRunUtc,
                j.LastStatus,
                j.FailureMessage
            })
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("audit/recent")]
    public async Task<IActionResult> GetRecentAudit()
    {
        var rows = await _db.AuditLogs
            .OrderByDescending(a => a.TimestampUtc)
            .Take(25)
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("export/failed-jobs.csv")]
    public async Task<IActionResult> ExportFailedJobs()
    {
        var jobs = await _db.MaintenanceJobs
            .Include(j => j.Site)
            .Where(j => j.LastStatus == "Failed" || j.LastStatus == "Warning")
            .OrderByDescending(j => j.LastRunUtc)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Site,JobName,JobType,LastRunUtc,LastStatus,FailureMessage");

        foreach (var job in jobs)
        {
            csv.AppendLine(string.Join(",",
                Csv(job.Site?.Name),
                Csv(job.JobName),
                Csv(job.JobType),
                Csv(job.LastRunUtc.ToString("O")),
                Csv(job.LastStatus),
                Csv(job.FailureMessage)));
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "failed-jobs.csv");
    }

    [HttpGet("export/incidents.csv")]
    public async Task<IActionResult> ExportIncidents()
    {
        var incidents = await _db.Incidents
            .Include(i => i.Site)
            .OrderByDescending(i => i.DetectedAtUtc)
            .ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Site,Title,Severity,Status,DetectedAtUtc,ResolvedAtUtc,RootCause,ResolutionSummary");

        foreach (var incident in incidents)
        {
            csv.AppendLine(string.Join(",",
                Csv(incident.Site?.Name),
                Csv(incident.Title),
                Csv(incident.Severity),
                Csv(incident.Status),
                Csv(incident.DetectedAtUtc.ToString("O")),
                Csv(incident.ResolvedAtUtc?.ToString("O")),
                Csv(incident.RootCause),
                Csv(incident.ResolutionSummary)));
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "incidents.csv");
    }

    private static string Csv(string? value)
    {
        value ??= string.Empty;
        value = value.Replace("\"", "\"\"");

        if (value.Length > 0 && "=+-@\t\r\n".Contains(value[0]))
        {
            value = "'" + value;
        }

        return $"\"{value}\"";
    }
}
