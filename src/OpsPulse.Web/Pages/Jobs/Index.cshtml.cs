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

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<MaintenanceJob> Jobs { get; set; } = new();
    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public MaintenanceJob NewJob { get; set; } = new();

    public async Task OnGetAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();

        Jobs = await _db.MaintenanceJobs
            .Include(j => j.Site)
            .OrderByDescending(j => j.LastRunUtc)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        NewJob.LastRunUtc = DateTime.UtcNow;

        _db.MaintenanceJobs.Add(NewJob);
        await _db.SaveChangesAsync();

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
}
