using Microsoft.AspNetCore.Mvc.RazorPages;
using OpsPulse.Web.Services;

namespace OpsPulse.Web.Pages;

public class IndexModel : PageModel
{
    private readonly DashboardService _dashboardService;

    public IndexModel(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public DashboardViewModel Dashboard { get; set; } = new();

    public async Task OnGetAsync()
    {
        Dashboard = await _dashboardService.GetDashboardAsync();
    }

    public string GetHealth(DateTime lastRunUtc, string status)
    {
        return JobHealthService.Classify(lastRunUtc, status, DateTime.UtcNow);
    }

    public string GetBadgeCss(string health)
    {
        return JobHealthService.GetBadgeCss(health);
    }
}
