using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;
using OpsPulse.Web.Services;

namespace OpsPulse.Web.Pages.Validation;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly AuditService _audit;

    public IndexModel(AppDbContext db, AuditService audit)
    {
        _db = db;
        _audit = audit;
    }

    public List<ValidationTaskItem> Tasks { get; set; } = new();
    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public ValidationTaskItem NewTask { get; set; } = new();

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

        _db.ValidationTasks.Add(NewTask);
        await _db.SaveChangesAsync();

        await _audit.LogAsync("Created", "ValidationTask", NewTask.Id, $"Created validation task '{NewTask.TaskName}'.");

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCompleteAsync(int id)
    {
        var task = await _db.ValidationTasks.FindAsync(id);

        if (task is not null)
        {
            task.IsComplete = true;
            task.ValidatedBy = "Demo User";
            task.ValidatedUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _audit.LogAsync("Completed", "ValidationTask", task.Id, $"Completed validation task '{task.TaskName}'.");
        }

        return RedirectToPage();
    }

    private async Task LoadPageDataAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();

        Tasks = await _db.ValidationTasks
            .Include(t => t.Site)
            .OrderBy(t => t.IsComplete)
            .ThenBy(t => t.Site!.Name)
            .ToListAsync();
    }
}
