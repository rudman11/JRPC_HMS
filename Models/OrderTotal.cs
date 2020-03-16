namespace JRPC_HMS.Models
{
    public class OrderTotal
    {
        public decimal PendingToday { get; set; }
        public decimal PendingWeek { get; set; }
        public decimal PendingMonth { get; set; }
        public decimal PendingYear { get; set; }
        public decimal PendingAllTime { get; set; }
        public decimal CompletedToday { get; set; }
        public decimal CompletedWeek { get; set; }
        public decimal CompletedMonth { get; set; }
        public decimal CompletedYear { get; set; }
        public decimal CompletedAllTime { get; set; }
    }
}
