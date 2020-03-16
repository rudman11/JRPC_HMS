using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Models
{
    public class EmailMessage
    {
        public UniqueId MessageId { get; set; }
        public EmailMessage()
        {
            ToAddresses = new List<InternetAddress>();
            FromAddresses = new List<InternetAddress>();
        }

        public List<InternetAddress> ToAddresses { get; set; }
        public List<InternetAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? MailDate { get; set; }
        public string Attachments { get; set; }
    }
}
