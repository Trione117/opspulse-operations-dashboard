using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<MaintenanceJob> MaintenanceJobs => Set<MaintenanceJob>();
    public DbSet<ValidationTaskItem> ValidationTasks => Set<ValidationTaskItem>();
    public DbSet<Incident> Incidents => Set<Incident>();
    public DbSet<IncidentTimelineEntry> IncidentTimelineEntries => Set<IncidentTimelineEntry>();
    public DbSet<AuditLogEntry> AuditLogs => Set<AuditLogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Incident>()
            .HasMany(i => i.TimelineEntries)
            .WithOne(t => t.Incident)
            .HasForeignKey(t => t.IncidentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
