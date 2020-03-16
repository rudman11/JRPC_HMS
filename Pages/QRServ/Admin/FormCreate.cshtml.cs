using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    [Authorize(Roles = "Admin, User")]
    public class FormCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FormCreateModel> _logger;

        public FormCreateModel(ApplicationDbContext context, ILogger<FormCreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Form Form { get; set; }
        public SelectList Status { get; set; } = new SelectList(new[] { "Active", "Inactive" });

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PageModelExtensions.SetStatusMessage(this, "Model is invalid");
                _logger.LogError("Invalid Model");
                return Page();
            }

            _context.Forms.Add(Form);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Successfully added new Form";
            return RedirectToPage("./FormIndex");
        }
    }
}