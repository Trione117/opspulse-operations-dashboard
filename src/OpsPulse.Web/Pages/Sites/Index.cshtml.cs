using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpsPulse.Web.Data;
using OpsPulse.Web.Models;

namespace OpsPulse.Web.Pages.Sites;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Site> Sites { get; set; } = new();

    [BindProperty]
    public Site NewSite { get; set; } = new();

    public async Task OnGetAsync()
    {
        Sites = await _db.Sites.OrderBy(s => s.Name).ToListAsync();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        _db.Sites.Add(NewSite);
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }
}
