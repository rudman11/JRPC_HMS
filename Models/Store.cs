using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Store
    {
        [Key]
        public int Store_ID { get; set; }
        public string StoreName { get; set; }
    }
}
