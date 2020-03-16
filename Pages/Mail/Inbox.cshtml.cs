using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace JRPC_HMS.Pages.Mail
{
    public class InboxModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<InboxModel> _logger;

        public InboxModel(IEmailSender emailSender, ILogger<InboxModel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });

        public IList<EmailMessage> EmailMessages { get; set; }
        public IMailFolder InboxFolder { get; set; }
        public int Unread { get; set; }
        public IList<IMailFolder> Folders { get; set; }
        [BindProperty]
        public List<UniqueId> DeleteMail { get; set; }


        public IActionResult OnGet(int currentPage, int pageSize)
        {
            var emails = from e in _emailSender.ReceiveEmails().OrderByDescending(e => e.MailDate)
                         select e;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = emails.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Folders = _emailSender.RetrieveFolders();
            InboxFolder = _emailSender.RetrieveInbox();
            EmailMessages = emails
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            return Page();
        }

        public IActionResult OnPost(int currentPage, int pageSize)
        {
            if (DeleteMail != null)
            {
                foreach (var mailId in DeleteMail)
                {
                    _emailSender.DeleteMail(mailId);
                }
            }

            var emails = from e in _emailSender.ReceiveEmails().OrderByDescending(e => e.MailDate)
                         select e;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = emails.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Folders = _emailSender.RetrieveFolders();
            InboxFolder = _emailSender.RetrieveInbox();
            EmailMessages = emails
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            return Page();
        }
    }
}