using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public int Question_ID { get; set; }
        public string QAnswer { get; set; }
        public int Form_ID { get; set; }
        public string Feedback_ID { get; set; }
    }
}
