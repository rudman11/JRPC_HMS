using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JRPC_HMS.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        public string StockName { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Unit { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal InStock { get; set; }
        public int SupplierId { get; set; }
        public string TransferApprovals { get; set; }
    }
}
