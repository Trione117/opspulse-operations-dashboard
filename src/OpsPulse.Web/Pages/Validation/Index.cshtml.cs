using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Pages.Validation;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<ValidationTaskItem> Tasks { get; set; } = new();
    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public ValidationTaskItem NewTask { get; set; } = new();

    public async Task OnGetAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();

        Tasks = await _db.ValidationTasks
            .Include(t => t.Site)
            .OrderBy(t => t.IsComplete)
            .ThenBy(t => t.Site!.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        _db.ValidationTasks.Add(NewTask);
        await _db.SaveChangesAsync();

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
        }

        return RedirectToPage();
    }
}
