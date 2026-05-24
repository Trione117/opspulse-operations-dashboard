using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;
using OpsPulse.Web.Services;

namespace OpsPulse.Web.Pages.Incidents;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly AuditService _audit;

    public DetailsModel(AppDbContext db, AuditService audit)
    {
        _db = db;
        _audit = audit;
    }

    public Incident Incident { get; set; } = new();

    [BindProperty]
    public string EventType { get; set; } = "Update";

    [BindProperty]
    public string TimelineNote { get; set; } = string.Empty;

    [BindProperty]
    public string RootCause { get; set; } = string.Empty;

    [BindProperty]
    public string ResolutionSummary { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var incident = await FindIncidentAsync(id);
        if (incident is null) return NotFound();
        Incident = incident;
        return Page();
    }

    public async Task<IActionResult> OnPostAddTimelineAsync(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident is null) return NotFound();
        if (string.IsNullOrWhiteSpace(TimelineNote)) return RedirectToPage(new { id });

        _db.IncidentTimelineEntries.Add(new IncidentTimelineEntry { IncidentId = id, EventTimeUtc = DateTime.UtcNow, EventType = EventType, Note = TimelineNote, CreatedBy = "Demo User" });
        if (incident.Status == "Open") incident.Status = "Investigating";

        await _db.SaveChangesAsync();
        await _audit.LogAsync("Added timeline", "Incident", id, $"Added {EventType} note to incident {id}");
        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostResolveAsync(int id)
    {
        var incident = await _db.Incidents.FindAsync(id);
        if (incident is null) return NotFound();

        incident.Status = "Resolved";
        incident.ResolvedAtUtc = DateTime.UtcNow;
        incident.RootCause = RootCause;
        incident.ResolutionSummary = ResolutionSummary;

        _db.IncidentTimelineEntries.Add(new IncidentTimelineEntry { IncidentId = id, EventTimeUtc = DateTime.UtcNow, EventType = "Resolved", Note = string.IsNullOrWhiteSpace(ResolutionSummary) ? "Incident marked resolved." : ResolutionSummary, CreatedBy = "Demo User" });

        await _db.SaveChangesAsync();
        await _audit.LogAsync("Resolved", "Incident", id, $"Resolved incident: {incident.Title}");
        return RedirectToPage(new { id });
    }

    public int PriorityScore()
    {
        var ageHours = (int)Math.Floor((DateTime.UtcNow - Incident.DetectedAtUtc).TotalHours);
        return IncidentPriorityService.GetPriorityScore(Incident.Severity, ageHours);
    }

    public string SeverityBadge() => IncidentPriorityService.GetBadgeCss(Incident.Severity);

    private async Task<Incident?> FindIncidentAsync(int id)
    {
        return await _db.Incidents.Include(i => i.Site).Include(i => i.TimelineEntries).FirstOrDefaultAsync(i => i.Id == id);
    }
}
