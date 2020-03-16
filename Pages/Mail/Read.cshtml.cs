using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace JRPC_HMS.Pages.Mail
{
    public class ReadModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ReadModel> _logger;

        public ReadModel(IEmailSender emailSender, ILogger<ReadModel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public EmailMessage EmailMessage { get; set; }
        public string MContent { get; set; }
        public IList<EmailMessage> EmailMessages { get; set; }
        public IList<IMailFolder> Folders { get; set; }

        public void OnGet([FromBody]UniqueId messageId)
        {
            EmailMessage = _emailSender.ReadEmail(messageId);
            Folders = _emailSender.RetrieveFolders();
        }
    }
}