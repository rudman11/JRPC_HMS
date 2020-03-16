using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
    }
}
