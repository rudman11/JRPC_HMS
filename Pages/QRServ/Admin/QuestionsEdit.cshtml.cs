using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    [Authorize(Roles = "Admin, User")]
    public class QuestionsEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public QuestionsEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question Question { get; set; }
        public IList<SelectListItem> Forms { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Forms = _context.Forms.Select(n => new SelectListItem
            {
                Value = n.Form_ID.ToString(),
                Text = n.FormName
            }).ToList();
            Question = await _context.Questions.FirstOrDefaultAsync(m => m.Question_ID == id);

            if (Question == null)
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

            _context.Attach(Question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(Question.Question_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["StatusMessage"] = "Question " + Question.QuestionName + " successfully edited.";
            return RedirectToPage("./CatIndex");
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Question_ID == id);
        }
    }
}