using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class Incident
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Title { get; set; } = string.Empty;

    public int SiteId { get; set; }
    public Site? Site { get; set; }

    [Required]
    public string Severity { get; set; } = "Medium";

    [Required]
    public string Status { get; set; } = "Open";

    public DateTime DetectedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAtUtc { get; set; }

    public string? RootCause { get; set; }
    public string? ResolutionSummary { get; set; }

    public string CreatedBy { get; set; } = "Demo User";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<IncidentTimelineEntry> TimelineEntries { get; set; } = new List<IncidentTimelineEntry>();
}
