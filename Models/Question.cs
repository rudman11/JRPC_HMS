using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Question
    {
        [Key]
        public int Question_ID { get; set; }
        [DisplayName("Question Name")]
        public string QuestionName { get; set; }
        [DisplayName("Form Name")]
        public int Form_ID { get; set; }
        public string Description { get; set; }
    }
}
