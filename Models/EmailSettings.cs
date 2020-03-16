namespace JRPC_HMS.Models
{
    public class EmailSettings
    {
        public string PopMailServer { get; set; }
        public int PopMailPort { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}
