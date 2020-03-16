using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Category
    {
        [Key]
        public int Cat_ID { get; set; }
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Category Description")]
        public string CategoryDescription { get; set; }
    }
}
