using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class StockToProduct
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public decimal QuantityUse { get; set; }
    }
}
