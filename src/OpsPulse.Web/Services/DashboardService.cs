using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Services;

public class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var cutoff = now.AddHours(-24);

        var jobs = await _db.MaintenanceJobs.Include(j => j.Site).OrderByDescending(j => j.LastRunUtc).ToListAsync();
        var sites = await _db.Sites.ToListAsync();

        var recentProblemJobs = jobs
            .Where(j => j.LastStatus == "Failed" || j.LastStatus == "Warning" || j.LastRunUtc < cutoff)
            .Take(10)
            .ToList();

        var openIncidents = await _db.Incidents
            .Include(i => i.Site)
            .Where(i => i.Status != "Resolved")
            .OrderByDescending(i => i.DetectedAtUtc)
            .Take(10)
            .ToListAsync();

        return new DashboardViewModel
        {
            TotalSites = sites.Count,
            HealthySites = sites.Count(s => s.Status == "Healthy"),
            WarningSites = sites.Count(s => s.Status == "Warning"),
            CriticalSites = sites.Count(s => s.Status == "Critical"),
            TotalJobs = jobs.Count,
            FailedJobsLast24Hours = jobs.Count(j => j.LastStatus == "Failed" && j.LastRunUtc >= cutoff),
            JobsOlderThan24Hours = jobs.Count(j => j.LastRunUtc < cutoff),
            PendingValidationTasks = await _db.ValidationTasks.CountAsync(t => !t.IsComplete),
            OpenIncidents = await _db.Incidents.CountAsync(i => i.Status != "Resolved"),
            HighSeverityOpenIncidents = await _db.Incidents.CountAsync(i => i.Status != "Resolved" && (i.Severity == "High" || i.Severity == "Critical")),
            RecentProblemJobs = recentProblemJobs,
            RecentOpenIncidents = openIncidents
        };
    }
}

public class DashboardViewModel
{
    public int TotalSites { get; set; }
    public int HealthySites { get; set; }
    public int WarningSites { get; set; }
    public int CriticalSites { get; set; }
    public int TotalJobs { get; set; }
    public int FailedJobsLast24Hours { get; set; }
    public int JobsOlderThan24Hours { get; set; }
    public int PendingValidationTasks { get; set; }
    public int OpenIncidents { get; set; }
    public int HighSeverityOpenIncidents { get; set; }
    public List<MaintenanceJob> RecentProblemJobs { get; set; } = new();
    public List<Incident> RecentOpenIncidents { get; set; } = new();
}
