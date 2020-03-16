using System;
using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Paid { get; set; }
        public decimal Change { get; set; }
        public decimal VatTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string PreparationStatus { get; set; }
        public string Username { get; set; }
    }
}
