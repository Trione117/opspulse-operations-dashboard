using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class IncidentTimelineEntry
{
    public int Id { get; set; }
    public int IncidentId { get; set; }
    public Incident? Incident { get; set; }

    public DateTime EventTimeUtc { get; set; } = DateTime.UtcNow;

    [Required]
    public string EventType { get; set; } = "Update";

    [Required]
    public string Note { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = "Demo User";
}
