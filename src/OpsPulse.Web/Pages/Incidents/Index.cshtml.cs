using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;
using OpsPulse.Web.Services;

namespace OpsPulse.Web.Pages.Incidents;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly AuditService _audit;

    public IndexModel(AppDbContext db, AuditService audit)
    {
        _db = db;
        _audit = audit;
    }

    public List<Incident> Incidents { get; set; } = new();
    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public Incident NewIncident { get; set; } = new();

    public async Task OnGetAsync() => await LoadAsync();

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadAsync();
            return Page();
        }

        NewIncident.Status = "Open";
        NewIncident.CreatedAtUtc = DateTime.UtcNow;
        NewIncident.DetectedAtUtc = DateTime.UtcNow;
        NewIncident.CreatedBy = "Demo User";

        _db.Incidents.Add(NewIncident);
        await _db.SaveChangesAsync();

        _db.IncidentTimelineEntries.Add(new IncidentTimelineEntry
        {
            IncidentId = NewIncident.Id,
            EventTimeUtc = DateTime.UtcNow,
            EventType = "Opened",
            Note = "Incident opened from the OpsPulse web interface.",
            CreatedBy = "Demo User"
        });

        await _db.SaveChangesAsync();
        await _audit.LogAsync("Created", "Incident", NewIncident.Id, $"Opened incident: {NewIncident.Title}");

        return RedirectToPage();
    }

    public string SeverityBadge(string severity) => IncidentPriorityService.GetBadgeCss(severity);

    private async Task LoadAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();
        Incidents = await _db.Incidents.Include(i => i.Site).OrderBy(i => i.Status == "Resolved").ThenByDescending(i => i.DetectedAtUtc).ToListAsync();
    }
}
