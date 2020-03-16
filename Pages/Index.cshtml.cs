using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<IndexModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        #region Properties
        public IList<Store> Stores { get; set; }
        public IList<Order> Orders { get; set; }
        public IList<Change> ChangeLog { get; set; }
        public List<StockItem> LowStockItems { get; set; }
        public IList<Form> Forms { get; set; }
        public IList<Feedback> Feedbacks { get; set; }
        public IList<Customer> Customers { get; set; }
        public IList<Answer> Answers { get; set; }
        public IList<Question> Questions { get; set; }
        public TopCountsViewModel TopCountsViewModel { get; set; }
        public OrdersBarChartViewModel OrdersBarChartViewModel { get; set; }
        public OrderTotal OrderTotals { get; set; }
        public BestSeller BestSellers { get; set; }
        [BindProperty(SupportsGet=true)]
        public string Metrics { get; set; }
        #endregion

        #region OnGet
        public void OnGet()
        {
            // Notifications
            //TempData["StatusMessage"] = "Page Loaded";
            ChangeLog = _context.ChangeLog.ToList();
            Metrics = "Weekly";
        }
        #endregion

        #region Supporting Methods
        public int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public decimal AllTotalAmounts(string status, string thisDate)
        {
            DateTime today = DateTime.Today;
            DateTime startOfToday = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            DateTime endOfToday = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);
            decimal total = 0;
            List<Order> listOrders = new List<Order>();

            if (thisDate == "AllTime")
            {
                if (status == "Pending")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus != "Completed").ToList();
                }
                else if (status == "Completed")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus == "Completed").ToList();
                }

            }
            if (thisDate == "ThisYear")
            {
                if (status == "Pending")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, 1, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
                }
                else if (status == "Completed")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, 1, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
                }
            }
            if (thisDate == "ThisMonth")
            {
                if (status == "Pending")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, today.Month, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
                }
                else if (status == "Completed")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, today.Month, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
                }
            }
            if (thisDate == "ThisWeek")
            {
                if (status == "Pending")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= FirstDateOfWeekISO8601(today.Year, GetIso8601WeekOfYear(today)) && o.OrderDate <= endOfToday).ToList();
                }
                else if (status == "Completed")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= FirstDateOfWeekISO8601(today.Year, GetIso8601WeekOfYear(today)) && o.OrderDate <= endOfToday).ToList();
                }
            }
            if (thisDate == "Today")
            {
                if (status == "Pending")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= startOfToday && o.OrderDate <= endOfToday).ToList();
                }
                else if (status == "Completed")
                {
                    listOrders = _context.Orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= startOfToday && o.OrderDate <= endOfToday).ToList();
                }
            }

            foreach (Order order in listOrders)
            {
                total = total + order.GrandTotal;
            }

            return total;
        }
        #endregion

        #region Yearly
        public Dictionary<string, int> YearlyChartData(DateTime start, DateTime end)
        {
            Dictionary<string, int> allValues = new Dictionary<string, int>();
            List<string> dateStringRange = new List<string>();
            List<DateTime> dateRange = new List<DateTime>();
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i == 1)
                    {
                        dateStringRange.Add(start.ToString("yyyy/MM"));
                    }
                    else
                    {
                        dateStringRange.Add(start.AddMonths(i).ToString("yyyy/MM"));
                    }
                }
                foreach (string dateString in dateStringRange)
                {
                    dateRange.Add(Convert.ToDateTime(dateString));
                }
                foreach (DateTime date in dateRange)
                {
                    int count = 0;
                    DateTime startDateMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
                    DateTime endDateMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
                    count = _context.Orders.Where(o => o.OrderDate >= startDateMonth && o.OrderDate <= endDateMonth && o.OrderStatus == "Completed").Count();
                    allValues.Add(endDateMonth.ToString("MMM yy"), count);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Yearly Chart Data");
            }
            return allValues;
        }
        #endregion

        #region WeeklyMonthly

        public DateTime[] GetDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            try
            {                
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    allDates.Add(date);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Get Dates Between");
            }
            return allDates.ToArray();
        }

        public Dictionary<string, int> WeeklyChartData(DateTime start, DateTime end)
        {
            DateTime today = DateTime.Today;
            Dictionary<string, int> allValues = new Dictionary<string, int>();
            try
            {
                foreach (DateTime date in GetDatesBetween(start, end))
                {
                    int count = 0;
                    DateTime startDateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endDateTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    count = _context.Orders.Where(o => o.OrderDate >= startDateTime && o.OrderDate <= endDateTime && o.OrderStatus == "Completed").Count();
                    allValues.Add(date.ToString("yy-MM-dd"), count);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Getting Weekly Data for Chart");
            }
            return allValues;
        }

        public Dictionary<string, int> FeedbackData()
        {
            DateTime today = DateTime.Today;
            Dictionary<string, int> allValues = new Dictionary<string, int>();
            DateTime start = new DateTime();
            switch (Metrics)
            {
                case "Daily":
                    start = DateTime.Today;
                    break;
                case "Weekly":
                    start = DateTime.Today.AddDays(-7);
                    break;
                case "Monthly":
                    start = DateTime.Today.AddMonths(-1);
                    break;
                case "Overall":
                    start = new DateTime(2018, 01, 01, 00, 00, 00);
                    break;
                default:
                    start = DateTime.Today.AddDays(-7);
                    break;
            }
            try
            {
                foreach (DateTime date in GetDatesBetween(start, today))
                {
                    int count = 0;
                    DateTime startDateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime endDateTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    count = _context.Feedbacks.Where(o => o.SubmittedDate >= startDateTime && o.SubmittedDate <= endDateTime).Count();
                    allValues.Add(date.ToString("yy-MM-dd"), count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Getting Weekly Data for Chart");
            }
            return allValues;
        }
        #endregion

        #region GetTopCounts
        public TopCountsViewModel GetTopCounts()
        {
            Orders = _context.Orders.ToList();
            TopCountsViewModel topCountsViewModel = new TopCountsViewModel
            {
                AllOrdersCount = Orders.Where(o => o.OrderDate >= DateTime.Today.AddDays(-7) && o.OrderDate <= DateTime.Today).Count(),
                IncompleteOrderCount = Orders.Where(o => o.OrderStatus == "Incompleted" && o.OrderDate >= DateTime.Today.AddDays(-7) && o.OrderDate <= DateTime.Today).Count(),
                LowStockCount = _context.Stock.Where(s => s.InStock <= 10m).Count(),
                VoidedOrdersCount = Orders.Where(o => o.OrderStatus == "Voided" && o.OrderDate >= DateTime.Today.AddDays(-7) && o.OrderDate <= DateTime.Today).Count(),
                Feedbacks = _context.Feedbacks.Where(f => f.SubmittedDate >= DateTime.Today.AddDays(-7) && f.SubmittedDate >= DateTime.Today).Count(),
                Customers = _context.Customers.Where(c => c.ForFeedback == "Yes").Count(),
                TotalNegative = _context.Feedbacks.Where(f => f.Status == "NEGATIVE" && f.SubmittedDate >= DateTime.Today.AddDays(-7) && f.SubmittedDate >= DateTime.Today).Count(),
                TotalPositive = _context.Feedbacks.Where(f => f.Status == "POSITIVE" && f.SubmittedDate >= DateTime.Today.AddDays(-7) && f.SubmittedDate >= DateTime.Today).Count()
            };

            return topCountsViewModel;
        }
        public PartialViewResult OnGetTopCountsView()
        {
            return new PartialViewResult
            {
                ViewName = "_TopCounts",
                ViewData = new ViewDataDictionary<TopCountsViewModel>(ViewData, GetTopCounts())
            };
        }
        #endregion

        #region GetOrderBarChart
        public IActionResult OnGetOrdersBarChartView()
        {
            DateTime start = new DateTime();
            switch(Metrics)
            {
                case "Daily": 
                    start = DateTime.Today;
                    break;
                case "Weekly":
                    start = DateTime.Today.AddDays(-7);
                    break;
                case "Monthly":
                    start = DateTime.Today.AddMonths(-1);
                    break;
                case "Overall":
                    start = new DateTime(2018, 01, 01, 00, 00, 00);
                    break;
                default:
                    start = DateTime.Today.AddDays(-7);
                    break;
            }

            object[] result = new object[2];
            result[0] = WeeklyChartData(start, DateTime.Today).Keys;
            result[1] = WeeklyChartData(start, DateTime.Today).Values;
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
        #endregion

        #region GetFeedbackBarChart
        public IActionResult OnGetFeedbackBarChartView()
        {
            object[] result = new object[2];
            result[0] = FeedbackData().Keys;
            result[1] = FeedbackData().Values;
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
        #endregion

        #region LatestOrdersView
        public PartialViewResult OnGetLatestOrdersView()
        {
            return new PartialViewResult
            {
                ViewName = "_LatestOrders",
                ViewData = new ViewDataDictionary<List<Order>>(ViewData, _context.Orders.OrderByDescending(s => s).Take(3).ToList())
            };
        }
        #endregion

        #region GetLowStockItems()
        public List<LowStockItem> GetLowStockItems()
        {
            Stores = _context.Stores.ToList();
            LowStockItems = _context.Stock.Where(s => s.InStock <= 10m).ToList();
            List<LowStockItem> lowStockItems = new List<LowStockItem>();
            foreach(var lsi in LowStockItems)
            {
                LowStockItem lowStockItem = new LowStockItem
                {
                    StoreName = Stores.FirstOrDefault(s => s.Store_ID == lsi.StoreId).StoreName,
                    StockName = lsi.StockName,
                    InStock = lsi.InStock
                };
                lowStockItems.Add(lowStockItem);
            }

            return lowStockItems;
        }

        public PartialViewResult OnGetLowStockView()
        {
            return new PartialViewResult
            {
                ViewName = "_LowStockList",
                ViewData = new ViewDataDictionary<List<LowStockItem>>(ViewData, GetLowStockItems())
            };
        }
        #endregion

        #region GetOrderTotals
        public OrderTotal GetOrderTotals()
        {
            DateTime today = DateTime.Today;
            DateTime startOfToday = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
            DateTime endOfToday = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59);

            var orders = from p in _context.Orders
                         select p;

            var pendingTodayOrders = orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= startOfToday && o.OrderDate <= endOfToday).ToList();
            var pendingWeekOrders = orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= FirstDateOfWeekISO8601(today.Year, GetIso8601WeekOfYear(today)) && o.OrderDate <= endOfToday).ToList();
            var pendingMonthOrders = orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, today.Month, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
            var pendingYearOrders = orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, 1, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
            var pendingAllOrders = orders.Where(o => o.OrderStatus != "Completed").ToList();
            var completedTodayOrders = orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= startOfToday && o.OrderDate <= endOfToday).ToList();
            var completedWeekOrders = orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= FirstDateOfWeekISO8601(today.Year, GetIso8601WeekOfYear(today)) && o.OrderDate <= endOfToday).ToList();
            var completedMonthOrders = orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, today.Month, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
            var completedYearOrders = orders.Where(o => o.OrderStatus == "Completed" && o.OrderStatus != "Voided" && o.OrderDate >= new DateTime(today.Year, 1, 1, 0, 0, 0) && o.OrderDate <= endOfToday).ToList();
            var completedAllOrders = orders.Where(o => o.OrderStatus == "Completed").ToList();

            OrderTotal orderTotals = new OrderTotal
            {
                PendingToday = TotalOrderGrandTotals(pendingTodayOrders),
                PendingWeek = TotalOrderGrandTotals(pendingWeekOrders),
                PendingMonth = TotalOrderGrandTotals(pendingMonthOrders),
                PendingYear = TotalOrderGrandTotals(pendingYearOrders),
                PendingAllTime = TotalOrderGrandTotals(pendingAllOrders),
                CompletedToday = TotalOrderGrandTotals(completedTodayOrders),
                CompletedWeek = TotalOrderGrandTotals(completedWeekOrders),
                CompletedMonth = TotalOrderGrandTotals(completedMonthOrders),
                CompletedYear = TotalOrderGrandTotals(completedYearOrders),
                CompletedAllTime = TotalOrderGrandTotals(completedAllOrders)
            };

            return orderTotals;
        }
        public decimal TotalOrderGrandTotals(IList<Order> listOrders)
        {
            decimal total = 0;
            foreach (Order order in listOrders)
            {
                total = total + order.GrandTotal;
            }
            return total;
        }

        public PartialViewResult OnGetOrderTotalsView()
        {
            return new PartialViewResult
            {
                ViewName = "_OrderTotals",
                ViewData = new ViewDataDictionary<OrderTotal>(ViewData, GetOrderTotals())
            };
        }
        #endregion

        #region BestSellers
        public List<BestSeller> GetBestsellers()
        {
            var products = from p in _context.Products
                           select p;
            List<BestSeller> bestSellers = new List<BestSeller>();
            Dictionary<int, int> orderItemsDic = new Dictionary<int, int>();
            var orderItems = _context.OrderItems.ToList();
            foreach (var oi in orderItems)
            {
                int count = 0;
                if (orderItemsDic.ContainsKey(oi.Product_ID))
                {
                    count = oi.Quantity + orderItemsDic[oi.Product_ID];
                    orderItemsDic[oi.Product_ID] = count;
                }
                else
                {
                    orderItemsDic.Add(oi.Product_ID, oi.Quantity);
                }
            }

            int first = 0, second = 0, third = 0, firstV = 0, secondV = 0, thirdV = 0;
            first = orderItemsDic.OrderByDescending(x => x.Value).First().Key;
            second = orderItemsDic.OrderByDescending(x => x.Value).Skip(1).First().Key;
            third = orderItemsDic.OrderByDescending(x => x.Value).Skip(2).First().Key;

            firstV = orderItemsDic.OrderByDescending(x => x.Value).First().Value;
            secondV = orderItemsDic.OrderByDescending(x => x.Value).Skip(1).First().Value;
            thirdV = orderItemsDic.OrderByDescending(x => x.Value).Skip(2).First().Value;

            BestSeller bs1 = new BestSeller
            {
                ProductName = products.FirstOrDefault(p => p.Product_ID == first).ProductName,
                Quantity = firstV,
                Total = products.FirstOrDefault(p => p.Product_ID == first).SellingPrice * firstV
            };
            BestSeller bs2 = new BestSeller
            {
                ProductName = products.FirstOrDefault(p => p.Product_ID == second).ProductName,
                Quantity = secondV,
                Total = products.FirstOrDefault(p => p.Product_ID == second).SellingPrice * secondV
            };
            BestSeller bs3 = new BestSeller
            {
                ProductName = products.FirstOrDefault(p => p.Product_ID == third).ProductName,
                Quantity = thirdV,
                Total = products.FirstOrDefault(p => p.Product_ID == third).SellingPrice * thirdV
            };

            bestSellers.Add(bs1);
            bestSellers.Add(bs2);
            bestSellers.Add(bs3);

            return bestSellers;
        }

        public PartialViewResult OnGetBestsellersView()
        {
            return new PartialViewResult
            {
                ViewName = "_BestSellers",
                ViewData = new ViewDataDictionary<List<BestSeller>>(ViewData, GetBestsellers())
            };
        }
        #endregion

        #region Feedbacks
        public List<FeedbackDBViewModel> GetFeedbackData()
        {
            DateTime start = new DateTime();
            switch (Metrics)
            {
                case "Daily":
                    start = DateTime.Today;
                    break;
                case "Weekly":
                    start = DateTime.Today.AddDays(-7);
                    break;
                case "Monthly":
                    start = DateTime.Today.AddMonths(-1);
                    break;
                case "Overall":
                    start = new DateTime(2018, 01, 01, 00, 00, 00);
                    break;
                default:
                    start = DateTime.Today.AddDays(-7);
                    break;
            }
            List<FeedbackDBViewModel> feedbackDBViewModels = new List<FeedbackDBViewModel>();
            Feedbacks = _context.Feedbacks.ToList();
            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                foreach (Form form in context.Forms.ToList())
                {
                    Dictionary<string, double> perQPerc = new Dictionary<string, double>();
                    List<double> overallCounts = new List<double>();
                    foreach (var question in context.Questions.Where(q => q.Form_ID == form.Form_ID))
                    {
                        int count = 0;
                        double formAnswerCount = 0;
                        foreach (var answer in context.Answers.Where(a => a.Question_ID == question.Question_ID && a.Form_ID == form.Form_ID))
                        {
                            foreach (var feedback in _context.Feedbacks.Where(f => f.SubmittedDate >= start && f.SubmittedDate <= DateTime.Today && f.Form_ID == form.Form_ID && f.RefNo == answer.Feedback_ID))
                            {
                                if (answer.QAnswer != "")
                                {
                                    formAnswerCount += Convert.ToDouble(answer.QAnswer);
                                }
                                else
                                {
                                    formAnswerCount += 0;
                                }
                                count++;
                            }
                        }
                        formAnswerCount = formAnswerCount / count;
                        formAnswerCount = Math.Round(formAnswerCount, 2);
                        if (Double.IsNaN(formAnswerCount))
                        {
                            perQPerc.Add(question.Description, 0);
                        }
                        else
                        {
                            perQPerc.Add(question.Description, formAnswerCount);
                        }
                        overallCounts.Add(formAnswerCount);
                    }

                    int c = 0;
                    double overallCount = 0;
                    foreach (var value in overallCounts)
                    {
                        overallCount += value;
                        c++;
                    }
                    overallCount = overallCount / c;
                    if (Double.IsNaN(overallCount))
                    {
                        overallCount = 0;
                    }
                    else
                    {
                        overallCount = Math.Round(overallCount, 2);
                    }
                    FeedbackDBViewModel fdbv = new FeedbackDBViewModel();
                    fdbv.Metrics = Metrics;
                    fdbv.FormName = form.FormName;
                    fdbv.FormFeedbackCount = Feedbacks.Where(f => f.Form_ID == form.Form_ID && f.SubmittedDate >= DateTime.Today.AddDays(-7) && f.SubmittedDate <= DateTime.Today).Count();
                    fdbv.Overall = overallCount;
                    fdbv.QuestionsPerc = perQPerc;
                    feedbackDBViewModels.Add(fdbv);
                }
            }

            return feedbackDBViewModels;
        }

        public PartialViewResult OnGetFeedbacksView()
        {
            return new PartialViewResult
            {
                ViewName = "_FeedbackDBBlocks",
                ViewData = new ViewDataDictionary<List<FeedbackDBViewModel>>(ViewData, GetFeedbackData())
            };
        }
        #endregion

        #region UserReadWhatsNew
        public IActionResult OnGetUserReadWhatsNew()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var hr = _userManager.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).ReadWhatsNew;

            result.Add("hasread", hr);
            ChangeLog = _context.ChangeLog.ToList();
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public async Task<IActionResult> OnPostHasRead()
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            user.ReadWhatsNew = "Yes";
            await _userManager.UpdateAsync(user);
            ChangeLog = _context.ChangeLog.ToList();
            return Page();
        }
        #endregion

        #region MetricsChange
        public IActionResult OnPostMetricsChange(string metrics)
        {
            ChangeLog = _context.ChangeLog.ToList();
           // Metrics = metrics;
            return Page();
        }
        #endregion
    }
}
