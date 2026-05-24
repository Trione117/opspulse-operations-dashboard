using OpsPulse.Web.Models;

namespace OpsPulse.Web.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        EnsureSites(db);
        EnsureJobs(db);
        EnsureValidation(db);
        EnsureIncidents(db);
        EnsureAudit(db);
    }

    private static void EnsureSites(AppDbContext db)
    {
        if (db.Sites.Any()) return;

        db.Sites.AddRange(
            new Site { Name = "North Hangar", AppServer = "APP-NORTH-01", DatabaseServer = "DB-NORTH-01", Environment = "Production", Status = "Warning" },
            new Site { Name = "Training Annex", AppServer = "APP-TRAINING-01", DatabaseServer = "DB-TRAINING-01", Environment = "Production", Status = "Healthy" },
            new Site { Name = "Maintenance Bay", AppServer = "APP-MAINT-01", DatabaseServer = "DB-MAINT-01", Environment = "Production", Status = "Critical" },
            new Site { Name = "Dev Sandbox", AppServer = "APP-DEV-01", DatabaseServer = "DB-DEV-01", Environment = "Development", Status = "Healthy" }
        );

        db.SaveChanges();
    }

    private static void EnsureJobs(AppDbContext db)
    {
        if (db.MaintenanceJobs.Any()) return;

        var north = db.Sites.First(s => s.Name == "North Hangar");
        var training = db.Sites.First(s => s.Name == "Training Annex");
        var maint = db.Sites.First(s => s.Name == "Maintenance Bay");
        var dev = db.Sites.First(s => s.Name == "Dev Sandbox");

        db.MaintenanceJobs.AddRange(
            new MaintenanceJob { SiteId = north.Id, JobName = "Full Database Backup", JobType = "Backup", LastRunUtc = DateTime.UtcNow.AddHours(-7), LastStatus = "Succeeded" },
            new MaintenanceJob { SiteId = north.Id, JobName = "Copy Backup to Offsite Share", JobType = "Copy Offsite", LastRunUtc = DateTime.UtcNow.AddHours(-6), LastStatus = "Failed", FailureMessage = "Sample failure: destination share unavailable during copy window." },
            new MaintenanceJob { SiteId = training.Id, JobName = "Nightly Integrity Check", JobType = "Validation", LastRunUtc = DateTime.UtcNow.AddHours(-4), LastStatus = "Succeeded" },
            new MaintenanceJob { SiteId = maint.Id, JobName = "Transaction Log Backup", JobType = "Backup", LastRunUtc = DateTime.UtcNow.AddHours(-29), LastStatus = "Warning", FailureMessage = "Sample warning: expected backup window was missed." },
            new MaintenanceJob { SiteId = dev.Id, JobName = "Maintenance Log Cleanup", JobType = "Cleanup", LastRunUtc = DateTime.UtcNow.AddHours(-2), LastStatus = "Succeeded" }
        );

        db.SaveChanges();
    }

    private static void EnsureValidation(AppDbContext db)
    {
        if (db.ValidationTasks.Any()) return;

        var north = db.Sites.First(s => s.Name == "North Hangar");
        var maint = db.Sites.First(s => s.Name == "Maintenance Bay");

        db.ValidationTasks.AddRange(
            new ValidationTaskItem { SiteId = north.Id, TaskName = "Confirm scheduler service is running", Category = "Migration Readiness", IsComplete = true, ValidatedBy = "Demo User", ValidatedUtc = DateTime.UtcNow.AddHours(-3) },
            new ValidationTaskItem { SiteId = north.Id, TaskName = "Confirm latest full backup exists", Category = "Migration Readiness", IsComplete = true, ValidatedBy = "Demo User", ValidatedUtc = DateTime.UtcNow.AddHours(-3) },
            new ValidationTaskItem { SiteId = north.Id, TaskName = "Confirm offsite copy completes after backup", Category = "Post-Migration", IsComplete = false },
            new ValidationTaskItem { SiteId = maint.Id, TaskName = "Validate application endpoint after server change", Category = "Post-Migration", IsComplete = false }
        );

        db.SaveChanges();
    }

    private static void EnsureIncidents(AppDbContext db)
    {
        if (db.Incidents.Any()) return;

        var north = db.Sites.First(s => s.Name == "North Hangar");
        var maint = db.Sites.First(s => s.Name == "Maintenance Bay");

        var inc1 = new Incident
        {
            SiteId = north.Id,
            Title = "Offsite backup copy failed after full backup",
            Severity = "High",
            Status = "Investigating",
            DetectedAtUtc = DateTime.UtcNow.AddHours(-5),
            CreatedAtUtc = DateTime.UtcNow.AddHours(-5),
            CreatedBy = "Demo User"
        };

        var inc2 = new Incident
        {
            SiteId = maint.Id,
            Title = "Expected log backup window missed",
            Severity = "Medium",
            Status = "Open",
            DetectedAtUtc = DateTime.UtcNow.AddHours(-2),
            CreatedAtUtc = DateTime.UtcNow.AddHours(-2),
            CreatedBy = "Demo User"
        };

        db.Incidents.AddRange(inc1, inc2);
        db.SaveChanges();

        db.IncidentTimelineEntries.AddRange(
            new IncidentTimelineEntry { IncidentId = inc1.Id, EventTimeUtc = DateTime.UtcNow.AddHours(-5), EventType = "Detected", Note = "Dashboard flagged failed copy job after backup completed.", CreatedBy = "Demo User" },
            new IncidentTimelineEntry { IncidentId = inc1.Id, EventTimeUtc = DateTime.UtcNow.AddHours(-4), EventType = "Triage", Note = "Confirmed failure is isolated to the offsite copy step.", CreatedBy = "Demo User" },
            new IncidentTimelineEntry { IncidentId = inc2.Id, EventTimeUtc = DateTime.UtcNow.AddHours(-2), EventType = "Detected", Note = "Job did not complete inside the expected 24-hour window.", CreatedBy = "Demo User" }
        );

        db.SaveChanges();
    }

    private static void EnsureAudit(AppDbContext db)
    {
        if (db.AuditLogs.Any()) return;

        db.AuditLogs.AddRange(
            new AuditLogEntry { TimestampUtc = DateTime.UtcNow.AddHours(-6), Username = "Demo User", Action = "Seeded", EntityType = "System", Summary = "Created sample data for demonstration." },
            new AuditLogEntry { TimestampUtc = DateTime.UtcNow.AddHours(-5), Username = "Demo User", Action = "Created", EntityType = "Incident", Summary = "Opened incident for failed offsite backup copy." },
            new AuditLogEntry { TimestampUtc = DateTime.UtcNow.AddHours(-4), Username = "Demo User", Action = "Added timeline", EntityType = "Incident", Summary = "Added triage note to incident timeline." }
        );

        db.SaveChanges();
    }
}
