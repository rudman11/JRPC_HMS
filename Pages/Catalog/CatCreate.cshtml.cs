using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class CatCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CatCreateModel> _logger;

        public CatCreateModel(ApplicationDbContext context, ILogger<CatCreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PageModelExtensions.SetStatusMessage(this, "Model is invalid");
                _logger.LogError("Invalid Model");
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Successfully added new Category";
            return RedirectToPage("./CatIndex");
        }
    }
}