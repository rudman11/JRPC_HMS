using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Reports
{
    [Authorize(Roles = "Admin, User")]
    public class SalesReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public SalesReportModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Properties
        public IList<SelectListItem> MonthNames { get; set; }
        public List<string> Months = new List<string>(){ "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public IList<SelectListItem> Years { get; set; }
        [BindProperty(SupportsGet =true)]
        public string StoreCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StoreData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UserCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UserData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategoryCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategoryData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SalesCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SalesData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProfitCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProfitData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrdersCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrdersData { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderStatusCats { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderStatusData { get; set; }

        [BindProperty(SupportsGet = true)]
        public int TotProdSold { get; set; }
        [BindProperty(SupportsGet = true)]
        public int TotOrders { get; set; }
        [BindProperty(SupportsGet = true)]
        public int TotOrdersVoided { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal TotSales { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal TotProfit { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal TotCost { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotProdSoldPerc { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotOrdersPerc { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotOrdersVoidedPerc { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotSalesPerc { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotProfitPerc { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TotCostPerc { get; set; }

        public IList<Product> Products { get; set; }
        public IList<OrderItems> OrderItemss { get; set; }
        public IList<StockToProduct> StockToProducts { get; set; }
        public IList<StockItem> StockItems { get; set; }
        public IList<Order> AllOrders { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<Order> FilteredOrders { get; set; }
        public IList<Store> AllStores { get; set; }
        public IList<SelectListItem> StoreList { get; set; }
        public IList<SelectListItem> UserList { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });
        [BindProperty(SupportsGet = true)]
        public string ShowHide { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MonthSelected { get; set; }
        [BindProperty(SupportsGet = true)]
        public string YearSelected { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedMonth { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedYear { get; set; }
        
        public ThisPageModel thisPageModel { get; set; }
        public Dictionary<string, decimal> storeAmounts { get; set; }
        public Dictionary<string, decimal> userAmounts { get; set; }
        public Dictionary<string, decimal> categoryAmounts { get; set; }
        public Dictionary<string, decimal> salesAmounts { get; set; }
        public Dictionary<string, decimal> profitAmounts { get; set; }
        public Dictionary<string, int> ordersAmounts { get; set; }
        public Dictionary<string, int> orderStatusAmounts { get; set; }
        public Dictionary<string, decimal> catAmounts { get; set; }
        #endregion

        #region OnGet
        public IActionResult OnGet(int currentPage, int pageSize)
        {
            AllOrders = _context.Orders.ToList();
            AllStores = _context.Stores.ToList();
            MonthNames = Months.Select(n => new SelectListItem
            {
                Value = n,
                Text = n
            }).ToList();

            var orders = from p in _context.Orders
                         select p;

            List<string> yearsList = new List<string>();
            foreach(var order in _context.Orders.ToList())
            {
                if (!yearsList.Contains(order.OrderDate.ToString("yyyy")))
                {
                    yearsList.Add(order.OrderDate.ToString("yyyy"));
                }
            }
            Years = yearsList.Select(n => new SelectListItem
            {
                Value = n,
                Text = n
            }).ToList();

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = orders.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Orders = orders
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ShowHide = "none";
            return Page();
        }

        public IActionResult OnGetExport(int currentPage, int pageSize, string month, string year)
        {
            int monthInt = 0;
            if (!string.IsNullOrEmpty(month))
            {
                monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(month) + 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedMonth))
                {
                    monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(SelectedMonth) + 1;
                }
            }

            int yearInt = Convert.ToInt32(year);

            PopulatePage(monthInt, yearInt, currentPage, pageSize);

            return Page();
        }
        #endregion

        #region PopulatePage
        public void PopulatePage(int month, int year, int currentPage, int pageSize)
        {
            var stores = _context.Stores.AsNoTracking().ToList();
            var orderItems = _context.OrderItems.AsNoTracking().ToList();
            var orderS = _context.Orders.AsNoTracking().ToList();
            var products = _context.Products.AsNoTracking().ToList();
            var categories = _context.Categories.AsNoTracking().ToList();

            DateTime ddate = new DateTime(year, month, 1, 0, 0, 0);
            DateTime lastMonth = ddate.AddMonths(-1);
            var orders = from p in orderS.Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year && o.OrderStatus == "Completed")
                         select p;
            FilteredOrders = orders.ToList();
            IEnumerable<Order> prevOrders = null;
            IEnumerable<Order> prevVoidOrders = null;
            if (ddate.Month == 1)
            {
                prevOrders = from p in orderS.Where(o => o.OrderDate.Month == lastMonth.Month && o.OrderDate.Year == lastMonth.Year && o.OrderStatus == "Completed")
                             select p;
                prevVoidOrders = from p in orderS.Where(o => o.OrderDate.Month == ddate.AddMonths(-1).Month && o.OrderDate.Year == lastMonth.Year && o.OrderStatus == "Voided")
                                 select p;
            }
            else
            {
                prevOrders = from p in orderS.Where(o => o.OrderDate.Month == ddate.AddMonths(-1).Month && o.OrderStatus == "Completed")
                             select p;
                prevVoidOrders = from p in orderS.Where(o => o.OrderDate.Month == ddate.AddMonths(-1).Month && o.OrderStatus == "Voided")
                                 select p;
            }
            var voidOrders = from p in orderS.Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year && o.OrderStatus == "Voided")
                             select p;

            int prevTotProdSold = 0;
            int totProdSold = 0, totOrderStatus = 0;
            decimal prevTotSales = 0, prevTotCost = 0;
            decimal totSales = 0, totCost = 0, storeAmount = 0, userAmount = 0;
            storeAmounts = new Dictionary<string, decimal>();
            userAmounts = new Dictionary<string, decimal>();
            categoryAmounts = new Dictionary<string, decimal>();
            salesAmounts = new Dictionary<string, decimal>();
            profitAmounts = new Dictionary<string, decimal>();
            ordersAmounts = new Dictionary<string, int>();
            orderStatusAmounts = new Dictionary<string, int>();
            catAmounts = new Dictionary<string, decimal>();
            foreach (var order in orders)
            {
                totOrderStatus++;
                string storeName = stores.FirstOrDefault(s => s.Store_ID == order.StoreId).StoreName;
                if (!storeAmounts.ContainsKey(storeName))
                {
                    storeAmounts.Add(storeName, order.GrandTotal);
                }
                else
                {
                    storeAmount = storeAmounts[storeName];
                    storeAmounts[storeName] = storeAmount + order.GrandTotal;
                }
                string user = order.Username;
                if (!userAmounts.ContainsKey(user))
                {
                    userAmounts.Add(user, order.GrandTotal);
                }
                else
                {
                    userAmount = userAmounts[user];
                    userAmounts[user] = userAmount + order.GrandTotal;
                }

                totSales += order.GrandTotal;
                List<OrderItems> oItems = new List<OrderItems>();
                oItems = orderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                foreach (var oItem in oItems)
                {
                    if (oItem != null)
                    {
                        Product prod = products.FirstOrDefault(p => p.Product_ID == oItem.Product_ID);
                        Category cat = categories.FirstOrDefault(c => c.Cat_ID == prod.Cat_ID);

                        if (!catAmounts.ContainsKey(cat.CategoryName))
                        {
                            catAmounts.Add(cat.CategoryName, prod.SellingPrice);
                        }
                        else
                        {
                            decimal catAmount = catAmounts[cat.CategoryName];
                            catAmounts[cat.CategoryName] = catAmount + prod.SellingPrice;
                        }

                        totCost += prod.CostPrice;
                        totProdSold += oItem.Quantity;
                    }
                }
            }

            foreach (var order in prevOrders)
            {
                string storeName = stores.FirstOrDefault(s => s.Store_ID == order.StoreId).StoreName;
                prevTotSales += order.GrandTotal;
                List<OrderItems> oItems = new List<OrderItems>();
                oItems = orderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                foreach (var oItem in oItems)
                {
                    if (oItem != null)
                    {
                        Product prod = products.FirstOrDefault(p => p.Product_ID == oItem.Product_ID);
                        prevTotCost += prod.CostPrice;
                        prevTotProdSold += oItem.Quantity;
                    }
                }
            }

            foreach (Order order in orderS)
            {
                if (!orderStatusAmounts.ContainsKey(order.OrderStatus))
                {
                    orderStatusAmounts.Add(order.OrderStatus, totOrderStatus);
                }
                else
                {
                    int orderStatusAmount = orderStatusAmounts[order.OrderStatus];
                    orderStatusAmounts[order.OrderStatus] = orderStatusAmount + totOrderStatus;
                }
            }
            TotProdSold = totProdSold;
            if (GetPercentage(totProdSold, prevTotProdSold) > 0)
            {
                TotProdSoldPerc = string.Format("{0}% Increase from last Month", GetPercentage(totProdSold, prevTotProdSold));
            }
            else if (GetPercentage(totProdSold, prevTotProdSold) < 0)
            {
                TotProdSoldPerc = string.Format("{0}% Decrease from last Month", GetPercentage(totProdSold, prevTotProdSold));
            }
            else
            {
                TotProdSoldPerc = string.Format("No Change from last Month");
            }
            TotOrders = orders.Count();
            if (GetPercentage(orders.Count(), prevOrders.Count()) > 0)
            {
                TotOrdersPerc = string.Format("{0}% Increase from last Month", GetPercentage(orders.Count(), prevOrders.Count()));
            }
            else if (GetPercentage(orders.Count(), prevOrders.Count()) < 0)
            {
                TotOrdersPerc = string.Format("{0}% Decrease from last Month", GetPercentage(orders.Count(), prevOrders.Count()));
            }
            else
            {
                TotOrdersPerc = string.Format("No Change from last Month");
            }
            TotOrdersVoided = voidOrders.Count();
            if (GetPercentage(voidOrders.Count(), prevVoidOrders.Count()) > 0)
            {
                TotOrdersVoidedPerc = string.Format("{0}% Increase from last Month", GetPercentage(voidOrders.Count(), prevVoidOrders.Count()));
            }
            else if (GetPercentage(orders.Count(), prevOrders.Count()) < 0)
            {
                TotOrdersVoidedPerc = string.Format("{0}% Decrease from last Month", GetPercentage(voidOrders.Count(), prevVoidOrders.Count()));
            }
            else
            {
                TotOrdersVoidedPerc = string.Format("No Change from last Month");
            }
            TotSales = totSales;
            if (GetPercentage(totSales, prevTotSales) > 0)
            {
                TotSalesPerc = string.Format("{0}% Increase from last Month", GetPercentage(totSales, prevTotSales));
            }
            else if (GetPercentage(totSales, prevTotSales) < 0)
            {
                TotSalesPerc = string.Format("{0}% Decrease from last Month", GetPercentage(totSales, prevTotSales));
            }
            else
            {
                TotSalesPerc = string.Format("No Change from last Month");
            }
            TotProfit = totSales - totCost;
            if (GetPercentage((totSales - totCost), (prevTotSales - prevTotCost)) > 0)
            {
                TotProfitPerc = string.Format("{0}% Increase from last Month", GetPercentage((totSales - totCost), (prevTotSales - prevTotCost)));
            }
            else if (GetPercentage((totSales - totCost), (prevTotSales - prevTotCost)) < 0)
            {
                TotProfitPerc = string.Format("{0}% Decrease from last Month", GetPercentage((totSales - totCost), (prevTotSales - prevTotCost)));
            }
            else
            {
                TotProfitPerc = string.Format("No Change from last Month");
            }
            TotCost = totCost;
            if (GetPercentage(totCost, prevTotCost) > 0)
            {
                TotCostPerc = string.Format("{0}% Increase from last Month", GetPercentage(totCost, prevTotCost));
            }
            else if (GetPercentage(totCost, prevTotCost) < 0)
            {
                TotCostPerc = string.Format("{0}% Decrease from last Month", GetPercentage(totCost, prevTotCost));
            }
            else
            {
                TotCostPerc = string.Format("No Change from last Month");
            }
            StoreCats = JsonConvert.SerializeObject(storeAmounts.Keys.ToArray());
            StoreData = JsonConvert.SerializeObject(storeAmounts.Values.ToArray());
            UserCats = JsonConvert.SerializeObject(userAmounts.Keys.ToArray());
            UserData = JsonConvert.SerializeObject(userAmounts.Values.ToArray());
            CategoryCats = JsonConvert.SerializeObject(catAmounts.Keys.ToArray());
            CategoryData = JsonConvert.SerializeObject(catAmounts.Values.ToArray());

            foreach(DateTime date in AllDatesInMonth(year, month))
            {
                List<Order> orderObjects = new List<Order>();
                orderObjects = orders.Where(o => o.OrderDate >= new DateTime(date.Year, date.Month, date.Day, 0, 0, 0) && o.OrderDate <= new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)).ToList();
                ordersAmounts.Add(date.ToString("dd"), orderObjects.Count);
                if (orderObjects.Count != 0)
                {
                    foreach (Order ord in orderObjects)
                    {
                        decimal grandTotal = 0;
                        if (!salesAmounts.ContainsKey(date.ToString("dd")))
                        {
                            salesAmounts.Add(date.ToString("dd"), ord.GrandTotal);
                            grandTotal = ord.GrandTotal;
                        }
                        else
                        {
                            decimal salesAmount = salesAmounts[date.ToString("dd")];
                            grandTotal = salesAmount + ord.GrandTotal;
                            salesAmounts[date.ToString("dd")] = salesAmount + ord.GrandTotal;
                        }
                        List<OrderItems> oItems = new List<OrderItems>();
                        oItems = orderItems.Where(oi => oi.OrderId == ord.OrderId).ToList();
                        decimal totCost2 = 0;
                        foreach(var oItem in oItems)
                        {
                            Product prod = products.FirstOrDefault(p => p.Product_ID == oItem.Product_ID);
                            totCost2 += prod.CostPrice;
                        }
                        if (!profitAmounts.ContainsKey(date.ToString("dd")))
                        {
                            decimal profamount = grandTotal - totCost2;
                            profitAmounts.Add(date.ToString("dd"), profamount);
                        }
                        else
                        {
                            decimal tot2Amount = profitAmounts[date.ToString("dd")];
                            decimal profamount = grandTotal - (totCost2 + tot2Amount);
                            profitAmounts[date.ToString("dd")] = profamount;
                        }                        
                    }
                }
                else
                {
                    salesAmounts.Add(date.ToString("dd"), 0);
                    profitAmounts.Add(date.ToString("dd"), 0);
                }
            }
            SalesCats = JsonConvert.SerializeObject(salesAmounts.Keys.ToArray());
            SalesData = JsonConvert.SerializeObject(salesAmounts.Values.ToArray());

            ProfitCats = JsonConvert.SerializeObject(profitAmounts.Keys.ToArray());
            ProfitData = JsonConvert.SerializeObject(profitAmounts.Values.ToArray());

            OrdersCats = JsonConvert.SerializeObject(ordersAmounts.Keys.ToArray());
            OrdersData = JsonConvert.SerializeObject(ordersAmounts.Values.ToArray());

            OrderStatusCats = JsonConvert.SerializeObject(orderStatusAmounts.Keys.ToArray());
            OrderStatusData = JsonConvert.SerializeObject(orderStatusAmounts.Values.ToArray());

            AllOrders = _context.Orders.ToList();
            AllStores = _context.Stores.ToList();
            MonthSelected = new DateTime(DateTime.Today.Year, month, 1, 0, 0, 0).ToString("MMMM");

            MonthNames = Months.Select(n => new SelectListItem
            {
                Value = n,
                Text = n
            }).ToList();

            List<string> yearsList = new List<string>();
            foreach (var order in orderS)
            {
                if (!yearsList.Contains(order.OrderDate.ToString("yyyy")))
                {
                    yearsList.Add(order.OrderDate.ToString("yyyy"));
                }
            }
            Years = yearsList.Select(n => new SelectListItem
            {
                Value = n,
                Text = n
            }).ToList();
            SelectedYear = year.ToString();
            YearSelected = year.ToString();
            SelectedMonth = ddate.ToString("MMMM");

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = orders.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Orders = orders
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            ShowHide = "block";

            ThisPageModel model = new ThisPageModel
            {
                TotProdSold = TotProdSold,
                MonthSelected = MonthSelected,
                YearSelected = YearSelected,
                TotProdSoldPerc = TotProdSoldPerc,
                TotOrders = TotOrders,
                TotOrdersPerc = TotOrdersPerc,
                TotOrdersVoided = TotOrdersVoided,
                TotOrdersVoidedPerc = TotOrdersVoidedPerc,
                TotSales = TotSales,
                TotSalesPerc = TotSalesPerc,
                TotCost = TotCost,
                TotCostPerc = TotCostPerc,
                TotProfit = TotProfit,
                TotProfitPerc = TotProfitPerc,
                Orders = FilteredOrders,
                AllStores = AllStores,
                StoreCats = StoreCats,
                StoreData = StoreData,
                UserCats = UserCats,
                UserData = UserData,
                CategoryCats = CategoryCats,
                CategoryData = CategoryData,
                SalesCats = SalesCats,
                SalesData = SalesData,
                ProfitCats = ProfitCats,
                ProfitData = ProfitData,
                OrdersCats = OrdersCats,
                OrdersData = OrdersData,
                OrderStatusCats = OrderStatusCats,
                OrderStatusData = OrderStatusData
            };
            thisPageModel = new ThisPageModel();
            thisPageModel = model;
        }
        #endregion

        #region Supporting Methods
        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

        public double GetPercentage(int currentMonth, int prevMonth)
        {
            double perc = 0;
            if (currentMonth != 0 && prevMonth != 0)
            {
                var change = currentMonth - prevMonth;
                var measure = change / prevMonth;
                perc = measure * 100;
            }
            perc = Math.Round(perc, 2);
            return perc;
        }

        public decimal GetPercentage(decimal currentMonth, decimal prevMonth)
        {
            decimal perc = 0;
            if (currentMonth != 0 && prevMonth != 0)
            {
                var change = currentMonth - prevMonth;
                var measure = change / prevMonth;
                perc = measure * 100;
            }
            perc = Math.Round(perc, 2);
            return perc;
        }
        #endregion

        #region OnPost
        public IActionResult OnPost(int currentPage, int pageSize, string month)
        {
            int monthInt = 0;
            if (!string.IsNullOrEmpty(month))
            {
                monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(month) + 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedMonth))
                {
                    monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(SelectedMonth) + 1;
                }
            }
            int year = 0;
            if (!string.IsNullOrEmpty(SelectedYear))
            {
                year = Convert.ToInt32(SelectedYear);
            }
            else
            {
                year = Convert.ToInt32(YearSelected);
            }

            PopulatePage(monthInt, year, currentPage, pageSize);
            return Page();
        }

        public IActionResult OnPostPrint(int currentPage, int pageSize, string month, string year)
        {
            int monthInt = 0;
            if (!string.IsNullOrEmpty(month))
            {
                monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(month) + 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedMonth))
                {
                    monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(SelectedMonth) + 1;
                }
            }
            int yearInt = 0;
            if (!string.IsNullOrEmpty(year))
            {
                yearInt = Convert.ToInt32(year);
            }
            else
            { 
                if (!string.IsNullOrEmpty(SelectedYear))
                {
                    yearInt = Convert.ToInt32(SelectedYear);
                }
                else
                {
                    yearInt = Convert.ToInt32(YearSelected);
                }
            }

            PopulatePage(monthInt, yearInt, currentPage, pageSize);
            return Page();
        }

        public IActionResult OnPostExport(int currentPage, int pageSize, string month, string year)
        {
            int monthInt = 0;
            if (!string.IsNullOrEmpty(month))
            {
                monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(month) + 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedMonth))
                {
                    monthInt = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(SelectedMonth) + 1;
                }
            }

            int yearInt = Convert.ToInt32(year);

            PopulatePage(monthInt, yearInt, currentPage, pageSize);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Monthly_Sales_Report.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();

                package.Workbook.Worksheets.Add("Figures");
                ExcelWorksheet figuresWorkSheet = package.Workbook.Worksheets["Figures"];
                var productsShape = figuresWorkSheet.Drawings.AddShape("productsShape", eShapeStyle.Rect);
                productsShape.SetPosition(1, 5, 1, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                productsShape.SetSize(350, 150);             //Size in pixels
                productsShape.Text = string.Format("Total Products Sold{0}{1}{2}{3}", Environment.NewLine, TotProdSold, Environment.NewLine, TotProdSoldPerc);
                productsShape.Font.Size = 18;
                productsShape.Fill.Color = Color.FromArgb(0, 192, 239);

                var ordersShape = figuresWorkSheet.Drawings.AddShape("ordersShape", eShapeStyle.Rect);
                ordersShape.SetPosition(1, 5, 9, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                ordersShape.SetSize(350, 150);             //Size in pixels
                ordersShape.Text = string.Format("Total Orders{0}{1}{2}{3}", Environment.NewLine, TotOrders, Environment.NewLine, TotOrdersPerc);
                ordersShape.Font.Size = 18;
                ordersShape.Fill.Color = Color.FromArgb(0, 192, 239);

                var ordersVoidedShape = figuresWorkSheet.Drawings.AddShape("ordersVoidedShape", eShapeStyle.Rect);
                ordersVoidedShape.SetPosition(1, 5, 17, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                ordersVoidedShape.SetSize(350, 150);             //Size in pixels
                ordersVoidedShape.Text = string.Format("Total Orders Voided{0}{1}{2}{3}", Environment.NewLine, TotOrdersVoided, Environment.NewLine, TotOrdersVoidedPerc);
                ordersVoidedShape.Font.Size = 18;
                ordersVoidedShape.Fill.Color = Color.FromArgb(221, 75, 57);

                var salesShape = figuresWorkSheet.Drawings.AddShape("salesShape", eShapeStyle.Rect);
                salesShape.SetPosition(10, 5, 1, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                salesShape.SetSize(350, 150);             //Size in pixels
                salesShape.Text = string.Format("Total Sales{0}R {1}{2}{3}", Environment.NewLine, TotSales, Environment.NewLine, TotSalesPerc);
                salesShape.Font.Size = 18;
                salesShape.Fill.Color = Color.FromArgb(0, 115, 183);

                var costShape = figuresWorkSheet.Drawings.AddShape("costShape", eShapeStyle.Rect);
                costShape.SetPosition(10, 5, 9, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                costShape.SetSize(350, 150);             //Size in pixels
                costShape.Text = string.Format("Total Cost{0}R {1}{2}{3}", Environment.NewLine, TotCost, Environment.NewLine, TotCostPerc);
                costShape.Font.Size = 18;
                costShape.Fill.Color = Color.FromArgb(255, 133, 27);

                var profitShape = figuresWorkSheet.Drawings.AddShape("profitShape", eShapeStyle.Rect);
                profitShape.SetPosition(10, 5, 17, 5);       //Position Row, RowOffsetPixels, Column, ColumnOffsetPixels
                profitShape.SetSize(350, 150);             //Size in pixels
                profitShape.Text = string.Format("Total Profit{0}R {1}{2}{3}", Environment.NewLine, TotProfit, Environment.NewLine, TotProfitPerc);
                profitShape.Font.Size = 18;
                profitShape.Fill.Color = Color.FromArgb(0, 166, 90);

                package.Workbook.Worksheets.Add("By Store");
                ExcelWorksheet storeWorkSheet = package.Workbook.Worksheets["By Store"];
                ExcelChart storeChart = storeWorkSheet.Drawings.AddChart("chart", eChartType.Pie);
                storeChart.SetPosition(1, 0, 5, 0);
                storeChart.Legend.Remove();
                storeChart.Title.Text = "By Store";
                storeChart.Title.Font.Bold = true;
                storeChart.Title.Font.Size = 12;
                storeChart.SetSize(500, 400);
                storeWorkSheet.Cells[1, 1].LoadFromCollection(storeAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var storeWorkSheetSeries = storeChart.Series.Add(("B1:" + "B" + (storeAmounts.Count + 1)), ("A1:" + "A" + (storeAmounts.Count + 1)));
                var storeSeries = (ExcelPieChartSerie)storeWorkSheetSeries;
                storeSeries.DataLabel.ShowCategory = true;
                storeSeries.DataLabel.ShowPercent = true;
                storeSeries.DataLabel.ShowValue = true;
                storeSeries.DataLabel.ShowLeaderLines = true;
                storeSeries.DataLabel.Separator = "- ";
                storeSeries.DataLabel.Position = eLabelPosition.OutEnd;

                package.Workbook.Worksheets.Add("By User");
                ExcelWorksheet userWorkSheet = package.Workbook.Worksheets["By User"];
                ExcelChart userChart = userWorkSheet.Drawings.AddChart("chart", eChartType.Pie);
                userChart.SetPosition(1, 0, 5, 0);
                userChart.Legend.Remove();
                userChart.Title.Text = "By User";
                userChart.Title.Font.Bold = true;
                userChart.Title.Font.Size = 12;
                userChart.SetSize(500, 400);
                userWorkSheet.Cells[1, 1].LoadFromCollection(userAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var userWorkSheetSeries = userChart.Series.Add(("B1:" + "B" + (userAmounts.Count + 1)), ("A1:" + "A" + (userAmounts.Count + 1)));
                var userSeries = (ExcelPieChartSerie)userWorkSheetSeries;
                userSeries.DataLabel.ShowCategory = true;
                userSeries.DataLabel.ShowPercent = true;
                userSeries.DataLabel.ShowValue = true;
                userSeries.DataLabel.ShowLeaderLines = true;
                userSeries.DataLabel.Separator = "- ";
                userSeries.DataLabel.Position = eLabelPosition.OutEnd;

                package.Workbook.Worksheets.Add("By Category");
                ExcelWorksheet categoryWorkSheet = package.Workbook.Worksheets["By Category"];
                ExcelChart categoryChart = categoryWorkSheet.Drawings.AddChart("chart", eChartType.ColumnClustered);
                categoryChart.Title.Text = "By Category";
                categoryChart.XAxis.Title.Text = "Categories";
                categoryChart.YAxis.Title.Text = "Amounts";
                categoryChart.SetSize(1200, 300);
                categoryChart.SetPosition(1, 0, 5, 0);
                categoryChart.Legend.Remove();
                categoryWorkSheet.Cells[1, 1].LoadFromCollection(categoryAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var workSheetSeries = categoryChart.Series.Add(("B1:" + "B" + (categoryAmounts.Count + 1)), ("A1:" + "A" + (categoryAmounts.Count + 1)));
                var categorySeries = (ExcelBarChartSerie)workSheetSeries;
                categorySeries.DataLabel.Position = eLabelPosition.OutEnd;
                categorySeries.DataLabel.ShowPercent = false;
                categorySeries.DataLabel.ShowValue = true;
                categorySeries.DataLabel.Font.Bold = true;

                package.Workbook.Worksheets.Add("Sales Overview");
                ExcelWorksheet salesWorkSheet = package.Workbook.Worksheets["Sales Overview"];
                ExcelChart salesChart = salesWorkSheet.Drawings.AddChart("chart", eChartType.Line);
                salesChart.Title.Text = "Sales Overview";
                salesChart.XAxis.Title.Text = "Dates";
                salesChart.YAxis.Title.Text = "Amounts";
                salesChart.SetSize(1200, 300);
                salesChart.SetPosition(1, 0, 5, 0);
                salesChart.Legend.Remove();
                salesWorkSheet.Cells[1, 1].LoadFromCollection(salesAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var salesWorkSheetSeries = salesChart.Series.Add(("B1:" + "B" + (salesAmounts.Count + 1)), ("A1:" + "A" + (salesAmounts.Count + 1)));
                var salesSeries = (ExcelLineChartSerie)salesWorkSheetSeries;
                salesSeries.DataLabel.Position = eLabelPosition.OutEnd;
                salesSeries.DataLabel.ShowPercent = false;
                salesSeries.DataLabel.ShowValue = true;
                salesSeries.DataLabel.Font.Bold = true;

                package.Workbook.Worksheets.Add("Profit");
                ExcelWorksheet profitWorkSheet = package.Workbook.Worksheets["Profit"];
                ExcelChart profitChart = profitWorkSheet.Drawings.AddChart("chart", eChartType.ColumnClustered);
                profitChart.Title.Text = "Profit";
                profitChart.XAxis.Title.Text = "Dates";
                profitChart.YAxis.Title.Text = "Amounts";
                profitChart.SetSize(1200, 300);
                profitChart.SetPosition(1, 0, 5, 0);
                profitChart.Legend.Remove();
                profitWorkSheet.Cells[1, 1].LoadFromCollection(profitAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var profitWorkSheetSeries = profitChart.Series.Add(("B1:" + "B" + (profitAmounts.Count + 1)), ("A1:" + "A" + (profitAmounts.Count + 1)));
                var profitSeries = (ExcelBarChartSerie)profitWorkSheetSeries;
                profitSeries.DataLabel.Position = eLabelPosition.OutEnd;
                profitSeries.DataLabel.ShowPercent = false;
                profitSeries.DataLabel.ShowValue = true;
                profitSeries.DataLabel.Font.Bold = true;

                package.Workbook.Worksheets.Add("Orders");
                ExcelWorksheet ordersWorkSheet = package.Workbook.Worksheets["Orders"];
                ExcelChart ordersChart = ordersWorkSheet.Drawings.AddChart("chart", eChartType.Line);
                ordersChart.Title.Text = "Orders";
                ordersChart.XAxis.Title.Text = "Dates";
                ordersChart.YAxis.Title.Text = "Amounts";
                ordersChart.SetSize(1200, 300);
                ordersChart.SetPosition(1, 0, 5, 0);
                ordersChart.Legend.Remove();
                ordersWorkSheet.Cells[1, 1].LoadFromCollection(ordersAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var ordersWorkSheetSeries = ordersChart.Series.Add(("B1:" + "B" + (ordersAmounts.Count + 1)), ("A1:" + "A" + (ordersAmounts.Count + 1)));
                var ordersSeries = (ExcelLineChartSerie)ordersWorkSheetSeries;
                ordersSeries.DataLabel.Position = eLabelPosition.OutEnd;
                ordersSeries.DataLabel.ShowPercent = false;
                ordersSeries.DataLabel.ShowValue = true;
                ordersSeries.DataLabel.Font.Bold = true;

                package.Workbook.Worksheets.Add("By Order Status");
                ExcelWorksheet orderStatusWorkSheet = package.Workbook.Worksheets["By Order Status"];
                ExcelChart orderStatusChart = orderStatusWorkSheet.Drawings.AddChart("chart", eChartType.Pie);
                orderStatusChart.SetPosition(1, 0, 5, 0);
                orderStatusChart.Legend.Remove();
                orderStatusChart.Title.Text = "By User";
                orderStatusChart.Title.Font.Bold = true;
                orderStatusChart.Title.Font.Size = 12;
                orderStatusChart.SetSize(500, 400);
                orderStatusWorkSheet.Cells[1, 1].LoadFromCollection(orderStatusAmounts, false, OfficeOpenXml.Table.TableStyles.Medium1);
                var orderStatusWorkSheetSeries = orderStatusChart.Series.Add(("B1:" + "B" + (orderStatusAmounts.Count + 1)), ("A1:" + "A" + (orderStatusAmounts.Count + 1)));
                var orderStatusSeries = (ExcelPieChartSerie)orderStatusWorkSheetSeries;
                orderStatusSeries.DataLabel.ShowCategory = true;
                orderStatusSeries.DataLabel.ShowPercent = true;
                orderStatusSeries.DataLabel.ShowValue = true;
                orderStatusSeries.DataLabel.ShowLeaderLines = true;
                orderStatusSeries.DataLabel.Separator = "- ";
                orderStatusSeries.DataLabel.Position = eLabelPosition.OutEnd;

                package.Workbook.Worksheets.Add("Order Data");
                ExcelWorksheet orderDataSheet = package.Workbook.Worksheets["Order Data"];
                using (var range = orderDataSheet.Cells["A1:K1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(51, 122, 183));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                orderDataSheet.Cells[1, 1].Value = "Order Status";
                orderDataSheet.Cells[1, 2].Value = "Prep Status";
                orderDataSheet.Cells[1, 3].Value = "Order Id";
                orderDataSheet.Cells[1, 4].Value = "Payment Status";
                orderDataSheet.Cells[1, 5].Value = "Payment Method";
                orderDataSheet.Cells[1, 6].Value = "Order Date";
                orderDataSheet.Cells[1, 7].Value = "Paid";
                orderDataSheet.Cells[1, 8].Value = "Change";
                orderDataSheet.Cells[1, 9].Value = "Order Discount";
                orderDataSheet.Cells[1, 10].Value = "Sub Total";
                orderDataSheet.Cells[1, 11].Value = "VAT Total";
                orderDataSheet.Cells[1, 12].Value = "Grand Total";
                orderDataSheet.Cells[1, 13].Value = "User";
                orderDataSheet.Cells[1, 14].Value = "Store Name";
                orderDataSheet.Cells[1, 1, 1, 14].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                int count = 0;
                for (int r = 0; r < Orders.Count(); r++)
                {
                    orderDataSheet.Cells[r + count + 2, 1].Value = Orders.ToList()[r].OrderStatus;
                    orderDataSheet.Cells[r + count + 2, 2].Value = Orders.ToList()[r].PreparationStatus;
                    orderDataSheet.Cells[r + count + 2, 3].Value = Orders.ToList()[r].OrderId;
                    orderDataSheet.Cells[r + count + 2, 4].Value = Orders.ToList()[r].PaymentStatus;
                    orderDataSheet.Cells[r + count + 2, 5].Value = Orders.ToList()[r].PaymentMethod;
                    orderDataSheet.Cells[r + count + 2, 6].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                    orderDataSheet.Cells[r + count + 2, 6].Value = Orders.ToList()[r].OrderDate;
                    orderDataSheet.Cells[r + count + 2, 7].Value = Orders.ToList()[r].Paid;
                    orderDataSheet.Cells[r + count + 2, 8].Value = Orders.ToList()[r].Change;
                    orderDataSheet.Cells[r + count + 2, 9].Value = Orders.ToList()[r].OrderDiscount;
                    orderDataSheet.Cells[r + count + 2, 10].Value = Orders.ToList()[r].OrderTotal;
                    orderDataSheet.Cells[r + count + 2, 11].Value = Orders.ToList()[r].VatTotal;
                    orderDataSheet.Cells[r + count + 2, 12].Value = Orders.ToList()[r].GrandTotal;
                    orderDataSheet.Cells[r + count + 2, 13].Value = Orders.ToList()[r].Username;
                    orderDataSheet.Cells[r + count + 2, 14].Value = _context.Stores.FirstOrDefault(s => s.Store_ID == Orders.ToList()[r].StoreId).StoreName;
                    orderDataSheet.Cells[1, 1, 1, 14].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        #endregion
    }
}