using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpsPulse.Web.Pages.Runbook;

public class IndexModel : PageModel
{
    public List<RunbookStep> Steps { get; set; } = new();

    public void OnGet()
    {
        Steps = new List<RunbookStep>
        {
            new RunbookStep("1", "Confirm scope", "Identify whether the issue is isolated to one site, one job type, or all locations."),
            new RunbookStep("2", "Check recent failures", "Review jobs that failed or missed the expected window before making infrastructure assumptions."),
            new RunbookStep("3", "Validate dependencies", "Confirm the scheduler/service account, database availability, and destination share are reachable."),
            new RunbookStep("4", "Compare normal vs bad window", "Look for what changed during the failure window instead of treating all warnings as root cause."),
            new RunbookStep("5", "Document the evidence", "Capture the failed job, timestamp, affected site, and validation result before escalating.")
        };
    }
}

public record RunbookStep(string Number, string Title, string Description);
