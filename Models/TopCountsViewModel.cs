namespace JRPC_HMS.Models
{
    public class TopCountsViewModel
    {
        public int AllOrdersCount { get; set; }
        public int IncompleteOrderCount { get; set; }
        public int LowStockCount { get; set; }
        public int VoidedOrdersCount { get; set; }
        public int Feedbacks { get; set; }
        public int Customers { get; set; }
        public int TotalNegative { get; set; }
        public int TotalPositive { get; set; }
    }
}
