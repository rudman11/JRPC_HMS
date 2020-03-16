using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Models
{
    public class OrderOrderItems
    {
        public Order Order { get; set; }

        public IList<OOrderItem> OOrderItems { get; set; }
    }

    public class OOrderItem
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
