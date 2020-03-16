using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JRPC_HMS.Models
{
    public class Product
    {
        [Key]
        public int Product_ID { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Short Description")]
        public string ProductShortDescription { get; set; }
        [Display(Name = "Product Detailed Description")]
        public string ProductDetailedDescription { get; set; }
        [Display(Name = "Selling Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SellingPrice { get; set; }
        [Display(Name = "Cost Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; }
        [Display(Name = "Profit Margin")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProfitMargin { get; set; }
        [Display(Name = "Picture")]
        public string ImageUrl { get; set; }
        [Display(Name = "Category")]
        public int Cat_ID { get; set; }
    }
}
