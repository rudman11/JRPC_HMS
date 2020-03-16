using JRPC_HMS.Models;
using System.Collections.Generic;

namespace JRPC_HMS.Data
{
    public class ThisPageModel
    {
        public int TotProdSold { get; set; }
        public string MonthSelected { get; set; }
        public string YearSelected { get; set; }
        public string TotProdSoldPerc { get; set; }
        public int TotOrders { get; set; }
        public string TotOrdersPerc { get; set; }
        public int TotOrdersVoided { get; set; }
        public string TotOrdersVoidedPerc { get; set; }
        public decimal TotSales { get; set; }
        public string TotSalesPerc { get; set; }
        public decimal TotCost { get; set; }
        public string TotCostPerc { get; set; }
        public decimal TotProfit { get; set; }
        public string TotProfitPerc { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<Store> AllStores { get; set; }
        public string StoreCats { get; set; }
        public string StoreData { get; set; }
        public string UserCats { get; set; }
        public string UserData { get; set; }
        public string CategoryCats { get; set; }
        public string CategoryData { get; set; }
        public string SalesCats { get; set; }
        public string SalesData { get; set; }
        public string ProfitCats { get; set; }
        public string ProfitData { get; set; }
        public string OrdersCats { get; set; }
        public string OrdersData { get; set; }
        public string OrderStatusCats { get; set; }
        public string OrderStatusData { get; set; }
    }
}
