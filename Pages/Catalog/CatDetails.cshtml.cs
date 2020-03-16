using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class CatDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CatDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}