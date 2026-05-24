using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class AuditLogEntry
{
    public int Id { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public string Username { get; set; } = "Demo User";

    [Required]
    public string Action { get; set; } = string.Empty;

    [Required]
    public string EntityType { get; set; } = string.Empty;

    public int? EntityId { get; set; }
    public string Summary { get; set; } = string.Empty;
}
