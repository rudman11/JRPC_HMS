using JRPC_HMS.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Services.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> emailSettings, IHostingEnvironment env, ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
            _logger = logger;
        }

        public List<EmailMessage> ReceiveEmails(int maxCount = 10)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                
                List<EmailMessage> emails = new List<EmailMessage>();
                foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId))
                {
                    var emailMessage = new EmailMessage
                    {
                        MessageId = summary.UniqueId,
                        FromAddresses = summary.Envelope.From.ToList(),
                        ToAddresses = summary.Envelope.To.ToList(),
                        Subject = summary.Envelope.Subject,
                        Content = "",
                        MailDate = summary.Envelope.Date
                    };
                    emails.Add(emailMessage);
                }

                return emails;
            }
        }

        public EmailMessage ReadEmail(UniqueId uid)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var message = inbox.GetMessage(uid);

                EmailMessage emailMessage = new EmailMessage()
                {
                    MessageId = uid,
                    FromAddresses = message.From.ToList(),
                    ToAddresses = message.To.ToList(),
                    Subject = message.Subject,
                    Content = message.HtmlBody.ToString(),
                    MailDate = message.Date
                };

                return emailMessage;
            }
        }

        public IMailFolder RetrieveInbox()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                return inbox;
            }
        }

        public List<IMailFolder> RetrieveFolders()
        {
            List<IMailFolder> folders = new List<IMailFolder>();
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                var personal = client.GetFolder(client.PersonalNamespaces[0]);
                foreach (var folder in personal.GetSubfolders(false))
                {
                    folders.Add(folder);
                }

                return folders;
            }
        }

        public void DeleteMail(UniqueId uid)
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                client.Inbox.AddFlags(new UniqueId[] { uid }, MessageFlags.Deleted, true);
            }            
        }

        public void SendMail(EmailMessage emailMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.To.AddRange(emailMessage.ToAddresses);
                message.From.AddRange(emailMessage.FromAddresses);

                message.Subject = emailMessage.Subject;

                var builder = new BodyBuilder();

                // Set the plain-text version of the message text
                builder.TextBody = emailMessage.Content;

                if (!string.IsNullOrEmpty(emailMessage.Attachments))
                {
                    // We may also want to attach a calendar event for Monica's party...
                    builder.Attachments.Add(emailMessage.Attachments);
                }
                // Now we just need to set the message body and we're done
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                    //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    //client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Authenticate("rudman11@gmail.com", "Mickey2Mouse");

                    client.Send(message);

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException, "Error sending Email");
            }
        }
    }
}
