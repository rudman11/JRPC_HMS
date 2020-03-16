using System;
using System.ComponentModel.DataAnnotations;

namespace JRPC_HMS.Models
{
    public class Requisition
    {
        [Key]
        public int Id { get; set; }
        public string ReqNo { get; set; }
        public int SupplierId { get; set; }
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime ReqDate { get; set; }
        public string Requester { get; set; }
        public string Approved { get; set; }
        public string Approver { get; set; }
        public DateTime ApprovedDate { get; set; }
    }
}
