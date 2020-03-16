using JRPC_HMS.Models;
using MailKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JRPC_HMS.Services.Mail
{
    public interface IEmailSender
    {
        void SendMail(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmails(int maxCount = 10);
        IMailFolder RetrieveInbox();
        List<IMailFolder> RetrieveFolders();
        void DeleteMail(UniqueId uid);
        EmailMessage ReadEmail(UniqueId uid);
    }
}
