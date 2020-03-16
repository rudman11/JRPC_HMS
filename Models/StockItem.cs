using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class StockItem
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Stock Name")]
        public string StockName { get; set; }
        public decimal Unit { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "In Stock")]
        public decimal InStock { get; set; }
        public int StoreId { get; set; }
        public string TransferApprovals { get; set; }
    }
}
