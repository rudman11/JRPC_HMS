using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
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

namespace JRPC_HMS.Pages.QRServ
{
    [Authorize(Roles = "Admin, User")]
    public class FeedbackIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public FeedbackIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Properties
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 5, 10, 20, 50, 100, 200 });
        public SelectList StatusList { get; } = new SelectList(new string[] { "NEGATIVE", "POSITIVE" });
        public SelectList ContactCustomerList { get; } = new SelectList(new string[] { "Yes", "No" });
        public IList<SelectListItem> ServantList { get; set; }
        public IList<SelectListItem> FormList { get; set; }
        public IList<SelectListItem> CustomerList { get; set; }

        public IList<Feedback> Feedbacks { get; set; }
        public IList<Form> AllForms { get; set; }
        public IList<Customer> Customers { get; set; }
        public IList<Question> Questions { get; set; }
        public IList<Answer> Answers { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StartDateSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EndDateSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FormSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ServantSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StatusSearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ContactCustomerSearch { get; set; }
        #endregion

        #region InitiateView
        public async Task<List<Feedback>> InitiateView(int currentPage, int pageSize, string startDate, string endDate, string customer, string form, string servant, string status, string contactCustomer)
        {
            CustomerList = _context.Customers.Where(c => c.ForFeedback == "Yes").Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();

            FormList = _context.Forms.Select(n => new SelectListItem
            {
                Value = n.Form_ID.ToString(),
                Text = n.FormName
            }).ToList();

            ServantList = _context.Feedbacks.Select(n => new SelectListItem
            {
                Value = n.Servant,
                Text = n.Servant
            }).ToList();

            var feedbacks = from p in _context.Feedbacks
                         select p;

            if (!string.IsNullOrEmpty(StartDateSearch))
            {
                feedbacks = feedbacks.Where(o => o.SubmittedDate >= Convert.ToDateTime(StartDateSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDateSearch = startDate;
                    feedbacks = feedbacks.Where(o => o.SubmittedDate >= Convert.ToDateTime(StartDateSearch));
                }
            }
            if (!string.IsNullOrEmpty(EndDateSearch))
            {
                feedbacks = feedbacks.Where(o => o.SubmittedDate <= Convert.ToDateTime(EndDateSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDateSearch = endDate;
                    feedbacks = feedbacks.Where(o => o.SubmittedDate <= Convert.ToDateTime(EndDateSearch));
                }
            }
            if (!string.IsNullOrEmpty(CustomerSearch))
            {
                feedbacks = feedbacks.Where(o => o.Customer_ID == Convert.ToInt32(CustomerSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(customer))
                {
                    CustomerSearch = customer;
                    feedbacks = feedbacks.Where(o => o.Customer_ID == Convert.ToInt32(CustomerSearch));
                }
            }
            if (!string.IsNullOrEmpty(FormSearch))
            {
                feedbacks = feedbacks.Where(o => o.Form_ID == Convert.ToInt32(FormSearch));
            }
            else
            {
                if (!string.IsNullOrEmpty(form))
                {
                    FormSearch = form;
                    feedbacks = feedbacks.Where(o => o.Form_ID == Convert.ToInt32(FormSearch));
                }
            }
            if (!string.IsNullOrEmpty(ServantSearch))
            {
                feedbacks = feedbacks.Where(o => o.Servant == ServantSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(servant))
                {
                    ServantSearch = servant;
                    feedbacks = feedbacks.Where(o => o.Servant == ServantSearch);
                }
            }
            if (!string.IsNullOrEmpty(StatusSearch))
            {
                feedbacks = feedbacks.Where(o => o.Status == StatusSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(status))
                {
                    StatusSearch = status;
                    feedbacks = feedbacks.Where(o => o.Status == StatusSearch);
                }
            }
            if (!string.IsNullOrEmpty(ContactCustomerSearch))
            {
                feedbacks = feedbacks.Where(o => o.ContactCustomer == ContactCustomerSearch);
            }
            else
            {
                if (!string.IsNullOrEmpty(contactCustomer))
                {
                    ContactCustomerSearch = contactCustomer;
                    feedbacks = feedbacks.Where(o => o.ContactCustomer == ContactCustomerSearch);
                }
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = feedbacks.Count();

            PageSize = pageSize == 0 ? 5 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            Customers = _context.Customers.ToList();
            Questions = _context.Questions.ToList();
            Answers = _context.Answers.ToList();
            AllForms = _context.Forms.ToList();
            return await feedbacks
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string startDate, string endDate, string customer, string form, string servant, string status, string contactCustomer)
        {
            Feedbacks = await InitiateView(currentPage, pageSize, startDate, endDate, customer, form, servant, status, contactCustomer);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string startDate, string endDate, string customer, string form, string servant, string status, string contactCustomer)
        {
            Feedbacks = await InitiateView(currentPage, pageSize, startDate, endDate, customer, form, servant, status, contactCustomer);
        }

        public async Task<IActionResult> OnPostExport(int currentPage, int pageSize, string startDate, string endDate, string customer, string form, string servant, string status, string contactCustomer)
        {
            Feedbacks = await InitiateView(currentPage, pageSize, startDate, endDate, customer, form, servant, status, contactCustomer);

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
                for (int r = 0; r < Feedbacks.Count(); r++)
                {
                    //orderSheet.Cells[r + 2, 1].Value = Feedbacks.ToList()[r].OrderStatus;
                    //orderSheet.Cells[r + 2, 2].Value = Feedbacks.ToList()[r].OrderId;
                    //orderSheet.Cells[r + 2, 3].Value = Feedbacks.ToList()[r].PaymentStatus;
                    //orderSheet.Cells[r + 2, 4].Value = Feedbacks.ToList()[r].PaymentMethod;
                    //orderSheet.Cells[r + 2, 5].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                    //orderSheet.Cells[r + 2, 5].Value = Feedbacks.ToList()[r].OrderDate;
                    //orderSheet.Cells[r + 2, 6].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 6].Value = Feedbacks.ToList()[r].Paid;
                    //orderSheet.Cells[r + 2, 7].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 7].Value = Feedbacks.ToList()[r].Change;
                    //orderSheet.Cells[r + 2, 8].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 8].Value = Feedbacks.ToList()[r].OrderDiscount;
                    //orderSheet.Cells[r + 2, 8].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 9].Value = Feedbacks.ToList()[r].OrderTotal;
                    //orderSheet.Cells[r + 2, 9].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 10].Value = Feedbacks.ToList()[r].VatTotal;
                    //orderSheet.Cells[r + 2, 10].Style.Numberformat.Format = "R0.00";
                    //orderSheet.Cells[r + 2, 11].Value = Feedbacks.ToList()[r].GrandTotal;
                    //orderSheet.Cells[r + 2, 12].Value = Feedbacks.ToList()[r].PreparationStatus;
                    //orderSheet.Cells[r + 2, 13].Value = Feedbacks.ToList()[r].Username;
                    //orderSheet.Cells[r + 2, 14].Value = Forms.FirstOrDefault(s => s.Store_ID == Feedbacks.ToList()[r].StoreId).StoreName;
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