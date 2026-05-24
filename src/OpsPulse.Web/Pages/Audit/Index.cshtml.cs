using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Pages.Audit;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) { _db = db; }
    public List<AuditLogEntry> AuditEntries { get; set; } = new();
    public async Task OnGetAsync() => AuditEntries = await _db.AuditLogs.OrderByDescending(a => a.TimestampUtc).Take(100).ToListAsync();
}
