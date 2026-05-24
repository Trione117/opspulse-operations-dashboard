using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Services;

public class AuditService
{
    private readonly AppDbContext _db;

    public AuditService(AppDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(string action, string entityType, int? entityId, string summary, string username = "Demo User")
    {
        _db.AuditLogs.Add(new AuditLogEntry
        {
            TimestampUtc = DateTime.UtcNow,
            Username = username,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Summary = summary
        });

        await _db.SaveChangesAsync();
    }
}
