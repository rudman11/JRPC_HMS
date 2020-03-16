using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    [Authorize(Roles = "Admin, User")]
    public class FormEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FormEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Form Form { get; set; }
        public SelectList Status { get; set; } = new SelectList(new[] { "Active", "Inactive" });

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Form = await _context.Forms.FirstOrDefaultAsync(m => m.Form_ID == id);

            if (Form == null)
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

            _context.Attach(Form).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormExists(Form.Form_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["StatusMessage"] = "Form " + Form.FormName + " successfully edited.";
            return RedirectToPage("./FormIndex");
        }

        private bool FormExists(int id)
        {
            return _context.Forms.Any(e => e.Form_ID == id);
        }
    }
}