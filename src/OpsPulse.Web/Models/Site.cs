using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class Site
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Site Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "App Server")]
    public string AppServer { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Database Server")]
    public string DatabaseServer { get; set; } = string.Empty;

    [Required]
    public string Environment { get; set; } = "Production";

    [Required]
    public string Status { get; set; } = "Healthy";

    public ICollection<MaintenanceJob> MaintenanceJobs { get; set; } = new List<MaintenanceJob>();

    public ICollection<ValidationTaskItem> ValidationTasks { get; set; } = new List<ValidationTaskItem>();
}
