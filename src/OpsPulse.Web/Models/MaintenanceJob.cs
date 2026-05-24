using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class MaintenanceJob
{
    public int Id { get; set; }

    [Display(Name = "Site")]
    public int SiteId { get; set; }

    public Site? Site { get; set; }

    [Required]
    [Display(Name = "Job Name")]
    public string JobName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Job Type")]
    public string JobType { get; set; } = "Backup";

    [Display(Name = "Last Run UTC")]
    public DateTime LastRunUtc { get; set; } = DateTime.UtcNow;

    [Required]
    [Display(Name = "Last Status")]
    public string LastStatus { get; set; } = "Succeeded";

    [Display(Name = "Failure Message")]
    public string? FailureMessage { get; set; }
}
