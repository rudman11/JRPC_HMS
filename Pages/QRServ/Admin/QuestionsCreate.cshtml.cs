using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    [Authorize(Roles = "Admin, User")]
    public class QuestionsCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<QuestionsCreateModel> _logger;

        public QuestionsCreateModel(ApplicationDbContext context, ILogger<QuestionsCreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Question Question { get; set; }
        public IList<SelectListItem> Forms { get; set; }

        public IActionResult OnGet()
        {
            Forms = _context.Forms.Select(n => new SelectListItem
            {
                Value = n.Form_ID.ToString(),
                Text = n.FormName
            }).ToList();
            return Page();
        }        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PageModelExtensions.SetStatusMessage(this, "Model is invalid");
                _logger.LogError("Invalid Model");
                return Page();
            }

            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Successfully added new Question";
            return RedirectToPage("./QuestionsIndex");
        }
    }
}