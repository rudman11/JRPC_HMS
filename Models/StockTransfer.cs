using System;
using System.ComponentModel.DataAnnotations;


namespace JRPC_HMS.Models
{
    public class StockTransfer
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        public DateTime TransferDate { get; set; }
        public decimal Quantity { get; set; }
        public string Approvals { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
        public DateTime DateApproved { get; set; }
        public string Requester { get; set; }
    }
}
