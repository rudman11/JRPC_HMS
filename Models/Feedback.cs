using System;
using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Feedback
    {
        [Key]
        public string RefNo { get; set; }
        public int Customer_ID { get; set; }
        public string Servant { get; set; }
        public string Comments { get; set; }
        public int Form_ID { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Notes { get; set; }
        public string HONotes { get; set; }
        public string Status { get; set; }
        public string RefScore { get; set; }
        public string Photo { get; set; }
        public string ContactCustomer { get; set; }
    }
}
