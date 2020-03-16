using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Sales
{
    [Authorize(Roles = "Admin, User")]
    public class OrdersIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        #region Properties
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });
        public SelectList PaymentMethodList { get; } = new SelectList(new string[] { "Cash", "Card" });
        public SelectList PaymentStatusList { get; } = new SelectList(new string[] { "Paid", "Incompleted" });
        public SelectList OrderStatusList { get; } = new SelectList(new string[] { "Started", "In Progress", "Completed", "Incompleted", "Voided" });
        public SelectList PrepStatusList { get; } = new SelectList(new string[] { "Completed", "Incompleted" });
        public IList<SelectListItem> UsernameList { get; set; }

        public IList<Store> Stores { get; set; }
        public IList<Order> Orders { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StartOrderDateSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EndOrderDateSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PaymentMethodSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PaymentStatusSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderStatusSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PrepStatusSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UsernameSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderStatusSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PrepStatusSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PaymentStatusSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PaymentMethodSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string OrderDateSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UsernameSort { get; set; }
        #endregion

        #region InitiateView
        public async Task<List<Order>> InitiateView(int currentPage, int pageSize, string startOrderDate, string endOrderDate, string paymentMethod, string paymentStatus, string orderStatus, string prepStatus, string username, string sortOrder)
        {
            CurrentSort = sortOrder;
            OrderStatusSort = String.IsNullOrEmpty(sortOrder) ? "orderstatus_desc" : "";
            PrepStatusSort = String.IsNullOrEmpty(sortOrder) ? "prepstatus_desc" : "";
            PaymentStatusSort = String.IsNullOrEmpty(sortOrder) ? "paymentstatus_desc" : "";
            PaymentMethodSort = String.IsNullOrEmpty(sortOrder) ? "paymentmethod_desc" : "";
            OrderDateSort = String.IsNullOrEmpty(sortOrder) ? "orderdate_desc" : "";
            UsernameSort = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";

            var orders = from p in _context.Orders
                         select p;

            if (!string.IsNullOrEmpty(StartOrderDateSearch))
            {
                orders = orders.Where(o => o.OrderDate >= Convert.ToDateTime(StartOrderDateSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(startOrderDate))
                {
                    StartOrderDateSearch = startOrderDate;
                    orders = orders.Where(o => o.OrderDate >= Convert.ToDateTime(StartOrderDateSearch));
                }
            }
            if (!string.IsNullOrEmpty(EndOrderDateSearch))
            {
                orders = orders.Where(o => o.OrderDate <= Convert.ToDateTime(EndOrderDateSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(endOrderDate))
                {
                    EndOrderDateSearch = endOrderDate;
                    orders = orders.Where(o => o.OrderDate <= Convert.ToDateTime(EndOrderDateSearch));
                }
            }
            if (!string.IsNullOrEmpty(PaymentMethodSearch))
            {
                orders = orders.Where(o => o.PaymentMethod == PaymentMethodSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(paymentMethod))
                {
                    PaymentMethodSearch = paymentMethod;
                    orders = orders.Where(o => o.PaymentMethod == PaymentMethodSearch);
                }
            }
            if (!string.IsNullOrEmpty(PaymentStatusSearch))
            {
                orders = orders.Where(o => o.PaymentStatus == PaymentStatusSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    PaymentStatusSearch = paymentStatus;
                    orders = orders.Where(o => o.PaymentStatus == PaymentStatusSearch);
                }
            }
            if (!string.IsNullOrEmpty(OrderStatusSearch))
            {
                orders = orders.Where(o => o.OrderStatus == OrderStatusSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(orderStatus))
                {
                    OrderStatusSearch = orderStatus;
                    orders = orders.Where(o => o.OrderStatus == OrderStatusSearch);
                }
            }
            if (!string.IsNullOrEmpty(PrepStatusSearch))
            {
                orders = orders.Where(o => o.PreparationStatus == PrepStatusSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(prepStatus))
                {
                    PrepStatusSearch = prepStatus;
                    orders = orders.Where(o => o.PreparationStatus == PrepStatusSearch);
                }
            }
            if (!string.IsNullOrEmpty(UsernameSearch))
            {
                orders = orders.Where(o => o.Username.Contains(UsernameSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(username))
                {
                    UsernameSearch = username;
                    orders = orders.Where(o => o.Username.Contains(UsernameSearch));
                }
            }

            switch (sortOrder)
            {
                case "orderstatus_desc":
                    orders = orders.OrderByDescending(s => s.OrderStatus);
                    break;
                case "prepstatus_desc":
                    orders = orders.OrderByDescending(s => s.PreparationStatus);
                    break;
                case "paymentstatus_desc":
                    orders = orders.OrderByDescending(s => s.PaymentStatus);
                    break;
                case "paymentmethod_desc":
                    orders = orders.OrderByDescending(s => s.PaymentMethod);
                    break;
                case "orderdate_desc":
                    orders = orders.OrderByDescending(s => s.OrderDate);
                    break;
                case "username_desc":
                    orders = orders.OrderByDescending(s => s.Username);
                    break;
                default:
                    orders = orders.OrderByDescending(s => s.OrderId);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = orders.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Stores = _context.Stores.ToList();
            UsernameList = _userManager.Users.Select(n => new SelectListItem
            {
                Value = n.UserName,
                Text = n.UserName
            }).ToList();
            return await orders
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion


        public async Task OnGetAsync(int currentPage, int pageSize, string startOrderDate, string endOrderDate, string paymentMethod, string paymentStatus, string orderStatus, string prepStatus, string username, string sortOrder)
        {
            Orders = await InitiateView(currentPage, pageSize, startOrderDate, endOrderDate, paymentMethod, paymentStatus, orderStatus, prepStatus, username, sortOrder);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string startOrderDate, string endOrderDate, string paymentMethod, string paymentStatus, string orderStatus, string prepStatus, string username, string sortOrder)
        {
            Orders = await InitiateView(currentPage, pageSize, startOrderDate, endOrderDate, paymentMethod, paymentStatus, orderStatus, prepStatus, username, sortOrder);
        }

        public async Task<IActionResult> OnPostExport(int currentPage, int pageSize, string startOrderDate, string endOrderDate, string paymentMethod, string paymentStatus, string orderStatus, string prepStatus, string username, string sortOrder)
        {
            Orders = await InitiateView(currentPage, pageSize, startOrderDate, endOrderDate, paymentMethod, paymentStatus, orderStatus, prepStatus, username, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Orders Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Orders");
                ExcelWorksheet orderSheet = package.Workbook.Worksheets["Orders"];
                using (var range = orderSheet.Cells["A1:N1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                orderSheet.Cells[1, 1].Value = "Order Status";
                orderSheet.Cells[1, 2].Value = "Order Id";
                orderSheet.Cells[1, 3].Value = "Payment Status";
                orderSheet.Cells[1, 4].Value = "Payment Method";
                orderSheet.Cells[1, 5].Value = "Order Date";
                orderSheet.Cells[1, 6].Value = "Paid";
                orderSheet.Cells[1, 7].Value = "Change";
                orderSheet.Cells[1, 8].Value = "Order Discount";
                orderSheet.Cells[1, 9].Value = "Sub Total";
                orderSheet.Cells[1, 10].Value = "VAT Total";
                orderSheet.Cells[1, 11].Value = "Grand Total";
                orderSheet.Cells[1, 12].Value = "Preparation Status";
                orderSheet.Cells[1, 13].Value = "Username";
                orderSheet.Cells[1, 14].Value = "Store";
                for (int r = 0; r < Orders.Count(); r++)
                {
                    orderSheet.Cells[r + 2, 1].Value = Orders.ToList()[r].OrderStatus;
                    orderSheet.Cells[r + 2, 2].Value = Orders.ToList()[r].OrderId;
                    orderSheet.Cells[r + 2, 3].Value = Orders.ToList()[r].PaymentStatus;
                    orderSheet.Cells[r + 2, 4].Value = Orders.ToList()[r].PaymentMethod;
                    orderSheet.Cells[r + 2, 5].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                    orderSheet.Cells[r + 2, 5].Value = Orders.ToList()[r].OrderDate;
                    orderSheet.Cells[r + 2, 6].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 6].Value = Orders.ToList()[r].Paid;
                    orderSheet.Cells[r + 2, 7].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 7].Value = Orders.ToList()[r].Change;
                    orderSheet.Cells[r + 2, 8].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 8].Value = Orders.ToList()[r].OrderDiscount;
                    orderSheet.Cells[r + 2, 8].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 9].Value = Orders.ToList()[r].OrderTotal;
                    orderSheet.Cells[r + 2, 9].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 10].Value = Orders.ToList()[r].VatTotal;
                    orderSheet.Cells[r + 2, 10].Style.Numberformat.Format = "R0.00";
                    orderSheet.Cells[r + 2, 11].Value = Orders.ToList()[r].GrandTotal;
                    orderSheet.Cells[r + 2, 12].Value = Orders.ToList()[r].PreparationStatus;
                    orderSheet.Cells[r + 2, 13].Value = Orders.ToList()[r].Username;
                    orderSheet.Cells[r + 2, 14].Value = Stores.FirstOrDefault(s => s.Store_ID == Orders.ToList()[r].StoreId).StoreName;
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}