using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;

namespace JRPC_HMS.Pages.Mail
{
    public class ComposeModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ComposeModel> _logger;

        public ComposeModel(IEmailSender emailSender, ILogger<ComposeModel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public string ToEmailAddress { get; set; }
        [BindProperty]
        public string Subject { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public string Status { get; set; }
        public IList<IMailFolder> Folders { get; set; }

        public void OnGet()
        {
            Status = "";
            ToEmailAddress = "";
            Subject = "";
            Message = "";
            Folders = _emailSender.RetrieveFolders();
        }

        public IActionResult OnPost()
        {
            var toEmailAddress = ToEmailAddress.Split(';');
            try
            {
                List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
                List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
                foreach (var address in toEmailAddress)
                {
                    MailboxAddress toAddress = new MailboxAddress(address);
                    toEmailAddresses.Add(toAddress);
                }
                MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
                fromEmailAddresses.Add(fromAddress);

                EmailMessage emailMessage = new EmailMessage()
                {
                    ToAddresses = toEmailAddresses,
                    FromAddresses = fromEmailAddresses,
                    Subject = Subject,
                    Content = Message
                };

                _emailSender.SendMail(emailMessage);
                Status = "Email Sent";
                Folders = _emailSender.RetrieveFolders();
                return Redirect("./Inbox");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException, "Error sending email");
                Status = "Error in sending email.";
                return Page();
            }
        }
    }
}