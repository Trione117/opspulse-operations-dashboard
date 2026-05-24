using System.ComponentModel.DataAnnotations;

namespace OpsPulse.Web.Models;

public class ValidationTaskItem
{
    public int Id { get; set; }

    public int SiteId { get; set; }

    public Site? Site { get; set; }

    [Required]
    [Display(Name = "Task Name")]
    public string TaskName { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = "Post-Migration";

    [Display(Name = "Completed")]
    public bool IsComplete { get; set; }

    [Display(Name = "Validated By")]
    public string? ValidatedBy { get; set; }

    [Display(Name = "Validated UTC")]
    public DateTime? ValidatedUtc { get; set; }
}
