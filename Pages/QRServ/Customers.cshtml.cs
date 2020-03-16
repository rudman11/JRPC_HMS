using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JRPC_HMS.Pages.QRServ
{
    [Authorize(Roles = "Admin, User")]
    public class CustomersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public CustomersModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });

        public IList<Customer> Customers { get; set; } = new List<Customer>();
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public List<int> DeleteCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CustomerSort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Customer>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            CustomerSort = String.IsNullOrEmpty(sortOrder) ? "customer_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var customers = from p in _context.Customers.Where(c => c.ForFeedback == "Yes")
                             select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                customers = customers.Where(s => s.Name.Contains(SearchString) || s.Phone.Contains(SearchString) || s.Email.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "customer_desc":
                    customers = customers.OrderByDescending(s => s.Name);
                    break;
                default:
                    customers = customers.OrderBy(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = customers.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await customers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Customers = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteCategory != null)
            {
                foreach (var catId in DeleteCategory)
                {
                    _context.Categories.Remove(_context.Categories.FirstOrDefault(c => c.Cat_ID == catId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Category " + _context.Categories.FirstOrDefault(c => c.Cat_ID == catId).CategoryName + " successfully deleted.";
                }
            }
            Customers = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string categoryName, string sortOrder)
        {
            Customers = await InitiateView(currentPage, pageSize, categoryName, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Customers Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Customers");
                ExcelWorksheet customersSheet = package.Workbook.Worksheets["Customers"];
                using (var range = customersSheet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                customersSheet.Cells[1, 1].Value = "Customer Id";
                customersSheet.Cells[1, 2].Value = "Customer Name";
                customersSheet.Cells[1, 3].Value = "Customer Phone";
                customersSheet.Cells[1, 4].Value = "Customer Email";
                customersSheet.Cells[1, 1, 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Customers.Count(); r++)
                {
                    Customer customerC = _context.Customers.FirstOrDefault(c => c.Id == Customers.ToList()[r].Id);
                    customersSheet.Cells[r + 2, 1].Value = Customers.ToList()[r].Id;
                    customersSheet.Cells[r + 2, 2].Value = Customers.ToList()[r].Name;
                    customersSheet.Cells[r + 2, 3].Value = Customers.ToList()[r].Phone;
                    customersSheet.Cells[r + 2, 4].Value = Customers.ToList()[r].Email;
                    customersSheet.Cells[1, 1, 1, 4].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Customers successfully exported to file " + sFileName;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}