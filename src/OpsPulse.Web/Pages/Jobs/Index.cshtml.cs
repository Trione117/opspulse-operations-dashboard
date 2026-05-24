using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;
using OpsPulse.Web.Services;

namespace OpsPulse.Web.Pages.Jobs;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly AuditService _audit;

    public IndexModel(AppDbContext db, AuditService audit)
    {
        _db = db;
        _audit = audit;
    }

    public List<MaintenanceJob> Jobs { get; set; } = new();
    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public MaintenanceJob NewJob { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadPageDataAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadPageDataAsync();
            return Page();
        }

        NewJob.LastRunUtc = DateTime.UtcNow;

        _db.MaintenanceJobs.Add(NewJob);
        await _db.SaveChangesAsync();

        await _audit.LogAsync("Created", "MaintenanceJob", NewJob.Id, $"Created maintenance job '{NewJob.JobName}'.");

        return RedirectToPage();
    }

    public string GetHealth(DateTime lastRunUtc, string status)
    {
        return JobHealthService.Classify(lastRunUtc, status, DateTime.UtcNow);
    }

    public string GetBadgeCss(string health)
    {
        return JobHealthService.GetBadgeCss(health);
    }

    private async Task LoadPageDataAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();

        Jobs = await _db.MaintenanceJobs
            .Include(j => j.Site)
            .OrderByDescending(j => j.LastRunUtc)
            .ToListAsync();
    }
}
