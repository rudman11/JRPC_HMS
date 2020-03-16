using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace JRPC_HMS.Pages
{
    public class PostFormModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PostFormModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Feedback FeedbackResults { get; set; }
        public Customer Customer { get; set; }
        public Form Forms { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }

        public IActionResult OnGet(string refno)
        {
            FeedbackResults = _context.Feedbacks.FirstOrDefault(f => f.RefNo == refno);
            Customer = _context.Customers.FirstOrDefault(c => c.Id == FeedbackResults.Customer_ID);
            Forms = _context.Forms.FirstOrDefault(f => f.Form_ID == FeedbackResults.Form_ID);

            if (FeedbackResults.Status == "NEGATIVE")
            {
                Message1 = string.Format("Thank you for sharing your experience with {0} at JRPC Solutions.", Forms.FormName);
                Message2 = string.Format("One of our Managers will be reverting back to you at their soonest.");
            }
            else
            {
                Message1 = string.Format("Thank you for sharing your experience with {0} at JRPC Solutions.", Forms.FormName);
                Message2 = string.Format("See you soon.");
            }

            return Page();
        }
    }
}