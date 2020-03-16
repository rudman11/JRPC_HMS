using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.QRServ
{
    [Authorize(Roles = "Admin, User")]
    public class FeedbackEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public FeedbackEditModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Feedback Feedback { get; set; }
        public Customer Customer { get; set; }
        public IList<Form> Forms { get; set; }
        [BindProperty]
        public string FeedbackStatus { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Answer> Answers { get; set; }
        [BindProperty]
        public string Notes { get; set; }
        [BindProperty]
        public string HO_Notes { get; set; }

        public async Task<IActionResult> OnGet(string refno)
        {
            Feedback = _context.Feedbacks.FirstOrDefault(f => f.RefNo == refno);
            Customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == Feedback.Customer_ID);
            Forms = _context.Forms.ToList();
            FeedbackStatus = Feedback.Status;
            Notes = Feedback.Notes;
            HO_Notes = Feedback.HONotes;
            Answers = _context.Answers.Where(a => a.Feedback_ID == Feedback.RefNo).ToList();
            Questions = _context.Questions.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.RefNo == Feedback.RefNo);
            Feedback.Status = FeedbackStatus;
            Feedback.Notes = Notes;
            Feedback.HONotes = HO_Notes;
            _context.Feedbacks.Update(Feedback);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(Feedback.RefNo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["StatusMessage"] = "Feedback " + Feedback.RefNo + " successfully updated.";
            return Redirect("/QRServ/FeedbackIndex");
        }

        private bool FeedbackExists(string refno)
        {
            return _context.Feedbacks.Any(e => e.RefNo == refno);
        }
    }
}