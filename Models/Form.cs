using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Form
    {
        [Key]
        public int Form_ID { get; set; }
        [DisplayName("Form Name")]
        public string FormName { get; set; }
        public string Status { get; set; }
    }
}
