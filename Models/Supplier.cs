using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
