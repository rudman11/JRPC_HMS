using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class CatEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CatEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.Cat_ID == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.Cat_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["StatusMessage"] = "Category " + Category.CategoryName + " successfully edited.";
            return RedirectToPage("./CatIndex");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Cat_ID == id);
        }
    }
}